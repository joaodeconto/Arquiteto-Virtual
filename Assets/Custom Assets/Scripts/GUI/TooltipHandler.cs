using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

[AddComponentMenu("NGUI-Black Bugio/Tooltip Handler %#t")]
public class TooltipHandler : MonoBehaviour
{
	public string label = "";
	
	public bool getViaCode;
	public bool getViaLabel;
	
	#region Via Label
	public bool useI18n;
	#endregion
	
	#region GetComponets
	public GameObject gameObject;
	public List<Type> components;
	public int selectedComp = 0;
	#endregion
	
	#region GetValues
	public List<FieldInfo> vars;
	public int selectedVar = 0;
	#endregion
	
	private string lastLabel;
	private bool isOver;
	
	void Awake ()
	{
		label = useI18n ? I18n.GetInstance().t(label) : label;
	}
	
	void OnHover (bool isOver)
	{
		if (isOver)
		{
			UITooltip.ShowText (label);
		}
		else
		{
			UITooltip.ShowText (null);
		}
		
		this.isOver = isOver;
	}
	
	void OnClick ()
	{
		if (isOver)
		{
			UITooltip.ShowText (null);
		}
	}
	
	public void SetTooltip (string label)
	{
		this.label = label;
	}
}
