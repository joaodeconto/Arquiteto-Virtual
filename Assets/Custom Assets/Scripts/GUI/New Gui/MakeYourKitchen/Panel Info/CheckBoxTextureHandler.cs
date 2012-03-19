using UnityEngine;
using System.Collections;

public class CheckBoxTextureHandler : MonoBehaviour {
	
	public GameObject item;
	
	public Texture2D texture;
	
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
		infoController.item.GetComponent<InformacoesMovel>().ChangeTexture(texture, "Tampos");
		infoController.topMaterial.mainTexture = texture;
		GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
		if(furniture != null && furniture.Length != 0) {
			foreach(GameObject mobile in furniture) {
				mobile.GetComponent<InformacoesMovel>().ChangeTexture(texture, "Tampos");
			}
		}
	}
}
