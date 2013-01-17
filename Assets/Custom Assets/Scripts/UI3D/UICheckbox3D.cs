using UnityEngine;
using System.Collections.Generic;

// Need NGUI to use

[AddComponentMenu("BlackBugio/GUI/Interaction/Checkbox 3D")]

public class UICheckbox3D : MonoBehaviour
{
	
	public GameObject tweenTarget;
	public string[] materialsName;
	public bool changeColor = true;
	public Color hover = new Color(0.6f, 0.6f, 0.6f, 1f);
	public Color check = new Color(0.3f, 0.3f, 0.3f, 1f);
	public float duration = 0.2f;
	public bool startedChecked;
	public bool isChecked { get; set; }

	private List<Material> materials = new List<Material> ();
	
	protected Color[] currentColors;
	protected Color[] to;
	protected float[] factors;
	protected bool hasColor = false;
	protected bool isHighlighted = false;
	
	void Awake () { Init (); }

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
				if (!isChecked) to[i] = currentColors[i];
				else to[i] = check;
				factors[i] = 0f;
			}
		}
		
		if (!hasColor || !changeColor) enabled = false;
		
		if (startedChecked) Checked ();
	}
	
	void OnMouseEnter ()
	{
		if (enabled)
		{
			if (!isChecked && hasColor && changeColor) 
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
			if (!isChecked && hasColor && changeColor)
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
		
	void OnMouseUpAsButton ()
	{
		Checked ();
	}
	
	public void Checked ()
	{
		if (!isChecked)
		{
			if (enabled)
			{
				if (hasColor && changeColor)
				{
					for (int i = 0; i != materials.Count; i++)
					{
						to[i] = check;
						factors[i] = 0f;
					}
				}
				isChecked = true;
			}
			tweenTarget.SendMessage ("OnChecked", SendMessageOptions.DontRequireReceiver);
			SendMessageUpwards ("OnChecked", SendMessageOptions.RequireReceiver);
		}
	}
	
	public void Deschecked ()
	{
		if (isChecked)
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
				isChecked = false;
			}
			tweenTarget.SendMessage ("OnDeschecked", SendMessageOptions.DontRequireReceiver);
		}
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
