using UnityEngine;
using System.Collections;
using System;

public class ScreenshotButtonHandler : MonoBehaviour {

	private const string screenshotUploadFile = "uploadScreenshot.php";

	FileBrowserComponent fileBrowser;
	private string path;

	void OnClick ()
	{
		//TODO colocar isso no lugar certo
		//GameObject.Find("cfg").GetComponent<Configuration>().SaveCurrentState("lolol",true);
		//GameObject.Find("cfg").GetComponent<Configuration>().LoadState("teste/teste.xml",false);
		//GameObject.Find("cfg").GetComponent<Configuration>().RunPreset(0);

		GameController
			.GetInstance ().
				GetInterfaceManager ()
					.SetInterface ("DeactivateAll");
#if UNITY_WEBPLAYER
		MakeScreenshot();
#elif !(UNITY_ANDROID || UNITY_IPHONE)
		fileBrowser = new GameObject().AddComponent<FileBrowserComponent>();
		fileBrowser.Init (
            ScreenUtils.ScaledRectInSenseHeight(50, 50, 500, 400),
            "Salvar Screenshot",
            ImageSelectedCallback,
			"*.png"
        );
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

		MakeScreenshot();
    }

	private void MakeScreenshot ()
	{
		ScreenShootter ss = new GameObject().AddComponent<ScreenShootter>();

		ss.TakePhoto(IsDoneScreenshot);
	}

	byte[] imageData;
	public void IsDoneScreenshot(byte[] imageData)
	{
		GameController
			.GetInstance ().
				GetInterfaceManager ()
					.SetInterface ("Pause");

		this.imageData = imageData;

#if UNITY_WEBPLAYER
		StartCoroutine("UploadScreenshot");
#else
		System.IO.File.WriteAllBytes(path, imageData);
#endif
	}

	private IEnumerator UploadScreenshot()
	{
		WWWForm form = new WWWForm ();

		string filename = String.Format ("{0:yyyy-MM-dd-HH-mm-ss-}", DateTime.Now) + "screenshot.png";

		form.AddBinaryData ("file", imageData, filename, "multipart/form-data");

		WWW www = new WWW (screenshotUploadFile, form);

		yield return www;

		if (www.error != null)
		{
			print (www.error);
		}
		else
		{
			print ("Finished Uploading Screenshot");
			Application.ExternalCall ("tryToDownload", filename);
		}
	}
}
