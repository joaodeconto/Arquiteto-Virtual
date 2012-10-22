using UnityEngine;
using System.Collections;

[AddComponentMenu("Game/Controllers/Interface Manager")]
public class InterfaceManager : MonoBehaviour
{
	
	[System.Serializable]
	public class InterfaceObject
	{
		public string name;
		public GameObject main;
		public GameObject[] mobileInterfaces;
		public bool initializeWithThis;
		public GameObject[] gosDeactived;
	}
	
	public InterfaceObject[] interfaceObjects;
	
	public bool saveAnchorsSetting, savePanelsSetting, savePanelChildrenSetting;
	
	private string currentInterface = "";
	public string GetCurrentInterface
	{
		get
		{
			return currentInterface;
		}
	}
	
	public void Init ()
	{
		foreach (InterfaceObject io in interfaceObjects)
		{
			if (io.initializeWithThis)
			{
				SetInterface (io.name);
				break;
			}
		}
	}

	/// <summary>
	/// <para>Sets the interface.</para>
	/// <para>Example: SetInterface ("Pause");</para>
	/// </summary>
	/// <param name='name'>
	/// <para>Standart: DeactiveAll.</para>
	/// <para>Examples: Ingame, Pause.</para>
	/// </param>
	public void SetInterface (string name)
	{
		bool interfaceExists = false;
		
		if (currentInterface != "")
		{
			foreach (InterfaceObject io in interfaceObjects)
			{
				if (currentInterface.Equals (io.name))
				{
					if (saveAnchorsSetting)
					{
						foreach (UIAnchor anchor in io.main.GetComponentsInChildren<UIAnchor>())
						{
							if (!anchor.gameObject.active)
							{
								Visiorama.Utils.ArrayUtils.Add (io.gosDeactived, anchor.gameObject);
							}
						}
					}
					else if (savePanelsSetting)
					{
						foreach (UIPanel panel in io.main.GetComponentsInChildren<UIPanel>())
						{
							if (!panel.gameObject.active)
							{
								Visiorama.Utils.ArrayUtils.Add (io.gosDeactived, panel.gameObject);
							}
						}
					}
					else if (savePanelChildrenSetting)
					{
						foreach (UIPanel panel in io.main.GetComponentsInChildren<UIPanel>())
						{
							for (int i = 0; i < panel.transform.GetChildCount(); i++)
							{
								if (!panel.transform.GetChild(i).gameObject.active)
								{
									Visiorama.Utils.ArrayUtils.Add (io.gosDeactived, panel.transform.GetChild(i).gameObject);
								}
							}
						}
					}
				}
			}
		}
		
		foreach (InterfaceObject io in interfaceObjects)
		{
			if (name.ToLower().Equals (io.name.ToLower ()))
			{
				interfaceExists = true;
				
				io.main.SetActiveRecursively (true);
#if	!(UNITY_ANDROID || UNITY_IPHONE) || UNITY_EDITOR
				foreach (GameObject mi in io.mobileInterfaces)
				{
					mi.SetActiveRecursively (false);
				}
#endif
				if (currentInterface != "")
				{
					if (saveAnchorsSetting || savePanelsSetting || savePanelChildrenSetting)
					{
						if (io.gosDeactived.Length != 0)
						{
							foreach (GameObject goDeactived in io.gosDeactived)
							{
								goDeactived.SetActiveRecursively (false);
							}
							io.gosDeactived = new GameObject[0];
						}
					}
				}
				else
				{
					if (saveAnchorsSetting || savePanelsSetting || savePanelChildrenSetting)
					{
						io.gosDeactived = new GameObject[0];
					}
				}
				
				currentInterface = io.name;
			}
			else
			{
				io.main.SetActiveRecursively (false);
			}
		}
		
		if (!interfaceExists && !name.ToLower().Equals("deactiveall"))
		{
			Debug.LogError ("Don't exist the Interface called \"" + name + "\". Please verify the list of parameters.");
		}
	}
	
	/// <summary>
	/// <para>Get the interface.</para>
	/// <para>Example: GetInterface ("Pause");</para>
	/// </summary>
	public GameObject GetInterface (string name)
	{
		GameObject go = null;
		
		foreach (InterfaceObject io in interfaceObjects)
		{
			if (name.ToLower().Equals (io.name.ToLower ()))
			{
				go = io.main;
				break;
			}
		}
		
		if (go != null)
		{
			return go;
		}
		else
		{
			Debug.LogError ("Don't exist the Interface called \"" + name + "\". Please verify the list of parameters.");
			return null;
		}
	}
}