using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("BlackBugio/Tween/TweenPlayerController")]
public class TweenPlayerController : MonoBehaviour
{
	public string Name;
	public List<TweenPlayerButton> buttonList;
	private List<int> internalControlRegister;
	
	void Start ()
	{
		internalControlRegister = new List<int>();
		foreach(TweenPlayerButton btn in buttonList)
		{
			internalControlRegister.Add((int)Random.Range (0f, buttonList.Count * 100f));	
		}
	}
	
	void Update ()
	{
		float activeSince = float.MinValue;
		int activeBtnRegisterValue = -1;
		for (int i = 0; i != buttonList.Count; ++i)
		{
			if (buttonList [i] == null)
			{
				Debug.LogError ("whata?!");
				Debug.LogError (name + " i : " + i);
			}
			if (buttonList[i].IsActive && activeSince < buttonList[i].ActiveSince)
			{
				activeSince 		   = buttonList[i].ActiveSince;
				activeBtnRegisterValue = internalControlRegister[i];
			}
		}
		
		if (activeBtnRegisterValue == -1)
		{
			return;
		}
		
		for (int i = 0; i != buttonList.Count; ++i)
		{
			if (activeBtnRegisterValue != internalControlRegister[i])
			{
				buttonList[i].IsActive = false;
			}
		}
	}
	
}