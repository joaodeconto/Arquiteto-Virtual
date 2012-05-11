using UnityEngine;
using System.Collections;
using UnityEditor;

public class Build {

	[MenuItem ("Build/BuildAndroidStreamed")]
	static void MyBuild(){
	    string[] levels = {"Assets/_Scenes/SelectLanguage.unity", 
			"Assets/_Scenes/SelectKitchen.unity", 
			"Assets/_Scenes/WallBuilder.unity", 
			"Assets/_Scenes/MakeYourKitchen.unity"};
	    //BuildPipeline.BuildStreamedSceneAssetBundle(levels, "Build/arquiteto-virtual2-streamed.apk", BuildTarget.Android);
		BuildPipeline.BuildPlayer(levels, "Build/arquiteto-virtual2-streamed2.apk", BuildTarget.Android, BuildOptions.BuildAdditionalStreamedScenes);
	}
}
