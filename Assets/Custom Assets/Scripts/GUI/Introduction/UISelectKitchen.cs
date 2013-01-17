using UnityEngine;
using System.Collections;

[AddComponentMenu("BlackBugio/GUI/Kitchen/Select Kitchen")]

public class UISelectKitchen : MonoBehaviour
{
	public UISelectKitchenController selectKitchenController;
	public int index = 0;
	
	void Awake ()
	{
		Debug.Log (PlayerPrefs.GetInt ("SelectedKitchen", 0));
		if (index == PlayerPrefs.GetInt ("SelectedKitchen", 0))
		{
			GetComponent<UICheckbox3D> ().tweenTarget = gameObject;
			GetComponent<UICheckbox3D> ().Checked ();
		}
	}
	
	void OnChecked ()
	{
		if (selectKitchenController == null) selectKitchenController = GameObject.FindObjectOfType (typeof(UISelectKitchenController)) as UISelectKitchenController;
		selectKitchenController.SendMessage ("ChangeKitchen", index, SendMessageOptions.RequireReceiver);
	}
}
