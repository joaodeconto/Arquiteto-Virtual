using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class OptimizedCombineChildrenEditor : EditorWindow {

	/// Usually rendering with triangle strips is faster.
	/// However when combining objects with very low triangle counts, it can be faster to use triangles.
	/// Best is to try out which value is faster in practice.
	public TextureAtlasInfo textureAtlasProperties;// = new TextureCombineUtility.TextureAtlasInfo();
	
	SerializedObject m_Object;
	SerializedProperty m_Property;
	
	[MenuItem("BlackBugio/Create Utils/Combine Children Mesh")]
	static void Init ()
	{
		OptimizedCombineChildrenEditor window = 
			(OptimizedCombineChildrenEditor)EditorWindow.GetWindow (typeof (OptimizedCombineChildrenEditor));
		
		window.textureAtlasProperties = new TextureAtlasInfo();
		
//		m_Object = new SerializedObject(window.textureAtlasProperties);
	}
	
	void OnGUI ()
	{
		textureAtlasProperties.anisoLevel = EditorGUILayout.IntField("Ansio Level:", textureAtlasProperties.anisoLevel);
		textureAtlasProperties.compressTexturesInMemory = EditorGUILayout.Toggle("Compress Texture?", textureAtlasProperties.compressTexturesInMemory);
		textureAtlasProperties.filterMode = (FilterMode) EditorGUILayout.EnumPopup ("Filter Mode:", (System.Enum) textureAtlasProperties.filterMode);
		textureAtlasProperties.ignoreAlpha = EditorGUILayout.Toggle("Ignore Alpha?", textureAtlasProperties.ignoreAlpha);
		textureAtlasProperties.wrapMode = (TextureWrapMode) EditorGUILayout.EnumPopup ("Wrap Mode:", (System.Enum) textureAtlasProperties.wrapMode);
		
		EditorGUILayout.Space ();
		
		EditorGUILayout.LabelField ("Shader Porperties To Look For:");
		if (GUILayout.Button ("Add Shader")) textureAtlasProperties.shaderPropertiesToLookFor.Add(new ShaderProperties(false, ""));
		for (int i = 0; i != textureAtlasProperties.shaderPropertiesToLookFor.Count; i++)
		{
			ShaderProperties sp = textureAtlasProperties.shaderPropertiesToLookFor[i];
			
			EditorGUILayout.BeginHorizontal ("box");
			EditorGUILayout.BeginVertical ();
			sp.propertyName = EditorGUILayout.TextField("Property Name:", sp.propertyName);
			sp.markAsNormal = EditorGUILayout.Toggle("Mark as Normal?", sp.markAsNormal);
			EditorGUILayout.EndVertical ();
			if (GUILayout.Button ("Remove"))
			{
				textureAtlasProperties.shaderPropertiesToLookFor.Remove(sp);
				i--;
			}
			EditorGUILayout.EndHorizontal ();
		}
		
		EditorGUILayout.Space ();
		
		if (Selection.transforms.Length != 0)
		{
//			EditorGUILayout.LabelField ("Selected objects:");
//			foreach (Transform t in Selection.transforms)
//			{
//				EditorGUILayout.LabelField (t.name);
//			}
//			
//			EditorGUILayout.Space ();
//			EditorGUILayout.Space ();
//			
//			GUI.backgroundColor = Color.green;
//			if (GUILayout.Button ("Combine"))
//			{
//				foreach (Transform t in Selection.transforms)
//				{
//					Combine (t);
//				}
//			}
//			GUI.backgroundColor = Color.white;
			
			EditorGUILayout.LabelField ("Selected object:");
			EditorGUILayout.LabelField (Selection.transforms[0].name);
			
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button ("Combine"))
			{
				Combine (Selection.transforms[0]);
			}
			GUI.backgroundColor = Color.white;
		}
		else
		{
			EditorGUILayout.Space ();
			GUI.color = Color.red;
			EditorGUILayout.LabelField ("Don't have selected objects");
			GUI.color = Color.white;
		}
	}
	
	public void Combine (Transform t)
	{
		MeshFilter[] filters = t.GetComponentsInChildren<MeshFilter>();
		Matrix4x4 myTransform = t.worldToLocalMatrix;
		
		Dictionary<string, Dictionary<Material, List<MeshCombineUtilityDCM.MeshInstance>>> allMeshesAndMaterials = new Dictionary<string, Dictionary<Material, List<MeshCombineUtilityDCM.MeshInstance>>>();
		for(int i = 0; i < filters.Length; i++)
		{
			Renderer curRenderer = filters[i].renderer;
			MeshCombineUtilityDCM.MeshInstance instance = new MeshCombineUtilityDCM.MeshInstance();
			
			instance.mesh = filters[i].mesh;
			
			if(curRenderer != null && curRenderer.enabled && instance.mesh != null)
			{
				instance.transform = myTransform * filters[i].transform.localToWorldMatrix;
				
				Material[] materials = curRenderer.sharedMaterials;
				for(int m = 0; m < materials.Length; m++)
				{
					instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);
					
					if(!allMeshesAndMaterials.ContainsKey(materials[m].shader.ToString()))
					{
						allMeshesAndMaterials.Add(materials[m].shader.ToString(), new Dictionary<Material, List<MeshCombineUtilityDCM.MeshInstance>>());
					}

					if(!allMeshesAndMaterials[materials[m].shader.ToString()].ContainsKey(materials[m]))
					{
						allMeshesAndMaterials[materials[m].shader.ToString()].Add(materials[m], new List<MeshCombineUtilityDCM.MeshInstance>());
					}
					
					allMeshesAndMaterials[materials[m].shader.ToString()][materials[m]].Add(instance);
				}
			}
		}
		
		foreach(KeyValuePair<string, Dictionary<Material, List<MeshCombineUtilityDCM.MeshInstance>>>  firstPass in allMeshesAndMaterials)
		{
			Material[] allMaterialTextures = new Material[firstPass.Value.Keys.Count];
			int index = 0;
								
			foreach(KeyValuePair<Material, List<MeshCombineUtilityDCM.MeshInstance>> kv in firstPass.Value)
			{
				allMaterialTextures[index] = kv.Key;
				index++;
			}
			
			TextureCombineUtility.TexturePosition[] textureUVPositions;
			Material combined = TextureCombineUtility.combine(allMaterialTextures, out textureUVPositions, textureAtlasProperties);
			
			if(textureUVPositions != null)
			{
			
				List<MeshCombineUtilityDCM.MeshInstance> meshIntermediates = new List<MeshCombineUtilityDCM.MeshInstance>();
				foreach(KeyValuePair<Material, List<MeshCombineUtilityDCM.MeshInstance>> kv in firstPass.Value)
				{
					TextureCombineUtility.TexturePosition refTexture = textureUVPositions[0];
					
					for(int i = 0; i < textureUVPositions.Length; i++)
					{
						if(kv.Key.mainTexture.name == textureUVPositions[i].textures[0].name)
						{
							refTexture = textureUVPositions[i];										
							break;
						}
					}	
				
					for(int i = 0; i < kv.Value.Count; i++)
					{				
						Vector2[] uvCopy = kv.Value[i].mesh.uv;
					
						for(int j = 0; j < uvCopy.Length; j++)
						{
							uvCopy[j].x = refTexture.position.x + uvCopy[j].x * refTexture.position.width;
							uvCopy[j].y = refTexture.position.y + uvCopy[j].y * refTexture.position.height;
						}
					
						kv.Value[i].mesh.uv = uvCopy;				
					

						uvCopy = kv.Value[i].mesh.uv1;
						for(int j = 0; j < uvCopy.Length; j++)
						{
							uvCopy[j].x = refTexture.position.x + uvCopy[j].x * refTexture.position.width;
							uvCopy[j].y = refTexture.position.y + uvCopy[j].y * refTexture.position.height;
						}					
					
						kv.Value[i].mesh.uv1 = uvCopy;
					
					
	
						uvCopy = kv.Value[i].mesh.uv2;
						for(int j = 0; j < uvCopy.Length; j++)
						{
							uvCopy[j].x = refTexture.position.x + uvCopy[j].x * refTexture.position.width;
							uvCopy[j].y = refTexture.position.y + uvCopy[j].y * refTexture.position.height;
						}					
						
						kv.Value[i].mesh.uv2 = uvCopy;
					
					
						meshIntermediates.Add(kv.Value[i]);
					}					
				}
			
				Material mat = combined;
				
				Mesh[] combinedMeshes = MeshCombineUtilityDCM.Combine(meshIntermediates.ToArray());
			
				GameObject parent = new GameObject("Combined " + t.gameObject.name + " " + firstPass.Key + " Mesh Parent");
				parent.transform.position = t.position;
				parent.transform.rotation = t.rotation;
	
				for(int i = 0; i < combinedMeshes.Length; i++)
				{
					GameObject go = new GameObject("Combined " + t.gameObject.name + " Mesh");
					go.transform.parent = parent.transform;
					go.tag = t.gameObject.tag;
					go.layer = t.gameObject.layer;
					go.transform.localScale = Vector3.one;
					go.transform.localRotation = Quaternion.identity;
					go.transform.localPosition = Vector3.zero;
					MeshFilter filter = go.AddComponent<MeshFilter>();
					go.AddComponent<MeshRenderer>();
					go.renderer.sharedMaterial = mat;

					filter.mesh = combinedMeshes[i];
				}
				
				if (t.parent != null) parent.transform.parent = t.parent.transform;
				parent.transform.localScale = Vector3.one;
			}
		}
		//Destroy(t.gameObject);
		
		ClearLog ();
	}
	
	void ClearLog ()
	{
	    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
	    System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
	    System.Reflection.MethodInfo method = type.GetMethod("Clear");
		method.Invoke (new Object (), null);
	}
}
