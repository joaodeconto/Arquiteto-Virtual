using UnityEngine;
using System.Collections;

public class CatalogCategoryButtonHandler : MonoBehaviour {
	
	//public UIAtlas atlas;
	// Use this for initialization
	void Start () {
		Invoke("CatalogCategory", 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CatalogCategory () {
		//foreach (Category category in Line.CurrentLine.categories) {
		//	UISprite newSprite = new UISprite();
		//	newSprite.texture = category.Image;
		//	newSprite.spriteName = category.Name;
//			atlas.sprites.Add(newSprite);
	//	}
	}
}
