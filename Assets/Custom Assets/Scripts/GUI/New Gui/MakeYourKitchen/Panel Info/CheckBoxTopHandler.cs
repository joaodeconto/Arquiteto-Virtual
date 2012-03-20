using UnityEngine;
using System.Collections;

public class CheckBoxTopHandler : MonoBehaviour {
	
	public GameObject item;
	
	private InfoController infoController;
	private UICheckbox thisCheckbox;
	private Camera3d camera;
	
	void Start () {
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		thisCheckbox = GetComponent<UICheckbox>();
		camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera3d>();
	}
	
	void OnClick ()
	{
		GameObject furniture = GameObject.FindWithTag("MovelSelecionado");
		GameObject newFurniture = furniture.GetComponent<InformacoesMovel>().CloneGameObject(item, furniture.GetComponent<InformacoesMovel>(), "MovelSelecionado");
		newFurniture.GetComponent<SnapBehaviour>().Select = true;
		newFurniture.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		camera.GetComponent<RenderBounds>().Display = true;
		camera.GetComponent<RenderBounds>().SetBox(newFurniture);
		camera.GetComponent<RenderBounds>().UpdateObj();
		
		infoController.SendMessage("UpdateInfo", newFurniture.GetComponent<InformacoesMovel>());
	}
	
}
