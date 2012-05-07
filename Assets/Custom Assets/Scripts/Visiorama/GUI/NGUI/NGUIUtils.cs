using UnityEngine;
using System.Collections;

namespace Visiorama {
	public class NGUIUtils {
				
		public static bool ClickedInGUI (GameObject[] cameras, string nameLayer)
		{
			int layerGUI = LayerMask.NameToLayer (nameLayer);
			RaycastHit hit;
			Ray ray;
			
			foreach (GameObject goCamera in cameras)
			{
				ray = goCamera.camera.ScreenPointToRay (Input.mousePosition);
				
				if (Physics.Raycast (ray, out hit))
				{
					if (hit.transform.gameObject.layer == layerGUI)
					{
						return true;
					}
				}
			}
			return false;
		}
	
	}
}