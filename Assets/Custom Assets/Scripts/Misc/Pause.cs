using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
	public Texture2D textureMask;

	private GameObject mainCamera;
	private GameObject menuCamera;
	private Vector3 bkpPositonMainCamera;
	
	private Texture2D pauseTexture;
	private bool IsPaused;
	private bool AlreadyTakePhoto;
	
	public void Initialize ()
	{
		IsPaused = false;
		menuCamera = GameObject.Find("UI Root (2D)").transform.FindChild("CameraMenus").gameObject;
		mainCamera = GameObject.FindWithTag("MainCamera");
	}
	
	public void PauseCamera ()
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
	
	public void GUIDraw ()
	{
		if (IsPaused && AlreadyTakePhoto)
		{
			GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height),pauseTexture);
		}
	}
}
