using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class CameraController : MonoBehaviour {
	
	[System.Serializable]
	public class InterfaceGUI
	{
		public GameObject main;
		public GameObject panelFloor;
		public GameObject uiRootFPS;
		public GameObject viewUIPiso;
		public GameObject panelInfo;
		public GameObject lists;
		public GameObject panelMobile;
	}
	
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
	
	public InterfaceGUI interfaceGUI;

	public bool areWallsAlwaysVisible { get; private set; }
	
	public Camera mainCamera { get; private set; }
	public GameObject firstPersonCamera { get; private set; }
	
	public List<InfoWall> walls { get; private set; }
	
	internal bool setFirstPerson;
		
	private GameObject ceilParent;
	private GameObject floorParent;
	private GameObject wallParent;
				
	private bool showCeil;
	private bool showFloor;
	
	private bool isGuiLocked;
	
	private Vector3 lastCamPosition;
		
	void Start ()
	{
		mainCamera 		  = GameObject.FindWithTag ("MainCamera").camera;
		firstPersonCamera = GameObject.Find ("First Person Controller");
		firstPersonCamera.gameObject.SetActiveRecursively (false);//disable other cam
		setFirstPerson = false;
		
		//NGUI Monkey patch gonna patch...
		mainCamera.transform.RotateAround(mainCamera.transform.right, 0.2f);//It's Rad measure
		mainCamera.transform.position 		 	 = new Vector3 (WallBuilder.ROOT.x, 1.7f, WallBuilder.ROOT.z - 3.6f);
		firstPersonCamera.transform.position = new Vector3 (WallBuilder.ROOT.x		 , 1.5f, WallBuilder.ROOT.z);

		ceilParent  = GameObject.Find ("ParentTeto");
		floorParent = GameObject.Find ("ParentChao");
		wallParent  = GameObject.Find ("ParentParede");
				
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
		
		interfaceGUI.uiRootFPS.SetActiveRecursively (false);
		
		InvokeRepeating("VerifyWallVisibility", rateRefreshWallsVisibility, rateRefreshWallsVisibility);
	}
	
	void Update ()
	{
		if (setFirstPerson) return;
		
		//Show/Hide ceil and floor
		
		//Verify if changed state of showCeil
		if (showCeil != mainCamera.transform.position.y < ceilParent.transform.position.y + 2.45f)
		{
			showCeil = !showCeil;
			foreach (Transform ceil in ceilParent.transform)
			{
				ceil.renderer.enabled = showCeil;
				ceil.collider.enabled = showCeil;
			}
		}
		
		//Verify if changed state of showFloor 
		if (showFloor != mainCamera.transform.position.y > floorParent.transform.position.y)
		{
			showFloor = !showFloor;
			foreach (Transform floor in floorParent.transform)
			{
				if (!floor.name.Contains("Fantasma"))
				{
					floor.renderer.enabled = showFloor;
					floor.collider.enabled = showFloor;
				}
			}
		}
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
		
		firstPersonCamera.GetComponent<ColliderControl>().IsPanelFloor = 
			 interfaceGUI.panelFloor.active ? true : false;
		
		interfaceGUI.lists.SetActiveRecursively(false);
		
		foreach (Transform child in interfaceGUI.main.GetComponentsInChildren<Transform>()) {
			child.gameObject.SetActiveRecursively(false);
		}
		
		
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
		StartCoroutine(SendReportData("http://www.visiorama360.com.br/Telasul/uploadReport.php"));
	}
	
	public void ShowHideWalls ()
	{
		areWallsAlwaysVisible = !areWallsAlwaysVisible;
		if(areWallsAlwaysVisible)
		{
			foreach (Transform wall in wallParent.transform) 
			{
				wall.GetChild(0).renderer.material = wallMaterial;
				
				if (wall.name.Contains("Parede"))
					wall.GetChild(0).renderer.material.color = wall.gameObject.GetComponent<InfoWall>().color;
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
		if (setFirstPerson || areWallsAlwaysVisible)
		{
			return;
		}
		
		Vector3 camForwardVector = mainCamera.transform.forward;
		
		if (Mathf.Abs(Vector3.Distance(camForwardVector,lastCamPosition)) < 0.1f)
			return;
			
		foreach (Transform wall in wallParent.transform)
		{
			if (wall.name.Contains("Quina"))
				continue;
			//se a cor do InfoWall da parede for diferente da cor do material do renderer da parede
			//atualiza a cor do InfoWall
			if (wall.transform.GetChild(0).renderer.materials[0].color != wall.GetComponent<InfoWall> ().color)	
				wall.GetComponent<InfoWall> ().color = wall.transform.GetChild(0).renderer.materials[0].color;
				
			if (Vector3.Angle (camForwardVector, wall.transform.forward) < 120f)
			{
				ChangeWallMaterial (wall,
								  	wallMaterial,
								  	wall.GetComponent<InfoWall> ().color,
								   	true);
			}
			else
			{
				ChangeWallMaterial (wall,
								  	wallMaterialTransparent,
								  	wall.GetComponent<InfoWall> ().color,
								   	false);
			}
		}
	}
	
	private IEnumerator SendScreenshotToForm (string screenShotURL)
	{
		bool isPanelFloor = interfaceGUI.panelFloor.active ? true : false;
		
		bool isPanelInfo = interfaceGUI.panelInfo.active ? true : false;
		
		bool isPainter = mainCamera.GetComponentInChildren<Painter>().enabled ? true : false;
		
		interfaceGUI.lists.SetActiveRecursively(false);
		mainCamera.GetComponentInChildren<Painter>().enabled = false;
		
		Transform[] allchids = interfaceGUI.main.GetComponentsInChildren<Transform>();
		foreach (Transform child in allchids) {
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
		
		foreach (Transform child in allchids) {
				child.gameObject.SetActiveRecursively(true);
		}
		if (!isPanelFloor) {
			interfaceGUI.viewUIPiso.SetActiveRecursively(false);
			interfaceGUI.panelFloor.SetActiveRecursively(false);
		}
		if (!isPanelInfo) {
			interfaceGUI.panelInfo.SetActiveRecursively(false);
			interfaceGUI.panelMobile.SetActiveRecursively(false);
		}
		if (isPainter) {
			mainCamera.GetComponentInChildren<Painter>().enabled = true;
		}
		interfaceGUI.uiRootFPS.SetActiveRecursively(false);
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
			
	Transform wallChild;
	private void ChangeWallMaterial (Transform wall, Material newMaterial, Color selectedColor, bool enableWall)
	{
		wallChild = wall.transform.GetChild(0);
		if (!wallChild.renderer.material.name.Equals(newMaterial.name + " (Instance)"))
		{
			wallChild.renderer.material 	  = newMaterial;
			wallChild.renderer.material.color = selectedColor;
					
			float wallScaleX = wall.localScale.x;
				
			foreach (Material cMaterial in wall.transform.GetChild(0).renderer.materials)
			{
				cMaterial.mainTextureScale = new Vector2 (wallScaleX, 1);
				cMaterial.SetTextureScale ("_BumpMap", new Vector2 (wallScaleX, 1));
			}
				
			if (wall.collider != null)
			{
				wall.collider.enabled = enableWall;
			}
			
		}
	}		
}