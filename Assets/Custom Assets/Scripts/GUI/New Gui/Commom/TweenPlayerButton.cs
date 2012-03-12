using UnityEngine;
using System.Collections;

public class TweenPlayerButton : TweenPlayer {
		
	#region unity methods
	protected void Start ()
	{
		base.Start();
	}
	#endregion
		
	#region NGUI button behaviour
	public void OnClick ()
	{
		Play ();
	}
	#endregion
}
