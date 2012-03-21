using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class CameraController : MonoBehaviour {
	
	private const string pathExportReport = "upload/export/";
	private const string pathExportImage = "upload/images/";
	
	public float Step;
	public float Angle;
	public float ZoomSpeed;
	public float MaxHeight;
	public float MinHeight;
	public float PlayerHeight;
			
	public Material wallMaterial;
	public Material wallMaterialTransparent;
	
	public float rateRefreshWallsVisibility;

	public bool areWallsAlwaysVisible {get; private set; }
	
	public Camera mainCamera { get; private set; }
	public GameObject firstPersonCamera { get; private set; }
	
	public WallsParents wallParents { get; private set; }
	
	internal bool setFirstPerson;
		
	private GameObject ceilParent;
	private GameObject floorParent;
				
	private bool showCeil;
	private bool showFloor;
	
	private bool isGuiLocked;
		
	void Start ()
	{
		mainCamera 		  = GameObject.FindWithTag ("MainCamera").camera;
		firstPersonCamera = GameObject.Find ("First Person Controller");
		firstPersonCamera.gameObject.SetActiveRecursively (false);//disable other cam
		setFirstPerson = false;
		
		//NGUI Monkey patch gonna patch...
		mainCamera.transform.RotateAround(mainCamera.transform.right, 0.2f);//It's Rad measure
		mainCamera.transform.position 		 	 = new Vector3 (WallBuilder.ROOT.x - 0.5f, 2.5f, WallBuilder.ROOT.z - 5f);
		firstPersonCamera.transform.position = new Vector3 (WallBuilder.ROOT.x		 , 1.5f, WallBuilder.ROOT.z);
		
		wallParents = new WallsParents ();
		wallParents.parentWallBack  = GameObject.Find ("ParedesBack").transform;
		wallParents.parentWallFront = GameObject.Find ("ParedesFront").transform;
		wallParents.parentWallLeft  = GameObject.Find ("ParedesLeft").transform;
		wallParents.parentWallRight = GameObject.Find ("ParedesRight").transform;
		
		wallParents.colorWallLeft  = Color.white;
		wallParents.colorWallRight = Color.white;
		wallParents.colorWallBack  = Color.white;
		wallParents.colorWallFront = Color.white;
		
		ceilParent  = GameObject.Find ("ParentTeto");
		floorParent = GameObject.Find ("ParentChao");
		
		showCeil  = true;
		showFloor = true;
		
		//Checking public vars
		if (Step < 1) {
			Step = 1;
			Debug.LogError ("Step is a positive number and great than 1");
		}
		if (Angle < 1) {
			Angle = 1;
			Debug.LogError ("Angle is a positive number and great than 1");
		}
		if (ZoomSpeed < 1) {
			ZoomSpeed = 1;
			Debug.LogError ("ZoomSpeed is a positive number and great than 1");
		}
		if (MaxHeight < 0) {
			MaxHeight = 45;
			Debug.LogError ("MaxHeight is a positive number");
		}
		else if (MaxHeight < MinHeight) {
			MaxHeight = 45;
			MinHeight = -45;
			Debug.LogError ("MinHeight can't be greater than MaxHeight");
		}
		if (rateRefreshWallsVisibility < 0.01f || rateRefreshWallsVisibility > 10.0f) {
			rateRefreshWallsVisibility = 0.1f;
			Debug.LogError ("The var RateRefreshWallsVisibility can't be lower than 0.01 seconds neither greater than 10 seconds");
		}
		
		InvokeRepeating("VerifyWallVisibility", rateRefreshWallsVisibility, rateRefreshWallsVisibility);
	}
	
	void Update ()
	{
		if (setFirstPerson) return;
		
		//Show/Hide ceil and floor
		showCeil = mainCamera.transform.position.y < ceilParent.transform.position.y + 2.45f;
		//TODO add collider here ceilParent.collider.enabled = showCeil;
		ceilParent.renderer.enabled = ceilParent.collider.enabled = showCeil;
		
		showFloor = mainCamera.transform.position.y > floorParent.transform.position.y;
		//TODO add collider here floorParent.collider.enabled = showFloor;
		floorParent.renderer.enabled = floorParent.collider.enabled = showFloor;
	}
	
	#region GUI
	public void Move (float x, float y)
	{
		Vector3 rightVector = Vector3.right * x;
		Vector3 upVector    = Vector3.up    * y;
		
		mainCamera.transform.Translate((upVector + rightVector) * Time.deltaTime * Step);
		
	}
	
	public void Rotate (float x, float y)
	{
		float horizontalAngle = Angle * x * Time.deltaTime;
		float verticalAngle   = Angle * y * Time.deltaTime;
	
		mainCamera.transform.RotateAroundLocal (mainCamera.transform.right, verticalAngle );
		mainCamera.transform.RotateAround (Vector3.up, horizontalAngle);
	}
	
	public void Play ()
	{
		SnapBehaviour.DeactivateAll ();
		
		GameObject panelFloor = GameObject.Find("Panel Floor");
		firstPersonCamera.GetComponent<ColliderControl>().IsPanelFloor = 
			panelFloor != null ? true : false;
		
		foreach (Transform child in GameObject.Find("GUI").GetComponentsInChildren<Transform>()) {
			child.gameObject.SetActiveRecursively(false);
		}
		
		GameObject.Find("GUI/Lists").SetActiveRecursively(false);
		
		//Swap cameras
		mainCamera.gameObject.SetActiveRecursively(false);
		setFirstPerson = true;
		firstPersonCamera.gameObject.SetActiveRecursively(true);
		firstPersonCamera.GetComponent<ColliderControl>().Enable();
	}
	
	public void Screenshot ()
	{
		//TODO colocar isso no lugar certo
		//GameObject.Find("cfg").GetComponent<Configuration>().SaveCurrentState("lolol",true);
		//GameObject.Find("cfg").GetComponent<Configuration>().LoadState("teste/teste.xml",false);
		//GameObject.Find("cfg").GetComponent<Configuration>().RunPreset(0);
		StartCoroutine ("SendScreenshotToForm", "http://www.visiorama360.com.br/Telasul/uploadScreenshot.php");
	}
	
	public void Report ()
	{
		//TODO make this method works
		StartCoroutine(SendReportData("http://www.visiorama360.com.br/Telasul/teste_relatorio/uploadReport.php"));
	}
	
	public void ShowHideWalls ()
	{
		areWallsAlwaysVisible = !areWallsAlwaysVisible;
		if(areWallsAlwaysVisible)
		{
			for(int i = 0; i != 4; ++i)
			{
				wallParents[i].renderer.material = wallMaterial;
				wallParents[i].renderer.material.color = wallParents.GetWallColor(i);

			}
		}
	}
	
	public void Zoom (int step)
	{			
		Ray rayMouse = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2,Screen.height / 2,0));
		Vector3 cameraPos = (rayMouse.origin - mainCamera.transform.position);
		mainCamera.transform.localPosition += cameraPos * Time.deltaTime * 10 * step;
	}
	#endregion
	private void VerifyWallVisibility ()
	{
		if (setFirstPerson) return;
		
		if(areWallsAlwaysVisible)
		{
			return;
		}
		
		Vector3 camEulerAngles = mainCamera.transform.localEulerAngles;
	
		//paredes da esquerda
		if (camEulerAngles.y > 20 && camEulerAngles.y < 160) 
		{
			ChangeWallMaterial (wallParents.parentWallLeft,
							  	wallMaterialTransparent,
							  	wallParents.colorWallLeft,
							   	false);
			
		
			wallParents.colorWallLeft = wallParents.parentWallLeft.renderer.material.color;
	
		} else {
			ChangeWallMaterial (wallParents.parentWallLeft,
						   		wallMaterial,
						    	wallParents.colorWallLeft,
						    	true);
			wallParents.colorWallLeft = wallParents.parentWallLeft.renderer.material.color;
		}
		
		//paredes da direita
		if (camEulerAngles.y > 200 && camEulerAngles.y < 340) {
		
			ChangeWallMaterial(wallParents.parentWallRight,
							   wallMaterialTransparent,
							   wallParents.colorWallRight,
							   false);
			
			wallParents.colorWallRight = wallParents.parentWallRight.renderer.material.color;
			
		} else {
			ChangeWallMaterial (wallParents.parentWallRight,
							   	wallMaterial,
							   	wallParents.colorWallRight,
							   	true);
			
			wallParents.colorWallRight = wallParents.parentWallRight.renderer.material.color;
			
		}

		//paredes de atrás
		if (camEulerAngles.y > 290 || camEulerAngles.y < 70) {
			ChangeWallMaterial (wallParents.parentWallBack,
							   	wallMaterialTransparent,
							  	wallParents.colorWallBack,
							   	false);
			
			wallParents.colorWallBack = wallParents.parentWallBack.renderer.material.color;
			
		} else {
			ChangeWallMaterial (wallParents.parentWallBack,
							   	wallMaterial,
							   	wallParents.colorWallBack,
							   	true);
			
			wallParents.colorWallBack = wallParents.parentWallBack.renderer.material.color;
			
		}
		
		//paredes de frente
		if (camEulerAngles.y > 110 && camEulerAngles.y < 270) {
		
			ChangeWallMaterial (wallParents.parentWallFront,
							   	wallMaterialTransparent,
							   	wallParents.colorWallFront,
							   	false);
			
			wallParents.colorWallFront = wallParents.parentWallFront.renderer.material.color;
			
		} else {
		
			ChangeWallMaterial (wallParents.parentWallFront,
							   	wallMaterial,
							   	wallParents.colorWallFront,
							   	true);
			
			wallParents.colorWallFront = wallParents.parentWallFront.renderer.material.color;
			
		}
	}
	
	private IEnumerator SendScreenshotToForm (string screenShotURL)
	{
		Transform gui = GameObject.Find("GUI").transform;
		foreach (Transform child in gui) {
			child.gameObject.SetActiveRecursively(false);
		}
		
		yield return new WaitForSeconds(0.1f);
		yield return new WaitForEndOfFrame();
	    
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);
	    
		// Read screen contents into the texture
		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
		tex.Apply ();
	    
		GuiScript.ShowGUI = true;
	
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG ();
		Destroy (tex);
	
		// Create a Web Form
		WWWForm form = new WWWForm ();
//	    Debug.LogError( String.Format("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png");
	    
		string filename = String.Format ("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png";
	    
		form.AddBinaryData ("file", bytes, filename, "multipart/form-data");
		
		WWW www = new WWW (screenShotURL, form);
	
		yield return www;
	    
		if (www.error != null)
			print (www.error);
		else {
			print ("Finished Uploading Screenshot"); 
			Application.ExternalCall ("tryToDownload", pathExportImage + filename);
		}
	    
		foreach (Transform child in gui) {
			child.gameObject.SetActiveRecursively(true);
		}
	}
	
	private IEnumerator SendReportData (string urlForm)
	{
		GameObject[] mobiles = GameObject.FindGameObjectsWithTag ("Movel");
	
		string filename = String.Format ("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now) + ".csv";
		string csvString = "LINHA: " + Line.CurrentLine.Name + "\r\n";
	
		csvString += "NOME;CODIGO;LARGURA;ALTURA;PROFUNDIDADE;\r\n";
	
		foreach (GameObject mobile in mobiles) {
	
			InformacoesMovel info = mobile.GetComponent<InformacoesMovel> ();
	
			if (!"Extras".Equals (info.Categoria)) {
				if (Regex.Match(info.name, "(com cooktop|com cook top)").Success) {
					csvString += info.Nome + ";" + info.Codigo + ";" + info.Largura + ";" + info.Altura + ";" + info.Comprimento + ";" + "\r\n";
					if (Regex.Match(info.name, "(Balcão triplo|Balcao triplo)").Success) {
						csvString += "Tampo cooktop triplo" + ";" + "89121" + ";" + "1200" + ";" + "30" + ";" + "520" + ";" + "\r\n";
					} else {
						csvString += "Tampo cooktop duplo" + ";" + "89081" + ";" + "800" + ";" + "30" + ";" + "520" + ";" + "\r\n";
					}
					csvString += "Cooktop" + ";" + "89150" + ";" + "NA" + ";" + "NA" + ";" + "NA" + ";" + "\r\n";
				}
				else {
					csvString += info.Nome + ";" + info.Codigo + ";" + info.Largura + ";" + info.Altura + ";" + info.Comprimento + ";" + "\r\n";
				}
			}
		}
		
		print(csvString);
		
		WWWForm form = new WWWForm ();
		
		byte[] utf8String = Encoding.UTF8.GetBytes (csvString);
		form.AddField ("CSV-FILE", Encoding.UTF8.GetString (utf8String));
		form.AddField ("CSV-FILE-NAME", filename);
	
		WWW www = new WWW (urlForm, form);
	
		yield return www;
	
		if (www.error != null)
			print (www.error);
		else {
			Application.ExternalCall ("tryToDownload", pathExportReport + filename);
		}
	}
			
	private void ChangeWallMaterial (Transform wallParent, Material newMaterial, Color selectedColor, bool enableWall)
	{	
		if (!wallParent.renderer.material.name.Equals(newMaterial.name + " (Instance)")) {
			print(selectedColor);
			wallParent.renderer.material = newMaterial;
			wallParent.renderer.material.color = selectedColor;
			if (wallParent.collider != null) {
				wallParent.collider.enabled = enableWall;
			}
		}
	}
	
}