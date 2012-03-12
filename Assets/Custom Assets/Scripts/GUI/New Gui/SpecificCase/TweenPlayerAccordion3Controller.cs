using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenPlayerAccordion3Controller : MonoBehaviour
{
	public string Name;
	public bool RunOnStart;
	
	public TweenStateStream TodosFechadosPara1;
	public TweenStateStream TodosFechadosPara2;
	public TweenStateStream TodosFechadosPara3;
	
	public TweenStateStream _1ParaTodosFechados;
	public TweenStateStream _1Para2;
	public TweenStateStream _1Para3;
	
	public TweenStateStream _2ParaTodosFechados;
	public TweenStateStream _2Para1;
	public TweenStateStream _2Para3;
	
	public TweenStateStream _3ParaTodosFechados;
	public TweenStateStream _3Para1;
	public TweenStateStream _3Para2;
	
	private int currentTweenStream;
	private int currentTweenStateStream;
	
	private bool[] activeButtons;
	
	#region unity methods
	protected void Start ()
	{
		activeButtons = new bool[3];
	}
	#endregion
	
	public void ActiveButton (int indexButton)
	{
		#region tudo fechado
		if ( activeButtons [0] == false && 
			 activeButtons [1] == false && 
			 activeButtons [2] == false)
		{
			if (indexButton == 0)
			{
				RunTweenStateStreams (TodosFechadosPara1);
				ClearButtons(0);
			}
			else if (indexButton == 1)
			{
				RunTweenStateStreams (TodosFechadosPara2);
				ClearButtons (1);
			}
			else if (indexButton == 2)
			{
				RunTweenStateStreams (TodosFechadosPara2);
				ClearButtons (2);
			}
		}
		#endregion
		#region 1 aberto
		if ( activeButtons [0] == true && 
			 activeButtons [1] == false && 
			 activeButtons [2] == false)
		{
			if (indexButton == 0)
			{
				RunTweenStateStreams (_1ParaTodosFechados);
				ClearButtons();
			}
			else if (indexButton == 1)
			{
				RunTweenStateStreams (_1Para2);
				ClearButtons (1);
			}
			else if (indexButton == 2)
			{
				RunTweenStateStreams (_1Para3);
				ClearButtons (2);
			}
		}
		#endregion
		#region 2 aberto
		if ( activeButtons [0] == false && 
			 activeButtons [1] == true && 
			 activeButtons [2] == false) 
		{
			if (indexButton == 0)
			{
				RunTweenStateStreams (_2Para1);
				ClearButtons (0);
			}
			else if (indexButton == 1)
			{
				RunTweenStateStreams (_2ParaTodosFechados);
				ClearButtons ();
			}
			else if (indexButton == 2)
			{
				RunTweenStateStreams (_2Para3);
				ClearButtons (2);
			}
		}
		#endregion
		#region 3 aberto
		if ( activeButtons [0] == false && 
			 activeButtons [1] == false && 
			 activeButtons [2] == false)
		{
			if (indexButton == 0) {
				RunTweenStateStreams (_3Para1);
				ClearButtons (1);
			} else if (indexButton == 1) {
				RunTweenStateStreams (_3Para2);
				ClearButtons (2);
			} else if (indexButton == 2) {
				RunTweenStateStreams (_3ParaTodosFechados);
				ClearButtons ();
			}
		}
		#endregion
	}
	
	private void ClearButtons ()
	{
		ClearButtons (-1);
	}
	
	private void ClearButtons (int index)
	{
		for (int i = 0; i != 3; ++i) 
		{
			activeButtons [i] = false;
		}
		
		if (index != -1)
		{
			activeButtons [index] = true;
		}
	}

	private void RunTweenStateStreams (TweenStateStream tweenStateStream)
	{
		foreach (TweenStream tweenStream in tweenStateStream.tweenStreams)
		{
			foreach (NTweener tw in tweenStream.parallelTweens)
			{
				tw.enabled = true;
				tw.Play (true);
			}
		}
	}
}
