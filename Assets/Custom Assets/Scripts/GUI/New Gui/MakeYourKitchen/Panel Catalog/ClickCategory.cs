using UnityEngine;
using System.Collections;

public class ClickCategory : MonoBehaviour {

	void OnClick () {
		string separator = transform.name;
		int separatorNum = transform.name.IndexOf(" ");
		separator = separator.Remove(0, separatorNum+1);
		int sp = System.Convert.ToInt32(separator);
		GameObject item = transform.parent.GetComponent<CatalogCategoryButtonHandler>().item;
		Transform catalogItem = transform.parent.GetComponent<CatalogCategoryButtonHandler>().offsetCatalogItem;
		catalogItem.GetComponent<CatalogItemButtonHandler>().CallItems(sp, item);
		if (!catalogCategoryButtonHandler.isClicked) {
			catalogCategoryButtonHandler.tweenPlayerButton.Play();
			catalogCategoryButtonHandler.isClicked = true;
		}
	}
	
	public CatalogCategoryButtonHandler catalogCategoryButtonHandler {get; set;}
}
