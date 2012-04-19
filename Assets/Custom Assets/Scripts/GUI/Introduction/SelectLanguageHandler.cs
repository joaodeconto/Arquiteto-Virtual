using UnityEngine;
using System.Collections;

public class SelectLanguageHandler : MonoBehaviour {

	public enum LanguageEnum {
		Portuguese,
		English,
		Spanish,
	}
	
	public LanguageEnum selectedLanguage;
	
	private void OnClick(){
	
		switch(selectedLanguage){
			case LanguageEnum.Portuguese:
				PlayerPrefs.SetInt("SelectedLanguage",0);
				break;
			case LanguageEnum.English:
				PlayerPrefs.SetInt("SelectedLanguage",1);
				break;
			case LanguageEnum.Spanish:
				PlayerPrefs.SetInt("SelectedLanguage",2);
				break;
		}
		
		Application.LoadLevel(1);
		
	}
	
	void Start () {
		QualitySettings.currentLevel = QualityLevel.Fastest;
	}
}