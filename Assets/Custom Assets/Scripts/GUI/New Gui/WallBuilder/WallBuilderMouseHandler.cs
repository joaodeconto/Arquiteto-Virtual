using UnityEngine;
using System.Collections;

public class WallBuilderMouseHandler : MonoBehaviour {
	
	public WallBuilder wallBuilder;
	
	int i = 0;
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast (ray, out hit))
			{
				if (hit.transform.gameObject.layer != LayerMask.NameToLayer("GUI"))
				{
					wallBuilder.CreateWall();
				}
			}
			else
			{
				wallBuilder.CreateWall ();
			}
		}
		
		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.gameObject.layer != LayerMask.NameToLayer("GUI"))
				{
					wallBuilder.DestroyTile();
				}
			}
			else
			{
				wallBuilder.DestroyTile();
			}
		}
		
		if (Input.GetKey (KeyCode.X))
		{
			if (Input.GetKeyDown (KeyCode.Z))
			{
				wallBuilder.Undo ();
			}
			else if (Input.GetKeyDown (KeyCode.Y))
			{
				wallBuilder.Redo ();
			}
		}
		
		if (Input.GetKeyDown (KeyCode.P))
		{
			wallBuilder.ClearCaches ();
		}
	}
}
