using UnityEngine;
using System.Collections;

public class SelectKitchenHandler : MonoBehaviour {

	public enum KitchenEnum {
		America,
		Cristal,
		Diamante,
		Mirage,
		Maxima,
		MaximaBlack,
		Novita,
		Star,
	}
	
	public KitchenEnum selectedKitchen;
	
	private AsyncOperation ao;
	
	void OnGUI () {
		if (ao != null) {
			GUI.Box(new Rect(50, (Screen.height - 25) - 50, 200, 25), "Carregando: " + (int)(ao.progress * 100f) + "%");
		}
	}
	
	private void OnClick (){
	
		switch (selectedKitchen) {
			case KitchenEnum.Mirage:
				PlayerPrefs.SetInt ("SelectedKitchen", 0);
				break;
			case KitchenEnum.Star:
				PlayerPrefs.SetInt ("SelectedKitchen", 1);
				break;
			case KitchenEnum.Diamante:
				PlayerPrefs.SetInt ("SelectedKitchen", 2);
				break;
			case KitchenEnum.America:
				PlayerPrefs.SetInt ("SelectedKitchen", 3);
				break;
			case KitchenEnum.Novita:
				PlayerPrefs.SetInt ("SelectedKitchen", 4);
				break;
			case KitchenEnum.Cristal:
				PlayerPrefs.SetInt ("SelectedKitchen", 5);
				break;
			case KitchenEnum.Maxima:
				PlayerPrefs.SetInt ("SelectedKitchen", 6);
				break;
			case KitchenEnum.MaximaBlack:
				PlayerPrefs.SetInt ("SelectedKitchen", 7);
				break;
		}
		
		ao = Application.LoadLevelAsync(2);
		
	}
}
