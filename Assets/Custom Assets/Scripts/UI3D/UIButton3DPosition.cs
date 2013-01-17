using UnityEngine;

// Need NGUI to use

[AddComponentMenu("BlackBugio/GUI/Interaction/Button 3D Position")]

public class UIButton3DPosition : MonoBehaviour {
	
	public Transform tweenTarget;
	public Vector3 hover = new Vector3(0f, 0f, -1f);
	public Vector3 pressed = new Vector3(0f, 0f, -0.5f);
	public float duration = 0.2f;
	
	Vector3 mScale;
	bool isHighlighted = false;
	bool isPressed = false;
	
	void Awake ()
	{
		if (tweenTarget == null) tweenTarget = transform;
		mScale = tweenTarget.localPosition;
		hover = mScale + hover;
		pressed = mScale + pressed;
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
			if (!isPressed) TweenPosition.Begin(tweenTarget.gameObject, duration, hover).method = UITweener.Method.EaseInOut;
			isHighlighted = true;
		}
    }
	
	void OnMouseExit ()
	{
		if (enabled)
		{
			if (!isPressed) TweenPosition.Begin(tweenTarget.gameObject, duration, mScale).method = UITweener.Method.EaseInOut;
			isHighlighted = false;
		}
	}
	
	void OnMouseDown ()
	{
		if (enabled)
		{
			TweenPosition.Begin(tweenTarget.gameObject, duration, pressed).method = UITweener.Method.EaseInOut;
			isPressed = true;
		}
	}
	
	void OnMouseUp ()
	{
		if (enabled)
		{
			TweenPosition.Begin(tweenTarget.gameObject, duration, isHighlighted ? hover : mScale).method = UITweener.Method.EaseInOut;
			isPressed = false;
		}
	}
}
