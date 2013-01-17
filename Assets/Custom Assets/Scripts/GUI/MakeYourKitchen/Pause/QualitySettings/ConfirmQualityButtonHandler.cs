using UnityEngine;
using System.Collections;

public class ConfirmQualityButtonHandler : MonoBehaviour {

	public string interfaceName;
	
	public int currentQuality;
	
	void Start ()
	{
		currentQuality = PlayerPrefs.GetInt ("QualitySetting", QualitySettings.GetQualityLevel ());
		
	}
	
	void OnClick ()
	{
		if (currentQuality != QualitySettings.GetQualityLevel ())
		{
			QualitySettings.SetQualityLevel (currentQuality);
			PlayerPrefs.SetInt ("QualitySetting", currentQuality);
		}
		
		GameController.GetInstance ().GetInterfaceManager ().SetInterface (interfaceName);
	}
}
