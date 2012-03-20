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
		GameObject newFurniture = furniture.GetComponent<InformacoesMovel>().CloneGameObject(item, furniture.GetComponent<InformacoesMovel>(), "Movel");
		newFurniture.GetComponent<SnapBehaviour>().Select = false;
		newFurniture.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
		camera.GetComponent<RenderBounds>().Display = false;
//		camera.GetComponent<RenderBounds>().SetBox(newFurniture);
//		camera.GetComponent<RenderBounds>().UpdateObj();
		
		infoController.Close();
	}
	
}
