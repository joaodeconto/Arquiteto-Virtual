using UnityEngine;
using System.Collections;

public class CameraGUIController : MonoBehaviour {
	
	public Camera[] camerasGUI;
	
	public bool ClickInGUI () {
		int layerGUI = LayerMask.NameToLayer ("GUI");
		foreach (Camera camera in camerasGUI)
		{
			RaycastHit hit = new RaycastHit ();
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, Mathf.Infinity))
			{
				if (hit.transform.gameObject.layer == layerGUI)
				{
					return true;
				}
			}
		}
		return false;
	}

}
