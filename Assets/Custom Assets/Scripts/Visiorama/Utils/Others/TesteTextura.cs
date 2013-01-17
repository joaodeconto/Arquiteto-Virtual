using UnityEngine;
using System.Collections;

// Teste de troca de textura
public class TesteTextura : MonoBehaviour {
	private bool 	troca = false;
	// Use this for initialization
	IEnumerator Start () {
		// Chamar a cada 2 segundos a Troca de Textura e Material
		InvokeRepeating("TrocaTextura", 2f, 2f);
		yield return new WaitForSeconds(5f);
		print("Shaders:");
		Shader[] shaders = Resources.FindObjectsOfTypeAll(typeof(Shader)) as Shader[];
		foreach (Shader s in shaders)
			print(s.name);
		print("Textures:");
		Texture[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture)) as Texture[];
		foreach (Texture t in textures)
			print(t.name);
	}

	void TrocaTextura() {
		if (troca)
			renderer.material.shader = Shader.Find ("Diffuse");
		else {
			renderer.material.shader = Shader.Find ("Bumped Specular");
			Texture	bumpMap = Resources.LoadAssetAtPath ("Assets/Texturas/America_normal/CL_Arm_america-AO_u0_v1.tif",
			                                              typeof(Texture)) as Texture;
			renderer.material.SetTexture("_BumpMap", bumpMap);
		}

		troca = !troca;
	}
}
