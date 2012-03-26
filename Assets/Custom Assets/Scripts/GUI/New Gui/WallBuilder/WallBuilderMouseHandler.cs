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
					wallBuilder.CreateTile(ray,hit);
				}
			} else {
				wallBuilder.CreateTile (ray, hit);
			}
		}
		if (Input.GetMouseButtonDown(1)){
			
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.gameObject.layer != LayerMask.NameToLayer("GUI")) {
					wallBuilder.DestroyTile();
				}
			} else {
				wallBuilder.DestroyTile();
			}
		}
	}
}
