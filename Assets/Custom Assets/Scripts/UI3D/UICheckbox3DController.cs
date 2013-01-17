using UnityEngine;
using System.Collections;

[AddComponentMenu("BlackBugio/GUI/Interaction/Checkbox 3D Controller")]

public class UICheckbox3DController : MonoBehaviour {
	
	protected UICheckbox3D[] uiCheckboxs;
	protected UICheckbox3D currentCheckbox;
	
	void OnChecked ()
	{
		if (uiCheckboxs == null) uiCheckboxs = GetComponentsInChildren<UICheckbox3D> (true);
		
		foreach (UICheckbox3D uic in uiCheckboxs)
		{
			if (uic.isChecked && uic != currentCheckbox)
			{
				if (currentCheckbox != null)
				{
					currentCheckbox.Deschecked ();
				}
				currentCheckbox = uic;
			}
		}
	}
}
