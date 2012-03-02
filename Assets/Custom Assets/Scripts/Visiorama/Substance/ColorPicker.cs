using UnityEngine;

using System;
using System.Collections;

namespace Visiorama { 
	namespace Substance {

		//! This class draw a color picker. The data and the ui are unified.
		//! The object handle the update of each textures drawn in the ui and 
		//! also the value of the current color.
		public class ColorPicker
		{
			private Color current_RGB_color_; // Current RGB color
			private Color current_color_; // Current Saturated color
			private float saturation_ = 0.0F;
		
			private Texture2D hue_luminance_tex_; // Static texture
		
			// Dynamic textures
			private Texture2D saturation_tex_;
			private Texture2D thumbnail_tex_;
			
			private GameObject go_to_notify_;
			private String tweak_name_;
			
			private bool show_selected_color_;
			private bool show_saturation_bar_;
		
			public ColorPicker(Color color, Texture2D saturated_colors, GameObject go_to_notify, String tweak_name, bool show_selected_color, bool show_saturation_bar)
			{
				current_RGB_color_ = color;
		
				current_color_ = color;
		
				hue_luminance_tex_ = saturated_colors;
				go_to_notify_ = go_to_notify;
				tweak_name_ = tweak_name;
				
				show_selected_color_ = show_selected_color;
				show_saturation_bar_ = show_saturation_bar;
		
				thumbnail_tex_= new Texture2D(14, 14, TextureFormat.RGB24, false);
				saturation_tex_ = new Texture2D(94, 8, TextureFormat.RGB24, false);
				saturation_tex_.wrapMode = TextureWrapMode.Clamp;
		
				updateSaturationTexture();
				updateThumbnailTexture();
			}
		
			public String getTweakName(){
				return tweak_name_;
			}
		
			public Color getColorRGB(){
				return current_RGB_color_;
			}
		
			public float getSaturation(){
				return saturation_;
			}
			
			private void updateSaturationTexture(){    
				//Generate Saturation Texture
				float max = Math.Max(Math.Max(current_color_.r,current_color_.g),current_color_.b);
				Color maxColor = new Color(max,max,max);
				for(int y=0; y  < saturation_tex_.height; ++y)
				{
					for(int x=0; x  < saturation_tex_.width; ++x) 
					{	
						saturation_tex_.SetPixel(x, y, Color.Lerp(current_color_, maxColor, ((float)x/saturation_tex_.width)));
					}
				}
				saturation_tex_.Apply();	
			}
		
			private void updateThumbnailTexture(){
			
				current_RGB_color_ = saturation_tex_.GetPixel((int)(saturation_*saturation_tex_.width),0);
				for(int y=0; y  < thumbnail_tex_.height; ++y)
				{
					for(int x=0; x  < thumbnail_tex_.width; ++x) 
					{	
						thumbnail_tex_.SetPixel(x, y, current_RGB_color_);
					}
				}
				thumbnail_tex_.Apply();
				
				go_to_notify_.SendMessage("ColorPickerColorChange",this);
			}
		
			public void drawUI(Rect position)
			{
				if(GUI.RepeatButton(position, hue_luminance_tex_))
				{
			
					Vector2 pickpos = Event.current.mousePosition;
					int aaa = Convert.ToInt32(pickpos.x-position.x);
					int bbb = Convert.ToInt32(pickpos.y-position.y);
					current_color_ = hue_luminance_tex_.GetPixel(aaa,(int)(position.height - bbb));	
			
					//Vector2 pickpos = Event.current.mousePosition;
					
					//current_color_ = hue_luminance_tex_.GetPixel((int)(Convert.ToInt32(ScreenUtils.ScaleWidth(pickpos.x))),// - ScreenUtils.ScaleWidth(5)),
					//                                             (int)(Convert.ToInt32(ScreenUtils.ScaleWidth(pickpos.y))));// + ScreenUtils.ScaleHeight(30)));
					
					updateSaturationTexture();
					updateThumbnailTexture();
					
				}
				
				if(show_saturation_bar_){
					Rect saturation_position = ScreenUtils.ScaledRect(0,-10,0,15);
					
					saturation_position.x += position.x;
					saturation_position.y += position.y + position.height;
					saturation_position.width = position.width;
					
					if(GUI.RepeatButton(saturation_position, saturation_tex_))
					{
						Vector2 pickpos = Event.current.mousePosition;
						saturation_ = Convert.ToInt32( pickpos.x - (saturation_position.x) ) / (float) saturation_tex_.width;
						updateThumbnailTexture();	
					}
				}
		
				if(show_selected_color_){
					Rect thumbnail_position = ScreenUtils.ScaledRect(0,5,20,20);
					
					thumbnail_position.x += position.x + position.width;
					thumbnail_position.y += position.y;
					
					GUI.Box(thumbnail_position, thumbnail_tex_);
				}
			}
		};	
	}
}