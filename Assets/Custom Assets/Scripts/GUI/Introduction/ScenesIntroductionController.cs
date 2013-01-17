using UnityEngine;
using System.Collections;

public class ScenesIntroductionController : MonoBehaviour {
	public static GameObject SelectLanguage, SelectKitchen;
	
	void Start () {
		SelectLanguage = GameObject.Find("SelectLanguage");
		SelectKitchen = GameObject.Find("SelectKitchen");
		SelectKitchen.SetActiveRecursively(false);
	}
}
