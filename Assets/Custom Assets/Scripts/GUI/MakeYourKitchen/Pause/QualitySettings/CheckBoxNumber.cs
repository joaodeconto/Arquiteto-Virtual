using UnityEngine;
using System.Collections;

public class CheckBoxNumber : MonoBehaviour {
	
	public int thisQuality;
	public ConfirmQualityButtonHandler confirmQualityButtonHandler;
	
	void OnEnable ()
	{
		if (PlayerPrefs.GetInt ("QualitySetting", QualitySettings.GetQualityLevel ()) == thisQuality)
		{
			GetComponent<UICheckbox>().isChecked = true;
		}
	}
	
	void OnActivate ()
	{
		confirmQualityButtonHandler.currentQuality = thisQuality;
	}

}
