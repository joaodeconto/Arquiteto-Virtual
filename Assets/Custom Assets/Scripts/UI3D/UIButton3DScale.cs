using UnityEngine;

// Need NGUI to use

[AddComponentMenu("BlackBugio/GUI/Interaction/Button 3D Scale")]

public class UIButton3DScale : MonoBehaviour {
	
	public Transform tweenTarget;
	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);
	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);
	public float duration = 0.2f;
	
	Vector3 mScale;
	bool isHighlighted = false;
	bool isPressed = false;
	
	void Awake ()
	{
		if (tweenTarget == null) tweenTarget = transform;
		mScale = tweenTarget.localScale;
	}
	
	void OnDisable ()
	{
		if (tweenTarget != null)
		{
			TweenScale tc = tweenTarget.GetComponent<TweenScale>();

			if (tc != null)
			{
				tc.scale = mScale;
				tc.enabled = false;
			}
		}
	}
	
	void OnMouseEnter ()
	{
		if (enabled)
		{
			if (!isPressed) TweenScale.Begin(tweenTarget.gameObject, duration, Vector3.Scale(mScale, hover)).method = UITweener.Method.EaseInOut;
			isHighlighted = true;
		}
    }
	
	void OnMouseExit ()
	{
		if (enabled)
		{
			if (!isPressed) TweenScale.Begin(tweenTarget.gameObject, duration, mScale).method = UITweener.Method.EaseInOut;
			isHighlighted = false;
		}
	}
	
	void OnMouseDown ()
	{
		if (enabled)
		{
			TweenScale.Begin(tweenTarget.gameObject, duration, Vector3.Scale(mScale, pressed)).method = UITweener.Method.EaseInOut;
			isPressed = true;
		}
	}
	
	void OnMouseUp ()
	{
		if (enabled)
		{
			TweenScale.Begin(tweenTarget.gameObject, duration, isHighlighted ? Vector3.Scale(mScale, hover) : mScale).method = UITweener.Method.EaseInOut;
			isPressed = false;
		}
	}
}
