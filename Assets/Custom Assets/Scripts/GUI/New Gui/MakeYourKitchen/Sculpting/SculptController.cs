using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallExtrudeMovement
{
	public GameObject extrudedWall { get; private set; }
	public Vector3 	  lastPosition { get; private set; }
	public GameObject leftWall;
	public GameObject rightWall;
}

public class WallPartialExtrudeMovement : WallExtrudeMovement
{
	public GameObject[] extrudeWalls;
}

public class SculptController : MonoBehaviour
{
	public const float IPSLON = 0.0001f;
	public const float monkeyPatchUpperWallYScale = 0.0647338f; //I hate to do a shit like this...

	private Camera mainCamera;
	private GameObject wallParent;
			
	private Transform selectedWall;
	
	private bool IsExtruding;
	private bool IsSculptModeOn;
	private bool IsExtrudingXAxis;
	
	private Vector3 lastMousePosition;
		
	private int cWindowType;
	public GameObject[] WindowsModels;
	private float WindowPivotY;
	
	void Start ()
	{
		IsExtruding	   = false;
		IsSculptModeOn = false;
		IsExtrudingXAxis=false;
		
		mainCamera = GameObject.FindWithTag("MainCamera").camera;
		wallParent = GameObject.Find ("ParentParede");
		
		WindowPivotY = 0.7f;
//		WindowsModels = new Vector2[6]{new Vector2 (1.0f, 1.2f), 
//									new Vector2 (1.2f, 1.2f), 
//									new Vector2 (1.4f, 1.2f), 
//									new Vector2 (1.6f, 1.2f), 
//									new Vector2 (1.8f, 1.2f), 
//									new Vector2 (2.1f, 1.2f) };
		cWindowType = 0;
	}
	
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Keypad9))
		{
			cWindowType = cWindowType != WindowsModels.Length ? cWindowType + 1 : 0;
		}
		
		if (Input.GetKeyDown (KeyCode.J) ||
			Input.GetKeyDown (KeyCode.K))
		{
			RaycastHit hit;
			Ray ray = mainCamera.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
				
			if (!Physics.Raycast (ray, out hit))
			{
				Debug.Log ("Não bateu em nada.");
				Debug.DrawRay (ray.origin, ray.direction * 1000);
				Debug.Break ();
			}
			else if (hit.transform.tag == "Parede" && Input.GetKeyDown (KeyCode.J))
			{
				CreateWindow(ray, hit);
			}
			else if (hit.transform.tag == "Janela" && Input.GetKeyDown (KeyCode.K))
			{
				RemoveWindow(ray, hit);
			}
		}
		else if (IsSculptModeOn && Input.GetMouseButtonDown(0))
		{
			lastMousePosition = Input.mousePosition;
						
			IsExtruding = true;
		}
		else if (IsExtruding)
		{			
			lastMousePosition = Input.mousePosition;
						
			if (Input.GetMouseButtonUp (0))
			{	
				IsExtruding = false;
			}
		}
	}
	
	private void CreateWindow(Ray ray, RaycastHit hit)
	{  
		if (hit.transform.name.Equals("Upper Wall") ||
			hit.transform.name.Equals("Lower Wall"))
		{
			Debug.LogWarning ("Não foi criada a janela, pois foi selecionada a paredes de cima e de baixo de uma janela pré-existente.");
			return;
		}
	
		Vector3 hitPoint 	 = hit.point;
		Transform wallTrans  = hit.transform;
		Vector3 wallPosition = wallTrans.position;
		Vector3 wallRotation = wallTrans.eulerAngles;
		Vector3 cWallSize 	 = WindowsModels [cWindowType].transform.GetChild (0).localScale;
		Vector3 maxWallPosition = wallPosition + (wallTrans.transform.right.normalized * cWallSize.x);
		
		hitPoint.y = 0.0f;
		
		cWallSize = MeshUtils.FixZYFromBlender (cWallSize);
			
		if (cWallSize.x > Vector3.Distance (hitPoint, wallPosition) ||
			cWallSize.x > Vector3.Distance (hitPoint, maxWallPosition))
		{
			Debug.LogWarning ("Não foi criada a janela por que foi clicado muito perto do canto da parede.");
			return;
		}
			
		InfoWall infoWall = wallTrans.GetComponent<InfoWall> ();
        	
		float cRightScaleX	= Vector3.Distance (wallPosition, hitPoint) - (cWallSize.x / 2.0f); 
		float cLeftScaleX 	= (wallTrans.localScale.x - cRightScaleX) 	- (cWallSize.x);
		float cMiddleScaleX = wallTrans.localScale.x - cRightScaleX - cLeftScaleX;
		
		float cMiddleUpperScaleY = wallTrans.localScale.y - cWallSize.y - WindowPivotY - monkeyPatchUpperWallYScale;
		float cMiddleLowerScaleY = WindowPivotY;

			
		Vector3 leftWallPosition = wallPosition - 
									   	(wallTrans.transform.right.normalized *
										(cRightScaleX + cWallSize.x));
		Vector3 middleWallPosition = wallPosition - 
									   	(wallTrans.transform.right.normalized * cRightScaleX);
		leftWallPosition.y = 0.0f;

		GameObject rightWall = Instantiate (wallTrans.gameObject,
    										wallTrans.position,
    										wallTrans.rotation) as GameObject;
		rightWall.name = "Right Wall";
		rightWall.transform.localScale = new Vector3 (cRightScaleX,
    												  wallTrans.localScale.y,
    												  wallTrans.localScale.z);
		ResizeWallMaterial (rightWall, cRightScaleX, wallTrans.localScale.y, 0, 0);

		middleWallPosition.y = 0.0f;
		GameObject lowerWall = Instantiate (wallTrans.gameObject,
    									    middleWallPosition,
    									    wallTrans.rotation) as GameObject;
		lowerWall.name = "Lower Wall";
		lowerWall.transform.localScale = new Vector3 (cMiddleScaleX,
    												  cMiddleLowerScaleY,
    												  wallTrans.localScale.z);
		ResizeWallMaterial (lowerWall, cMiddleScaleX, cMiddleLowerScaleY, cRightScaleX, 0.003f);

		middleWallPosition.y = cWallSize.y + WindowPivotY;
		GameObject upperWall = Instantiate (wallTrans.gameObject,
    									    middleWallPosition,
    									    wallTrans.rotation) as GameObject;
		upperWall.name = "Upper Wall";
		upperWall.transform.localScale = new Vector3 (cMiddleScaleX,
    												  cMiddleUpperScaleY,
    												  wallTrans.localScale.z);
		ResizeWallMaterial (upperWall,
							cMiddleScaleX,
							cMiddleUpperScaleY,
							cRightScaleX,
							cMiddleLowerScaleY + cWallSize.y + 0.01f);

		GameObject leftWall = Instantiate (wallTrans.gameObject,
    									   leftWallPosition,
    									   wallTrans.rotation) as GameObject;
		leftWall.name = "Left Wall";
		leftWall.transform.localScale = new Vector3 (cLeftScaleX,
    												 wallTrans.localScale.y,
    												 wallTrans.localScale.z);
		ResizeWallMaterial (leftWall,
							cLeftScaleX,
							wallTrans.localScale.y,
							cRightScaleX + cMiddleScaleX,
							0);

		GameObject wnd = Instantiate (WindowsModels [cWindowType],
										new Vector3(middleWallPosition.x,
													WindowPivotY + cWallSize.y / 2.0f,
													middleWallPosition.z) 
											- wallTrans.right.normalized * cWallSize.x / 2.0f,
										wallTrans.rotation) as GameObject;
			
		//Rotacionando para o lado correto (ao contrário da parede)
		wnd.transform.Rotate (Vector3.up * 180);
			
		 leftWall.AddComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
		rightWall.AddComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
		lowerWall.AddComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
		upperWall.AddComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
			
		GameObject packWall = new GameObject ("PackSculptWall");

		packWall.AddComponent<SculptedWall> ().sculptedWindow  = wnd;
		wnd.AddComponent<SculptedWindow> ().sculptedWall 	   = packWall;
					
		packWall.transform.parent = wallTrans.parent;
						   
		 leftWall.transform.parent = packWall.transform;
		rightWall.transform.parent = packWall.transform;
		lowerWall.transform.parent = packWall.transform;
		upperWall.transform.parent = packWall.transform;
			
		GameObject wnds = GameObject.Find ("Windows");
		if (wnds == null)
		{
			wnds = new GameObject ("Windows");
			wnds.transform.parent = wallParent.transform.parent;
		}
			
		wnd.transform.parent = wnds.transform;

		Debug.Break ();

		Destroy (wallTrans.gameObject);
	}
	
	private void RemoveWindow (Ray ray, RaycastHit hit)
	{
		float WallSizeX = 0.0f;
		GameObject packWalls, newWall;
		Vector3    newWallPosition = Vector3.zero;
		Quaternion newWallRotation = Quaternion.identity;
		Debug.LogWarning ("hit.name: " + hit.transform.name);
		packWalls = hit.transform.GetComponent<SculptedWindow>().sculptedWall;
		
		foreach(Transform pWall in packWalls.transform)
		{
			if (pWall.name.Equals ("Left Wall" ) ||
				pWall.name.Equals ("Lower Wall"))
			{
				WallSizeX += pWall.transform.localScale.x;
			}
			else if (pWall.name.Equals ("Right Wall"))
			{
				newWallRotation = pWall.rotation;
				newWallPosition = pWall.position;
				WallSizeX 	   += pWall.transform.localScale.x;
			}
		}
		
		newWall = Instantiate(	packWalls.transform.GetChild(0).gameObject,
								newWallPosition,
								newWallRotation) as GameObject;
		newWall.transform.localScale = new Vector3( WallSizeX,
    												WallBuilder.WALL_HEIGHT,
    												hit.transform.localScale.z);
    	newWall.name = "Unpacked Wall";
    	newWall.transform.parent = packWalls.transform.parent;
    	ResizeWallMaterial (newWall, WallSizeX, WallBuilder.WALL_HEIGHT,0,0);
    	
    	Destroy (hit.transform.gameObject);
		Destroy (packWalls);
	}
	
	private void ResizeWallMaterial (GameObject cWall, float wallScaleX, float wallScaleY, float offsetX, float offsetY)
	{
		Vector2 textScale  = new Vector2 (wallScaleX, wallScaleY);
		Vector2 textOffset = new Vector2 (offsetX, offsetY);

		foreach (Material cMaterial in cWall.transform.GetChild(0).renderer.materials)
		{
			cMaterial.mainTextureScale  = textScale;
			cMaterial.mainTextureOffset = textOffset;
			cMaterial.SetTextureScale  ("_BumpMap", textScale);
			cMaterial.SetTextureOffset ("_BumpMap", textOffset);
		}
	}
}