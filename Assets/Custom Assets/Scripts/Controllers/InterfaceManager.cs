using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Game/Controllers/Interface Manager")]
public class InterfaceManager : MonoBehaviour
{
	
	[System.Serializable]
	public class InterfaceObject
	{
		public string name;
		public GameObject main;
		public GameObject[] mobileInterfaces;
		public string[] transformManagers;
		public MonoBehaviour[] scriptManagers;
		public bool initializeWithThis;
		public List<GameObject> gosDeactived { get; set; }
		public List<bool> scriptsDeactived { get; set; }
	}
	
	public InterfaceObject[] interfaceObjects;
	
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
			if (io.transformManagers.Length != 0) io.gosDeactived = new List<GameObject> ();
			if (io.scriptManagers.Length != 0) io.scriptsDeactived = new List<bool> ();
			
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
		
		
		foreach (InterfaceObject io in interfaceObjects)
		{
			if (currentInterface.Equals(""))
			{
				if (name.ToLower().Equals (io.name.ToLower ()))	currentInterface = io.name;
			}
			
			if (currentInterface.Equals (io.name))
			{
				if (io.transformManagers.Length != 0)
				{
					foreach (string tm in io.transformManagers)
					{
						foreach (Transform t in io.main.GetComponentsInChildren<Transform>())
						{
							Transform tmObject = t.name.Equals(tm) ? t : null;
							
							if (tmObject == null) continue;
							
							for (int i = 0; i < tmObject.GetChildCount(); i++)
							{
								if (!tmObject.GetChild(i).gameObject.active)
								{
									io.gosDeactived.Add (tmObject.GetChild(i).gameObject);
								}
							}
						}
					}
				}
				if (io.scriptManagers.Length != 0)
				{
					foreach (MonoBehaviour sm in io.scriptManagers)
					{
						io.scriptsDeactived.Add(sm.enabled);
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
				if (io.transformManagers.Length != 0)
				{
					if (io.gosDeactived.Count != 0)
					{
						foreach (GameObject goDeactived in io.gosDeactived)
						{
							goDeactived.SetActiveRecursively (false);
						}
						io.gosDeactived = new List<GameObject> ();
					}
				}
				
				if (io.scriptManagers.Length != 0)
				{
					if (io.scriptsDeactived.Count != 0)
					{
						for (int i = 0; i != io.scriptManagers.Length; i++)
						{
							io.scriptManagers[i].enabled = io.scriptsDeactived[i];
						}
						io.scriptsDeactived = new List<bool> ();
					}
				}

				currentInterface = io.name;
			}
			else
			{
				io.main.SetActiveRecursively (false);
				
				for (int i = 0; i != io.scriptManagers.Length; i++)
				{
					io.scriptManagers[i].enabled = false;
				}
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