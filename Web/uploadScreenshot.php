<?php
	if ($_FILES["file"]["error"] > 0){
		file_put_contents ( "error.txt", $_FILES["file"]["error"] );
	} else {
		move_uploaded_file($_FILES["file"]["tmp_name"], "upload/files/" . $_FILES["file"]["name"]);
	}
?>
