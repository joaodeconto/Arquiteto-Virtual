using UnityEngine;
using System.Collections;

public class LoadStreaming : MonoBehaviour {
	public int numeroCena = 1;
	
	private float progress, alpha;
	private bool done;
	private Texture2D blackDot;
	private int progressTotal;
	private string message;
	
	private void Start() {
		done = false;
		progress = 0.0f;
		alpha = 0.0f;
		blackDot = new Texture2D(1, 1);
		blackDot.SetPixel(0, 0, Color.black);
		blackDot.Apply();
	}
	
	private void Update() {
		//O valor de progresso vai variar de 0.0 a 1.0f.
		progress = Application.GetStreamProgressForLevel(numeroCena);
		
			if(progress >= 1.0f) {
				//A cena terminou de carregar
				if(!done) {
					//Iniciamos uma thread para fazer o "fade" da tela e carregar o level do game
					done = true;
					StartCoroutine(FadeAndLoadNextLevel());
				}
		}
	}
	
	private IEnumerator FadeAndLoadNextLevel()
	{
		while(alpha < 1.0f) {
			//Incrementamos o alpha
			alpha += Time.deltaTime;
			
			//Aguardamos o próximo frame
			yield return null;
		}
		
		//Quando o alpha for 1.0, carregamos o próximo level
		Application.LoadLevel(numeroCena);
	}
	
	private void OnGUI()
	{
		//Exibimos o progresso atual
		progressTotal = (int)(100.0f * progress);	
		
		if(progressTotal < 100) {
			message = "Carregando: " + progressTotal.ToString() + "%.";
		}
		else {
			message = "Carregado";
		}
		
		GUI.Label(new Rect(5, 5, 200, 20), message);
		
		if(alpha > 0.0f) {
			//Guardamos o estado atual da cor da GUI
			Color auxColor, currentColor;
			auxColor = currentColor = GUI.color;
			
			//Modificamos o alpha
			auxColor.a = alpha;
			GUI.color = auxColor;
			
			//Desenhamos a textura preta que criamos em toda a tela
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackDot);
			
			//Restauramos a cor da GUI
			GUI.color = currentColor;
		}
	}
}
