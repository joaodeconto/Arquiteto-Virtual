using UnityEngine;
using System.Collections;

public class WallBuilderMouseHandler : MonoBehaviour {
	
	public WallBuilder wallBuilder;
	
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.gameObject.layer != LayerMask.NameToLayer("GUI")) {
					wallBuilder.CreateTile();
				}
			} else {
				wallBuilder.CreateTile();
			}
		}
	}
}
