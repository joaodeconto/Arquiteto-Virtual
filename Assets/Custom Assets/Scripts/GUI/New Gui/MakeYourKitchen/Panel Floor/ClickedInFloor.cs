using UnityEngine;
using System.Collections;

public class ClickedInFloor : MonoBehaviour {
	
	public GameObject GUIPanelFloor;
	public GameObject ListPanelFloor;
	
	private CameraGUIController cameraGUIController;
	private Camera mainCamera;
	
	// Use this for initialization
	void Start () {
		cameraGUIController = GameObject.FindWithTag("GameController").GetComponentInChildren<CameraGUIController>();
		mainCamera = transform.parent.camera;
		if (GUIPanelFloor.active) {
			GUIPanelFloor.SetActiveRecursively(false);
			ListPanelFloor.SetActiveRecursively(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			if (!cameraGUIController.ClickInGUI()){
				if (GUIPanelFloor.active) {
					GUIPanelFloor.SetActiveRecursively(false);
					ListPanelFloor.SetActiveRecursively(false);
				}
			}
		}
		if (MouseUtils.ItemMouseButtonDoubleClickDown(0, 0.3f)) {
			if (!cameraGUIController.ClickInGUI()){
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit = new RaycastHit();
				
				if (Physics.Raycast(ray, out hit)) {
					if (hit.transform.tag == "ChaoParent") {
						GUIPanelFloor.SetActiveRecursively(true);
						ListPanelFloor.SetActiveRecursively(true);
					}
				}
			}
		}
	}
}
