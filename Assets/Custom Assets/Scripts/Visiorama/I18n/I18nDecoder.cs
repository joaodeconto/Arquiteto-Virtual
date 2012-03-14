using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class I18nDecoder : MonoBehaviour
{
	public List<UILabel> labels = new List<UILabel>();
	
	void Awake ()
	{
		I18n.Initialize();
		I18n.ChangeLanguage(PlayerPrefs.GetInt("SelectedLanguage"));
		
		for(int i = 0; i != labels.Count;++i)
		{
			labels[i].text = I18n.t(labels[i].text);
		}
	}
}
