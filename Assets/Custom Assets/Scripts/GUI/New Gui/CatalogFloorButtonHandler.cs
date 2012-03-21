using UnityEngine;
using System.Collections;

public class CatalogFloorButtonHandler : MonoBehaviour {
	
	[System.Serializable]
	public class Floor {
		public string name;
		public Texture2D texture;
		
	}
	
	public GameObject item;
	public Camera cameraTarget;
	public Transform rootForBounds;
	public Floor[] floors;

	void Start () {
		Invoke("CatalogFloor", 0.15f);
	}
	
	void CatalogFloor () {
		int i = 0;
		foreach (Floor floor in floors) {
			GameObject newItem = Instantiate(item) as GameObject;
			newItem.name = floor.name;
			newItem.GetComponent<TooltipHandler>().SetTooltip(floor.name);
			newItem.GetComponent<UIDragCamera>().target = cameraTarget;
			newItem.GetComponent<UIDragCamera>().rootForBounds = rootForBounds;
			newItem.transform.parent = transform;
			newItem.transform.localPosition = new Vector3(0, (i * -160), 0);
			newItem.transform.localScale = item.transform.localScale;
			newItem.AddComponent<ClickFloor>();
			newItem.GetComponent<ClickFloor>().textureFloor = floor.texture;
			foreach (UISprite sprite in newItem.GetComponentsInChildren<UISprite>()) {
				if (sprite.name.Equals("UISprite")) {
					sprite.spriteName = floor.texture.name;
					sprite.MakePixelPerfect();
					sprite.transform.localPosition = new Vector3(0, 0, -0.1f);
				}
			}
			++i;
		}
	}
}
