using UnityEngine;
using System.Collections;

public class PauseButtonHandler : MonoBehaviour
{
	public Texture2D textureMask;

	private GameObject mainCamera;
	private GameObject menuCamera;
	private Vector3 bkpPositonMainCamera;
	
	private Texture2D pauseTexture;
	private bool IsPaused;
	private bool AlreadyTakePhoto;
	
	void Start ()
	{
		IsPaused = false;
		menuCamera = GameObject.Find("UI Root (2D)").transform.FindChild("CameraMenus").gameObject;
		mainCamera = GameObject.FindWithTag("MainCamera");
	}
	
	void OnClick ()
	{
		if (IsPaused)
		{
			Transform[] btnsTransform = GameObject.Find ("UI Root (2D)").GetComponentsInChildren<Transform> ();
		
			foreach (Transform pButton in btnsTransform) {
				if (pButton.GetComponent<Collider> () != null) {
					pButton.GetComponent<Collider> ().enabled = true;
				}
			}
		
			//tirando do pause
			IsPaused = false;
			menuCamera.GetComponent<UICamera> ().enabled = true;
			mainCamera.SetActiveRecursively(true);
		}
		else
		{
			//pausando
			IsPaused = true;
			
			AlreadyTakePhoto = false;
			StartCoroutine ("TakeAPhoto");
		}
	}
	
	private IEnumerator TakeAPhoto ()
	{
		yield return new WaitForSeconds(0.1f);
		yield return new WaitForEndOfFrame();
	    
		int width  = Screen.width;
		int height = Screen.height;
		pauseTexture = new Texture2D (width, height, TextureFormat.RGB24, false);
	    
		pauseTexture.ReadPixels (new Rect (0, 0, width, height), 0, 0);
		pauseTexture.Apply ();
		pauseTexture.EncodeToPNG ();
		
		menuCamera.GetComponent<UICamera> ().enabled = false;
		mainCamera.SetActiveRecursively(false);
					
		AlreadyTakePhoto = true;
		
		Transform[] btnsTransform = GameObject.Find("UI Root (2D)").GetComponentsInChildren<Transform>();
		
		foreach (Transform pButton in btnsTransform)
		{
			if (pButton.GetComponent<Collider>() != null)
			{
				pButton.GetComponent<Collider> ().enabled = false;
			}
		}
	}
	
	private void RecursiveToggleCollidersInChildren (Transform cTransform)
	{
		Debug.LogWarning ("pButton.name: " + cTransform.name);
		Debug.LogWarning ("cTransform.childCount: " + cTransform.childCount);
		
		Transform pChild = null;
		
		for (int i = 0; i != cTransform.childCount; ++i) {
			pChild = cTransform.GetChild (i);
			Debug.LogWarning ("pButton.name: " + pChild.name);
			
			if (pChild.GetComponent<Collider> () != null) {
				pChild.GetComponent<Collider> ().enabled = !pChild.GetComponent<Collider> ().enabled;
			}
			
			RecursiveToggleCollidersInChildren (pChild);
		}
	}
	
	void OnGUI ()
	{
		if (IsPaused && AlreadyTakePhoto)
		{
			GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height),pauseTexture);
			GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), textureMask);
		}
	}
}
