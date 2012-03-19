using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CatalogItemButtonHandler : MonoBehaviour {

	public Camera cameraTarget;
	public Transform rootForBounds;
	public Camera camera3d;
	
	public void CallItems (int category, GameObject item) {
		if (transform.GetChildCount() != 0) {
			foreach (Transform child in transform) {
	        	Destroy(child.gameObject);
			}
		}
		
		List<GameObject> items = new List<GameObject>(Line.CurrentLine.categories[category].Furniture);
		int j = 0;
		for (int i = 0; i != items.Count; ++i){
			if (Regex.Match(items[i].name,".*(sem tampo|s tampo|cook top|cooktop|com pia|esquerda).*",RegexOptions.IgnoreCase).Success) {
				continue;
			}
			GameObject newItem = Instantiate(item) as GameObject;
			InformacoesMovel iM = items[i].GetComponent<InformacoesMovel>();
			iM.Initialize();
			newItem.name = iM.Nome;
			newItem.GetComponent<TooltipHandler>().SetTooltip(iM.Nome);
			newItem.GetComponent<UIDragCamera>().target = cameraTarget;
			newItem.GetComponent<UIDragCamera>().rootForBounds = rootForBounds;
			newItem.transform.parent = transform;
			newItem.transform.localPosition = new Vector3(0, (j * -185), 0);
			newItem.transform.localScale = item.transform.localScale;
			newItem.AddComponent<ClickItem>();
			newItem.GetComponent<ClickItem>().item = items[i];
			newItem.GetComponent<ClickItem>().camera = camera3d;
			List<string> listSprites;
			List<string> separateNameSprites = new List<string>();
			foreach (UISprite sprite in newItem.GetComponentsInChildren<UISprite>()) {
				if (sprite.name.Equals("UISprite")) {
					listSprites = new List<string>(sprite.atlas.GetListOfSprites());
					string spriteName = iM.Codigo;
					foreach(string s in listSprites) {
						if (Regex.Match(s, " e ").Success) {
							string[] codes = Regex.Split(s, " e ");
							foreach (string code in codes) {
								if (iM.Codigo == code) {
									spriteName = s;
								}
							}
						}
					}
					sprite.spriteName = spriteName;
					sprite.MakePixelPerfect();
					sprite.transform.localPosition = new Vector3(0, 0, -0.1f);
				}
			}
			++j;
		}
	}
}
