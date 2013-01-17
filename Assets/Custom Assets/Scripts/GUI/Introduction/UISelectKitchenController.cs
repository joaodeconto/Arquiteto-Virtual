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
	protected int CurrentIndex;
	
	void Start ()
	{
		CurrentIndex = PlayerPrefs.GetInt ("SelectedKitchen", 0);
		
		kitchens = new Transform[transform.GetChildCount ()];
		for (int i = 0; i < kitchens.Length; i++)
		{
			kitchens[i] = transform.GetChild (i);
		}
		
		iTween.MoveTo (	kitchens[CurrentIndex].gameObject,
						iTween.Hash (iT.MoveTo.position, mainPosition.position,
									 iT.MoveTo.time, time,
									 iT.MoveTo.easetype, easeType));
	}
	
	void ChangeKitchen (int index)
	{
		if (CurrentIndex != index)
		{
			PlayerPrefs.SetInt ("SelectedKitchen", index);
			
			if (kitchens != null)
			{
				iTween.MoveTo (	kitchens[CurrentIndex].gameObject, 
								iTween.Hash (iT.MoveTo.position, otherPosition.position,
											 iT.MoveTo.time, time,
											 iT.MoveTo.easetype, easeType));
				
				iTween.MoveTo (	kitchens[index].gameObject,
								iTween.Hash (iT.MoveTo.position, mainPosition.position,
											 iT.MoveTo.time, time,
											 iT.MoveTo.easetype, easeType));
			}
			
			CurrentIndex = index;
		}
	}
}
