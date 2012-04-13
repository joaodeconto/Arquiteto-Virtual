using UnityEngine;
using System.Collections;

public class CheckBoxTopHandler : MonoBehaviour {
	
	public GameObject item;
	
	private InfoController infoController;
	private UICheckbox thisCheckbox;
	private GameObject cCamera;
	
	void Start () {
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		thisCheckbox = GetComponent<UICheckbox>();
		cCamera = GameObject.FindWithTag("MainCamera");
	}
	
	void OnClick ()
	{
		GameObject furniture = GameObject.FindWithTag("MovelSelecionado");
		GameObject newFurniture = furniture.GetComponent<InformacoesMovel>().CloneGameObject(item, furniture.GetComponent<InformacoesMovel>(), "MovelSelecionado");
		newFurniture.GetComponent<SnapBehaviour>().Select = true;
		newFurniture.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		cCamera.GetComponent<RenderBounds>().Display = true;
		cCamera.GetComponent<RenderBounds>().SetBox(newFurniture);
		cCamera.GetComponent<RenderBounds>().UpdateObj();
		
		infoController.SendMessage("UpdateInfo", newFurniture.GetComponent<InformacoesMovel>());
	}
	
}
