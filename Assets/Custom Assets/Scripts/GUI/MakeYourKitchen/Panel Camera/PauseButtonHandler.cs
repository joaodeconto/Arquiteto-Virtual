using UnityEngine;
using System.Collections;

public class PauseButtonHandler : MonoBehaviour
{
	public Texture2D textureMask;
	
	private Pause pause;
	
	void Start ()
	{
		pause = gameObject.AddComponent<Pause> ();
		pause.Initialize (textureMask);
	}
	
	void OnClick ()
	{
		pause.TogglePause ();
	}
}
