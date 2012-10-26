<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<title>Arquiteto Virtual | Visiorama - Telasul </title>
		<script type="text/javascript" src="jquery-1.5.1.min.js"></script>
		<script type="text/javascript" src="http://webplayer.unity3d.com/download_webplayer-3.x/3.0/uo/UnityObject.js"></script>
		<script type="text/javascript">
		<!--
		function tryToDownload(filename){
			//alert();
			oIFrm = document.getElementById('myIFrm');
			oIFrm.src = "downloadFile.php?fileName=" + filename;
		}
		function GetUnity() {
			if (typeof unityObject != "undefined") {
				return unityObject.getObjectById("unityPlayer");
			}
			return null;
		}
		if (typeof unityObject != "undefined") {
			var params = {
				disableContextMenu: true,
				backgroundcolor: "354550",
				bordercolor: "000b12",
				logoimage: "images/logo-reduzido.png",
				progressbarimage: "images/filled-progress-bar.png",
				progressframeimage: "images/unfilled-progress-bar.png"

			};
			unityObject.embedUnity("unityPlayer", "Arquiteto.unity3d", 1024, 640, params);
		}
		-->
		</script>
		<style type="text/css">
		<!--
		body {
			font-family: Helvetica, Verdana, Arial, sans-serif;
			background-color: #000b12;
			color: black;
			text-align: center;
		}
		a:link, a:visited {
			color: #000;
		}
		a:active, a:hover {
			color: #555;
		}
		p.header {
			font-size: small;
		}
		p.header span {
			font-weight: bold;
		}
		p.footer {
			font-size: x-small;
		}
		div.content {
			left: 50%;
			margin: -320px 0 0 -512px;
			position: absolute;
			top: 50%;
			height: 640px;
			width: 1024px;
		}
		div.missing {
			margin: auto;
			position: relative;
			top: 50%;
			width: 193px;
		}
		div.missing a {
			height: 63px;
			position: relative;
			top: -31px;
		}
		div.missing img {
			border-width: 0px;
		}
		div#unityPlayer {
			visibility:visible !important;
			cursor: default;
			height: 640px;
			width: 1024px;
		}
		-->
		</style>
	</head>
	<body>
		<div class="content">
			<div id="unityPlayer" style="visibility:visible;">
				<div class="missing">
					<a href="http://unity3d.com/webplayer/" title="Unity Web Player. Install now!">
						<img alt="Unity Web Player. Install now!" src="http://webplayer.unity3d.com/installation/getunity.png" width="193" height="63" />
					</a>
				</div>
			</div>
		</div>
		<iframe id="myIFrm" src="" style="visibility:hidden; height:0; width:0; margin: 0; padding: 0;">
		</iframe>
	</body>
    <script type="text/javascript">
          var _gaq = _gaq || [];
          _gaq.push(['_setAccount', 'UA-33779864-1']);
          _gaq.push(['_trackPageview']);
          (function() {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
          })();
    </script>
</html>
