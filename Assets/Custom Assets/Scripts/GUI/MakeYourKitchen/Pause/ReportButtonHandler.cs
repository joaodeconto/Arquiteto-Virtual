using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class ReportButtonHandler : MonoBehaviour {

	private const string reportUploadFileUrl = "uploadReport.php";

	FileBrowserComponent fileBrowser;
	private string path;

	void OnClick ()
	{
#if UNITY_WEBPLAYER
		StartCoroutine ("ReportData");
#elif !(UNITY_ANDROID || UNITY_IPHONE) || UNITY_EDITOR
		fileBrowser = new GameObject().AddComponent<FileBrowserComponent>();
		fileBrowser.Init (
			ScreenUtils.ScaledRectInSenseHeight(50, 50, 500, 400),
			"Salvar Relatório",
			FileSelectedCallback,
			"*.xlsx"
		);
		GameController
			.GetInstance ().
				GetInterfaceManager ()
					.SetInterface ("DeactivateAll");
#endif
	}

	void FileSelectedCallback (string path)
	{
		if (string.IsNullOrEmpty(path))
			path = "relatório.xlsx";

		if (!path.Contains(".xlsx"))
			path += ".xlsx";

		this.path = path;

		Destroy(fileBrowser.gameObject);

		this.gameObject.active = true;

		StartCoroutine ("ReportData");
    }

	private IEnumerator ReportData ()
	{
		GameObject[] mobiles = GameObject.FindGameObjectsWithTag ("Movel");

		string filename = string.Format ("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now) + ".xlsx";
		string csvString = " ;Cozinhas Telasul Aço ; # # ";
		csvString += "Linha: ;" + Line.CurrentLine.Name + " ; ; # ";

		string shortenedBrandColorName = null;

		//obtendo tipo de tampo
		foreach (Transform check in GameObject.Find ("InfoController").GetComponent<InfoController>().checkBoxTextures.transform)
		{
			if (check.name == "Label")
				continue;

			if (check.GetComponent<UICheckbox>().isChecked)
			{
				csvString += " Tampo: ;" + check.GetComponent<CheckBoxTextureHandler> ().texture.name + " ; ; # ";
				break;
			}
		}

		csvString += " # REF.;Descrição;MEDIDAS (LxAxP);PESO LÍQ. (Kg) ; # ";

		foreach (GameObject mobile in mobiles)
		{
			InformacoesMovel info = mobile.GetComponent<InformacoesMovel> ();

			//Se for um item extra ou se for algum item que não possua código, como as lâmpadas, não adicionada no arquivo.
			if ("Extras".Equals (info.Categoria) || String.IsNullOrEmpty(info.Codigo.Trim()))
				continue;

			if (info.HasDetailMaterial())
			{
				shortenedBrandColorName = BrandColor.GetShortenedColorName(Line.CurrentLine.colors[Line.CurrentLine.GlobalDetailColorIndex]);
			}
			else
			{
				shortenedBrandColorName = "";
			}

			if (Regex.Match(info.name, "(com cooktop|com cook top)").Success)
			{
				csvString += info.Codigo		+ " ; " +
							 info.NomeP			+ " ; " +
							 info.Largura		+ "x" + info.Altura	+ "x" + info.Comprimento + " ; " +
							 info.PesoLiquido	+ " ; #";

				if (Regex.Match(info.name, "(Balcão triplo|Balcao triplo)").Success)
				{
					csvString += "89121" + " ; " +
								 "Tampo cooktop triplo" + " ; " +
								 "1200" + "x" + "30" + "x" + "520 ; " +
								 "33,56" + " ; #";
				}
				else
				{
					csvString += "89081" + ";" +
								 "Tampo cooktop duplo" + ";" +
								 "800" + "x" + "30"  + "x" + "520;" +
								 "24,08" + " ; #";
				}
				csvString += "Cooktop" + " ; " +
							 "89150"   + " ; " +
							 "NA" + "x" + "NA" + "x" + "NA" + " ; " +
							 "NA" + " ; #";
			}
			else
			{
				csvString += info.Codigo 	  + shortenedBrandColorName + " ; " +
							 info.NomeP		  + " ; " +
							 info.Largura	  + "x" + info.Altura 	  + "x" + info.Comprimento + " ; " +
							 info.PesoLiquido + " ; #";
			}
		}

		//Debug.Log ("csvString: " + csvString);

		//removing newlines
		csvString = csvString.Replace("\n"," , ");

		int maxRowSize = 1;
		string[] rows = csvString.Split ('#');
		List<List<string>> cells = new List<List<string>> ();

		foreach (string row in rows)
		{
			string[] col = row.Split(';');
			cells.Add ( new List<string> (col));

			if (maxRowSize < col.Length)
				maxRowSize = col.Length;
		}

		string[,] datatable = new string[rows.Length - 1, maxRowSize - 1];


		for (int j = 0; j != rows.Length - 1; ++j)
		{
			for (int i = 0; i != maxRowSize - 1; ++i)
			{
				if(cells.Count > j && cells[j].Count > i)
					datatable[j,i] = cells[j][i];

				//se a célula está vazia ou não foi preenchida, a preenche
				if(string.IsNullOrEmpty(datatable[j,i]))
					datatable[j,i] = " ";
			}
		}


		for (int j = 0; j != rows.Length- 1; ++j)
		{
			for (int i = 0; i != maxRowSize - 1; ++i)
			{
				Debug.Log (" j:" + j + " i:" + i + " - " + datatable[j,i]);
			}
		}

		XLSXReporter reporter = GameObject.Find ("XLSXReporter").GetComponent<XLSXReporter>();

		reporter.Init ();
		reporter.BuildXLSX (datatable);

#if UNITY_WEBPLAYER

		WWWForm form = new WWWForm ();

		form.AddField ("shared-strings-data", 		reporter.GetSharedStrings ());
		form.AddField ("sheet1-data",				reporter.GetSheet1());
		form.AddField ("filename", filename);

		WWW www = new WWW (reportUploadFileUrl, form);

		yield return www;

		if (www.error != null)
			print (www.error);
		else
			Application.ExternalCall ("tryToDownload", filename);
#else
		//System.IO.File.WriteAllText(m_textPath, data);

		reporter.SaveXLSXToFile (path);

		yield return new WaitForEndOfFrame();

#endif

		GameController
			.GetInstance ().
				GetInterfaceManager ()
					.SetInterface ("Pause");
	}
}
