using UnityEngine;

public class TextFileFinder : MonoBehaviour
{
    
	protected string m_textPath;
	protected FileBrowserSave m_fileBrowser;
	[SerializeField]
	protected GUISkin m_guiSkin;
	[SerializeField]
	protected Texture2D m_directoryImage,
                        m_fileImage;
    
	protected void OnGUI ()
	{
        GUI.skin = m_guiSkin;
		if (m_fileBrowser != null) {
			m_fileBrowser.OnGUI ();
		} else {
			OnGUIMain ();
		}
	}
    
	protected void OnGUIMain ()
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Save:", GUILayout.Width (100));
//		GUILayout.FlexibleSpace ();
//		GUILayout.Label (m_textPath ?? "none selected");
		if (GUILayout.Button ("...", GUILayout.ExpandWidth (false))) {
			m_fileBrowser = new FileBrowserSave (
                    new Rect (100, 100, 600, 500),
                    "Choose Text File",
                    FileSelectedCallback
                );
			m_fileBrowser.SelectionPattern = "*.png";
			m_fileBrowser.DirectoryImage = m_directoryImage;
			m_fileBrowser.FileImage = m_fileImage;
		}
		GUILayout.EndHorizontal ();
	}
    
	protected void FileSelectedCallback (string path)
	{
		m_fileBrowser = null;
        m_textPath = path;
		if (m_textPath != "") {
			if (m_textPath.Contains(".png")) {
				Application.CaptureScreenshot(m_textPath);
			} else {
				m_textPath += ".png";
				Application.CaptureScreenshot(m_textPath);
			}
		}
    }
}