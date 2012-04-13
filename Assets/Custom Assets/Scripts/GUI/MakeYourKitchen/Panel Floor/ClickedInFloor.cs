using UnityEngine;
using System.Collections;
using Visiorama;

public class ClickedInFloor : MonoBehaviour {
	
	public GameObject GUIPanelFloor;
	public GameObject ListPanelFloor;
	
	private Camera mainCamera;
	
	private GameObject[] cameras;
		
	// Use this for initialization
	void Start () {
		mainCamera = transform.parent.camera;
		cameras = GameObject.FindGameObjectsWithTag("GUICamera");
		if (GUIPanelFloor.active)
		{
			GUIPanelFloor.SetActiveRecursively(false);
			ListPanelFloor.SetActiveRecursively(false);
		}
	}
	
	// Update is called once per frame
	void OnGUI () {
		if (Input.GetMouseButtonDown(0)) 
		{
			if (!NGUIUtils.ClickedInGUI(cameras, "GUI"))
			{
				if (GUIPanelFloor.active) 
				{
					GUIPanelFloor.SetActiveRecursively(false);
					ListPanelFloor.SetActiveRecursively(false);
				}
			}
		}
		if (MouseUtils.GUIMouseButtonDoubleClick(0))
		{
			if (!NGUIUtils.ClickedInGUI(cameras, "GUI"))
			{
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit = new RaycastHit();
				
				if (Physics.Raycast(ray, out hit))
				{
					if (hit.transform.tag == "Chao")
					{
						GUIPanelFloor.SetActiveRecursively(true);
						ListPanelFloor.SetActiveRecursively(true);
					}
				}
			}
		}
	}
}
