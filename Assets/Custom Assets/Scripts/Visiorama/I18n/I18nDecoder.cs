using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class I18nDecoder : MonoBehaviour
{
	public List<UILabel> labels = new List<UILabel>();
	
	void Awake ()
	{
		ChangeLanguage ();
	}
	
	void ChangeLanguage ()
	{
		I18n.Initialize();
		I18n.ChangeLanguage(PlayerPrefs.GetInt("SelectedLanguage"));
		
		for(int i = 0; i != labels.Count;++i)
		{
			if (labels [i] == null)
			{
				Destroy(labels[i]);
				continue;
			}
			if (labels [i].gameObject.active == false)
			{
				labels[i].gameObject.active = true;
				labels[i].text = I18n.t(labels[i].text);
				labels [i].gameObject.active = false;
			}
			else
			{
				labels [i].text = I18n.t (labels [i].text);
			}
		}

	}
}
