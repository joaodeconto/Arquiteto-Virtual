using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable()]
public enum SerializableTypeTop
{
	WithTop,
	WithoutTop,
	Cooktop,
	Sink,
	CannotUseTop,
}

[System.Serializable()]
public class SerializableVec4 : ISerializable
{
	public float x, y, z, w;

	public SerializableVec4 (Quaternion quat)
	{
		this.x = quat.x;
		this.y = quat.y;
		this.z = quat.z;
		this.w = quat.w;
	}

	public SerializableVec4 (Color nColor)
	{
		this.x = nColor.r;
		this.y = nColor.g;
		this.z = nColor.b;
		this.w = nColor.a;
	}

	public SerializableVec4 (float x, float y, float z, float w)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}

	public SerializableVec4 (SerializationInfo info, StreamingContext ctxt)
	{
		this.x = (float)info.GetValue ("x", typeof(float));
		this.y = (float)info.GetValue ("y", typeof(float));
		this.z = (float)info.GetValue ("z", typeof(float));
		this.w = (float)info.GetValue ("w", typeof(float));
	}

	#region ISerializable implementation
	void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
	{
		info.AddValue ("x", this.x);
		info.AddValue ("y", this.y);
		info.AddValue ("z", this.z);
		info.AddValue ("w", this.w);
	}
	#endregion

	public Quaternion ToQuaternion ()
	{
		return new Quaternion (x, y, z, w);
	}

	public Color ToColor ()
	{
		return new Color (x, y, z, w);
	}

	public override string ToString ()
	{
		return 	" x: " + x +
				" y: " + y +
				" z: " + z +
				" w: " + w;
	}
}

[System.Serializable()]
public class SerializableVec3 : ISerializable
{
	public float x, y, z;

	public SerializableVec3 (Vector3 vec)
	{
		this.x = vec.x;
		this.y = vec.y;
		this.z = vec.z;
	}

	public SerializableVec3 (float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public SerializableVec3 (SerializationInfo info, StreamingContext ctxt)
	{
		this.x = (float)info.GetValue ("x", typeof(float));
		this.y = (float)info.GetValue ("y", typeof(float));
		this.z = (float)info.GetValue ("z", typeof(float));
	}

	#region ISerializable implementation
	void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
	{
		info.AddValue ("x", this.x);
		info.AddValue ("y", this.y);
		info.AddValue ("z", this.z);
	}
	#endregion

	public Vector3 ToVector3 ()
	{
		return new Vector3 (x, y, z);
	}

	public override string ToString ()
	{
		return 	" x: " + x +
				" y: " + y +
				" z: " + z;
	}
}

[System.Serializable()]
public class PresetObjectData : ISerializable
{
	public SerializableVec3 Position { get; protected set; }
	public SerializableVec4 Rotation { get; protected set; }
	public SerializableVec4 color { get; protected set; }

	public string Name { get; set; }

	protected PresetObjectData()
	{
	}

	public PresetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
		this.Position = (SerializableVec3)info.GetValue ("Position", typeof(SerializableVec3));
		this.Rotation = (SerializableVec4)info.GetValue ("Rotation", typeof(SerializableVec4));
		this.Name  	  = (string)info.GetValue ("Name", typeof(string));
		this.color 	  = (SerializableVec4)info.GetValue ("color", typeof(SerializableVec4));
	}

	#region ISerializable implementation
	void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
	{
		AddSerializationData (info);
	}

	protected void AddSerializationData (SerializationInfo info)
	{
		info.AddValue ("Position", this.Position);
		info.AddValue ("Rotation", this.Rotation);
		info.AddValue ("Name", this.Name);
		info.AddValue ("color", this.color);
	}
	#endregion
}

[System.Serializable()]
public class PresetFloorData : PresetObjectData, ISerializable
{
	public SerializableVec3 Scale { get; protected set; }
	public string TextureName { get; protected set; }

	public PresetFloorData ()
	{
	}

	public PresetFloorData (Vector3 Position,
							Quaternion Rotation,
							Vector3 scale,
							string name,
							string textureName)
	{
		this.Position 	= new SerializableVec3 (Position);
		this.Rotation 	= new SerializableVec4 (Rotation);
		this.Scale	  	= new SerializableVec3 (scale);
		this.Name 	  	= name;
		this.color    	= new SerializableVec4 (Color.white);
		this.TextureName= textureName;
	}

	public PresetFloorData (SerializationInfo info, StreamingContext ctxt) : base (info, ctxt)
	{
		this.Scale 		 = (SerializableVec3)info.GetValue ("Scale", typeof(SerializableVec3));
		this.TextureName = (string)info.GetValue ("TextureName", typeof(string));
	}

	#region ISerializable implementation
	void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
	{
		base.AddSerializationData (info);

		info.AddValue ("Scale", 	  this.Scale);
		info.AddValue ("TextureName", this.TextureName);
	}
	#endregion
}

[System.Serializable()]
public class PresetWallData : PresetObjectData, ISerializable
{
	public SerializableVec3 Scale { get; protected set; }

	public string TextureName { get; protected set; }

	public PresetWallData ()
	{
	}

	public PresetWallData (Vector3 Position,
							Quaternion Rotation,
							Vector3 scale,
							string name,
							Color color,
							string textureName)
	{
		this.Position = new SerializableVec3 (Position);
		this.Rotation = new SerializableVec4 (Rotation);
		this.Scale = new SerializableVec3 (scale);
		this.Name = name;
		this.color = new SerializableVec4 (color);
		this.TextureName = textureName;
	}

	public PresetWallData (SerializationInfo info, StreamingContext ctxt) : base (info, ctxt)
	{
		this.Scale = (SerializableVec3)info.GetValue ("Scale", typeof(SerializableVec3));
		this.TextureName = (string)info.GetValue ("TextureName", typeof(string));
	}

	#region ISerializable implementation
	void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
	{
		base.AddSerializationData (info);

		info.AddValue ("Scale", this.Scale);
		info.AddValue ("TextureName", this.TextureName);
	}
	#endregion
}

[System.Serializable()]
public class PresetModuleData : PresetObjectData, ISerializable
{
	public string CategoryName 	{ get; private set; }
	public string BrandName		{ get; private set; }
	public SerializableTypeTop top { get; private set; }

	public PresetModuleData (Vector3 Position,
							Quaternion Rotation,
							string Id,
							string CategoryId,
							string BrandId)
	{
		this.Position = new SerializableVec3 (Position);
		this.Rotation = new SerializableVec4 (Rotation);
		this.Name = Id;
		this.CategoryName = CategoryId;
		this.BrandName = BrandId;
		this.color = new SerializableVec4 (Color.white);
	}

	public PresetModuleData (Vector3 Position,
							Quaternion Rotation,
							string Id,
							string CategoryId,
							string BrandId,
							Color color)
	{
		this.Position = new SerializableVec3 (Position);
		this.Rotation = new SerializableVec4 (Rotation);
		this.Name 			= Id;
		this.CategoryName 	= CategoryId;
		this.BrandName 		= BrandId;
		this.color	 		= new SerializableVec4 (color);
	}

	public PresetModuleData (SerializationInfo info, StreamingContext ctxt) : base (info, ctxt)
	{
		this.CategoryName = (string)info.GetValue ("CategoryName", typeof(string));
		this.BrandName 	  = (string)info.GetValue ("BrandName", typeof(string));
	}

	#region ISerializable implementation
	void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
	{
		base.AddSerializationData (info);

		info.AddValue ("CategoryName", this.CategoryName);
		info.AddValue ("BrandName", this.BrandName);
	}
	#endregion

	public override string ToString ()
	{
		return "\nPosition: " + this.Position +
               "\nRotation: " + this.Rotation +
               "\nName: " + 	this.Name +
               "\nCategoryName: " + this.CategoryName +
               "\nBrandName: " + this.BrandName +
	           "\ncolor: " + this.color.ToColor ();
	}
}