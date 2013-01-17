using UnityEngine;
using System.Collections;

public class ClickCategory : MonoBehaviour {
		
	public CatalogCategoryButtonHandler catalogCategoryButtonHandler { get; set; }
	void OnClick () {
		string separator = transform.name;
		int separatorNum = transform.name.IndexOf(" ");
		separator = separator.Remove(0, separatorNum+1);
		int sp = System.Convert.ToInt32(separator);
		GameObject item = transform.parent.GetComponent<CatalogCategoryButtonHandler>().item;
		
		catalogCategoryButtonHandler = transform.parent.GetComponent<CatalogCategoryButtonHandler>();
		
		Transform catalogItem = catalogCategoryButtonHandler.offsetCatalogItem;
		Camera itemCameraTarget = catalogItem.GetComponent<CatalogItemButtonHandler>().cameraTarget.camera;
		Vector3 posCamItem = itemCameraTarget.transform.position;
		catalogItem.GetComponent<CatalogItemButtonHandler>().CallItems(sp, item);
		if (!catalogCategoryButtonHandler.isClicked)
		{
			catalogCategoryButtonHandler.tweenPlayerButton.Play();
			catalogCategoryButtonHandler.isClicked = true;
		}
		
		Invoke ("ResetItemCamPosition",0.3f);
	}
	
	void ResetItemCamPosition ()
	{
		GameObject cameraTarget = catalogCategoryButtonHandler.
									offsetCatalogItem.
										GetComponent<CatalogItemButtonHandler> ().
											cameraTarget.gameObject;
									
		//Destrói script de reposicionamento da câmera criado pela NGUI
		//caso ele estiver presente, por que senão ele reposiciona a câmera no lugaar errado		
		if (cameraTarget.GetComponent<SpringPosition> () != null)
		{
			DestroyImmediate (cameraTarget.GetComponent<SpringPosition> ());
		}
			
		cameraTarget.transform.localPosition = new Vector3(4500, -50, 0);// = new Vector3 (11.25f, -0.125f, 0);
	}
	
}
