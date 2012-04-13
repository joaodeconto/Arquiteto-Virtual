using UnityEngine;
using System.Collections;

public class CheckBoxTextureHandler : MonoBehaviour {
	
	public Texture2D texture;
	
	private InfoController infoController;
	private UICheckbox checkbox;
	
	void Start () {
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		checkbox = GetComponent<UICheckbox>();
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
