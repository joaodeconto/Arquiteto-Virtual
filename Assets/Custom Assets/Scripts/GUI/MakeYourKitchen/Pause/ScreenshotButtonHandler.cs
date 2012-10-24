using UnityEngine;
using System.Collections;

public class ScreenshotButtonHandler : MonoBehaviour {

	void OnClick ()
	{
		GameObject.FindWithTag("GameController").GetComponentInChildren<GUICameraController>().Screenshot ();
	}
	
	private IEnumerator MakeScreenshot ()
	{
		SaveLastGUIState ();

		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
		tex.Apply ();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG ();
		Destroy (tex);

#if UNITY_WEBPLAYER

		// Create a Web Form
		WWWForm form = new WWWForm ();
//	    Debug.LogError( String.Format("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png");

		string filename = String.Format ("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png";

		form.AddBinaryData ("file", bytes, filename, "multipart/form-data");

		WWW www = new WWW (screenshotUploadFile, form);

		yield return www;

		if (www.error != null){
			print (www.error);
		} else {
			print ("Finished Uploading Screenshot");
			Application.ExternalCall ("tryToDownload", pathExportImage + filename);
		}
#else
		System.IO.File.WriteAllBytes(m_textPath, bytes);
#endif

		LoadLastGUIState ();
	}
}
