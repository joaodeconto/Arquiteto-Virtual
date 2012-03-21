using UnityEngine;
using System.Collections;

public class CatalogExtrasButtonHandler : MonoBehaviour {
	
	public GameObject item;
	public Camera cameraTarget;
	public Camera camera3d;
	public Transform rootForBounds;
	public TweenPlayerButton tweenPlayerButton;
	public GameObject extras;
	
	public bool isClicked { get; set; }
	
	private CameraGUIController cameraGUIController;
	
	void Start ()
	{
		cameraGUIController = GameObject.FindWithTag ("GameController").GetComponentInChildren<CameraGUIController> ();
		Invoke ("CatalogExtras", 0.1f);
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
			foreach (UISprite sprite in newItem.GetComponentsInChildren<UISprite>()) {
				if (sprite.name.Equals("UISprite")) {
//					sprite.spriteName = extra.GetComponent<InformacoesMovel>().Codigo;
					sprite.spriteName = extra.name;
					sprite.MakePixelPerfect();
					sprite.transform.localPosition = new Vector3(0, 0, -5f);
				}
			}
			++i;
		}
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown (0) && isClicked) {
			if (!cameraGUIController.ClickInGUI ()) {
				tweenPlayerButton.Play ();
				isClicked = false;
			}
		}
	}
}
