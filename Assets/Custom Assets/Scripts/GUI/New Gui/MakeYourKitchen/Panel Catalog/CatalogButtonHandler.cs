using UnityEngine;
using System.Collections;

public class CatalogButtonHandler : MonoBehaviour
{
		
	public enum CatalogButtonHandlerEnum
	{
		CatalogExtras,
		CatalogFurniture,
		CatalogIllumination,
	}
	
	public CatalogButtonHandlerEnum catalogButtonHandlerEnum;
	
	private CatalogController catalogController;
	
	void Start ()
	{
		catalogController = GameObject.Find("CatalogController").GetComponent<CatalogController>();
	}
	
	void OnClick ()
	{
		switch (catalogButtonHandlerEnum)
		{
			case CatalogButtonHandlerEnum.CatalogExtras:
				//TODO something
				break;
			case CatalogButtonHandlerEnum.CatalogFurniture:
				catalogController.ShowCategories();
				break;
			case CatalogButtonHandlerEnum.CatalogIllumination:
				//TODO something
				break;
		}
	}
}
