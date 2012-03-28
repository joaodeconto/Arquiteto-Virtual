using UnityEngine;
using System.Collections;

public enum Orientacao {
	Frente, Atras, Direita, Esquerda, Cima, Baixo, Perspectiva
}

public class FocarOrientacao : MonoBehaviour {
	
	public Orientacao orientacao = Orientacao.Frente;
	private float tweenTime = 2.0f;
	
	private float distanceRate = 1.5f;
	private float cameraInclination = 11f;
	
	private Vector3 newDirection = Vector3.zero;
	private Vector3 newPosition = Vector3.zero;
	private float distanceFromMiddle = 0f; 
	
	private GameObject mainCamera;
	
	void Start ()
	{
		tweenTime = 1.0f;
	}
				
	void OnMouseUp () {
		
		mainCamera = GameObject.FindWithTag("MainCamera");
		
		if (mainCamera == null)
		{
			Debug.LogError ("Main camera can't be found");
			return;
		}
		
		GameObject[] phantomFloors = GameObject.FindGameObjectsWithTag ("ChaoVazio");
		if (phantomFloors == null || phantomFloors.Length == 0)
		{
			Debug.LogError ("Não foi possível encontrar nenhum \"ChaoVazio\" por favor verifique se as cenas estão interligadas corretamente.");
			return;	
		}
		
		Debug.LogWarning ("distanceFromMiddle: " + distanceFromMiddle);
		Debug.LogWarning ("Mathf.Sqrt(distanceRate): " + Mathf.Sqrt (distanceRate));
		distanceFromMiddle = phantomFloors.Length * distanceRate;
		distanceFromMiddle /= Mathf.Sqrt(distanceFromMiddle); 
		Debug.LogWarning ("distanceFromMiddle: " + distanceFromMiddle);
		
		Debug.LogError ("orientacao: " + orientacao.ToString());
		newDirection   = Vector3.zero;
		newDirection.x = cameraInclination;
		newPosition    = WallBuilder.ROOT;
		newPosition.y  = 1.7f;
		switch (orientacao)
		{
			case Orientacao.Frente:
				newDirection.y = 0f;
				newPosition.z -= distanceFromMiddle;
				break;
			case Orientacao.Direita:
				newDirection.y = 270f;
				newPosition.x += distanceFromMiddle;
				break;
			case Orientacao.Atras:
				newDirection.y = 180f;
				newPosition.z += distanceFromMiddle;
				break;
			case Orientacao.Esquerda:
				newDirection.y  = 90f;
				newPosition.x -= distanceFromMiddle;
				break;
			case Orientacao.Cima:
				newDirection.x  = 90;
				newPosition.y += distanceFromMiddle;
				break;
			case Orientacao.Baixo:
				newDirection.x  = -90;
				newPosition.y -= distanceFromMiddle;
				break;
			case Orientacao.Perspectiva:
				newDirection   = new Vector3(45f,45f,0f);
				newPosition.x -= distanceFromMiddle;
				newPosition.y += distanceFromMiddle;
				newPosition.z -= distanceFromMiddle;
				break;
		}
		
		mainCamera.GetComponent<Camera3d>().FreezeCamera ();
		
		iTween.RotateTo (mainCamera, iTween.Hash (	iT.RotateTo.rotation, newDirection,
													iT.RotateTo.time, tweenTime,
													iT.RotateTo.easetype, iTween.EaseType.easeInOutSine));
		
		iTween.MoveTo (mainCamera, iTween.Hash (iT.MoveTo.position, newPosition,
												iT.MoveTo.time, tweenTime,
												iT.MoveTo.easetype, iTween.EaseType.easeInOutSine,
												iT.MoveTo.oncomplete, "ReleaseCamera",
												iT.MoveTo.oncompletetarget, this.gameObject));
	}
	
	void ReleaseCamera ()
	{
		Debug.LogWarning ("Was called ReleaseCamera");
		mainCamera.GetComponent<Camera3d> ().FreeCamera ();
	}
}
