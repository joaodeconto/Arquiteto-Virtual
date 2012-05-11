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
		
		GameObject newFurniture = Instantiate(gameObject) as GameObject;
							
		newFurniture.tag = "Movel";
		newFurniture.layer = LayerMask.NameToLayer("Moveis");
		
		foreach (Animation anim in newFurniture.GetComponentsInChildren<Animation>()) {
			anim.Stop();
			anim.playAutomatically = false;
		}
		
		newFurniture.AddComponent<SnapBehaviour>();
		newFurniture.AddComponent<CalculateBounds>();

		if (newFurniture.GetComponent<Rigidbody> () == null)
			newFurniture.AddComponent<Rigidbody>();
		
		if (newFurniture.GetComponent<InformacoesMovel>().tipoMovel != TipoMovel.MOVEL)
		{
			newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | 
												 RigidbodyConstraints.FreezeRotation;
		} else {
			newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}
						
		newFurniture.GetComponent<InformacoesMovel>().Initialize();
		
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
	
		newFurniture.transform.position = nearestAvailableGround;
		#endregion
		
		#region colocar objeto virado para a câmera
		float yRotation  = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation.eulerAngles.y;
		
		Debug.Log("yRotation: " + yRotation);
		
		if(yRotation < 55 || yRotation > 325) {
			newFurniture.transform.eulerAngles = new Vector3(0,180,0);
		}
		else if(yRotation < 145 && yRotation > 55) {
			newFurniture.transform.eulerAngles = new Vector3(0,270,0);
		}
		else if(yRotation < 235 && yRotation > 145) {
			newFurniture.transform.eulerAngles = new Vector3(0,0,0);
		}
		else if(yRotation < 325 && yRotation > 235) {
			newFurniture.transform.eulerAngles = new Vector3(0,90,0);
		} else { Debug.LogError(" Something gone wrong! ");}
		#endregion
		
		newFurniture.transform.parent = MoveisGO.transform;
	}
}
