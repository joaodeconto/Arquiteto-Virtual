using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

[AddComponentMenu("Arquiteto Virtual/Moveis/Make Brand")]
public class MakeBrand : MonoBehaviour
{
	
	public Material materialPortaP;
	public Texture2D normalMapPortaP;
	public Material materialPortaM;
	public Texture2D normalMapPortaM;
	public Material materialPortaG;
	public Texture2D normalMapPortaG;
	public Material materialPortaVidro;
	public Texture2D texturaVidro;
	public Material materialFruteira;
	public Texture2D texturaFruteira;

	public void ChangeDoor ()
	{
		if (materialPortaP != null && normalMapPortaP != null)
		{
			materialPortaP.SetTexture("_BumpMap", normalMapPortaP);
		}
		if (materialPortaM != null && normalMapPortaM != null)
		{
			materialPortaM.SetTexture("_BumpMap", normalMapPortaM);
		}
		if (materialPortaG != null && normalMapPortaG != null)
		{
			materialPortaG.SetTexture("_BumpMap", normalMapPortaG);
		}
		if (materialPortaVidro != null && texturaVidro != null)
		{
			materialPortaVidro.mainTexture = texturaVidro;
		}
		if (materialFruteira != null && texturaFruteira != null)
		{
			materialFruteira.SetTexture("_BumpMap", texturaFruteira);
		}
	}
}
