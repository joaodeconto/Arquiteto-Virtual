using UnityEngine;
using System.Collections;

public class SelectKitchenHandler : MonoBehaviour {
	
	public UILimitDraggable limitDraggable;
	private int lastIndex;
	
	IEnumerator Start () {
		yield return new WaitForSeconds(0.1f);
		limitDraggable.SetIndex(PlayerPrefs.GetInt ("SelectedKitchen", 0));
		lastIndex = limitDraggable.index;
	}
	
	void Update () {
		if (limitDraggable.index != lastIndex) {
			PlayerPrefs.SetInt ("SelectedKitchen", limitDraggable.index);
			lastIndex = limitDraggable.index;
		}
	}
}
