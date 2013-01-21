using UnityEngine;
using System.Collections.Generic;

// Need NGUI to use

[AddComponentMenu("BlackBugio/GUI/Interaction/Button 3D")]

public class UIButton3D : MonoBehaviour {

	public GameObject tweenTarget;
	public string[] materialsName;
	public bool changeColor = true;
	public Color hover = new Color(0.6f, 0.6f, 0.6f, 1f);
	public Color pressed = new Color(0.3f, 0.3f, 0.3f, 1f);
	public float duration = 0.2f;

	private List<Material> materials = new List<Material> ();
	
	protected Color[] currentColors;
	protected Color[] to;
	protected float[] factors;
	protected bool hasColor = false;
	protected bool isPressed = false;
	protected bool isHighlighted = false;
	
	void Awake () { Init (); }

	void OnDisable ()
	{
//		if (tweenTarget != null)
//		{
//			TweenColor tc = tweenTarget.GetComponent<TweenColor>();
//
//			if (tc != null)
//			{
//				tc.color = mColor;
//				tc.enabled = false;
//			}
//		}
	}

	protected void Init ()
	{
		if (tweenTarget == null) tweenTarget = gameObject;

		if (tweenTarget.renderer != null)
		{
			hasColor = true;
			foreach (Material m in tweenTarget.renderer.materials)
			{
				if (materialsName.Length != 0)
				{
					foreach (string mn in materialsName)
					{
						int mNameIndex = m.name.IndexOf(" (Instance)");
						
						string mName;
						if (mNameIndex != -1)
							mName = m.name.Remove(mNameIndex);
						else
							mName = m.name;
						
						if (mName.Equals(mn))
						{
							materials.Add(m);
						}
					}
				}
				else
				{
					materials.Add(m);
				}
			}
			
			currentColors = new Color[materials.Count];
			to = new Color[materials.Count];
			factors = new float[materials.Count];
			for (int i = 0; i != materials.Count; i++)
			{
				currentColors[i] = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, materials[i].color.a);
				to[i] = currentColors[i];
				factors[i] = 0f;
			}
		}
		
		if (!hasColor || !changeColor) enabled = false;
	}
	
	void OnMouseEnter ()
	{
		if (enabled)
		{
			if (!isPressed && hasColor && changeColor) 
			{
				for (int i = 0; i != materials.Count; i++)
				{
					to[i] = hover;
					factors[i] = 0f;
				}
			}
			isHighlighted = true;
		}
    }
	
	void OnMouseExit ()
	{
		if (enabled)
		{
			if (!isPressed && hasColor && changeColor)
			{
				for (int i = 0; i != materials.Count; i++)
				{
					to[i] = currentColors[i];
					factors[i] = 0f;
				}
			}
			isHighlighted = false;
		}
	}
	
	void OnMouseDown ()
	{
		if (enabled)
		{
			if (hasColor && changeColor)
			{
				for (int i = 0; i != materials.Count; i++)
				{
					to[i] = pressed;
					factors[i] = 0f;
				}
			}
			isPressed = true;
		}
	}
	
	void OnMouseUp ()
	{
		if (enabled)
		{
			if (hasColor && changeColor)
			{
				for (int i = 0; i != materials.Count; i++)
				{
					to[i] = isHighlighted ? hover : currentColors[i];
					factors[i] = 0f;
				}
			}
			isPressed = false;
		}
	}
	
	void OnMouseUpAsButton ()
	{
		gameObject.SendMessage ("OnClick", SendMessageOptions.DontRequireReceiver);
		tweenTarget.SendMessage ("OnClick", SendMessageOptions.DontRequireReceiver);
	}
	
	void Update ()
	{
		for (int i = 0; i != materials.Count; i++)
		{
			if (to[i] != materials[i].color)
			{
				materials[i].color = Color.Lerp(materials[i].color, to[i], factors[i]);
				factors[i] += (Time.deltaTime / duration);
			}
		}
	}
}
