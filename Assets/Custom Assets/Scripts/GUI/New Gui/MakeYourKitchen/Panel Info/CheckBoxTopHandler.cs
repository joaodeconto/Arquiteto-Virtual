using UnityEngine;
using System.Collections;

public class CheckBoxTopHandler : MonoBehaviour {
	
	public GameObject item;
	
	private InfoController infoController;
	private UICheckbox checkbox;
	private Camera3d camera;
	
	void Start () {
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		checkbox = GetComponent<UICheckbox>();
		camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera3d>();
	}
	
	void OnClick ()
	{
		infoController.Close();
		GameObject furniture = GameObject.FindWithTag("MovelSelecionado");
		furniture.GetComponent<InformacoesMovel>().Clone(item, furniture.GetComponent<InformacoesMovel>(), "MovelSelecionado");
		furniture.GetComponentInChildren<SnapBehaviour>().Select = true;
		furniture.GetComponentInChildren<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		camera.GetComponent<RenderBounds>().SetBox(furniture);
		camera.GetComponent<RenderBounds>().UpdateObj();
		infoController.Open(furniture.GetComponent<InformacoesMovel>());
	}
}
