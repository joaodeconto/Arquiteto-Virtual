using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {
	
	public ColorPicker colorPicker;
	public UICheckbox checkbox;
	
	public bool dropperBool;
	
	// Update is called once per frame
	void Update () {
		if (dropperBool) {
			if (Input.GetMouseButtonUp(0)) {
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					if (hit.transform.gameObject.layer != LayerMask.NameToLayer("GUI")) {
						if (hit.transform.renderer != null) {
						colorPicker.color = hit.transform.renderer.material.color;
						}
					}
				}
				dropperBool = false;
				checkbox.isChecked = true;
				return;
			}
		} else {
			if (Input.GetMouseButtonDown(0)) {
			    RaycastHit hit;
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				
			    if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer("GUI"))
						return;
				}
				colorPicker.CloseColorPicker();
			}
			if (MouseUtils.MouseButtonDoubleClickUp(0, 0.3f)) {
			    RaycastHit hit;
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			    if (!Physics.Raycast (ray, out hit))
			        return;
				
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("GUI"))
					return;
				
				colorPicker.CallColorPicker(hit.transform.gameObject);
			}
		}
	}
	
	public void OnActivateDropper () {
		dropperBool	= !dropperBool;
	}
}
