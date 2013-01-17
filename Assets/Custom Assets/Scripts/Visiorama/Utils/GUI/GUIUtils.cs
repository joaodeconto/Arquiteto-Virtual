using UnityEngine;
using System.Collections;

namespace Visiorama {
	namespace Utils {
		public class GUIUtils {
			
			// idSelect é o valor do número do accordion que está no momento;
			// Inicia-se com -1, para não mostrar nenhum dos objetos 
			static int s_SelectedId = -1;
			#region Esse accordion pega pelo número de GUIStyles
			
				#region Nesse casos, o rectContent só passa um valor que será padrão
				
				// Aqui o rect distance pega pelo valor da rect.height, ou seja, o tamanho do botão
				public static int AccordionVertical (Rect rect, int cAccordionId, float rectContent, GUIStyle[] guiStyles) {
					return AccordionVertical(rect, cAccordionId, rect.height, rectContent, guiStyles);
				}
				
				// Esse é o resposável pelo código acima, sendo que você só passa um valor a mais, que seria o recDistance
				public static int AccordionVertical (Rect rect, int cAccordionId, float rectDistance, float rectContent, GUIStyle[] guiStyles) {
					if (cAccordionId == -1) {
						for (int i = 0; i != guiStyles.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
							                       				  rect.width, rect.height), "", guiStyles[i])) {
								cAccordionId = i;
							}
						}
					}
					else {
						for (int i = 0; i != (cAccordionId + 1); ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
							                        rect.width, rect.height), "", guiStyles[i])) {
								if (cAccordionId != i)
									cAccordionId = i;
								else {
									cAccordionId = -1;
									return cAccordionId;
								}
							}
						}
						for (int i = (cAccordionId + 1); i != guiStyles.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, (rect.y + (rectDistance * i)) + rectContent, 
							                        			  rect.width, rect.height), "", guiStyles[i])) {
								cAccordionId = i;
							}
						}
					}
					return cAccordionId;
				}
				#endregion
				
				#region Nesse casos, o rectContent pode ser passados valores alternados, porém terá que passar cada um na mão
				
				// Aqui o rect distance pega pelo valor da rect.height, ou seja, o tamanho do botão
				public static int AccordionVertical (Rect rect, int cAccordionId, float[] rectContent, GUIStyle[] guiStyles) {
					return AccordionVertical(rect, cAccordionId, rect.height, rectContent, guiStyles);
				}
				
				// Esse é o resposável pelo código acima, sendo que você só passa um valor a mais, que seria o recDistance
				public static int AccordionVertical (Rect rect, int cAccordionId, float rectDistance, float[] rectContent, GUIStyle[] guiStyles) {
					if (cAccordionId == -1) {
						for (int i = 0; i != guiStyles.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
							                        			  rect.width, rect.height), "", guiStyles[i])) {
								cAccordionId = i;
							}
						}
					}
					else {
						for (int i = 0; i != (cAccordionId + 1); ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
							                        			  rect.width, rect.height), "", guiStyles[i])) {
								if (cAccordionId != i)
									cAccordionId = i;
								else {
									cAccordionId = -1;
									return cAccordionId;
								}
							}
						}
						for (int i = (cAccordionId + 1); i != guiStyles.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, (rect.y + (rectDistance * i)) + rectContent[cAccordionId], 
							                        			  rect.width, rect.height), "", guiStyles[i])) {
								cAccordionId = i;
							}
						}
					}
					return cAccordionId;
				}
				#endregion
				
			#endregion
			
			#region Esse accordion pega pelas labels(strings)
				
				#region Nesse casos, o rectContent só passa um valor que será padrão
				
				// Aqui o rect distance pega pelo valor da rect.height, ou seja, o tamanho do botão
				// e também o GUIStyle é pré-definido com o botão padrão da Unity
				public static int AccordionVertical (Rect rect, int id,	string[] labels, float rectContent) {
					return AccordionVertical(rect, id, labels, rect.height, rectContent, "button");
				}
				
				// Aqui o rect distance pega pelo valor da rect.height, ou seja, o tamanho do botão
				public static int AccordionVertical (Rect rect, int id,	string[] labels, float rectContent, GUIStyle guiStyle) {
					return AccordionVertical(rect, id, labels, rect.height, rectContent, guiStyle);
				}
				
				// Aqui o GUIStyle é pré-definido com o botão padrão da Unity
				public static int AccordionVertical (Rect rect,	int id, string[] labels, float rectDistance, float rectContent) {
					return AccordionVertical(rect, id, labels, rectDistance, rectContent, "button");
				}
				
				// Esse é o resposável pelo código acima, sendo que você só passa um valor a mais, que seria o recDistance			
				public static int AccordionVertical (Rect rect,
													 int cAccordionId,
				                                     string[] labels,
				                                     float rectDistance,
				                                     float rectContent,
				                                     GUIStyle guiStyle){
					if (cAccordionId == -1) {
						for (int i = 0; i != labels.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
				                        			rect.width, rect.height), labels[i], guiStyle)) {
								cAccordionId = i;
							}
						}
					}
					else {
						for (int i = 0; i != (s_SelectedId + 1); ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
			                        				rect.width, rect.height), labels[i], guiStyle)) {
								if (cAccordionId != i)
									cAccordionId = i;
								else {
									cAccordionId = -1;
									return cAccordionId;
								}
							}
						}
						for (int i = (cAccordionId + 1); i != labels.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, (rect.y + (rectDistance * i)) + rectContent,
							                   		rect.width, rect.height), labels[i], guiStyle)) {
								cAccordionId = i;
							}
						}
					}
					return cAccordionId;
				}
				
				public static int AccordionVertical (Rect rect,
													 int cAccordionId,
				                                     string[] labels,
				                                     string[] tips,
				                                     float rectDistance,
				                                     float rectContent,
				                                     GUIStyle guiStyle){
					if (cAccordionId == -1) {
						for (int i = 0; i != labels.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
				                        			rect.width, rect.height), new GUIContent(labels[i], tips[i]), guiStyle)) {
								cAccordionId = i;
							}
						}
					}
					else {
						for (int i = 0; i != (cAccordionId + 1); ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
			                        				rect.width, rect.height), new GUIContent (labels[i], tips[i]), guiStyle)) {
								if (cAccordionId != i)
									cAccordionId = i;
								else {
									cAccordionId = -1;
									return cAccordionId;
								}
							}
						}
						for (int i = (cAccordionId + 1); i != labels.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, (rect.y + (rectDistance * i)) + rectContent,
							                   		rect.width, rect.height), new GUIContent (labels[i], tips[i]), guiStyle)) {
								cAccordionId = i;
							}
						}
					}
					return cAccordionId;
				}
				#endregion
				
				#region Nesse casos, o rectContent pode ser passados valores alternados, porém terá que passar cada um na mão
				
				// Aqui o rect distance pega pelo valor da rect.height, ou seja, o tamanho do botão
				// e também o GUIStyle é pré-definido com o botão padrão da Unity
				public static int AccordionVertical (Rect rect,	string[] labels, float[] rectContent) {
					return AccordionVertical(rect, labels, rect.height, rectContent, "button");
				}
				
				// Aqui o rect distance pega pelo valor da rect.height, ou seja, o tamanho do botão
				public static int AccordionVertical (Rect rect,	string[] labels, float[] rectContent, GUIStyle guiStyle) {
					return AccordionVertical(rect, labels, rect.height, rectContent, guiStyle);
				}
				
				// Aqui o GUIStyle é pré-definido com o botão padrão da Unity
				public static int AccordionVertical (Rect rect,	string[] labels, float rectDistance, float[] rectContent) {
					return AccordionVertical(rect, labels, rectDistance, rectContent, "button");
				}
				
				// Esse é o resposável pelo código acima, sendo que você só passa um valor a mais, que seria o recDistance			
				public static int AccordionVertical (Rect rect,
				                              string[] labels,
				                              float rectDistance,
				                              float[] rectContent,
				                              GUIStyle guiStyle
				                              ) {
					if (s_SelectedId == -1) {
						for (int i = 0; i != labels.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
							                        			  rect.width, rect.height), labels[i], guiStyle)) {
								s_SelectedId = i;
							}
						}
					}
					else {
						for (int i = 0; i != (s_SelectedId + 1); ++i) {
							if (GUI.Button(new Rect(rect.x, rect.y + (rectDistance * i), 
							                        			  rect.width, rect.height), labels[i], guiStyle)) {
								if (s_SelectedId != i)
									s_SelectedId = i;
								else {
									s_SelectedId = -1;
									return s_SelectedId;
								}
							}
						}
						for (int i = (s_SelectedId + 1); i != labels.Length; ++i) {
							if (GUI.Button(new Rect(rect.x, (rect.y + (rectDistance * i)) + rectContent[s_SelectedId],
							                        			  rect.width, rect.height), labels[i], guiStyle)) {
								s_SelectedId = i;
							}
						}
					}
					return s_SelectedId;
				}
				
				#endregion
				
			#endregion
			
			#region Seria o AccordionHorizontal, mas está incompleto e precisa ser terminado
			/*public static int AccordionHorizontal (Rect rect,
			                              string[] labels,
			                              float rectDistance,
			                              float[] rectContent,
			                              GUIStyle guiStyle
			                              ) {
				if (idSelectH == -1) {
					for (int i = 0; i != labels.Length; ++i) {
						if (GUI.Button(new Rect(rect.x + (rectDistance * 1), rect.y, 
						                        			  rect.width, rect.height), labels[i], guiStyle)) {
							idSelectH = i;
						}
					}
				}
				else {
					for (int i = 0; i != (idSelectH + 1); ++i) {
						if (GUI.Button(new Rect(rect.x + (rectDistance * i), rect.y , 
						                        			  rect.width, rect.height), labels[i], guiStyle)) {
							idSelectH = i;
						}
					}
					for (int i = idSelectH; i != labels.Length; ++i) {
						if (GUI.Button(new Rect(rect.x + (rectDistance * i) + rectContent[idSelectH], rect.y, 
						                        			  rect.width, rect.height), labels[i], guiStyle)) {
							if (idSelectH != i)
								idSelectH = i;
							else
								idSelectH = -1;
						}
					}
				}
				return idSelectH;
			}*/
			#endregion
		}
	}
}