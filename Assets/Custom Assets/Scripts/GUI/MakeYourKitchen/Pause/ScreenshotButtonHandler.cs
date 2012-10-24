using UnityEngine;
using System.Collections;
using System;

public class ScreenshotButtonHandler : MonoBehaviour {

	private const string pathExportImage = "upload/images/";
	private const string screenshotUploadFile = "uploadScreenshot.php";

	FileBrowserComponent fileBrowser;
	private string path;

	void OnClick ()
	{
		//TODO colocar isso no lugar certo
		//GameObject.Find("cfg").GetComponent<Configuration>().SaveCurrentState("lolol",true);
		//GameObject.Find("cfg").GetComponent<Configuration>().LoadState("teste/teste.xml",false);
		//GameObject.Find("cfg").GetComponent<Configuration>().RunPreset(0);

#if UNITY_WEBPLAYER
		StartCoroutine ("MakeScreenshot");
#elif !(UNITY_ANDROID || UNITY_IPHONE) || UNITY_EDITOR
		fileBrowser = new GameObject().AddComponent<FileBrowserComponent>();
		fileBrowser.Init (
            ScreenUtils.ScaledRectInSenseHeight(50, 50, 500, 400),
            "Salvar Screenshot",
            ImageSelectedCallback,
			"*.png"
        );

		GameController
			.GetInstance ().
				GetInterfaceManager ()
					.SetInterface ("DeactivateAll");
#endif
	}

	void ImageSelectedCallback (string path)
	{
		if (string.IsNullOrEmpty(path))
			path = "screenshot.png";

		if (!path.Contains(".png"))
			path += ".png";

		this.path = path;

		Destroy(fileBrowser.gameObject);

		this.gameObject.active = true;

		StartCoroutine ("MakeScreenshot");
    }

	private IEnumerator MakeScreenshot ()
	{
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
		System.IO.File.WriteAllBytes(path, bytes);
#endif

		GameController
			.GetInstance ().
				GetInterfaceManager ()
					.SetInterface ("Pause");

	}
}
