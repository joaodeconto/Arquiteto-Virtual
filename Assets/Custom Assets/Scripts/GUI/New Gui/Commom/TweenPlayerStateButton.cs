using UnityEngine;
using System.Collections;

public class TweenPlayerStateButton : MonoBehaviour {
	
	public bool RunOnStart;
	public bool IsToggleButton;
	public int TweenStateIndex;
		
	public TweenPlayerStateController tweenPlayerStateController;
			
	#region unity methods
	protected void Start ()
	{
		if (TweenStateIndex < 1)
		{
			Debug.LogError ("TweenStateIndex não pode ser negativo por medidas de organização");
			Debug.Break ();
		}
		else if (TweenStateIndex == 0)
		{
			Debug.LogError ("TweenStateIndex não pode ser zero, pois esse é o estado default.");
			Debug.Break ();
		}
		
		//TODO arrumar isso
		//tweenPlayerStateController.AddButton(this);
		
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
