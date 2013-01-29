using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

[AddComponentMenu("Arquiteto Virtual/Make Brand")]
public class MakeBrandIntroduction : MonoBehaviour
{
	
	public Material materialPortaP;
	public Texture2D normalMapPortaP;
	public Material materialPortaM;
	public Texture2D normalMapPortaM;
	public Material materialPortaG;
	public Texture2D normalMapPortaG;
	public Material materialPortaVidro;
	public Texture2D texturaVidro;

	void Start ()
	{
		foreach (Renderer r in transform.GetComponentsInChildren<Renderer> ())
		{
			foreach (Material m in r.materials)
			{
				Regex regexType;
				if (materialPortaP != null && normalMapPortaP != null)
				{
					regexType = new Regex("("+materialPortaP.name+").*", RegexOptions.IgnoreCase);
					if (regexType.Match(m.name).Success)
					{
						m.SetTexture("_BumpMap", normalMapPortaP);
						continue;
					}
				}
				if (materialPortaM != null && normalMapPortaM != null)
				{
					regexType = new Regex("("+materialPortaM.name+").*", RegexOptions.IgnoreCase);
					if (regexType.Match(m.name).Success)
					{
						m.SetTexture("_BumpMap", normalMapPortaM);
						continue;
					}
				}
				if (materialPortaG != null && normalMapPortaG != null)
				{
					regexType = new Regex("("+materialPortaG.name+").*", RegexOptions.IgnoreCase);
					if (regexType.Match(m.name).Success)
					{
						m.SetTexture("_BumpMap", normalMapPortaG);
						continue;
					}
				}
				if (materialPortaVidro != null && texturaVidro != null)
				{
					regexType = new Regex("("+materialPortaVidro.name+").*", RegexOptions.IgnoreCase);
					if (regexType.Match(m.name).Success)
					{
						materialPortaVidro.mainTexture = texturaVidro;
						continue;
					}
				}
			}
		}
	}
}
