using UnityEngine;
using System.Collections;

public class CatalogExtrasButtonHandler : MonoBehaviour {
	
	public GameObject item;
	public Camera cameraTarget;
	public Transform rootForBounds;
	public GameObject extras;
	
	private Camera camera3d;
	
	// Use this for initialization
	void Start () {
		camera3d = GameObject.FindWithTag("MainCamera").camera;
		Invoke("CatalogExtras", 0.1f);
	}
	
	void CatalogExtras () {
		int i = 0;
		foreach (Transform extra in extras.transform) {
			GameObject newItem = Instantiate(item) as GameObject;
			string nameObject = extra.name;
			newItem.name = nameObject;
			newItem.GetComponent<TooltipHandler>().SetTooltip(nameObject);
			newItem.GetComponent<UIDragCamera>().target = cameraTarget;
			newItem.GetComponent<UIDragCamera>().rootForBounds = rootForBounds;
			newItem.transform.parent = transform;
			newItem.transform.localPosition = new Vector3(0, (i * -160), 0);
			newItem.transform.localScale = item.transform.localScale;
			newItem.AddComponent<ClickItem>();
			newItem.GetComponent<ClickItem>().item = extra.gameObject;
			newItem.GetComponent<ClickItem>().camera = camera3d;
//			foreach (UISprite sprite in newItem.GetComponentsInChildren<UISprite>()) {
//				if (sprite.name.Equals("UISprite")) {
//					sprite.spriteName = extra.imageReference;
//					sprite.MakePixelPerfect();
//					sprite.transform.localPosition = new Vector3(0, 0, -0.1f);
//				}
//			}
			++i;
		}
	}
}
