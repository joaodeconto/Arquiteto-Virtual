using UnityEngine;
using System.Collections; 
using Visiorama;

public class CatalogJanelasButtonHandler : MonoBehaviour {
	
	public GameObject item;
	public Camera cameraTarget;
	public Camera camera3d;
	public Transform rootForBounds;
	public TweenPlayerButton tweenPlayerButton;
	public GameObject janelas;
	
	public bool isClicked { get; set; }
	
	private GameObject[] cameras;
	
	void Start ()
	{
		Invoke ("CatalogJanelas", 0.1f);
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown(0) && isClicked)
		{
			if (!NGUIUtils.ClickedInGUI (cameras,"GUI"))
			{
				tweenPlayerButton.Play();
				isClicked = false;
			}
		}
	}
	
	void CatalogJanelas ()
	{
		int i = 0;
		foreach (Transform janela in janelas.transform)
		{
			GameObject newItem = Instantiate(item) as GameObject;
			string nameObject = janela.name;
			newItem.name = nameObject;
			newItem.GetComponent<TooltipHandler>().SetTooltip(nameObject);
			newItem.GetComponent<UIDragCamera>().target = cameraTarget;
			newItem.GetComponent<UIDragCamera>().rootForBounds = rootForBounds;
			newItem.transform.parent = transform;
			newItem.transform.localPosition = new Vector3(0, (i * -160), 0);
			newItem.transform.localScale = item.transform.localScale;
			newItem.AddComponent<ClickWindowItem>();
			newItem.GetComponent<ClickWindowItem>().item   = janela.gameObject;
			newItem.GetComponent<ClickWindowItem>().camera = camera3d;
			
			foreach (UISprite sprite in newItem.GetComponentsInChildren<UISprite>())
			{
				if (sprite.name.Equals("UISprite"))
				{
					sprite.spriteName = janela.name;
					sprite.MakePixelPerfect();
					sprite.transform.localPosition = new Vector3(0, 0, -5f);
				}
			}
			++i;
		}
	}
}
