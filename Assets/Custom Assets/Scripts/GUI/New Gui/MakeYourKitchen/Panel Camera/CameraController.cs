using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float Step;
	public float Angle;
	public float ZoomSpeed;
	public float MaxHeight;
	public float MinHeight;
	public float PlayerHeight;
			
	public Material wallMaterial;
	public Material wallMaterialTransparent;
	
	public bool AreWallsAlwaysVisible;
	public float rateRefreshWallsVisibility;
	
	private Camera mainCamera;
	private GameObject firstPersonCamera;
	
	public WallsParents wallParents { get; private set; }
	private GameObject ceilParent;
	private GameObject floorParent;
				
	private bool showCeil;
	private bool showFloor;
	
	private bool isGuiLocked;
		
	void Start()
	{
		mainCamera 		  = GameObject.Find ("Main Camera").camera;
		firstPersonCamera = GameObject.Find ("First Person Camera");
		firstPersonCamera.gameObject.SetActiveRecursively (false);//disable other cam
		
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
		
		MeshUtils.CombineMesh (wallParents.parentWallFront, true);
		MeshUtils.CombineMesh (wallParents.parentWallLeft, true);
		MeshUtils.CombineMesh (wallParents.parentWallRight, true);
		MeshUtils.CombineMesh (wallParents.parentWallBack, true);
		MeshUtils.CombineMesh (floorParent.transform, true);
		MeshUtils.CombineMesh (ceilParent.transform, false);
		
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
		//Show/Hide ceil and floor
		showCeil = mainCamera.transform.position.y < ceilParent.transform.position.y + 2.45f;
		//TODO add collider here ceilParent.collider.enabled = showCeil;
		ceilParent.renderer.enabled = showCeil;
		
		showFloor = mainCamera.transform.position.y > floorParent.transform.position.y;
		//TODO add collider here floorParent.collider.enabled = showFloor;
		floorParent.renderer.enabled = showFloor;
		
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
		
		GameObject rotationCube = GameObject.Find ("RotacaoCubo");
		if(rotationCube != null)
		{	
			foreach (Transform child in rotationCube.transform)
			{
				child.gameObject.SetActiveRecursively (false);
			}
		}
		else 
		{
			Debug.LogWarning ("Rotation Cube doesn't present in this scene!");
		}
		
		//Swap cameras
		mainCamera.enabled = false;
		firstPersonCamera.gameObject.SetActiveRecursively(true);
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
		Debug.LogError ("Report");
	}
	
	public void ShowHideWalls ()
	{
		AreWallsAlwaysVisible = !AreWallsAlwaysVisible;
		if(AreWallsAlwaysVisible)
		{
			for(int i = 0; i != 4; ++i)
			{
				wallParents[i].renderer.material = wallMaterial;
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
		if(AreWallsAlwaysVisible)
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
		} else {
			ChangeWallMaterial (wallParents.parentWallLeft,
						   		wallMaterial,
						    	wallParents.colorWallLeft,
						    	true);
		}
		
		//paredes da direita
		if (camEulerAngles.y > 200 && camEulerAngles.y < 340) {
		
			ChangeWallMaterial(wallParents.parentWallRight,
							   wallMaterialTransparent,
							   wallParents.colorWallRight,
							   false);
		} else {
			ChangeWallMaterial (wallParents.parentWallRight,
							   	wallMaterial,
							   	wallParents.colorWallRight,
							   	true);
		}

		//paredes de atrás
		if (camEulerAngles.y > 290 || camEulerAngles.y < 70) {
			ChangeWallMaterial (wallParents.parentWallBack,
							   	wallMaterialTransparent,
							  	wallParents.colorWallBack,
							   	false);
		} else {
			ChangeWallMaterial (wallParents.parentWallBack,
							   	wallMaterial,
							   	wallParents.colorWallBack,
							   	true);
		}
		
		//paredes de frente
		if (camEulerAngles.y > 110 && camEulerAngles.y < 270) {
		
			ChangeWallMaterial (wallParents.parentWallFront,
							   	wallMaterialTransparent,
							   	wallParents.colorWallFront,
							   	false);
		} else {
		
			ChangeWallMaterial (wallParents.parentWallFront,
							   	wallMaterial,
							   	wallParents.colorWallFront,
							   	true);
		}
	}
			
	private void ChangeWallMaterial (Transform wallParent, Material newMaterial, Color selectedColor, bool enableWall)
	{	
		if (wallParent.renderer.material.name != newMaterial.name + " (Instance)") {
			wallParent.renderer.material = newMaterial;
			wallParent.renderer.material.color = selectedColor;
			if (wallParent.collider != null) {
				wallParent.collider.enabled = enableWall;
			}
		}
		
		selectedColor = wallParent.renderer.material.color;
	
	}
	
	/*
	void MoveCamera ()
	{
		Vector3 direcao = (transform.right * Input.GetAxis ("Horizontal") * Time.deltaTime * 5) + (transform.forward * Input.GetAxis ("Vertical") * Time.deltaTime * 5);
		direcao = new Vector3 (direcao.x, 0, direcao.z);
		transform.position += direcao;
		if (Input.GetKey (KeyCode.Q))
			transform.eulerAngles -= new Vector3 (0, 1, 0);
		if (Input.GetKey (KeyCode.E))
			transform.eulerAngles += new Vector3 (0, 1, 0);
	}
	
	void MouseMoveCamera ()
	{
		if (!isDoingLerp) {
			if (Input.GetMouseButton (1)) {
				float x = Input.GetAxis ("Mouse X") * 2;
				float y = Input.GetAxis ("Mouse Y") * 2;
				
				camEulerAngles += new Vector3 (-y, x, 0);	
			}
			
			if (Input.GetMouseButton (2)) {
				float x = Input.GetAxis ("Mouse X");
				float y = Input.GetAxis ("Mouse Y");
				
				transform.localPosition += transform.TransformDirection (new Vector3 (x, y, 0));
			}
			
			transform.localPosition = new Vector3 (Mathf.Clamp (transform.localPosition.x, 955f, 1045f), 
			                                      Mathf.Clamp (transform.localPosition.y, -10f, 10f), 
			                                      Mathf.Clamp (transform.localPosition.z, 955f, 1045f));
			
			// Mouse wheel moving forward
			if (Input.GetAxisRaw ("Mouse ScrollWheel") > 0f && transform.position.y > -10f) {
				if (!MouseUtils.MouseClickedInArea (guiCatalogo.wndAccordMain) &&
				    !MouseUtils.MouseClickedInArea (guiCamera.wndOpenMenu) &&
				    !MouseUtils.MouseClickedInArea (guiDescription.window)) {
					Ray rayMouse = camera.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (rayMouse) || !Physics.Raycast (rayMouse)) {
						Vector3 cameraPosition = (rayMouse.origin - transform.position) * SpeedZoom;
						StartCoroutine (LerpCamera (cameraPosition, true));
					}
				}
			}
			
			// Mouse wheel moving backward
			if (Input.GetAxisRaw ("Mouse ScrollWheel") < 0f && transform.position.y < 10f) {
				if (!MouseUtils.MouseClickedInArea (guiCatalogo.wndAccordMain) &&
				    !MouseUtils.MouseClickedInArea (guiCamera.wndOpenMenu) &&
				    !MouseUtils.MouseClickedInArea (guiDescription.window)) {
					Ray rayMouse = camera.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (rayMouse) || !Physics.Raycast (rayMouse)) {
						Vector3 cameraPosition = (rayMouse.origin - transform.position) * SpeedZoom;
						StartCoroutine (LerpCamera (cameraPosition, false));
					}
				}
			}
		}
	}
	
	public static bool isDoingLerp = false;

	public static IEnumerator LerpCamera (Vector3 position, bool positive)
	{
		float i = 0;
		isDoingLerp = true;
		while (i < 1) {
			if (sCamera.transform.position.y > -10f) {
				if (positive)
					sCamera.transform.localPosition += position * Time.deltaTime * StepZoom;
				else
					sCamera.transform.localPosition -= position * Time.deltaTime * StepZoom;
				i += Time.deltaTime * 2;
				yield return null;
			} else
				break;
		}
		isDoingLerp = false;
	}
	*/
}