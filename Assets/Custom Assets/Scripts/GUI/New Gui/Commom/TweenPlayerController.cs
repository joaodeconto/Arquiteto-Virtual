using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenPlayerController : MonoBehaviour
{
	public string Name;
	private List<TweenPlayerButton> buttonList;
	
	public int AddButton (TweenPlayerButton button)
	{
		if (buttonList == null) {
			buttonList = new List<TweenPlayerButton> ();
		}
		
		buttonList.Add (button);
		return buttonList.Count - 1;
	}
	
	public void NotifyActiveButton (int index)
	{
		int count = buttonList.Count;
		
		for (int i = 0; i != count; ++i) {
			if (i != index) {
				buttonList [i].IsActive = false;
			}
		}
	}
	
}