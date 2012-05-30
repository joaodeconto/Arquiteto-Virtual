using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Client")]
public class Client
{
	public int id_client 	{ get; set; }
	public string firstname { get; set; }
	public string lastname 	{ get; set; }
	public string email 	{ get; set; }
	public System.DateTime dt_birth 		  { get; set; }
	public System.DateTime dt_registration { get; set; }
	public int id_city { get; set; }

	[XmlArray("Projects")]
	[XmlArrayItem("Project")]
	public List<Project> Projects { get; set; }

	public Client ()
	{
		Projects = new List<Project> ();
	}
}

public class Project
{
	public int id_project { get; set; }
	public int id_client { get; set; }
	public byte[] data 	{ get; set; }

	public Project ()
	{
	}

	public Project (byte[] data)
	{
		this.data = data;
	}
}
