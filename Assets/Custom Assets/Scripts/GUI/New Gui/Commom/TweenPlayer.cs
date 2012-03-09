using UnityEngine;
using System.Collections;

public class TweenPlayer : MonoBehaviour {
	
	public NTweener[] tweenSequence;
	public bool RunOnStart;
	
	private int currentTween;
	
	// Use this for initialization
	protected void Start () {
		if (tweenSequence.Length != 0)
		{
			currentTween = 0;
			
			for(int i = 0; i != tweenSequence.Length; ++i)
			{
				if (tweenSequence[i] == null)
				{
					Debug.LogError("Tween não selecionada");
					Debug.Break();
				}
				
				tweenSequence[i].callWhenFinished = "PlayNextTween";
				tweenSequence[i].enabled = false;
			}
			if (RunOnStart)
			{
				tweenSequence[0].enabled = true;
				tweenSequence[0].Play(true);
			}
		}
	}
	
	public void PlayNextTween ()
	{
		if (++currentTween != tweenSequence.Length)
		{
			tweenSequence[currentTween].enabled = true;
			tweenSequence[currentTween].Play(true);
		}
	}
}
