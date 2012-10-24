using UnityEngine;

class FileBrowserComponent : MonoBehaviour
{
	#region Save Screenshot GUI vars
	private string m_textPath;
	private FileBrowserSave m_fileBrowser;
	[SerializeField]
	protected GUISkin m_guiSkin;
	[SerializeField]
	protected Texture2D m_directoryImage,
                        m_fileImage;
	#endregion

	public FileBrowserComponent Init (  Rect rect,
										string message,
										FileBrowserSave.FinishedCallback callback,
										string pattern)
	{
		m_textPath = "";
		m_fileBrowser = new FileBrowserSave(rect,
											message,
											callback);

		m_fileBrowser.SelectionPattern = pattern;
		m_fileBrowser.DirectoryImage   = m_directoryImage;
		m_fileBrowser.FileImage        = m_fileImage;

		return this;
	}

	#region GUI only File Browser Save
	void OnGUI ()
	{
		if (m_fileBrowser != null) {
	        GUI.skin = m_guiSkin;
			m_fileBrowser.OnGUI ();
		}
	}
	#endregion
}
