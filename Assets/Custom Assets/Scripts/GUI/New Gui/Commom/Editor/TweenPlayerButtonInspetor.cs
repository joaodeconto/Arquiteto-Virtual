using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TweenPlayerButton))]
public class TweenPlayerButtonInspetor : Editor
{
	public string Name;
	public bool IsToggle;
	public bool PlayNextOnLastTweenFinish;
	public bool RunOnStart;
	public bool IsActive;
	
	public NTweener[] parallelTweens;
	public NTweener[] parallelTweensStandard;
	
	private bool isForwardDirection;
	
	private TweenPlayerButton btnTweenPlayer;
	private NTweener tempTween;
	/*
	#region unity methods
	void Start ()
	{		
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
	*/
	void OnEnable ()
	{
		btnTweenPlayer = target as TweenPlayerButton;
	}
	
	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Blackbugio:");
		GUILayout.Space (10f);
		
		#region comportamento 1
		GUILayout.Label ("Comportamento 1:");
		GUILayout.Space (10f);
		
		tempTween = EditorGUILayout.ObjectField ("Adicionar tween:",
												  tempTween, 
												  typeof(NTweener), 
												  true) as NTweener;
		if (tempTween != null)
		{
			if (btnTweenPlayer.parallelTweens.Contains (tempTween))
			{
				Debug.LogError ("O Label já foi adicionado ao I18n.");
			}
			else
			{
				btnTweenPlayer.parallelTweens.Add (tempTween);
			}
			
			tempTween = null;
		}
		
		GUILayout.Space (10f);
		GUILayout.Label ("Tweens:");
		GUILayout.Space (5f);
		if (btnTweenPlayer.parallelTweens.Count != 0)
		{
			for (int i = 0; i != btnTweenPlayer.parallelTweens.Count; ++i)
			{
				GUILayout.BeginHorizontal ();
				GUILayout.Label (btnTweenPlayer.parallelTweens [i].name);
				btnTweenPlayer.parallelTweens [i] = EditorGUILayout.ObjectField (btnTweenPlayer.parallelTweens [i],
																    typeof(NTweener),
																    true,
																    GUILayout.Width (200f)) as NTweener;
																    
				if (GUILayout.Button ("Deletar", GUILayout.Width (60f)))
				{
					btnTweenPlayer.parallelTweens.RemoveAt (i);
					break;
				}
				GUILayout.EndHorizontal();
			}
		}
		#endregion
		
		#region comportamento padrão
		GUILayout.Label ("Comportamento padrão:");
		GUILayout.Space (10f);
		
		tempTween = EditorGUILayout.ObjectField ("Adicionar tween:",
												  tempTween, 
												  typeof(NTweener), 
												  true) as NTweener;
		if (tempTween != null)
		{
			if (btnTweenPlayer.parallelTweensStandard.Contains (tempTween))
			{
				Debug.LogError ("O Label já foi adicionado ao I18n.");
			}
			else
			{
				btnTweenPlayer.parallelTweensStandard.Add (tempTween);
			}
			
			tempTween = null;
		}
		
		GUILayout.Space (10f);
		GUILayout.Label ("Tweens:");
		GUILayout.Space (5f);
		if (btnTweenPlayer.parallelTweensStandard.Count != 0)
		{
			for (int i = 0; i != btnTweenPlayer.parallelTweensStandard.Count; ++i)
			{
				GUILayout.BeginHorizontal ();
				GUILayout.Label (btnTweenPlayer.parallelTweensStandard [i].name);
				btnTweenPlayer.parallelTweensStandard [i] = EditorGUILayout.ObjectField (btnTweenPlayer.parallelTweensStandard [i],
																    typeof(NTweener),
																    true,
																    GUILayout.Width (200f)) as NTweener;
																    
				if (GUILayout.Button ("Deletar", GUILayout.Width (60f)))
				{
					btnTweenPlayer.parallelTweensStandard.RemoveAt (i);
					break;
				}
				GUILayout.EndHorizontal();
			}
		}
		#endregion
	}
}