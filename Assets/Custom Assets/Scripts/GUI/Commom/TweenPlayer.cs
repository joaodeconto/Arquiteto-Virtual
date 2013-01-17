using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenPlayer : MonoBehaviour
{	
	public string Name;
	
	public bool RunOnStart;
	public bool AutoCompleteFromValues;
	//public bool PlayNextOnLastTweenFinish;
	
	public UITweener[] parallelTweens;
			
	#region unity methods
	void Start ()
	{						
		UITweener currentTween;
		int parallelTweensLength = parallelTweens.Length;
			
		int indexMaxValue = 0;
		int indexMinValue = 0;
		
		float maxTime = float.MinValue;
		float minTime = float.MaxValue;
		
		#region validate tweens
		for (int i = 0; i != parallelTweensLength; ++i)
		{
			currentTween = parallelTweens [i]; 
			if (currentTween == null)
			{
				continue;	
			}
		
			if (AutoCompleteFromValues)
			{
				//Fixing from values
				if (parallelTweens [i] is TweenPosition)
				{
					(parallelTweens [i] as TweenPosition).from = parallelTweens [i].transform.localPosition;
				} else if (parallelTweens [i] is TweenScale)
				{
					(parallelTweens [i] as TweenScale).from = parallelTweens [i].transform.localScale;
				} else if (parallelTweens [i] is TweenRotation)
				{
					(parallelTweens [i] as TweenRotation).from = parallelTweens [i].transform.localEulerAngles;
				}
			}
			
			if (currentTween.duration > maxTime) {
				maxTime = currentTween.duration;
				indexMaxValue = i;
			} else if (currentTween.duration < minTime) {
				minTime = currentTween.duration;
				indexMinValue = i;
			}
		
			currentTween.callWhenFinished = "DoNothing";
			currentTween.enabled = false;
		}
			
		/*if (PlayNextOnLastTweenFinish) {
			parallelTweens[indexMaxValue].callWhenFinished = "Play";
			//parallelTweens[indexMaxValue].eventReceiver = NextTweenPlayer.gameObject;
		} else {
			parallelTweens[indexMinValue].callWhenFinished = "Play";
			//parallelTweens [indexMaxValue].eventReceiver = NextTweenPlayer.gameObject;
		}*/
		#endregion		
		
		if (RunOnStart) {
			Play ();
		}
	}
	#endregion
	
	public void Play ()
	{
		PlayTween ();
	}
	
	private void PlayTween ()
	{
		NTweener[] tweensBrothers;
		for (int i = 0; i != parallelTweens.Length; ++i) {
			if (parallelTweens [i] == null) {
				continue;
			}
			
			tweensBrothers = parallelTweens [i].gameObject.GetComponents<NTweener> ();
			if (tweensBrothers != null) {
				foreach (NTweener tw in parallelTweens[i].gameObject.GetComponents<NTweener>()) {
					if (tw.tweenGroup == parallelTweens [i].tweenGroup) {
						tw.enabled = true;
						tw.Play (true);
					}
				}
			}
			
			parallelTweens [i].enabled = true;
			parallelTweens [i].Play (true);
		}
	}
}