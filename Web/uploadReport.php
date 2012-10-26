<?php
	if (preg_match("/\.xlsx/i", $_POST["filename"]))
	{
		require_once ("lib/zip_wrapper.php");

		$baseExportPath = "upload/files/";

		$filename      = $baseExportPath . $_POST["filename"];
		$excelBaseFile = $baseExportPath . "base.xlsx";

		$sharedStrings  = "xl/sharedStrings.xml";
		$sheet1         = "xl/worksheets/sheet1.xml";

		copy($excelBaseFile, $filename);

		$zipArchive = new ZipArchive();
		if (!$zipArchive->open($filename))
			die("Failed to create archive\n");

		$zipArchive->AddFromString ($sharedStrings, $_POST["shared-strings-data"]);
		$zipArchive->AddFromString ($sheet1, $_POST["sheet1-data"]);

		$zipArchive->close();
	}
?>
