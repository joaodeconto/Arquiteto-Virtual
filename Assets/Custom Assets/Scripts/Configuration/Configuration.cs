using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using Visiorama.Utils;

public class Configuration : MonoBehaviour {

	private string testFilename = "teste.xml";

	public Texture2D[] availableFloorTextures;
	public Texture2D[] availableTopTextures;
	public GameObject[] availableLamps;
	public GameObject wall, floor;

	public List<string> PresetNames { get; private set; }
//	public List<ConfigurationPreset> Presets { get; private set; }

	private Dictionary<string, Texture2D> availableWallTextures;
	private Dictionary<string, Texture2D> availableFloorTexturesHash;

	public void Initialize()
	{
		PresetNames = new List<string>();
		PresetNames.Add("www.visiorama360.com.br/Telasul/Teste/Presets/configuracao1.xml");

		foreach(string filepath in PresetNames)
		{
			StartCoroutine("LoadStateWeb",filepath);
		}
	}

	#region UnityMethods
	void Start ()
	{
		Texture2D[] textures = GameObject.FindWithTag ("MainCamera").GetComponentInChildren<Painter>().wallTextures;

		availableWallTextures = new Dictionary<string, Texture2D>();
		foreach (Texture2D tex in textures)
		{
			availableWallTextures.Add (tex.name, tex);
		}

		availableFloorTexturesHash = new Dictionary<string, Texture2D>();
		foreach (Texture2D floorTex in availableFloorTextures)
		{
			availableFloorTexturesHash.Add (floorTex.name, floorTex);
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.C) &&
			Input.GetKeyDown (KeyCode.S))
		{
			//save scene
			SaveCurrentState(testFilename ,false);
			Debug.Log ("Salvou cena");
		}

		if (Input.GetKeyDown (KeyCode.C) &&
			Input.GetKeyDown (KeyCode.L)) {
			//load scene
			LoadState (testFilename, false);
		}
	}
	#endregion

	public bool SaveCurrentState(string path, bool isWeb)
	{
		/*List<GameObject> modules= new List<GameObject> (GameObject.FindGameObjectsWithTag ("Movel"));
		List<GameObject> floors	= new List<GameObject> (GameObject.FindGameObjectsWithTag ("Chao"));

		GameObject selectedMobile = GameObject.FindGameObjectWithTag ("MovelSelecionado");
		Transform trnsWallParent  = GameObject.Find ("ParentParede").transform;

		if (selectedMobile != null)
		{
			modules.Add(selectedMobile);
		}

		if (modules.Count == 0)
		{
			Debug.LogError ("Nao ha moveis na cena");
			return false;
		}

		bool someModuleHasTop = false;
		ConfigurationPreset cfgPreset  = new ConfigurationPreset();

		foreach(GameObject mobile in modules)
		{
			if (mobile.GetComponent<InformacoesMovel>().Categoria == "Extras")
			{
				cfgPreset.AddPreset(new PresetModuleData(mobile.transform.position,
														 mobile.transform.rotation,
														 mobile.name,
														 mobile.GetComponent<InformacoesMovel>().Categoria,
														 Line.CurrentLine.Name,
														 mobile.transform.GetComponentInChildren<Renderer>().materials[0].color));
			}
			else
			{
				cfgPreset.AddPreset(new PresetModuleData(mobile.transform.position,
														 mobile.transform.rotation,
														 mobile.name,
														 mobile.GetComponent<InformacoesMovel>().Categoria,
														 Line.CurrentLine.Name));

				#region Verificando a textura de top escolhida

				//Se não tiver guardado o nome da textura de tampo, a guarda
				if (cfgPreset.TopTextureName == "")
				{
					Renderer[] renders = mobile.GetComponentsInChildren<Renderer> ();

					Regex regexType = new Regex (@"Tampos.+", RegexOptions.IgnoreCase);

					foreach (Renderer r in renders)
					{
						if (regexType.Match (r.material.name).Success)
						{
							cfgPreset.TopTextureName = r.material.mainTexture.name;
						}
					}
				}
				#endregion
			}
		}

		foreach (Transform wall in trnsWallParent)
		{
			cfgPreset.AddPreset (new PresetWallData (wall.position,
													 wall.rotation,
													 wall.localScale,
													 wall.name,
													 wall.GetComponentInChildren<InfoWall>().color,
													 wall.GetComponentInChildren<InfoWall>().texture.name));
		}

		foreach (GameObject floor in floors)
		{
			cfgPreset.AddPreset(new PresetFloorData (floor.transform.position,
													 floor.transform.rotation,
													 floor.transform.localScale,
													 floor.transform.name,
													 floor.transform.GetComponentInChildren<Renderer>().materials[0].mainTexture.name));
		}


//		cfgPreset.GroundTextureIndex = 2;
		cfgPreset.RotationOfIllumination = new SerializableVec4( GameObject.Find("Sol").transform.rotation );
		cfgPreset.SetColor (Line.CurrentDetailColor);

//		System.IO.MemoryStream stream = new System.IO.MemoryStream ();
		System.IO.Stream stream = File.Open(path, FileMode.Create);
		BinaryFormatter bFormatter = new BinaryFormatter();
		bFormatter.Serialize(stream, cfgPreset);

//		byte[] b = stream.ToArray ();
//		string s = System.Text.Encoding.UTF8.GetString (b);
//		Debug.Log ("Serialized Objects: " + s.Length);

		stream.Close();

		/*

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

		/*//TODO remember me
		XmlNode floorNode = xmlDoc.CreateElement("piso");
		XmlAttribute floorIndexTextureAttr = xmlDoc.CreateAttribute("indicetextura");
		Transform parentTextureBtns = GameObject.Find("Lists").transform.FindChild ("View UI Piso").GetChild(0).GetChild(0);
		floorIndexTextureAttr.Value = parentTextureBtns.GetComponent<CatalogFloorButtonHandler> ().SelectedFloorIndex.ToString();
		floorNode.Attributes.Append(floorIndexTextureAttr);
		sceneNode.AppendChild(floorNode);

		Debug.LogError (xmlDoc.OuterXml);
		System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding ();

		FileUtils.WriteFile(path, enc.GetBytes(xmlDoc.OuterXml),true);
				*/
		return false;
	}

	public void LoadState(string path, bool isWeb)
	{
		/*ConfigurationPreset cfgPreset;
		Stream stream = File.Open (path, FileMode.Open);
		BinaryFormatter bFormatter = new BinaryFormatter ();
		cfgPreset = (ConfigurationPreset)bFormatter.Deserialize (stream);
		stream.Close();

		RunPreset (cfgPreset);

		/*
		if(isWeb){
			StartCoroutine("LoadStateWeb",path);
		} else {
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding ();
			Debug.LogError (enc.GetString (FileUtils.LoadFile(path,true)));
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(enc.GetString (FileUtils.LoadFile(path,true)));
			LoadState(xmlDoc);
		}*/
	}

	public void RunPreset(int index)
	{
		//RunPreset(Presets[index]);
	}

	/*private void RunPreset (ConfigurationPreset preset)
	{
		GameObject.Find("Sol").transform.rotation = preset.RotationOfIllumination.ToQuaternion ();

		int topTextureIndex    = -1;
		int selectedColorIndex = -1;

		if (preset.TopTextureName != "")
		{
			for (int i = 0; i != availableTopTextures.Length; ++i)
			{
				if (availableTopTextures[i].name == preset.TopTextureName)
				{
					topTextureIndex = i;
				}
			}
		}

		//verificando validade do índice da textura
		if (topTextureIndex == -1)
			topTextureIndex = 0;

		BrandColorEnum[] colors  = Line.CurrentLine.colors;
		BrandColorEnum brandEnumColor = BrandColor.GetEnumColor(preset.BrandDetailColor.ToColor());
		for(int i = 0; i != colors.Length; ++i)
		{
			if (brandEnumColor == colors[i])
			{
				selectedColorIndex = i;
			}
		}

		GameObject moveisGO = GameObject.Find ("Moveis GO");
		if (moveisGO == null)
		{
			moveisGO = new GameObject ("Moveis GO");
		}

		GameObject extras = GameObject
								.Find ("View UI Extras")
									.GetComponentInChildren<CatalogExtrasButtonHandler>().extras;

		foreach (PresetFloorData data in preset.PresetDataFloors)
		{
			GameObject cFloor = Instantiate (floor,
										    data.Position.ToVector3 (),
										  	data.Rotation.ToQuaternion ()) as GameObject;

			if (data.Name.LastIndexOf ("(") != -1) {//Retirar o "(Clone)" de trás do objeto
				data.Name = data.Name.Substring (0, data.Name.LastIndexOf ("("));
			}

			cFloor.name = data.Name;
			cFloor.transform.localScale = data.Scale.ToVector3 ();
			cFloor.transform.parent = GameObject.Find ("ParentChao").transform;

			Renderer r = cFloor.transform.GetComponentInChildren<Renderer> ();
			r.materials [0].color = data.color.ToColor ();
			r.materials [0].mainTexture = availableFloorTexturesHash [data.TextureName];//Utilizado Hash para melhor performance e entendimento do code :D

			MaterialUtils.ResizeWallMaterial (cFloor,
											  data.Scale.x,
											  data.Scale.y);
		}

		foreach ( PresetWallData data in preset.PresetDataWalls)
		{
			GameObject cWall = Instantiate (wall,
										    data.Position.ToVector3 (),
										  	data.Rotation.ToQuaternion ()) as GameObject;

			if (data.Name.LastIndexOf ("(") != -1) {//Retirar o "(Clone)" de trás do objeto
				data.Name = data.Name.Substring (0, data.Name.LastIndexOf ("("));
			}

			cWall.name = data.Name;
			cWall.transform.localScale = data.Scale.ToVector3 ();
			cWall.transform.parent 	   = GameObject.Find ("ParentParede").transform;

			Renderer r = cWall.transform.GetComponentInChildren<Renderer> ();
			r.materials [0].color 		= data.color.ToColor ();
			r.materials [0].mainTexture = availableWallTextures [data.TextureName];//Utilizado Hash para melhor performance e entendimento do code :D

			MaterialUtils.ResizeWallMaterial (cWall,
											  data.Scale.x,
											  data.Scale.y);
		}

		foreach(PresetModuleData data in preset.PresetDataModules)
		{
			GameObject module = null;
			if (data.CategoryName == "Extras")
			{
				foreach (Transform extraModule in extras.transform)
				{
					if (data.Name.LastIndexOf ("(") != -1) {//Retirar o "(Clone)" de trás do objeto
						data.Name = data.Name.Substring (0, data.Name.LastIndexOf ("("));
					}

					if (data.Name == extraModule.name)
					{
						module = Instantiate (extraModule.gameObject,
											  data.Position.ToVector3 (),
											  data.Rotation.ToQuaternion ()) as GameObject;

						module.GetComponentInChildren<Renderer>().materials[0].color = data.color.ToColor ();

						if (module.rigidbody == null)
							module.AddComponent<Rigidbody> ();

						module.GetComponent<InformacoesMovel> ().Initialize ();
						break;
					}
				}
			}
			else if (data.CategoryName == "")
			{
				foreach (GameObject lamp in availableLamps)
				{
					if (lamp.name == data.Name)
					{
						module = Instantiate (lamp,
											  data.Position.ToVector3 (),
											  data.Rotation.ToQuaternion ()) as GameObject;

						if (module.rigidbody == null)
							module.AddComponent<Rigidbody> ();

						module.GetComponent<InformacoesMovel> ().Initialize ();
					}
				}
			}
			else
			{
				module = Instantiate (Line.Lines [0]//getBrandId (data.BrandName)]
										.categories [getCategoryId (data.CategoryName, data.BrandName)]
											.Furniture [getMobileId (data.Name, data.CategoryName, data.BrandName)],
									  data.Position.ToVector3 (),
									  data.Rotation.ToQuaternion ()) as GameObject;

				if (module.rigidbody == null)
					module.AddComponent<Rigidbody> ();

				module.GetComponent<InformacoesMovel> ().Initialize ();
				module.GetComponent<InformacoesMovel> ().ChangeDetailColor (selectedColorIndex);
			}

			module.layer = LayerMask.NameToLayer ("Moveis");
			module.tag = "Movel";
			module.AddComponent<SnapBehaviour> ();
			module.AddComponent<CalculateBounds> ();
			module.GetComponent<InformacoesMovel> ().Categoria = data.CategoryName;
			module.GetComponent<InformacoesMovel> ().ChangeTexture (availableTopTextures[topTextureIndex], "Tampos");

			foreach (Animation anim in module.GetComponentsInChildren<Animation>()) {
				anim.Stop ();
				anim.playAutomatically = false;
			}


			if (module.GetComponent<InformacoesMovel> ().tipoMovel != TipoMovel.MOVEL)
			{
				module.rigidbody.constraints = RigidbodyConstraints.FreezePositionY
											 | RigidbodyConstraints.FreezeRotation;
			}
			else
			{
				module.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}

			module.transform.parent = moveisGO.transform;
		}
	}

	private int getMobileId	  (string mobileName, string categoryName, string brandName)
	{
		int index = 0;
		if (mobileName.LastIndexOf("(") != -1)//Retirar o "(Clone)" de trás do objeto
		{
			mobileName = mobileName.Substring (0, mobileName.LastIndexOf ("("));
		}

		Category category = Line.Lines[getBrandId(brandName)]
									.categories [getCategoryId (categoryName, brandName)];

		foreach (GameObject cMobile in category.Furniture)
		{
			if (cMobile.name.Equals (mobileName))
			{
				break;
			}
			++index;
		}
		return index;
	}
	private int getCategoryId (string categoryName, string brandName)
	{
		int index = 0;
		foreach (Category category in Line.Lines[getBrandId(brandName)].categories)
		{
			if (category.Name.Equals (categoryName))
			{
				break;
			}
			++index;
		}
		return index;
	}
	private int getBrandId 	  (string brandName)
	{
		int index = 0;
		foreach (Line line in Line.Lines)
		{
			if (line.Name.Equals (brandName))
			{
				break;
			}
			++index;
		}
		return 0;
	}

	private int getMobileId   (GameObject mobile)
	{
		Category category = Line.CurrentLine.categories [getCategoryId (mobile)];
		int index = 0;

		foreach (GameObject cMobile in category.Furniture) {
			if (cMobile.name.Equals (mobile.name.Substring (0, mobile.name.LastIndexOf ("(")))) {//Retirar o "(Clone)" de trás do objeto
				break;
			}
			++index;
		}
//		Debug.LogError ("index: " + index);
		return index;
	}
	private int getCategoryId (GameObject mobile)
	{

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
	private int getBrandId	  (GameObject mobile)
	{
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

	private IEnumerator LoadStateWeb(string path)
	{
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

	/*
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
	*/
}
