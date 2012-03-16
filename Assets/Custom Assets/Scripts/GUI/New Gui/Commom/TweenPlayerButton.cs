using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenPlayerButton : MonoBehaviour
{	
	public string Name;
	public bool IsToggle;
	public bool PlayNextOnLastTweenFinish;
	public bool RunOnStart;
	public bool IsActive;
	
	public float ActiveSince { get; set; }
	
	public List<NTweener> parallelTweens;
	public List<NTweener> parallelTweensStandard;
	
	private bool isForwardDirection;
	
	public void ApplyTweenPlayerButton (TweenPlayerButton tweenPlayerButton) {
		this.Name = tweenPlayerButton.Name;
		this.IsToggle = tweenPlayerButton.IsToggle;
		this.PlayNextOnLastTweenFinish = tweenPlayerButton.PlayNextOnLastTweenFinish;
		this.RunOnStart = tweenPlayerButton.RunOnStart;
		this.parallelTweens = new List<NTweener>(tweenPlayerButton.parallelTweens.Count);
		int i = 0;
		foreach (NTweener nt in tweenPlayerButton.parallelTweens) {
			this.parallelTweens[i] = nt;
			++i;
		}
	}
	
	#region unity methods
	void Start ()
	{		
		NTweener currentTween;
		int parallelTweensLength = parallelTweens.Count;
			
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
			ValidFromValues (parallelTweens [i]);
			
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
		if (IsActive && IsToggle)
		{
			IsActive = false;
			isForwardDirection = false;
			
			PlayTween ();
		}
		else if (!IsActive)
		{
			IsActive = true;
			ActiveSince = Time.time;
						
			isForwardDirection = true;
			
			PlayTween ();
		}
	}
	
	private void PlayTween ()
	{
		NTweener[] tweensBrothers;
		for (int i = 0; i != parallelTweens.Count; ++i)
		{
			if (parallelTweens [i] == null)
			{
				continue;
			}
			
			tweensBrothers = parallelTweens [i].gameObject.GetComponents<NTweener> ();
			
			if (isForwardDirection)
			{
				ValidFromValues (parallelTweens [i]);
			}
			
			parallelTweens [i].enabled = true;
			parallelTweens [i].Play (isForwardDirection);
		}
	}
	
	private void ValidFromValues (NTweener tween)
	{
		if (tween is TweenPosition)
		{
			(tween as TweenPosition).from = tween.transform.localPosition;
		}
		else if (tween  is TweenScale)
		{
			(tween as TweenScale).from = tween.transform.localScale;
		}
		else if (tween  is TweenRotation)
		{
			(tween as TweenRotation).from = tween.transform.localEulerAngles;
		}
	}
}