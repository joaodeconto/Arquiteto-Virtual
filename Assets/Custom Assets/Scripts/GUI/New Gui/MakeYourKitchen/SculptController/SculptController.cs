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
	private Transform tmpTransf;
	
	private bool IsExtruding;
	private bool IsSculptModeOn;
	
	private List<WallExtrudeMovement> wallExtrudeMovements;
	
	void Start ()
	{
		IsExtruding	   = false;
		IsSculptModeOn = false;
		
		wallExtrudeMovements = new List<WallExtrudeMovement>();
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
		
		if (IsSculptModeOn && Input.GetMouseButton(0))
		{
			SelectWallToExtrude ();
		}
		
		if (IsExtruding)
		{
			
		}
	}
	
	private void SelectWallToExtrude ()
	{
		ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		
		if (Physics.Raycast (ray, out hit))
		{
			tmpTransf = hit.transform;
			if (tmpTransf.gameObject.layer != LayerMask.NameToLayer("GUI"))
			{
				if (tmpTransf.tag == "Parede")
				{
					//Achando paredes adjacentes
					GameObject leftWall  = null;
					GameObject rightWall = null;
					float minDistanceLeftWall  = float.MaxValue;
					float minDistanceRightWall = float.MaxValue;
					
					Vector3 wallPosition = tmpTransf.position;
					
					GameObject[] walls = GameObject.FindGameObjectsWithTag ("Parede");
					foreach (GameObject wall in walls)
					{
						//verificando distancia parede adjacente direita
//						if (wall.transform.position)
					}
					wallExtrudeMovements.Add(new WallExtrudeMovement());
				}
			}
		}
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