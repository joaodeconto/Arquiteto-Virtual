using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("BlackBugio/Tween/TweenPlayerButton")]
public class TweenPlayerButton : MonoBehaviour
{
	public string Name;
	public bool IsToggle;
	public bool PlayNextOnLastTweenFinish;
	public bool RunOnStart;
	public bool IsActive;
	
	public float ActiveSince { get; set; }
	
	public List<iTweenMotion> parallelTweens;
	public List<iTweenMotion> parallelTweensStandard;
		
	public void ApplyTweenPlayerButton (TweenPlayerButton tweenPlayerButton) {
		this.Name = tweenPlayerButton.Name;
		this.IsToggle = tweenPlayerButton.IsToggle;
		this.PlayNextOnLastTweenFinish = tweenPlayerButton.PlayNextOnLastTweenFinish;
		this.RunOnStart = tweenPlayerButton.RunOnStart;
		this.parallelTweens = new List<iTweenMotion>(tweenPlayerButton.parallelTweens.Count);
		int i = 0;
		foreach (iTweenMotion nt in tweenPlayerButton.parallelTweens) {
			this.parallelTweens[i] = nt;
			++i;
		}
	}
	
	#region unity methods
	void Start ()
	{		
		iTweenMotion currentTween;
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
					
			if (currentTween.duration > maxTime)
			{
				maxTime = currentTween.duration;
				indexMaxValue = i;
			}
			else if (currentTween.duration < minTime)
			{
				minTime = currentTween.duration;
				indexMinValue = i;
			}
		
//			currentTween.callWhenFinished = "DoNothing";
//			currentTween.enabled = false;
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
			//Play ();
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
			
			PlayTween ();
		}
		else if (!IsActive)
		{
			IsActive = true;
			
			ActiveSince = Time.time;
			
			PlayTween ();
		}
	}
	
	private void PlayTween ()
	{
		if (IsActive) {
			for (int i = 0; i != parallelTweens.Count; ++i)
			{
				if ( parallelTweens [i] == null)
				{
					Debug.LogWarning (name + " i : " + i);
				}
//				ValidFromValues (parallelTweens [i]);
				parallelTweens [i].enabled = true;
				parallelTweens [i].Play (true);
			}
		}
		else
		{
			for (int i = 0; i != parallelTweensStandard.Count; ++i)
			{
//				ValidFromValues (parallelTweensStandard [i]);
				parallelTweensStandard [i].enabled = true;
				parallelTweensStandard [i].Play (true);
			}
		}
	}
	
	private void ValidFromValues (NTweener tween)
	{
		if (tween is TweenPosition)
		{
			(tween as TweenPosition).from = tween.transform.localPosition;
		}
		else if (tween is TweenScale)
		{
			(tween as TweenScale).from = tween.transform.localScale;
		}
		else if (tween is TweenRotation)
		{
			(tween as TweenRotation).from = tween.transform.localEulerAngles;
		}
	}
}