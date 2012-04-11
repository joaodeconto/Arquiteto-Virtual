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

	private Camera mainCamera;
		
	private RaycastHit hit;
	private Ray ray;
	private Transform selectedWall;
	
	private bool IsExtruding;
	private bool IsSculptModeOn;
	private bool IsExtrudingXAxis;
	
	private Vector3 lastMousePosition;
		
	private int cWindowType;
	private Vector2[] WindowType;
	private float WindowPivotY;
	
	void Start ()
	{
		IsExtruding	   = false;
		IsSculptModeOn = false;
		IsExtrudingXAxis=false;
		
		mainCamera = GameObject.FindWithTag("MainCamera").camera;
		
		WindowPivotY = 0.6f;
		WindowType = new Vector2[6]{new Vector2 (1.0f, 1.2f), 
									new Vector2 (1.2f, 1.2f), 
									new Vector2 (1.4f, 1.2f), 
									new Vector2 (1.6f, 1.2f), 
									new Vector2 (1.8f, 1.2f), 
									new Vector2 (2.1f, 1.2f) };
		cWindowType = 0;		
	}
	
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Keypad9))
		{
			cWindowType = cWindowType != WindowType.Length ? cWindowType + 1 : 0;
		}
		
		if (Input.GetKeyDown (KeyCode.J))
		{
			IsSculptModeOn = !IsSculptModeOn;
			//Debug.LogWarning ("IsSculptModeOn: " + IsSculptModeOn);
			
			ray = mainCamera.ScreenPointToRay(new Vector3( Screen.width / 2, Screen.height / 2, 0));
			
	        if(Physics.Raycast(ray, out hit))
	        {
	        	if (hit.transform.tag != "Parede")
	        		return;
	        	
				Transform wallTrans = hit.transform;
				
	        	InfoWall infoWall = wallTrans.GetComponent<InfoWall>();
	        	
	        	Vector3 wallPosition = wallTrans.position;
	        	Vector3 wallRotation = wallTrans.eulerAngles;
	        	
	        	Vector3 hitPoint = hit.point;
	
//				if (Vector3.Distance (wallPosition, hitPoint) < (WindowType [cWindowType].x / 2.0f))
//				{
//					hitPoint = hitPoint.normalized * (WindowType [cWindowType].x * 1.5f) ;
//				}
				
				hitPoint.y = 0.1f;
				
				float cRightScale 	= Vector3.Distance (wallPosition, hitPoint) - (WindowType [cWindowType].x / 2.0f); 
	        	float cLeftScale 	= (wallTrans.localScale.x - cRightScale) 				- (WindowType [cWindowType].x);
	        	float cMiddleScale 	= wallTrans.localScale.x - cRightScale - cLeftScale;
				float cMiddleUpperScale = wallTrans.localScale.y - WindowType [cWindowType].y - WindowPivotY;
				float cMiddleLowerScale = WindowPivotY;
				
				Vector3 leftWallPosition = wallPosition - 
										   	(wallTrans.transform.right.normalized *
											(cRightScale + (WindowType [cWindowType].x)));
				Vector3 middleWallPosition = wallPosition - 
										   	(wallTrans.transform.right.normalized * cRightScale);
				leftWallPosition.y = 0.1f;
										
	        	GameObject rightWall = Instantiate(	wallTrans.gameObject,
	        										wallTrans.position,
	        										wallTrans.rotation) as GameObject;
				rightWall.name = "Right Wall";
	        	rightWall.transform.localScale = new Vector3(cRightScale,
	        												 wallTrans.localScale.y,
	        												 wallTrans.localScale.z);
	        	
				GameObject leftWall = Instantiate (wallTrans.gameObject,
	        									   leftWallPosition,
	        									   wallTrans.rotation) as GameObject;
				leftWall.name = "Left Wall";
				leftWall.transform.localScale = new Vector3 (cLeftScale,
	        												 wallTrans.localScale.y,
	        												 wallTrans.localScale.z);
	        												
				middleWallPosition.y = WindowType [cWindowType].y + WindowPivotY;
				GameObject upperWall = Instantiate (wallTrans.gameObject,
	        									    middleWallPosition,
	        									    wallTrans.rotation) as GameObject;
				upperWall.name = "Upper Wall";
				upperWall.transform.localScale = new Vector3 (cMiddleScale,
	        												  cMiddleUpperScale,
	        												  wallTrans.localScale.z);
				middleWallPosition.y = 0;
				GameObject lowerWall = Instantiate (wallTrans.gameObject,
	        									    middleWallPosition,
	        									    wallTrans.rotation) as GameObject;
				lowerWall.name = "Lower Wall";
				lowerWall.transform.localScale = new Vector3 (cMiddleScale,
	        												  cMiddleLowerScale,
	        												  wallTrans.localScale.z);
	        	Destroy (wallTrans.gameObject);
	        	
	        	return;
	        }
		}
		
		if (IsSculptModeOn && Input.GetMouseButtonDown(0))
		{
			lastMousePosition = Input.mousePosition;
						
			IsExtruding = true;
		}
		
		if (IsExtruding)
		{			
			lastMousePosition = Input.mousePosition;
						
			if (Input.GetMouseButtonUp (0))
			{	
				IsExtruding = false;
			}
		}
	}
}