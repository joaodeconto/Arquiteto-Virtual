using UnityEngine;
using System.Collections;

[System.Serializable]
public class TweenPlayer : MonoBehaviour {
	
	public string Name;
	public bool RunOnStart;
	public TweenStream[] tweenStreams;
	
	private int currentStream;
	
	#region unity methods
	protected void Start ()
	{
		#region validate tweens
		for (int i = 0; i != tweenStreams.Length; ++i)
		{
			foreach (NTweener tw in tweenStreams[i].parallelTweens)
			{
				if (tw == null)
				{
					Debug.LogError ("A TweenStream " + tweenStreams[i].name + 
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
		#endregion
		
		currentStream = 0;
				
		if (RunOnStart)
		{
			PlayNextTween();
		}
	}
	#endregion
	
	public void Play ()
	{
		currentStream = 0;
		
		PlayNextTween ();
	}
	
	public void PlayNextTween ()
	{
		if (currentStream++ != tweenStreams.Length)
		{
			foreach(TweenStream tweenStream in tweenStreams)
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
			currentStream = 0;
		}
	}
}
