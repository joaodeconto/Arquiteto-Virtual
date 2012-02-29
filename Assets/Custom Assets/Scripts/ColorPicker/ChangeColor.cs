using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {
	
	public ColorPicker colorPicker;
	public UICheckbox checkbox;
	public Camera cameraRender;
	
	private bool dropperBool;
	
	// Update is called once per frame
	void Update () {
		if (checkbox.isChecked) {
			if (Input.GetMouseButtonUp(0)) {
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					if (hit.transform.gameObject.layer != LayerMask.NameToLayer("GUI"))
						return;
				}
				Ray rayRender = cameraRender.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitRender;
				if (Physics.Raycast(rayRender, out hitRender)) {
					if (hitRender.transform.renderer != null) {
						colorPicker.color = hitRender.transform.renderer.material.color;
					}
					checkbox.isChecked = false;
					return;
				}
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
				Ray ray = cameraRender.ScreenPointToRay(Input.mousePosition);
			    if (!Physics.Raycast (ray, out hit))
			        return;
				
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("GUI"))
					return;
				
				print(hit.transform.name);
				
				colorPicker.CallColorPicker(hit.transform.gameObject);
			}
		}
	}
	
	public void OnActivateDropper () {
		dropperBool	= !dropperBool;
	}
}
