using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID || UNITY_IPHONE
		if (Input.touchCount == 1) { 
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				Camera cam = GetComponentInChildren<Camera>();
				Ray ray = cam.ScreenPointToRay(touch.position);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					PlayCollider(hit);
				}
			}
		}
		#else
		if (Input.GetMouseButtonUp(0)) {
			Transform cam = GameObject.FindWithTag("Player").transform;
			RaycastHit hit;
			if (Physics.Raycast(cam.position, cam.forward, out hit)) {
				PlayCollider(hit);
			}
		}
		#endif
	}
	
	void PlayCollider (RaycastHit hit) {
		if (hit.transform.tag == "Movel" ||
			hit.transform.tag == "MovelSelecionado") {
			InformacoesMovel movel = hit.transform.GetComponentInChildren<InformacoesMovel>();
			Animation[] animacoes = hit.transform.GetComponentsInChildren<Animation>();
			foreach (Animation animacao in animacoes) {
				if (animacao.isPlaying)
					return;
			}
			foreach (Animation animacao in animacoes) {
				if (animacao.clip != null) {
					if (movel.portas == Portas.FECHADAS) {
						animacao[animacao.clip.name].speed = 1;
						animacao[animacao.clip.name].time = 0;
						animacao.Play();
					}
					else {
						animacao[animacao.clip.name].speed = -1;
						animacao[animacao.clip.name].time = animacao[animacao.clip.name].length;
						animacao.Play();
					}
				}
			}
			if (movel.portas == Portas.FECHADAS)
				movel.portas = Portas.ABERTAS;
			else
				movel.portas = Portas.FECHADAS;
		}
	}
}
