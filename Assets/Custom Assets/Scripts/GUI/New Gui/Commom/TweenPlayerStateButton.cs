using UnityEngine;
using System.Collections;

public class TweenPlayerStateButton : MonoBehaviour {
	
	public bool RunOnStart;
	public int TweenStateIndex;
	
	public TweenPlayerStateController tweenPlayerStateController;
	
	#region unity methods
	protected void Start ()
	{
		if (TweenStateIndex < 0)
		{
			Debug.LogError ("TweenStateIndex não pode ser negativo por medidas de organização");
			Debug.Break ();
		}
		
		if (RunOnStart)
		{
			tweenPlayerStateController.PlayState(TweenStateIndex);
		}
	}
	#endregion
	
	#region NGUI button behaviour
	public void OnClick ()
	{
		tweenPlayerStateController.PlayState (TweenStateIndex);
	}
	#endregion
}
