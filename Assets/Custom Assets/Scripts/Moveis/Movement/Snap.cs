using UnityEngine;
using System.Collections;

public class Snap : MonoBehaviour {
	
	private Camera mainCamera;
	
	void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera").camera;
	}
	
	void OnMouseDown () {
		Bounds b = GetComponentInChildren<MeshFilter>().mesh.bounds;
        Vector3 offset = -1 * b.center;
        p = last_p = new Vector3(offset.x / b.extents.x, offset.y / b.extents.y, offset.z / b.extents.z);
	}
	
	Vector3 p; //Pivot value -1..1, calculated from Mesh bounds
    Vector3 last_p; //Last used pivot
	RaycastHit hit;
	void OnMouseDrag () {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Movel"))){
			if( p != last_p &&
				MinMax(p.x, -1f, 1f) &&
				MinMax(p.y, -1f, 1f) &&
				MinMax(p.z, -1f, 1f)) { 
				//Detects user input on any of the three sliders
                UpdatePivot(hit.transform.GetComponentInChildren<MeshFilter>().mesh);
            }
			else {
	       		transform.position = (hit.point.x * transform.parent.right)	+ 
									 (hit.point.y * transform.parent.up)	+ 
									 (hit.point.z * transform.parent.forward);
			}
		}	
	}
	
	bool MinMax (float value, float min, float max)
	{
	    return (value > min) && (value < max);
	}
	
	void UpdatePivot (Mesh mesh) {
		Vector3 diff = Vector3.Scale(mesh.bounds.extents, last_p - p); //Calculate difference in 3d position
    	transform.position += Vector3.Scale(diff, transform.localScale); //Move object position
        last_p = p;
		if(collider) {
            if(collider is BoxCollider) {
                ((BoxCollider) collider).center += diff;
            } else if(collider is CapsuleCollider) {
                ((CapsuleCollider) collider).center += diff;
            } else if(collider is SphereCollider) {
                ((SphereCollider) collider).center += diff;
            }
        }
	}
	
	void OnMouseUp() {
		
	}
}
