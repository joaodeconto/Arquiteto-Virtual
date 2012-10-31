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

		TooltipHandler tipHandler = gameObject.AddComponent<TooltipHandler> ();
		tipHandler.gameObject = this.gameObject;
		tipHandler.SetTooltip (I18n.GetInstance().t (gameObject.name.ToLower ()));
	}
	
	void OnClick ()
	{
		GameObject furniture = GameObject.FindWithTag("MovelSelecionado");
		
		bool isDoorOpen = furniture.GetComponent<InformacoesMovel>().portas == Portas.ABERTAS ? true : false;
		
		GameObject newFurniture = furniture.GetComponent<InformacoesMovel>().ChangeGameObject (item, "MovelSelecionado");
		
		#region Abrir porta quando trocado por novo item
		if (isDoorOpen)
		{
			Animation[] animations = newFurniture.GetComponentsInChildren<Animation>();
			foreach (Animation anim in animations) {
				if (anim.clip != null) {
					anim[anim.clip.name].speed = 1;
					anim[anim.clip.name].time = 0;
					anim.Play();
				}
			}
			newFurniture.GetComponent<InformacoesMovel>().portas = Portas.ABERTAS;
		}
		#endregion
		
		newFurniture.GetComponent<SnapBehaviour>().Select = true;
		newFurniture.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		cCamera.GetComponent<RenderBounds>().Display = true;
		cCamera.GetComponent<RenderBounds>().SetBox(newFurniture);
		cCamera.GetComponent<RenderBounds>().UpdateObj();
		
		infoController.SendMessage("UpdateInfo", newFurniture.GetComponent<InformacoesMovel>());
	}
	
}
