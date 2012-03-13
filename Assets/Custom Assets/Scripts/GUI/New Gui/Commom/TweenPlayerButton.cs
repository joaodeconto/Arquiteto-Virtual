using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenPlayerButton : MonoBehaviour
{	
	public string Name;
	public bool IsToggle;
	public bool PlayNextOnLastTweenFinish;
	public bool RunOnStart;
	
	public TweenPlayerController tweenPlayerController;
	public NTweener[] parallelTweens;
	
	public bool IsActive { get; set; }
	
	private int internalControllerRegister;
	private bool isForwardDirection;
	
	#region unity methods
	void Start ()
	{		
		if (tweenPlayerController != null)
		{
			internalControllerRegister = tweenPlayerController.AddButton (this);
		}
				
		NTweener currentTween;
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
			
			if (currentTween.duration > maxTime)
			{
				maxTime = currentTween.duration;
				indexMaxValue = i;
			} else if (currentTween.duration < minTime)
			{
				minTime = currentTween.duration;
				indexMinValue = i;
			}
		
			currentTween.callWhenFinished = "DoNothing";
			currentTween.enabled = false;
		}
			
		if (PlayNextOnLastTweenFinish)
		{
			//parallelTweens[indexMaxValue].callWhenFinished = "PlayNextTween";
		}
		else
		{
			//parallelTweens[indexMinValue].callWhenFinished = "PlayNextTween";
		}
		#endregion		
		
		if (RunOnStart)
		{
			//OnClick ();
		}
	}
	#endregion
		
	#region NGUI button behaviour
	public void OnClick ()
	{
		Play ();
	}
	#endregion
	
	public void Play ()
	{
		if (IsActive && IsToggle) {
			
			IsActive = false;
			isForwardDirection = false;
			
			PlayTween ();
		} else {
			IsActive = true;
			
			if (tweenPlayerController != null)
			{
				tweenPlayerController.NotifyActiveButton (internalControllerRegister);
			}
			
			isForwardDirection = true;
			
			PlayTween ();
		}
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
						tw.Play (isForwardDirection);
					}
				}
			}
			
			parallelTweens [i].enabled = true;
			parallelTweens [i].Play (isForwardDirection);
		}
	}
}