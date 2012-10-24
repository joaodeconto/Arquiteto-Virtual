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
	private GameObject cCamera;
	
	void Awake ()
	{
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		cCamera = GameObject.FindWithTag("MainCamera");
	}
	
	void OnClick ()
	{
		bool success = false;
		GameObject selectedModule = GameObject.FindWithTag("MovelSelecionado");
		
		if (selectedModule == null)
		{
			Debug.LogError ("Não foi possível encontrar o móvel selecionado!");
		}
		
		bool isDoorOpen = selectedModule.GetComponent<InformacoesMovel>().portas == Portas.ABERTAS ? true : false;
		
		GameObject newModule = selectedModule.GetComponent<InformacoesMovel>().ToggleDoorSide();
		
		#region Abrir porta quando trocado por novo item
		if (isDoorOpen)
		{
			Animation[] animations = newModule.GetComponentsInChildren<Animation>();
			foreach (Animation anim in animations) {
				if (anim.clip != null) {
					anim[anim.clip.name].speed = 1;
					anim[anim.clip.name].time = 0;
					anim.Play();
				}
			}
			newModule.GetComponent<InformacoesMovel>().portas = Portas.ABERTAS;
		}
		#endregion
		
		newModule.GetComponent<SnapBehaviour>().Select = true;
		newModule.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		cCamera.GetComponent<RenderBounds>().Display = true;
		cCamera.GetComponent<RenderBounds>().SetBox(newModule);
		cCamera.GetComponent<RenderBounds>().UpdateObj();
		
		infoController.SendMessage("UpdateInfo", newModule.GetComponent<InformacoesMovel>());
		
	}
}
