using UnityEngine;
using System.Collections;

public class CatalogCategoryButtonHandler : MonoBehaviour {
	
	public GameObject item;
	public Camera cameraTarget;
	public Transform rootForBounds;
	public NTweener tween;
	
	// Use this for initialization
	void Start () {
		Invoke("CatalogCategory", 0.1f);
	}
	
	void CatalogCategory () {
		int i = 0;
		foreach (Category category in Line.CurrentLine.categories) {
			GameObject newItem = Instantiate(item) as GameObject;
			newItem.name = item.name;
			newItem.name += " " + i;
			newItem.GetComponent<TooltipHandler>().SetTooltip(Line.CurrentLine.categories[i].Name);
			newItem.GetComponent<UIDragCamera>().target = cameraTarget;
			newItem.GetComponent<UIDragCamera>().rootForBounds = rootForBounds;
			newItem.transform.parent = transform;
			newItem.transform.localPosition = new Vector3(0, (i * -185), 0);
			newItem.transform.localScale = item.transform.localScale;
			newItem.AddComponent<TweenPlayerButton>();
			newItem.GetComponent<TweenPlayerButton>().parallelTweens = new NTweener[1];
			newItem.GetComponent<TweenPlayerButton>().parallelTweens[0] = tween;
			foreach (UISprite sprite in newItem.GetComponentsInChildren<UISprite>()) {
				if (sprite.name.Equals("UISprite")) {
					sprite.spriteName = Line.CurrentLine.categories[i].ImageReference;
				}
			}
			++i;
		}
	}
}
