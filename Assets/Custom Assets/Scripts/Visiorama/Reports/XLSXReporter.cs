using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using System.Xml;
using System.Xml.Serialization;

using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

public class XLSXReporter : MonoBehaviour
{
	private static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private static string GetColumnName (int columnNumberIndex)
	{
		if (columnNumberIndex > 25)
			return alphabet[columnNumberIndex % 25].ToString () + GetColumnName (columnNumberIndex - 26);
		else if (columnNumberIndex >= 0)
			return alphabet[columnNumberIndex % 25].ToString ();
		else
			return "";
	}

	//Editor
	public TextAsset xlsxBase;

	private string baseXlsxUnzippedDir;

	private string sharedStringsFilepath;
	private string sheetFilepath;

	void Start ()
	{
		baseXlsxUnzippedDir   = Application.persistentDataPath + "/temp";
		sharedStringsFilepath = Application.persistentDataPath + "/temp/xl/sharedStrings.xml";
		sheetFilepath         = Application.persistentDataPath + "/temp/xl/worksheets/sheet1.xml";

		CreateCommonXLSX (new string[2,2]{	{"uma cozinha bunitona","cozinha massa mesmo"},
											{"uhulll","lol"}},
						  Application.persistentDataPath + "/temp-file.xlsx");
	}

	public bool CreateCommonXLSX (string[,] datatable, string filePath)
	{
		if (datatable == null ||
			datatable.GetLength (0) < 2 ||
			datatable.GetLength (1) < 2)
		{
			Debug.LogError ("é necessário ter mais do que uma coluna e uma linha na tabela a ser criada");
			return false;
		}

		//unzip file
		UnzipBaseXlsxFile ();

		File.Delete (sharedStringsFilepath);

		int cellCount = datatable.GetLength (0) * datatable.GetLength (1);

		int maxColumns  = datatable.GetLength (0);
		int maxRows		= datatable.GetLength (1);

		//criando arquivo sharedStrings
		using (XmlWriter writer = XmlWriter.Create(sharedStringsFilepath))
		{
			writer.WriteStartDocument (true);
			writer.WriteStartElement ("sst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
			writer.WriteAttributeString ("count", cellCount.ToString ());
			writer.WriteAttributeString ("uniqueCount", cellCount.ToString ());

			foreach (string celldata in datatable)
			{
				writer.WriteStartElement("si");
				writer.WriteElementString("t", celldata);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
			writer.WriteEndDocument();
		}

		//criando xml de relatório
		using (XmlWriter writer = XmlWriter.Create(sheetFilepath))
		{
			writer.WriteStartDocument (true);
			{
				writer.WriteStartElement ("worksheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
				writer.WriteAttributeString ("xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
				{
					writer.WriteStartElement ("sheetPr");
					writer.WriteAttributeString ("filterMode", "false");
					{
						writer.WriteStartElement ("pageSetUpPr");
						{
							writer.WriteAttributeString ("fitToPage", "false");
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();

					writer.WriteStartElement ("dimension");
					{
						writer.WriteAttributeString ("ref", "A1" + ":" + GetColumnName (maxColumns) + maxRows);
					}
					writer.WriteEndElement();
					writer.WriteStartElement ("sheetViews");
					{
						writer.WriteStartElement ("sheetView");
						{
							writer.WriteAttributeString("colorId","64");
							writer.WriteAttributeString("defaultGridColor","true");
							writer.WriteAttributeString("rightToLeft","false");
							writer.WriteAttributeString("showFormulas","false");

							writer.WriteAttributeString("showGridLines","true");
							writer.WriteAttributeString("showOutlineSymbols","true");
							writer.WriteAttributeString("showRowColHeaders","true");
							writer.WriteAttributeString("showZeros","true");
							writer.WriteAttributeString("tabSelected","true");

							writer.WriteAttributeString("topLeftCell","A1");
							writer.WriteAttributeString("view","normal");
							writer.WriteAttributeString("windowProtection","false");
							writer.WriteAttributeString("workbookViewId","0");
							writer.WriteAttributeString("zoomScale","100");
							writer.WriteAttributeString("zoomScaleNormal","100");

							writer.WriteAttributeString("zoomScalePageLayoutView","100");
							writer.WriteStartElement ("selection");
							{
								writer.WriteAttributeString("activeCell","A1");
								writer.WriteAttributeString("activeCellId","0");
								writer.WriteAttributeString("pane","topLeft");
								writer.WriteAttributeString("sqref","A1");
							}
							writer.WriteEndElement();
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();

					writer.WriteStartElement ("cols");
					{
						for (int i = 0; i != maxColumns; ++i)
						{
							writer.WriteStartElement ("col");
							{
								writer.WriteAttributeString("collapsed","false");
								writer.WriteAttributeString("hidden","false");
								writer.WriteAttributeString("max", i.ToString ());
								writer.WriteAttributeString("min", i.ToString ());
								writer.WriteAttributeString("style","0");
								writer.WriteAttributeString("width","13.5");
							}
							writer.WriteEndElement();
						}

						writer.WriteStartElement ("col");
						{
							writer.WriteAttributeString("collapsed","false");
							writer.WriteAttributeString("hidden","false");
							writer.WriteAttributeString("max", 1025.ToString ());
							writer.WriteAttributeString("min", (maxColumns).ToString ());
							writer.WriteAttributeString("style","0");
							writer.WriteAttributeString("width","13.5");
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();

					writer.WriteStartElement ("sheetData");
					{
						for (int i = 0; i != maxRows; ++i)
						{
							writer.WriteStartElement ("row");
							{
								writer.WriteAttributeString("collapsed","false");
								writer.WriteAttributeString("customFormat","false");
								writer.WriteAttributeString("customHeight","false");
								writer.WriteAttributeString("hidden","false");
								writer.WriteAttributeString("ht","12.85");
								writer.WriteAttributeString("outlineLevel","0");
								writer.WriteAttributeString("r",(i + 1).ToString ());

								for (int j = 0; j != maxColumns; ++j)
								{
									writer.WriteStartElement ("c");
									{
										writer.WriteAttributeString("r", ( GetColumnName (j) + (i + 1).ToString ()));
										writer.WriteAttributeString("s","0");
										writer.WriteAttributeString("t","s");

										writer.WriteStartElement ("v");
										{
											//isso serve para escolher as strings corretas do arquivo sharedStrings
											writer.WriteString (((i * maxColumns)+ j).ToString ());
										}
										writer.WriteEndElement();
									}
									writer.WriteEndElement();
								}
							}
							writer.WriteEndElement();
						}
					}
					writer.WriteEndElement();

					writer.WriteStartElement ("printOptions");
					{
						writer.WriteAttributeString("headings","false");
						writer.WriteAttributeString("gridLines","false");
						writer.WriteAttributeString("gridLinesSet","true");
						writer.WriteAttributeString("horizontalCentered","false");
						writer.WriteAttributeString("verticalCentered","false");
					}
					writer.WriteEndElement();

					writer.WriteStartElement ("pageMargins");
					{
						writer.WriteAttributeString("left","0.7875");
						writer.WriteAttributeString("right","0.7875");
						writer.WriteAttributeString("top","1.05277777777778");
						writer.WriteAttributeString("bottom","1.05277777777778");
						writer.WriteAttributeString("header","0.7875");
						writer.WriteAttributeString("footer","0.7875");
					}
					writer.WriteEndElement();

					writer.WriteStartElement ("pageSetup");
					{
						writer.WriteAttributeString("blackAndWhite","false");
						writer.WriteAttributeString("cellComments","none");
						writer.WriteAttributeString("copies","1");
						writer.WriteAttributeString("draft","false");
						writer.WriteAttributeString("firstPageNumber","1");
						writer.WriteAttributeString("fitToHeight","1");
						writer.WriteAttributeString("fitToWidth","1");
						writer.WriteAttributeString("horizontalDpi","300");
						writer.WriteAttributeString("orientation","portrait");
						writer.WriteAttributeString("pageOrder","downThenOver");
						writer.WriteAttributeString("paperSize","9");
						writer.WriteAttributeString("scale","100");
						writer.WriteAttributeString("useFirstPageNumber","true");
						writer.WriteAttributeString("usePrinterDefaults","false");
						writer.WriteAttributeString("verticalDpi","300");
					}
					writer.WriteEndElement();

					writer.WriteStartElement ("headerFooter");
					{
						writer.WriteAttributeString("differentFirst","false");
						writer.WriteAttributeString("differentOddEven","false");

						writer.WriteStartElement ("oddHeader");
						{
							writer.WriteString ("&amp;C&amp;&quot;Times New Roman,Normal&quot;&amp;12&amp;A");
						}
						writer.WriteEndElement();

						writer.WriteStartElement ("oddHeader");
						{
							writer.WriteString ("&amp;C&amp;&quot;Times New Roman,Normal&quot;&amp;12Página &amp;P");
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteEndDocument();
		}

		//After decompress and modify file
		ZipWrapper.CompressDirectoryToFile (baseXlsxUnzippedDir,
											filePath);

		DirectoryInfo di = new DirectoryInfo(baseXlsxUnzippedDir);

		foreach (FileInfo file in di.GetFiles())
			file.Delete();

		foreach (DirectoryInfo dir in di.GetDirectories())
			dir.Delete(true);

		di.Delete (true);

		return true;
	}

	private void UnzipBaseXlsxFile ()
	{
		MemoryStream ms = new MemoryStream(xlsxBase.bytes);
		Func<string, bool> excludeFromDecompression = path => { return false; };

		ms.DecompressToDirectory (  baseXlsxUnzippedDir,
									null,
									excludeFromDecompression);
	}
}
