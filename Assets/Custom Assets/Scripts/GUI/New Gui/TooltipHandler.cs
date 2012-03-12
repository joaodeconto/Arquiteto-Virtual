using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

[AddComponentMenu("NGUI-Black Bugio/Tooltip Handler")]
public class TooltipHandler : MonoBehaviour
{
	public string label { get; private set; }
	
	#region GetComponets
	public GameObject gameObject;
	public List<Type> components;
	public int selectedComp = 0;
	#endregion
	
	#region GetValues
	public List<FieldInfo> vars;
	public int selectedVar = 0;
	#endregion
	
	private bool isOver;
	
	void OnHover (bool isOver)
	{
		this.isOver = isOver;
		if (!isOver)
			UITooltip.Close ();
	}
	
	void Update ()
	{
		if (isOver)
			UITooltip.ShowText (label);
	}
	
	public void SetTooltip (string label)
	{
		this.label = label;
	}
}
