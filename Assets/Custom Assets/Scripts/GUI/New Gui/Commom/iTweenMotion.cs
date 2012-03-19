using UnityEngine;
using System.Collections;

public enum iTweenMotionType
{
	POSITION,
	SCALE,
	ROTATION,
}

[AddComponentMenu("BlackBugio/Tween/iTweenMotion")]
public class iTweenMotion : MonoBehaviour
{
	public string Name;
	public GameObject go;
	public iTweenMotionType type;
	public Vector3 to;
	public float duration;
	
	public void Play (bool SomeDayItWillBeUsed)
	{
		go = (go == null) ? this.gameObject : go;
	
		switch (type)
		{
			case iTweenMotionType.POSITION:
				iTween.MoveTo (go,iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, to, iT.MoveTo.time, duration, iT.MoveTo.easetype, "easeInOutQuint"));
				break;
			case iTweenMotionType.ROTATION:
				iTween.RotateTo (go,iTween.Hash(iT.RotateTo.islocal, true, iT.RotateTo.rotation, to, iT.RotateTo.time, duration, iT.MoveTo.easetype, "easeInOutQuint"));
				break;
			case iTweenMotionType.SCALE:
				iTween.ScaleTo (go, iTween.Hash (iT.ScaleTo.scale, to, iT.ScaleTo.time, duration, iT.MoveTo.easetype, "easeInOutQuint"));
				break;
		}
	}
}