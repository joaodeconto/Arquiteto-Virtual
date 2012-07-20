using UnityEngine;
using System.Collections;

public class ClickItem : MonoBehaviour {
	
	public GameObject item;
	public Camera camera;
	
	void OnClick () {
		InstanceNewObject (item);
	}
	
	void InstanceNewObject (GameObject gameObject) {
       	Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width / 2,Screen.height / 2));
		RaycastHit hit;
		
        if (!Physics.Raycast(ray, out hit))
        	return;
			
		#region travando a posição de todos os móveis
		GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
		for(int i = 0; i != furniture.Length; ++i){
			furniture[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		#endregion
		
		GameObject newModule = Instantiate(gameObject) as GameObject;
							
		newModule.tag = "Movel";
		Debug.Log(newModule.layer);
		if (!newModule.name.Contains("Painel") &&
			!newModule.name.Contains("Janela") &&
			!newModule.name.Contains("Porta") &&
			!newModule.name.Contains("Persiana"))
		{
			newModule.layer = LayerMask.NameToLayer("Moveis");
		}
		else
		{
			newModule.layer = LayerMask.NameToLayer("Painel");
		}
		
		foreach (Animation anim in newModule.GetComponentsInChildren<Animation>()) {
			anim.Stop();
			anim.playAutomatically = false;
		}

		if (newModule.name.LastIndexOf ("(") != -1) {//Retirar o "(Clone)" de trás do objeto
			newModule.name = newModule.name.Substring (0, newModule.name.LastIndexOf ("("));
		}
		
		newModule.AddComponent<SnapBehaviour>();
		newModule.AddComponent<CalculateBounds>();

		if (newModule.GetComponent<Rigidbody> () == null)
			newModule.AddComponent<Rigidbody>();
		
		if (newModule.GetComponent<InformacoesMovel>().tipoMovel != TipoMovel.MOVEL)
		{
			newModule.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | 
												 RigidbodyConstraints.FreezeRotation;
		} else {
			newModule.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}
						
		newModule.GetComponent<InformacoesMovel>().Initialize();
		
		GameObject MoveisGO = GameObject.Find("Moveis GO");
	
		if(MoveisGO == null){
			MoveisGO = new GameObject("Moveis GO");
		}
	
		#region Setando a posicao inicial do móvel se não estiver sobre um "Chao"
		GameObject[] ground = GameObject.FindGameObjectsWithTag("Chao");
		Vector3 nearestAvailableGround = Vector3.zero;
		float shortestDistance = float.MaxValue;
		float distance;
		foreach(GameObject groundPiece in ground)
		{
			if (groundPiece.collider.bounds.Contains (hit.point))
			{
				nearestAvailableGround = hit.point;
				break;
			}
		}
		if (nearestAvailableGround == Vector3.zero)
		{
			nearestAvailableGround = hit.point;
			nearestAvailableGround.y = 0.0f;
		}
	
		newModule.transform.position = nearestAvailableGround;
		#endregion
		
		#region colocar objeto virado para a câmera
		float yRotation  = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation.eulerAngles.y;
		
		Debug.Log("yRotation: " + yRotation);
		
		if(yRotation < 55 || yRotation > 325) {
			newModule.transform.eulerAngles = new Vector3(0,180,0);
		}
		else if(yRotation < 145 && yRotation > 55) {
			newModule.transform.eulerAngles = new Vector3(0,270,0);
		}
		else if(yRotation < 235 && yRotation > 145) {
			newModule.transform.eulerAngles = new Vector3(0,0,0);
		}
		else if(yRotation < 325 && yRotation > 235) {
			newModule.transform.eulerAngles = new Vector3(0,90,0);
		} else { Debug.LogError(" Something gone wrong! ");}
		#endregion
		
		newModule.transform.parent = MoveisGO.transform;
	}
}
