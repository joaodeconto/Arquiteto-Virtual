using UnityEngine;
using System.Collections;

public class StartBuilderHandler : MonoBehaviour
{
	public GameObject mainCamera;
	public GameObject intro;
	public GameObject scenario;
	public GameObject rulers;
	public GameObject wallBuilder;
	public Vector3 rotationTo;
	
	private Vector2 oldDegrees;
	private float oldRange;
	
	void Awake ()
	{
		Invoke ("DeactiveOthersScenes", 0.1f);
	}
	
	void OnClick ()
	{
		iTween.RotateTo (mainCamera, iTween.Hash(
				iT.RotateTo.rotation, rotationTo,
				iT.RotateTo.time, 2f
			));
		
		oldDegrees = mainCamera.GetComponent<PanWithMouse> ().degrees;
		oldRange = mainCamera.GetComponent<PanWithMouse> ().range;
		Destroy(mainCamera.GetComponent<PanWithMouse> ());
		
		scenario.SetActive (true);
		
		Invoke ("EndAnimation", 2.1f);
		
	}
	
	void DeactiveOthersScenes ()
	{
		scenario.SetActive (false);
		rulers.SetActive (false);
		wallBuilder.SetActive (false);
	}
	
	void EndAnimation ()
	{
		intro.SetActive (false);
		rulers.SetActive (true);
		wallBuilder.SetActive (true);
		mainCamera.AddComponent<PanWithMouse> ();
		mainCamera.GetComponent<PanWithMouse> ().degrees = oldDegrees;
		mainCamera.GetComponent<PanWithMouse> ().range = oldRange;
	}
	
	
}