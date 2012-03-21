using UnityEngine;
using System.Collections;

public enum DoorSideEnum
{
	LEFT,
	RIGHT,
}

public class CheckBoxDoorSideHandler : MonoBehaviour {
	
	public DoorSideEnum doorSideEnum;
	
	private InfoController infoController;
	private Camera3d camera3d;
	
	void Awake ()
	{
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		camera3d = GameObject.FindWithTag("MainCamera").GetComponent<Camera3d>();
	}
	
	void OnClick ()
	{
		bool success = false;
		GameObject selectedModule = GameObject.FindWithTag("MovelSelecionado");
		
		if (selectedModule == null)
		{
			Debug.LogError ("Não foi possível encontrar o móvel selecionado!");
		}
					
		GameObject newModule = selectedModule.GetComponent<InformacoesMovel>().ToggleDoorSide();
				
		newModule.GetComponent<SnapBehaviour>().Select = true;
		newModule.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		camera3d.GetComponent<RenderBounds>().Display = true;
		camera3d.GetComponent<RenderBounds>().SetBox(newModule);
		camera3d.GetComponent<RenderBounds>().UpdateObj();
		
		infoController.SendMessage("UpdateInfo", newModule.GetComponent<InformacoesMovel>());
		
	}
}
