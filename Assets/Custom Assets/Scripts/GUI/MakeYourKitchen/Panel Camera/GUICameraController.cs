using UnityEngine;/*{*/
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.RegularExpressions;

public class GUICameraController : MonoBehaviour {

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

	private const string reportUploadFile = "uploadReport.php";
	private const string screenshotUploadFile = "uploadScreenshot.php";

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

	public Pause pause;

	public bool areWallsAlwaysVisible { get; private set; }

	public Camera mainCamera { get; private set; }
	public GameObject firstPersonCamera { get; private set; }

	public List<InfoWall> walls { get; private set; }

	internal bool setFirstPerson;

	private GameObject ceilParent;
	private GameObject floorParent;
	public GameObject wallParent  { get; private set; }

	private bool showCeil;
	private bool showFloor;

	private bool isGuiLocked;

	private Vector3 lastCamPosition;

	#region Save Screenshot GUI vars
	private string m_textPath;
	private FileBrowserSave m_fileBrowser;
	[SerializeField]
	protected GUISkin m_guiSkin;
	[SerializeField]
	protected Texture2D m_directoryImage,
                        m_fileImage;
	#endregion

	void Start ()
	{
		pause.Initialize();

		mainCamera 		  = GameObject.FindWithTag ("MainCamera").camera;
		//disable other cam
		#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
		GameObject.Find ("First Person Controller").SetActiveRecursively (false);

		firstPersonCamera = GameObject.Find ("First Person Controller Mobile");
		firstPersonCamera.SetActiveRecursively (false);
		firstPersonCamera.transform.GetChild(0).localPosition = new Vector3(WallBuilder.ROOT.x,
																			1.5f,
																			WallBuilder.ROOT.z);
		#else
		GameObject.Find ("First Person Controller Mobile").SetActiveRecursively (false);

		firstPersonCamera = GameObject.Find ("First Person Controller");
		firstPersonCamera.SetActiveRecursively (false);
		firstPersonCamera.transform.localPosition = new Vector3 (WallBuilder.ROOT.x,
																 1.5f,
																 WallBuilder.ROOT.z);
		#endif
		setFirstPerson = false;

		//NGUI Monkey patch gonna patch...
		mainCamera.transform.RotateAround(mainCamera.transform.right, 0.2f);//It's Rad measure
		mainCamera.transform.position = new Vector3(WallBuilder.ROOT.x,
													1.7f,
													WallBuilder.ROOT.z - 3.6f);

		ceilParent  = GameObject.Find ("ParentTeto");
		floorParent = GameObject.Find ("ParentChao");
		wallParent  = GameObject.Find ("ParentParede");

		showCeil  = true;
		showFloor = true;

		//Checking public vars
		if (Step < 1)
		 {
			Step = 1;
			Debug.LogError ("Step is a positive number and great than 1");
		}
		if (Angle < 1)
		{
			Angle = 1;
			Debug.LogError ("Angle is a positive number and great than 1");
		}
		if (ZoomSpeed < 1)
		{
			ZoomSpeed = 1;
			Debug.LogError ("ZoomSpeed is a positive number and great than 1");
		}
		if (MaxHeight < 0)
		{
			MaxHeight = 45;
			Debug.LogError ("MaxHeight is a positive number");
		}
		else if (MaxHeight < MinHeight)
		{
			MaxHeight = 45;
			MinHeight = -45;
			Debug.LogError ("MinHeight can't be greater than MaxHeight");
		}
		if (rateRefreshWallsVisibility < 0.01f || rateRefreshWallsVisibility > 10.0f)
		{
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

	#region GUI only File Browser Save
	void OnGUI ()
	{
		if (m_fileBrowser != null) {
	        GUI.skin = m_guiSkin;
			pause.GUIDraw ();
			m_fileBrowser.OnGUI ();
		}
	}
	#endregion

	public void EnableCeilFloor () {
		showCeil = showFloor = true;
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
		Renderer cRenderer = null;
		foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Parede")) {

			if (wall.name.Contains ("Quina"))
				continue;

			cRenderer = wall.transform.GetChild (0).renderer;
			//se a cor do InfoWall da parede for diferente da cor do material do renderer da parede
			//atualiza a cor do InfoWall
			if (cRenderer.materials [0].color != wall.GetComponent<InfoWall> ().color) {
				wall.GetComponent<InfoWall> ().color = cRenderer.materials [0].color;
			}

			ChangeWallMaterial (wall.transform,
							  	wallMaterial,
							   	true);
		}

		SnapBehaviour.DeactivateAll ();

		firstPersonCamera.SetActiveRecursively(true);

		#if UNITY_ANDROID || UNITY_IPHONE
		firstPersonCamera.GetComponentInChildren<ColliderControl>().IsPanelFloor = interfaceGUI.panelFloor.active;
		firstPersonCamera.GetComponentInChildren<ColliderControl>().Enable();
		#else
		firstPersonCamera.GetComponent<ColliderControl> ().IsPanelFloor = interfaceGUI.panelFloor.active;
		firstPersonCamera.GetComponent<ColliderControl> ().Enable ();
		#endif

		interfaceGUI.lists.SetActiveRecursively(false);

		foreach (Transform child in interfaceGUI.main.GetComponentsInChildren<Transform>())
		{
//			print(child);
			child.gameObject.SetActiveRecursively(false);
		}

		mainCamera.gameObject.SetActiveRecursively(false);

		setFirstPerson = true;

	}

	public void Screenshot ()
	{
		pause.TogglePause();

		//TODO colocar isso no lugar certo
		//GameObject.Find("cfg").GetComponent<Configuration>().SaveCurrentState("lolol",true);
		//GameObject.Find("cfg").GetComponent<Configuration>().LoadState("teste/teste.xml",false);
		//GameObject.Find("cfg").GetComponent<Configuration>().RunPreset(0);

#if UNITY_WEBPLAYER
		StartCoroutine ("MakeScreenshot");
#elif !(UNITY_ANDROID || UNITY_IPHONE) || UNITY_EDITOR
		m_textPath = "";
		m_fileBrowser = new FileBrowserSave (
            ScreenUtils.ScaledRectInSenseHeight(50, 50, 500, 400),
            "Salvar Screenshot",
            ImageSelectedCallback
        );
		m_fileBrowser.SelectionPattern = "*.png";
		m_fileBrowser.DirectoryImage = m_directoryImage;
		m_fileBrowser.FileImage = m_fileImage;
#endif
	}

	void ImageSelectedCallback (string path)
	{
		m_fileBrowser = null;
        m_textPath = path;

		pause.TogglePause();

		if (String.IsNullOrEmpty(m_textPath))
			m_textPath = "screenshot.png";

		if (!m_textPath.Contains(".png"))
			m_textPath += ".png";

		StartCoroutine ("MakeScreenshot");
    }

	public void Report ()
	{
		pause.TogglePause();

		//TODO make this method works
#if UNITY_WEBPLAYER
		StartCoroutine ("ReportData");
#elif !(UNITY_ANDROID || UNITY_IPHONE) || UNITY_EDITOR

		m_textPath = "";
		m_fileBrowser = new FileBrowserSave (
			ScreenUtils.ScaledRectInSenseHeight(50, 50, 500, 400),
			"Salvar Relatório",
			FileSelectedCallback
		);

		m_fileBrowser.SelectionPattern = "*.xlsx";
		m_fileBrowser.DirectoryImage = m_directoryImage;
		m_fileBrowser.FileImage = m_fileImage;
#endif
	}

	void FileSelectedCallback (string path)
	{
		m_fileBrowser = null;
        m_textPath = path;

		pause.TogglePause();

		if (String.IsNullOrEmpty(m_textPath))
			m_textPath = "relatório.xlsx";

		if (!m_textPath.Contains(".xlsx"))
			m_textPath += ".xlsx";

		StartCoroutine ("ReportData");
    }

	public void ShowHideWalls ()
	{
		areWallsAlwaysVisible = !areWallsAlwaysVisible;
		if(areWallsAlwaysVisible)
		{
			foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Parede"))
			{
				wall.transform.GetChild(0).renderer.material = wallMaterial;

				if (wall.name.Contains("Parede"))
					wall.transform.GetChild(0).
										renderer.material.
												color = wall.GetComponent<InfoWall>().color;
			}
		}
	}

	public void Zoom (int step)
	{
		Ray rayMouse = mainCamera.ScreenPointToRay(
											new Vector3(Screen.width / 2,Screen.height / 2,0));
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
		Renderer cRenderer = null;
		if (Mathf.Abs(Vector3.Distance(camForwardVector,lastCamPosition)) < 0.1f)
			return;

		foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Parede"))
		{

			if (wall.name.Contains ("Quina"))
				continue;

			cRenderer = wall.transform.GetChild(0).renderer;
			//se a cor do InfoWall da parede for diferente da cor do material do renderer da parede
			//atualiza a cor do InfoWall
			if (cRenderer.materials[0].color != wall.GetComponent<InfoWall> ().color)
			{
				wall.GetComponent<InfoWall> ().color = cRenderer.materials[0].color;
			}
			if (Vector3.Angle (camForwardVector, wall.transform.forward) > 70f)
			{
				ChangeWallMaterial (wall.transform,
								  	wallMaterial,
								   	true);
			}
			else
			{
				ChangeWallMaterial (wall.transform,
								  	wallMaterialTransparent,
								   	false);
			}
		}
	}

	private IEnumerator MakeScreenshot ()
	{
		SaveLastGUIState ();

		yield return new WaitForSeconds(0.2f);
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
		tex.Apply ();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG ();
		Destroy (tex);

#if UNITY_WEBPLAYER

		// Create a Web Form
		WWWForm form = new WWWForm ();
//	    Debug.LogError( String.Format("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png");

		string filename = String.Format ("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png";

		form.AddBinaryData ("file", bytes, filename, "multipart/form-data");

		WWW www = new WWW (screenshotUploadFile, form);

		yield return www;

		if (www.error != null){
			print (www.error);
		} else {
			print ("Finished Uploading Screenshot");
			Application.ExternalCall ("tryToDownload", pathExportImage + filename);
		}
#else
		System.IO.File.WriteAllBytes(m_textPath, bytes);
#endif

		LoadLastGUIState ();
	}

	private IEnumerator ReportData ()
	{
		SaveLastGUIState ();

		GameObject[] mobiles = GameObject.FindGameObjectsWithTag ("Movel");

		string filename = String.Format ("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now) + ".csv";
		string csvString = "LINHA: " + Line.CurrentLine.Name + "\n";

		string shortenedBrandColorName = null;

		//obtendo tipo de tampo
		foreach (Transform check in GameObject.Find ("InfoController").GetComponent<InfoController>().checkBoxTextures.transform)
		{
			if (check.name == "Label")
				continue;

			if (check.GetComponent<UICheckbox>().isChecked)
			{
				csvString += "Tampo;" + check.GetComponent<CheckBoxTextureHandler> ().texture.name + "\n";
				break;
			}
		}

		csvString += "NOME;CODIGO;LARGURA;ALTURA;PROFUNDIDADE;\n";

		foreach (GameObject mobile in mobiles) {

			InformacoesMovel info = mobile.GetComponent<InformacoesMovel> ();

			//Se for um item extra ou se for algum item que não possua código, como as lâmpadas, não adicionada no arquivo.
			if ("Extras".Equals (info.Categoria) || String.IsNullOrEmpty(info.Codigo.Trim()))
				continue;

			if (info.HasDetailMaterial())
			{
				shortenedBrandColorName = BrandColor.GetShortenedColorName(Line.CurrentLine.colors[Line.CurrentLine.GlobalDetailColorIndex]);
			}
			else
			{
				shortenedBrandColorName = "";
			}

			if (Regex.Match(info.name, "(com cooktop|com cook top)").Success)
			{
				csvString += info.Nome 		+ ";" +
							 info.Codigo 	+ ";" +
							 info.Largura 	+ ";" +
							 info.Altura 	+ ";" +
							 info.Comprimento + ";" + "\n";

				if (Regex.Match(info.name, "(Balcão triplo|Balcao triplo)").Success)
				{
					csvString += "Tampo cooktop triplo" + ";" +
								 "89121" + ";" +
								 "1200"  + ";" +
								 "30" 	 + ";" +
								 "520" 	 + ";" + "\n";
				}
				else
				{
					csvString += "Tampo cooktop duplo" + ";" +
								 "89081" + ";" +
								 "800" 	 + ";" +
								 "30" 	 + ";" +
								 "520" 	 + ";" + "\n";
				}
				csvString += "Cooktop" + ";" +
							 "89150"   + ";" +
							 "NA"	   + ";" +
							 "NA" 	   + ";" +
							 "NA" 	   + ";" + "\n";
			}
			else
			{
				csvString += info.Nome 		  + ";" +
							 info.Codigo 	  + shortenedBrandColorName + ";" +
							 info.Largura	  + ";" +
							 info.Altura 	  + ";" +
							 info.Comprimento + ";" + "\n";
			}
		}

//		print(csvString);
		Debug.Log("csvString: " + csvString);
		byte[] utf8String = Encoding.UTF8.GetBytes (csvString);
		string data 	  = Encoding.UTF8.GetString (utf8String);

//		Debug.Log("csvString: " + new String());
#if UNITY_WEBPLAYER
		WWWForm form = new WWWForm ();

		form.AddField ("CSV-FILE", 		data);
		form.AddField ("CSV-FILE-NAME", filename);

		WWW www = new WWW (reportUploadFile, form);

		yield return www;

		if (www.error != null)
			print (www.error);
		else
			Application.ExternalCall ("tryToDownload", pathExportReport + filename);
		yield return new WaitForSeconds(0.1f);
#else
		//System.IO.File.WriteAllText(m_textPath, data);

		string[] rows = data.Split ('\n');
		List<List<string>> cells = new List<List<string>> ();
		int maxRowSize = 1;

		foreach (string row in rows)
		{
			string[] col = row.Split(';');
			cells.Add ( new List<string> (col));

			if (maxRowSize < col.Length)
				maxRowSize = col.Length;
		}

		string[,] datatable = new string[rows.Length, maxRowSize];

		for (int i = 0; i != maxRowSize; ++i)
		{
			for (int j = 0; j != rows.Length; ++j)
			{
				if (cells.Count > j && cells[j].Count > i)
					datatable[j,i] = cells[j][i];
				else
					datatable[j,i] = "";
			}
		}

		GameObject.Find ("XLSXReporter").GetComponent<XLSXReporter>().CreateCommonXLSX (datatable, m_textPath);

		yield return new WaitForEndOfFrame();

#endif

		LoadLastGUIState ();
	}

	Transform wallChild;
	private void ChangeWallMaterial (Transform wall, Material newMaterial, bool enableWall)
	{
		wallChild = wall.transform.GetChild(0);

		if (!wallChild.renderer.material.name.Equals(newMaterial.name + " (Instance)"))
		{
			wallChild.renderer.material 	  	= newMaterial;
			wallChild.renderer.material.color	= wall.GetComponent<InfoWall> ().color;

			if (enableWall)
			{
				wallChild.renderer.material.mainTexture = wall.GetComponent<InfoWall> ().texture;
			}

			Vector2 textScale  = new Vector2 (wall.localScale.x,  wall.localScale.y);
			Vector2 textOffset = GetTextOffset(wall);
			foreach (Material cMaterial in wall.transform.GetChild(0).renderer.materials)
			{
				cMaterial.mainTextureScale  = textScale;
				cMaterial.mainTextureOffset = textOffset;
				cMaterial.SetTextureScale  ("_BumpMap", textScale);
				cMaterial.SetTextureOffset ("_BumpMap", textOffset);
			}

			if (wall.collider != null)
			{
				wall.collider.enabled = enableWall;
			}

		}
	}

	Vector2 GetTextOffset (Transform wall)
	{
		Transform trnsParent = wall.transform.parent;

		//Apenas paredes esculpidas serão alteradas os offsets
		if (!"PackSculptWall".Equals (trnsParent.name) ||
			  "Unpacked Wall".Equals (trnsParent.name))
			return Vector2.zero;

		Vector2 textOffset  = Vector2.zero;
		float rightWallSize = 0.0f;

		//verificando se a parede da direita é um pack
		if (trnsParent.FindChild ("Right Wall") != null)
			rightWallSize = trnsParent.FindChild ("Right Wall").localScale.x;
		else
		{
			if (trnsParent.FindChild ("PackSculptWall"))
				rightWallSize = GetPackSculptWallSize (trnsParent.FindChild ("PackSculptWall"));
			else
				rightWallSize = trnsParent.FindChild ("Unpacked Wall").localScale.x;
		}


		switch(wall.name)
		{
			case "Lower Wall":
				textOffset.x = rightWallSize;
				textOffset.y = 0.003f;
				break;
			case "Upper Wall":
				Transform trnsWnd = trnsParent.GetComponent<SculptedWall>().sculptedWindow.transform;
				textOffset.x = rightWallSize;
				textOffset.y = trnsParent.FindChild ("Lower Wall").localScale.y +
							   trnsWnd.localScale.y + 0.01f;
				break;
			case "Left Wall":
				textOffset.x = trnsParent.FindChild ("Lower Wall").localScale.x +
							   rightWallSize;
				break;
			default:
				textOffset = Vector2.zero;
				break;
		}

		return textOffset;
	}

	float GetPackSculptWallSize(Transform pack)
	{
		float packSize = pack.FindChild ("Lower Wall").localScale.x;
		if (pack.FindChild ("Right Wall"))
		{
			packSize += pack.FindChild ("Right Wall").localScale.x;
		}
		if (pack.FindChild ("Left Wall"))
		{
			packSize += pack.FindChild ("Left Wall").localScale.x;
		}
		if (pack.FindChild ("PackSculptWall"))
		{
			packSize += GetPackSculptWallSize(pack.FindChild ("PackSculptWall"));
		}

		return packSize;
	}

	bool isPanelFloor;
	bool isPanelInfo;
	bool isPainter;

	private void SaveLastGUIState()
	{
		isPanelFloor = interfaceGUI.panelFloor.active;
		isPanelInfo  = interfaceGUI.panelInfo.active;
		isPainter    = mainCamera.GetComponentInChildren<Painter> ().enabled;

		interfaceGUI.lists.SetActiveRecursively (false);
		mainCamera.GetComponentInChildren<Painter> ().enabled = false;

		Transform[] allchids = interfaceGUI.main.GetComponentsInChildren<Transform> ();
		foreach (Transform child in allchids) {
			child.gameObject.SetActiveRecursively (false);
		}
	}

	private void LoadLastGUIState ()
	{
		Debug.Log ("veio");

		interfaceGUI.main.gameObject.SetActiveRecursively (true);

		if (!isPanelFloor) {
			interfaceGUI.viewUIPiso.SetActiveRecursively (false);
			interfaceGUI.panelFloor.SetActiveRecursively (false);
		}
		if (!isPanelInfo) {
			interfaceGUI.panelInfo.SetActiveRecursively (false);
			interfaceGUI.panelMobile.SetActiveRecursively (false);
		}
		if (isPainter) {
			mainCamera.GetComponentInChildren<Painter> ().enabled = true;
		}
		interfaceGUI.uiRootFPS.SetActiveRecursively(false);

	}
}
