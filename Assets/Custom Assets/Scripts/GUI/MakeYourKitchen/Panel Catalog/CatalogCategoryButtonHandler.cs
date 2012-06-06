using UnityEngine;
using System.Collections;
using Visiorama;

public class CatalogCategoryButtonHandler : MonoBehaviour {
	
	public GameObject item;
	public Camera cameraTarget;
	public Transform rootForBounds;
	public TweenPlayerButton tweenPlayerButton;
	public Transform offsetCatalogItem;
	
	public bool isClicked {get; set;}
		
	private GameObject[] cameras;
	
	void Start () {
		Invoke("CatalogCategory", 0.1f);
		cameras = GameObject.FindGameObjectsWithTag("GUICamera");
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && isClicked)
		{
			if (!NGUIUtils.ClickedInGUI (cameras,"GUI"))
			{
				tweenPlayerButton.Play();
				isClicked = false;
			}
		}
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
			newItem.transform.localPosition = new Vector3(0, (i * (-160 * item.transform.localScale.y)), 0);
			newItem.transform.localScale = item.transform.localScale;
//			newItem.AddComponent<TweenPlayerButton>();
//			TweenPlayerButton tpb = newItem.GetComponent<TweenPlayerButton>();
//			tpb.ApplyTweenPlayerButton(tweenPlayerButton);
			newItem.AddComponent<ClickCategory>();
			newItem.GetComponent<ClickCategory>().catalogCategoryButtonHandler = this;
			foreach (UISprite sprite in newItem.GetComponentsInChildren<UISprite>()) {
				if (sprite.name.Equals("UISprite")) {
					sprite.spriteName = Line.CurrentLine.categories[i].ImageReference;
					sprite.MakePixelPerfect();
					sprite.transform.localPosition = new Vector3(0, 0, -0.1f);
				}
			}
			++i;
		}
	}
}
