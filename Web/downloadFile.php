<?php
//"@[^\/]?(\.jpg|\.png)$@i"
if(preg_match("@[^\///]?(\.png|\.xlsx)$@i", $_GET["fileName"]))
	{
		$filename = "upload/files/" . $_GET['fileName'];

		/* Download file */
		// set the header values
		header('Content-Description: File Transfer');
		header('Content-Type: application/octet-stream');
		if(strrpos($filename, ".png") === false){
			header('Content-Disposition: attachment; filename="relatório.xlsx"');
			// echo the content to the client browser
			//$handle = fopen($filename, "r");
			//$contents = fread($handle, filesize($filename));
			//fclose($handle);
			//unlink($filename);
			//$filename = "tmp-" + $filename;
			//$otherHandle = fopen($filename, "w");
			//fwrite($otherHandle,utf8_decode($contents));
			//fclose($otherHandle);

		} else {
			header('Content-Disposition: attachment; filename="screenshot.png"');
		}

		header('Content-Transfer-Encoding: binary');
		header('Expires: 0');
		header('Cache-Control: must-revalidate');
		header('Pragma: public');
		header('Content-Length: ' . filesize($filename));
		ob_clean();
		readfile($filename);
		flush();
		unlink($filename);
	}
?>

<?php
////"@[^\/]?(\.jpg|\.png)$@i"
//if(preg_match("@[^\///]?(\.png|\.xlsx)$@i", $_GET["fileName"]))
	//{

		//$baseExportPath = "upload/files/";
		//$filename = $baseExportPath . $_GET['fileName'];

		//[> Download file <]
		//// set the header values
		//header('Content-Description: File Transfer');
		//header('Content-Type: application/octet-stream');
		//if(strrpos($filename, ".png") === false){
			//header('Content-Disposition: attachment; filename="relatório.xlsx"');
		//} else {
			//header('Content-Disposition: attachment; filename="screenshot.png"');
		//}

		//header('Content-Transfer-Encoding: binary');
		//header('Expires: 0');
		//header('Cache-Control: must-revalidate');
		//header('Pragma: public');
		//header('Content-Length: ' . filesize($filename));
		//ob_clean();
		//readfile($filename);
		//flush();
		////unlink($filename);
	//}
?>
