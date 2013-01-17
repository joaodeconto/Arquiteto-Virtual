using UnityEngine;
using System.Collections;

[AddComponentMenu("BlackBugio/GUI/Kitchen/Select Kitchen Controller")]

public class UISelectKitchenController : MonoBehaviour
{
	public Transform mainPosition;
	public Transform otherPosition;
	
	public iTween.EaseType easeType = iTween.EaseType.linear;
	public float time = 3f;
	
	protected Transform[] kitchens;
	protected int CurrentIndex = -1;
	
//	void Start ()
//	{
//		CurrentIndex = PlayerPrefs.GetInt ("SelectedKitchen", 0);
//	}
	
	void ChangeKitchen (int index)
	{
		if (CurrentIndex != index)
		{
			PlayerPrefs.SetInt ("SelectedKitchen", index);
			
			if (kitchens == null)
			{
				kitchens = new Transform[transform.GetChildCount ()];
				for (int i = 0; i < kitchens.Length; i++)
				{
					kitchens[i] = transform.GetChild (i);
					if (index != i)
					{
						kitchens[i].gameObject.SetActive (false);
					}
				}
				
				iTween.MoveTo (	kitchens[index].gameObject,
				iTween.Hash (iT.MoveTo.position, mainPosition.position,
							 iT.MoveTo.time, time,
							 iT.MoveTo.easetype, easeType));

			}
			else
			{
				iTween.MoveTo (	kitchens[CurrentIndex].gameObject, 
								iTween.Hash (iT.MoveTo.position, otherPosition.position,
											 iT.MoveTo.time, time,
											 iT.MoveTo.easetype, easeType));
				
				iTween.MoveTo (	kitchens[index].gameObject,
								iTween.Hash (iT.MoveTo.position, mainPosition.position,
											 iT.MoveTo.time, time,
											 iT.MoveTo.easetype, easeType));
				
				kitchens[CurrentIndex].gameObject.SetActive (false);
				kitchens[index].gameObject.SetActive (true);
			}
			
			CurrentIndex = index;
		}
	}
}
