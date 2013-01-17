using UnityEngine;
using System.Collections;

public delegate void ScreenShootterCallback(byte[] imageData);

class ScreenShootter : MonoBehaviour
{
	private ScreenShootterCallback callback;
	private bool killAfterTakePhoto;

	/// <summary>
	/// <para>This method takes a snapshot from screen</para>
	/// <para>Example: SetInterface ("Pause");</para>
	/// </summary>
	/// <param name='callback'>
	/// <para>Standart: DeactiveAll.</para>
	/// </param>
	/// <param name='callback'>
	/// <para>destroyAfterTakePhoto: If true, this object will destroy the gameObject that contains it.</para>
	/// </param>
	public void TakePhoto(ScreenShootterCallback callback, bool destroyAfterTakePhoto = true)
	{
		this.killAfterTakePhoto = destroyAfterTakePhoto;
		this.callback           = callback;

		StartCoroutine ("Take");
	}

	private IEnumerator Take()
	{
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		int width  = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D (width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		tex.ReadPixels (new Rect (0, 0, width, height), 0, 0);
		tex.Apply ();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG ();
		Destroy (tex);

		if(killAfterTakePhoto)
		{
			Destroy(this.gameObject);
		}

		callback(bytes);
	}
}
