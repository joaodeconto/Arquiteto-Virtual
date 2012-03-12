using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenPlayerAccordion3Button : MonoBehaviour {

	public string Name;
	public bool RunOnStart;
	
	public TweenStateStream[] tweenStateStreams;
	
	public int indexButton;
	public TweenPlayerAccordion3Controller tweenPlayerAccordion3Controller;
	
	#region unity methods
	protected void Start ()
	{
		#region validate tweens
		foreach (TweenStateStream tss in tweenStateStreams)
		{
			for (int i = 0; i != tss.tweenStreams.Length; ++i)
			{
				foreach (NTweener tw in tss.tweenStreams[i].parallelTweens)
				{
					if (tw == null)
					{
						Debug.LogError ("A TweenStream " + tss.tweenStreams[i].name + 
										" no objeto "  + this.gameObject.name + " está nula.");
						Debug.Break ();
					}
					else
					{
						tw.callWhenFinished = "PlayNextTween";
						tw.enabled = false;
					}
				}
			}
		}
		#endregion
	}
	#endregion

	public void OnClick ()
	{
		tweenPlayerAccordion3Controller.ActiveButton(indexButton);
	}	
}
