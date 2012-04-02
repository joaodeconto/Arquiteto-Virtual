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

	public Camera mainCamera;
		
	private RaycastHit hit;
	private Ray ray;
	private Transform selectedWall;
	
	private bool IsExtruding;
	private bool IsSculptModeOn;
	private bool IsExtrudingXAxis;
	
	private Vector3 lastMousePosition;
	
	private Stack<WallExtrudeMovement> wallExtrudeMovements;
	
	void Start ()
	{
		IsExtruding	   = false;
		IsSculptModeOn = false;
		IsExtrudingXAxis=false;
		
		wallExtrudeMovements = new Stack<WallExtrudeMovement>();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Keypad0))
		{
			IsSculptModeOn = !IsSculptModeOn;
			
			GameObject cameraController = GameObject.Find ("CameraController");
			cameraController.GetComponent<CameraController>().ShowHideWalls();
			
			Debug.LogWarning ("IsSculptModeOn: " + IsSculptModeOn);
		}
		
		if (IsSculptModeOn && Input.GetMouseButtonDown(0))
		{
			lastMousePosition = Input.mousePosition;
			
			SelectWallToExtrude ();
			
			IsExtruding = true;
		}
		
		if (IsExtruding)
		{
			//TODO update wall and adjacents
//			selectedWall.position = GetSelect ();
			
			lastMousePosition = Input.mousePosition;
						
			if (Input.GetMouseButtonUp (0))
			{
				//TODO add extrude to stack
//				wallExtrudeMovements.Add (new WallExtrudeMovement ());
				
				IsExtruding = false;
			}
		}
	}
	
	private void SelectWallToExtrude ()
	{
		ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast (ray, out hit))
		{
			selectedWall = hit.transform;
			if (selectedWall.gameObject.layer != LayerMask.NameToLayer("GUI"))
			{
				if (selectedWall.tag == "Parede")
				{
					if (selectedWall.eulerAngles.y > 65  && selectedWall.eulerAngles.y < 115 ||
						selectedWall.eulerAngles.y > 245 && selectedWall.eulerAngles.y < 295)
						IsExtrudingXAxis = true;
					else
						IsExtrudingXAxis = false;
				}
			}
		}
	}
	
	private Vector3 GetExtrudedWallPosition ()
	{
		ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast (ray, out hit))
		{
			
		}
		
		return Vector3.zero;
	}
	
	private void ExtrudeWholeWall ()
	{
		
	}
	
	//TODO
	private void ExtrudePartialWall ()
	{
		
	}
	
	//TODO
	private void ExtrudeDiagonalWall ()
	{
		
	}
	
}