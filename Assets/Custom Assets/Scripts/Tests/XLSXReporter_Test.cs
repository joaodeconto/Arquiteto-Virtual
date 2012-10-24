/**
 * @file XLSXReporter_Test.cs
 *
 * Unit tests for the Dummy class.
 */

using UnityEngine;
using System.Collections;
using SharpUnit;

public class XLSXReporter_Test : TestCase
{
	 //Member values
	private XLSXReporter m_xlsxReporter = null; //Dummy instance for testing
	private GameObject dummyGO;

	 //Setup test resources, called before each test.
	public override void SetUp()
	{
		dummyGO = new GameObject("Dummy");
		m_xlsxReporter = dummyGO.AddComponent<XLSXReporter>();
		m_xlsxReporter.xlsxBase = Resources.LoadAssetAtPath ("Assets/Custom Assets/Others/base.xlsx.txt", typeof(TextAsset)) as TextAsset;

		m_xlsxReporter.Init ();
	}

	 //Dispose of test resources, called after each test.
	public override void TearDown()
	{
		GameObject.Destroy(m_xlsxReporter);
		GameObject.Destroy(dummyGO);
	}
	 //Sample test that passes.
	[UnitTest]
	public void Test_LoadXLSXFile()
	{
		Assert.NotNull(m_xlsxReporter.xlsxBase);
	}

	[UnitTest]
	public void Test_if_sheet1_and_sharedStrings_are_empty_without_use()
	{
		Assert.Equal("", m_xlsxReporter.GetSharedStrings());
		Assert.Equal("", m_xlsxReporter.GetSheet1());
	}

	[UnitTest]
	public void Test_if_sheet1_and_sharedStrings_are_utf8_encoded()
	{
		m_xlsxReporter.BuildXLSX (new string[2,2]{	{"uma cozinha bunitona","cozinha massa mesmo"},
													{"uhulll","lol"}});
		Assert.True(m_xlsxReporter.GetSharedStrings().IndexOf("utf-16") == -1,m_xlsxReporter.GetSharedStrings());
		Assert.True(m_xlsxReporter.GetSharedStrings().IndexOf("UTF-16") == -1,m_xlsxReporter.GetSharedStrings());
		Assert.True(m_xlsxReporter.GetSheet1().IndexOf("utf-16") == -1, m_xlsxReporter.GetSheet1());
		Assert.True(m_xlsxReporter.GetSheet1().IndexOf("UTF-16") == -1, m_xlsxReporter.GetSheet1());
	}

	[UnitTest]
	public void Test_if_xmls_are_been_made_correctly()
	{
//		m_xlsxReporter.CreateCommonXLSX (new string[2,2]{	{"uma cozinha","outra cozinha"},
//													{"uhul","lol"}}, Application.persistentDataPath + "/temp-file.xlsx");
		//System.IO.MemoryStream ms = new System.IO.MemoryStream(Visiorama.Utils.FileUtils.LoadFile (@"C:\Users\Matheus\Documents\GitHub\Arquiteto-Virtual\asd.xlsx", false));
		//System.Func<string, bool> excludeFromDecompression = path => { return false; };

		//ms.DecompressToDirectory (  "asd/",
									//null,
									//excludeFromDecompression);
		//string checkSharedStrings;
		//string checkSheet1;
		//System.IO.StreamReader streamReader;

		//streamReader = new System.IO.StreamReader("asd/xl/sharedStrings.xml");
		//checkSharedStrings = streamReader.ReadToEnd();
		//streamReader.Close();

		//streamReader = new System.IO.StreamReader("asd/xl/worksheets/sheet1.xml");
		//checkSheet1 = streamReader.ReadToEnd();
		//streamReader.Close();

		//m_xlsxReporter.SaveXLSXToFile("trolol-temp-file.xlsx");

		//Assert.Equal(checkSharedStrings.Trim().ToLower().Replace("\n",""),
					 //m_xlsxReporter.GetSharedStrings().Trim().ToLower().Replace("\n", ""));
		//Assert.Equal(checkSheet1.Trim().ToLower().Replace("\n", ""),
					  //m_xlsxReporter.GetSheet1().Trim().ToLower().Replace("\n", ""));
	}
}
