using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable()]
public class ConfigurationPreset : ISerializable
{
	public Texture2D image { get; set; }

	public List<PresetFloorData> PresetDataFloors { get; private set; }
	public List<PresetWallData> PresetDataWalls { get; private set; }
	public List<PresetModuleData> PresetDataModules { get; private set; }
	public int GroundTextureIndex { get; set; }
	public string TopTextureName { get; set; }
	public SerializableVec4 RotationOfIllumination { get; set; }
	public SerializableVec4 BrandDetailColor { get; private set; }

	public ConfigurationPreset ()
	{
		PresetDataModules = new List<PresetModuleData> ();
		PresetDataWalls   = new List<PresetWallData> ();
		PresetDataFloors  = new List<PresetFloorData> ();
		TopTextureName = "";
	}

	public void AddPreset (PresetFloorData data)
	{
		PresetDataFloors.Add (data);
	}
	public void AddPreset(PresetWallData data)
	{
		PresetDataWalls.Add (data);
	}
	public void AddPreset (PresetModuleData data)
	{
		PresetDataModules.Add (data);
	}

	public void SetColor (Color color)
	{
		BrandDetailColor = new SerializableVec4(color);
	}

	public ConfigurationPreset (SerializationInfo info, StreamingContext ctxt)
	{
		//TODO image
		this.BrandDetailColor 		= (SerializableVec4)info.GetValue ("BrandDetailColor",  typeof(SerializableVec4));
		this.TopTextureName 		= (string)info.GetValue ("TopTextureName", typeof(string));
		this.GroundTextureIndex 	= (int)info.GetValue ("GroundTextureIndex", typeof(int));
		this.RotationOfIllumination	= (SerializableVec4)info.GetValue ("RotationOfIllumination", typeof(SerializableVec4));
		this.PresetDataFloors		= (List<PresetFloorData>)info.GetValue  ("PresetDataFloors", typeof(List<PresetFloorData>));
		this.PresetDataModules 		= (List<PresetModuleData>)info.GetValue ("PresetDataModules", typeof(List<PresetModuleData>));
		this.PresetDataWalls  		= (List<PresetWallData>)info.GetValue ("PresetDataWalls", typeof(List<PresetWallData>));
	}

	#region ISerializable implementation
	void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
	{
//		info.AddValue("WallCeilColor", this.WallCeilColor);
		info.AddValue ("BrandDetailColor",  this.BrandDetailColor);
		info.AddValue ("TopTextureName", 	this.TopTextureName);
		info.AddValue ("GroundTextureIndex",this.GroundTextureIndex);
		info.AddValue ("PresetDataFloors",  this.PresetDataFloors);
		info.AddValue ("PresetDataModules", this.PresetDataModules);
		info.AddValue ("PresetDataWalls", 	this.PresetDataWalls);
		info.AddValue ("RotationOfIllumination", this.RotationOfIllumination);
	}
	#endregion
}
