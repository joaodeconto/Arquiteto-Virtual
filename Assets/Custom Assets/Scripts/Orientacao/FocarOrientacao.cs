using UnityEngine;
using System.Collections;

public enum Orientacao {
	Frente, Atras, Direita, Esquerda, Cima, Baixo, Perspectiva
}

public class FocarOrientacao : MonoBehaviour {
	
	public Orientacao orientacao = Orientacao.Frente;
	private Vector3 posicao = Vector3.zero;
	private GameObject camera;
	
	public float minimumHeight = 2;
	
	void OnMouseUp () {
		
		camera = GameObject.Find("Main Camera");
		
		if (camera == null)
		{
			Debug.LogError ("Main camera can't be found");
			return;
		}
		
		if (orientacao != Orientacao.Perspectiva) {
		
			Debug.LogError ("orientacao: " + orientacao.ToString());
		
			if (orientacao == Orientacao.Frente){
				if (camera.transform.eulerAngles.y > 180)
					posicao = new Vector3(0, 360, 0);	
				else
					posicao = new Vector3(0, 0, 0);
			}
			
			if (orientacao == Orientacao.Atras)
				posicao = new Vector3(0, 180, 0);
			
			if (orientacao == Orientacao.Direita){
				if (camera.transform.eulerAngles.y < 180)
					posicao = new Vector3(0, -90, 0);
				else
					posicao = new Vector3(0, 270, 0);
			}
			if (orientacao == Orientacao.Esquerda)
				posicao = new Vector3(0, 90, 0);
			if (orientacao == Orientacao.Cima)
				posicao = new Vector3(90, 0, 0);
			if (orientacao == Orientacao.Baixo)
				posicao = new Vector3(-90, 0, 0);
			
			StartCoroutine(LerpCamera(camera.transform.localEulerAngles, posicao));
			
		} else {
			posicao = Vector3.zero;
		 	StartCoroutine(LookLerpCamera(camera.transform.position, posicao));
		}
	}
	
	IEnumerator LerpCamera (Vector3 start, Vector3 end){
	
		Transform groundTransform = GameObject.Find("ParentChao").transform;
		Vector3 middleOfGround = Vector3.zero;
		
		float i = 0;
		
		for(i = groundTransform.childCount - 1; i != -1; --i){
			middleOfGround += groundTransform.GetChild((int)i).position;
		}
		
		Quaternion bkpRotation = camera.transform.rotation;
		
		middleOfGround /= i;
		//monkeys gonna patch...
		middleOfGround += WallBuilder.ROOT;
		camera.transform.localEulerAngles = end;
		middleOfGround -= camera.transform.forward * Vector3.Distance(camera.transform.position, middleOfGround);
		camera.transform.rotation = bkpRotation;
		
		if(middleOfGround.y < minimumHeight){
			middleOfGround.y = minimumHeight;
		}
		
		Vector3 lastPosition = camera.transform.position;
		
		i = 0;
	    while(i < 1) {
			//camera.transform.LookAt(middleOfGround);//,end,i);
	    	camera.transform.localEulerAngles = Vector3.Lerp(start, end, i * 2f);
	        camera.transform.position		  = Vector3.Lerp(lastPosition, middleOfGround, i * 2f);
	        i += Time.deltaTime;
	        yield return null;
	    }
	}
	
	IEnumerator LookLerpCamera (Vector3 start, Vector3 end){
	
		Transform groundTransform = GameObject.Find("ParentChao").transform;
		Vector3 middleOfGround = Vector3.zero;
		
		float i = 0;
		
		for(i = groundTransform.childCount - 1; i != -1; --i){
			middleOfGround += groundTransform.GetChild((int)i).position;
		}
		
		Quaternion bkpRotation = camera.transform.rotation;
		
		middleOfGround /= i;
		camera.transform.localEulerAngles = end;
		middleOfGround -= camera.transform.forward * Vector3.Distance (camera.transform.position, middleOfGround);
		camera.transform.rotation = bkpRotation;
		
		
		if(middleOfGround.y < minimumHeight){
			middleOfGround.y = minimumHeight;
		}
		
		Vector3 lastPosition = camera.transform.position;
		
	    while(i < 1) {
			Quaternion rotation = Quaternion.LookRotation(end - start);
			camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, rotation, i * 2f);
	        camera.transform.position = Vector3.Lerp(lastPosition, middleOfGround, i * 2f);
	        i += Time.deltaTime;
	        yield return null;
	    }
	}

}
