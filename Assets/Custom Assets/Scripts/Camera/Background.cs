using UnityEngine;

public class Background : MonoBehaviour {
    
	public Texture2D image;
    public int gradientLayer = 7;

    void Awake () { 
        gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);
        if (!camera) {
            Debug.LogError ("Must attach Background script to the camera");
            return;
        }
		if (image == null) {
            Debug.LogError ("Must attach image to the Background script");
            return;
		}
		
        camera.clearFlags = CameraClearFlags.Depth;
        camera.cullingMask = camera.cullingMask & ~(1 << gradientLayer);
        Camera backgroundCam = new GameObject("CameraBackground",typeof(Camera)).camera;
        backgroundCam.depth = camera.depth-1;
        backgroundCam.cullingMask = 1 << gradientLayer;
		backgroundCam.backgroundColor = new Color(0, 0, 0.275f);
		
		GameObject backgroundPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		Destroy(backgroundPlane.collider);
		backgroundPlane.name = "Background";

		Material mat = new Material(Shader.Find("Diffuse"));
		mat.mainTexture = image;
		mat.color = Color.white;
        backgroundPlane.renderer.material = mat;
        backgroundPlane.layer = gradientLayer;
		backgroundPlane.transform.localPosition = new Vector3(backgroundPlane.transform.localPosition.x, 
		backgroundPlane.transform.localPosition.y, 
		backgroundPlane.transform.localPosition.z + 8.7f);
		backgroundPlane.transform.localEulerAngles = new Vector3(90, 180, 0);
		float sizeX = ((float)Screen.width / (float)Screen.height);
		sizeX = sizeX + (sizeX / (float)Screen.height);
		print(sizeX + " : " + Screen.width + " : " + Screen.height);
		backgroundPlane.transform.localScale = new Vector3(sizeX, 1, 1
			);
		backgroundPlane.transform.parent = backgroundCam.transform;
		backgroundCam.transform.position = new Vector3(1000f, 1000f, 1000f);
		backgroundCam.transform.parent = camera.transform;
    }
}