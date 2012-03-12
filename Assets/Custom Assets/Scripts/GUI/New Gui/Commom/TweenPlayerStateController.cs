using UnityEngine;
using System.Collections;

[System.Serializable]
public class TweenStateStream
{
	public string name;
	public TweenStream[] tweenStreams;
}

public class TweenPlayerStateController : MonoBehaviour {
	
	public string Name;
	public bool RunOnStart;
	
	public TweenStateStream[] tweenStateStreams;
	
	private int currentTweenStateStream;
	private int currentTweenStream;
	
	private int lastTweenStateStreamIndex;
	
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
		
		currentTweenStateStream = 0;
		currentTweenStream = 0;
		
		lastTweenStateStreamIndex = -1;
				
		if (RunOnStart)
		{
			PlayState(currentTweenStateStream);
		}
	}
	#endregion
		
	public void PlayState (int index)
	{
		if(index > 0 && index < tweenStateStreams.Length)
		{
			currentTweenStream = 0;
			currentTweenStateStream = 0;
			
			//Se o último indice
			if(lastTweenStateStreamIndex != index)
			{
				PlayNextTween (true);
			}
			else{
				PlayNextTween (false);
			}
			
			lastTweenStateStreamIndex = index;
			
		}
	}
	
	private void PlayNextTween (bool forward)
	{
		if (currentTweenStream++ != tweenStateStreams[currentTweenStateStream].tweenStreams.Length)
		{
			foreach(TweenStream tweenStream in tweenStateStreams[currentTweenStateStream].tweenStreams)
			{
				foreach(NTweener tw in tweenStream.parallelTweens)
				{
					tw.enabled = true;
					tw.Play (true);
				}
			}
		}
		else
		{
			currentTweenStream = 0;
		}
	}
}
