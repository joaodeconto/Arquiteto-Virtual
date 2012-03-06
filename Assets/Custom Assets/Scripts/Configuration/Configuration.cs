using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

using Visiorama.Utils;

[System.Serializable]
public class PresetMobileData {
	public Vector3 Position 	{ get; private set; }
	public Quaternion Rotation 	{ get; private set; }
	
	public int Id { get; private set;}
	public int CategoryId { get; private set; }
	public int BrandId 	 { get; private set; }
	public PresetMobileData(Vector3 position, Quaternion rotation, int id, int categoryId, int brandId){
		this.Position = position;
		this.Rotation = Rotation;
		this.Id 		= id;
		this.CategoryId	= categoryId;
		this.BrandId	= brandId;
	}
}

public class ConfigurationPreset {
	public Texture2D image { get; set; }
	public List<PresetMobileData> PresetDataMobiles { get; set; }
	public Color WallCeilColor { get; set; }
	public int WallCeilHueColor { get; set; }
	public int GroundTextureIndex { get; set; }
}

public class Configuration : MonoBehaviour {
	
	public List<string> PresetNames { get; private set; }
	public List<ConfigurationPreset> Presets { get; private set; } 
	
	public void Initialize(){
		
//		System.Xml.Serialization.XmlSerializer
		
		PresetNames = new List<string>();
		PresetNames.Add("www.visiorama360.com.br/Telasul/Teste/Presets/configuracao1.xml");
		
		foreach(string filepath in PresetNames){
			StartCoroutine("LoadStateWeb",filepath);
		}	
	}
	
	public bool SaveCurrentState(string path, bool isWeb){
				
		GameObject[] mobiles = GameObject.FindGameObjectsWithTag("Movel");
		
		if(mobiles == null || mobiles.Length == 0){
			Debug.LogError ("Nao ha moveis na cena");
			return false;
		}
		
		XmlDocument xmlDoc = new XmlDocument ();
		
		XmlNode docNode = xmlDoc.CreateXmlDeclaration ("1.0", "UTF-8", null);
		xmlDoc.AppendChild (docNode);
		
		//Nodo principal
		XmlNode rootNode = xmlDoc.CreateElement ("configuracao");
		xmlDoc.AppendChild (rootNode);
		
		//Nodo móveis
		XmlNode furnitureNode = xmlDoc.CreateElement ("moveis");
		rootNode.AppendChild(furnitureNode);
		//Acrescentando móveis da cena
		foreach(GameObject mobile in mobiles){
			
			XmlNode mobileNode = xmlDoc.CreateElement ("movel");
			
			XmlAttribute idAttr 		= xmlDoc.CreateAttribute("id");
			XmlAttribute idCategoryAttr = xmlDoc.CreateAttribute("idcategoria");
			XmlAttribute idBrandAttr 	= xmlDoc.CreateAttribute("idlinha");
			XmlAttribute posAttr		= xmlDoc.CreateAttribute("pos");
			XmlAttribute rotAttr		= xmlDoc.CreateAttribute("rot");
			Vector3 position = mobile.transform.position;
			
			idAttr.Value 		= getMobileId(mobile).ToString();
			idCategoryAttr.Value= getCategoryId(mobile).ToString();
			idBrandAttr.Value	= getBrandId(mobile).ToString();
			posAttr.Value		= position.x + "x" + position.y + "x" + position.z;
			rotAttr.Value		= mobile.transform.rotation.eulerAngles.y.ToString();
			
			mobileNode.Attributes.Append (idAttr);
			mobileNode.Attributes.Append (idCategoryAttr);
			mobileNode.Attributes.Append (idBrandAttr);
			mobileNode.Attributes.Append (posAttr);
			mobileNode.Attributes.Append (rotAttr);
			
			furnitureNode.AppendChild(mobileNode);
		
		}
		
		//Adicionando nodo cena
		XmlNode sceneNode 		= xmlDoc.CreateElement ("cena");
		rootNode.AppendChild (sceneNode);
		
		//Cor cena		
		XmlNode wallCeilingNode	= xmlDoc.CreateElement ("parede-teto");
		
		//Cor das paredes
		/*Color wallCeilingColor = wallCeilingColorPicker.getColorRGB();
		XmlAttribute wallCeilingAttr = xmlDoc.CreateAttribute("color");
		wallCeilingAttr.Value =	wallCeilingColorPicker.getColorRGB().r + "x" +
								wallCeilingColorPicker.getColorRGB().g + "x" +
								wallCeilingColorPicker.getColorRGB().b;
									 
		wallCeilingNode.Attributes.Append(wallCeilingAttr);
		
		wallCeilingAttr = xmlDoc.CreateAttribute("saturation");
		wallCeilingAttr.Value = wallCeilingColorPicker.getSaturation().ToString();
		wallCeilingNode.Attributes.Append(wallCeilingAttr);
		sceneNode.AppendChild(wallCeilingNode);
		*/
				
		XmlNode floorNode = xmlDoc.CreateElement("piso");
		XmlAttribute floorIndexTextureAttr = xmlDoc.CreateAttribute("indicetextura");
		floorIndexTextureAttr.Value = GameObject.Find("GUI").GetComponent<GuiCatalogo>().CurrentTextureIndex.ToString();
		floorNode.Attributes.Append(floorIndexTextureAttr);
		sceneNode.AppendChild(floorNode);
		
		Debug.LogError (xmlDoc.OuterXml);	
		System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding ();
		
		FileUtils.WriteFile(path, enc.GetBytes(xmlDoc.OuterXml),true);
				
		return false;
	}
	
	public void LoadState(string path, bool isWeb){
			
		if(isWeb){
			StartCoroutine("LoadStateWeb",path);
		} else {
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding ();
			Debug.LogError (enc.GetString (FileUtils.LoadFile(path,true)));
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(enc.GetString (FileUtils.LoadFile(path,true)));
			LoadState(xmlDoc);
		}
	}
	
	public void RunPreset(int index){
		foreach(PresetMobileData data in Presets[index].PresetDataMobiles){
		
			GameObject mobile = Instantiate(Line.Lines[data.BrandId].categories[data.CategoryId].Furniture[data.Id],data.Position,data.Rotation) as GameObject;
		
			mobile.AddComponent<SnapBehaviour> ();
			mobile.AddComponent<CalculateBounds> ();
			mobile.GetComponent<InformacoesMovel> ().Initialize ();
			mobile.GetComponent<InformacoesMovel> ().Categoria = Line.CurrentLine.categories[data.CategoryId].Name;
			mobile.AddComponent<Rigidbody> ();
			if (mobile.GetComponent<InformacoesMovel> ().tipoMovel == TipoMovel.FIXO) {
				mobile.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
			} else {
				mobile.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
			
			GameObject MoveisGO = GameObject.Find ("Moveis GO");
			
			if (MoveisGO == null) {
				MoveisGO = new GameObject ("Moveis GO");
			}
		}
	}
	
	private int getMobileId(GameObject mobile){
		
		Category category = Line.CurrentLine.categories[getCategoryId(mobile)];
		int index = 0;
		
		foreach(GameObject cMobile in category.Furniture){
			if(cMobile.name.Equals(mobile.name.Substring(0,mobile.name.LastIndexOf("(")))){//Retirar o "(Clone)" de trás do objeto
				break;
			}
			++index;
		}
		Debug.LogError ("index: " + index);
		return index;
	}
	
	private int getCategoryId(GameObject mobile){
		
		string categoryName = mobile.GetComponent<InformacoesMovel>().Categoria;
		int index = 0;
		
		foreach(Category category in Line.CurrentLine.categories){
			if(category.Name.Equals(categoryName)){
				break;
			}
			++index;
		}
		return index;
	}
	
	private int getBrandId(GameObject mobile){
		
		Line cLine = Line.CurrentLine;
		int index = 0;
		
		foreach(Line line in Line.Lines){
			if(line.Name.Equals(cLine.Name)){
				break;
			}
			++index;
		}
		return index;
	}
	
	private IEnumerator LoadStateWeb(string path){
	
		WWW www;
				
		XmlDocument xmlDoc;
		
		www = new WWW(path);
		
		yield return www;
			
		if (www.error != null){
			Debug.Log("Nao pode baixar o arquivo de configuracao.");
			Debug.Log(www.error);
		}
		
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(www.text);
	
	}
		
	private void LoadState(XmlDocument xml){
		
		XmlNode rootNode;
		ConfigurationPreset preset;
		List<PresetMobileData> presetDataMobiles;
		PresetMobileData tmpPresetMobileData;
		Vector3 tmpVec3;
		Quaternion tmpQuaternion;
		string[] splitedStr;

		rootNode = xml.GetElementsByTagName("configuracao")[0];
		
		preset			 = new ConfigurationPreset();
		presetDataMobiles= new List<PresetMobileData>();
		preset.PresetDataMobiles = presetDataMobiles;
		
		foreach(XmlNode child in rootNode.ChildNodes){
			if(child.Name.Equals("moveis")){
				foreach(XmlNode mobileNode in child.ChildNodes){
					//<movel id="0" idcategoria="0" idlinha="0" pos="0.0x0.0x0.0" rot="0.0" />
					splitedStr = mobileNode.Attributes["pos"].Value.Split('x');
					
					tmpVec3 	  = new Vector3(float.Parse(splitedStr[0]),float.Parse(splitedStr[1]),float.Parse(splitedStr[2]));
					tmpQuaternion = Quaternion.Euler(0,float.Parse(mobileNode.Attributes["rot"].Value),0);
					
					tmpPresetMobileData = new PresetMobileData(	tmpVec3,
																tmpQuaternion,
																int.Parse(mobileNode.Attributes["id"].Value),
																int.Parse(mobileNode.Attributes["idcategoria"].Value),
																int.Parse(mobileNode.Attributes["idlinha"].Value));
																
					presetDataMobiles.Add(tmpPresetMobileData);
				}
			}
			
			if(child.Name.Equals("cena")){
				foreach(XmlNode roomConfigNode in child.ChildNodes){
					if(roomConfigNode.Name.Equals("parede-teto")){
					
						//<parede-teto r="255" g="255" b="255" hue="0" />
						splitedStr = roomConfigNode.Attributes["color"].Value.Split('x'); 
						
						preset.WallCeilColor = new Color(float.Parse(splitedStr[0]),
														 float.Parse(splitedStr[1]),
														 float.Parse(splitedStr[2]));
														 
						preset.WallCeilHueColor = int.Parse(roomConfigNode.Attributes["saturation"].Value);
					} else if(roomConfigNode.Name.Equals("piso")){
						//<piso indicetextura="0" />
						preset.GroundTextureIndex = int.Parse(roomConfigNode.Attributes["indicetextura"].Value);
					}
				}
			}
		}
		if(Presets == null){
			Presets = new List<ConfigurationPreset>();
		}
		Presets.Add(preset);		
	}
		
}
