using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("BlackBugio/Tween/TweenPlayerButton")]
public class TweenPlayerButton : MonoBehaviour
{
	public string Name;
	public bool IsToggle;
	public bool CallWhenLastTweenFinish;
	public string CallWhenFinish;
	public GameObject EventReceiver;
	
	public bool RunOnStart;
	
	public bool IsActive;
	
	public float ActiveSince { get; set; }
	
	public List<iTweenMotion> parallelTweens;
	public List<iTweenMotion> parallelTweensStandard;
	
	public float MaxDuration { get; private set; }
	public float MinDuration { get; private set; }
		
	public void ApplyTweenPlayerButton (TweenPlayerButton tweenPlayerButton) {
		this.Name = tweenPlayerButton.Name;
		this.IsToggle = tweenPlayerButton.IsToggle;
		this.CallWhenLastTweenFinish = tweenPlayerButton.CallWhenLastTweenFinish;
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
		List<iTweenMotion> tweensWhoCall = IsToggle ? parallelTweensStandard : parallelTweens;
		
		iTweenMotion currentTween;
		int parallelTweensLength = tweensWhoCall.Count;
			
		int indexMaxValue = 0;
		int indexMinValue = 0;
		
		MaxDuration = float.MinValue;
		MinDuration = float.MaxValue;
		
		#region validate tweens
		for (int i = 0; i != parallelTweensLength; ++i)
		{
			currentTween = tweensWhoCall [i]; 
			if (currentTween == null)
			{
				continue;
			}
					
			if (currentTween.duration > MaxDuration)
			{
				MaxDuration = currentTween.duration;
				indexMaxValue = i;
			}
			else if (currentTween.duration < MinDuration)
			{
				MinDuration = currentTween.duration;
				indexMinValue = i;
			}
		
			currentTween.CallWhenFinish = "DoNothing";
			currentTween.enabled = false;
		}
			
		if (CallWhenLastTweenFinish)
		{
				Debug.LogWarning ("indexMaxValue: " + indexMaxValue);
		
			if (tweensWhoCall.Count != 0 && tweensWhoCall[indexMaxValue] != null)
				tweensWhoCall[indexMaxValue].CallWhenFinish = "TweensAreOver";
		}
		else
		{
				Debug.LogWarning ("indexMinValue: " + indexMinValue);
				
			if (tweensWhoCall.Count != 0 && tweensWhoCall[indexMinValue] != null)
				tweensWhoCall[indexMinValue].CallWhenFinish = "TweensAreOver";
		}
		#endregion		
		
		if (RunOnStart)
		{
			//Play ();
		}
	}
	#endregion
	
	private void TweensAreOver ()
	{
		EventReceiver.SendMessage (CallWhenFinish);
	}
		
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
				if (parallelTweensStandard [i] == null) {
					Debug.LogWarning (name + " i : " + i);
				}
//				ValidFromValues (parallelTweensStandard [i]);
				parallelTweensStandard [i].enabled = true;
				parallelTweensStandard [i].Play (true);
			}
		}
	}
	
	private void ValidFromValues (UITweener tween)
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