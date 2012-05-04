using UnityEngine;

interface GuiBase {
	
	/**
	 * Draw Gui content 
	 * */
	void Draw();
	
	/**
	 * @return This method should return the Rect's objects which are used in the GUI to show content.
	 * */
	Rect[] GetWindows();
	
}