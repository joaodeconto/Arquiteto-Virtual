using UnityEngine;
using System.Collections;

public class InfoOptionButtonHandler : MonoBehaviour {
	
	public enum InfoOptionButtonEnum { Info, Opcoes }
	public InfoOptionButtonEnum infoOptionButton;
	
	void OnClick ()
	{
		if (!GetComponent<TweenPlayerButton>().IsActive)
			return;
		
		Transform background = transform.parent.FindChild ("_BGMobiles");
		
		if (background == null)
		{
			Debug.LogError ("O nome do background do menu de opções está com o nome errado! Nome esperado: " + "_BGMobiles");
			Debug.Break ();
			return;
		}
		
		Vector3 backgroundScaleRoot = new Vector3(162, 0, 0);
		
		switch (infoOptionButton) {
		case InfoOptionButtonEnum.Info :
			backgroundScaleRoot.y = 280;
			foreach (iTweenMotion motion in background.gameObject.GetComponents<iTweenMotion>())
			{
				if (motion.Name == "descer")
				{
					motion.to = backgroundScaleRoot;
					motion.Play(true);
				}
			}
			break;
		case InfoOptionButtonEnum.Opcoes :
			Transform checksTransform = transform.parent.FindChild ("Checks");
			
			int yOffset = -60;
			Vector3 rootPosition = new Vector3(0,-50, 0);
			int activeCheckGroups = 0;
			foreach (Transform checkGroup in checksTransform)
			{
				if (checkGroup.gameObject.active == true)
				{
					//checkGroup.localPosition = rootPosition + Vector3.up * yOffset * activeCheckGroups++;
				}
			}
			
			backgroundScaleRoot.y = 70;
			foreach (iTweenMotion motion in background.gameObject.GetComponents<iTweenMotion>())
			{
				if (motion.Name == "descer")
				{
					//motion.to = backgroundScaleRoot + Vector3.up * -yOffset * activeCheckGroups;
					motion.Play(true);
				}
			}
			break;
		}
		
	}
}
