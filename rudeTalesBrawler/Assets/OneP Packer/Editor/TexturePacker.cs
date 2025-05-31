/// <summary>
/// 2015-08-10
/// Copyright OnePStudio
/// mail: onepstudio@gmail.com
/// </summary>
/// 
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace OnePStudio.OptimizeSprite
{
	public class TexturePacker 
	{
		[MenuItem("Assets/Tools/OneP Packer/Build Atlas")]
		static public void AutoBuildAtlas()
		{
			Object[] selection = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
			if(selection.Length<1)
			{
				EditorUtility.DisplayDialog("Error","Please choose some images File","OK");
				return;  
			}
			List<Texture2D> listInput=new List<Texture2D>();
			List<SpriteRawData> listSprite=new List<SpriteRawData>();
			for(int i=0;i<selection.Length;i++)
			{
				Object obj=selection[i];
				if(obj is Texture2D)
				{
					Texture2D tex=(Texture2D)obj;
					listInput.Add(tex);
					OptimizeSprite.SetSpriteRawFromTexture(ref listSprite,tex);
				}
			}
			if(listSprite.Count<0)
			{
				EditorUtility.DisplayDialog("Error","Please choose at least one image File","OK");
				return;
			}
			else
			{
				ToolBuildAtlasDialog.ShowBuildAtlasDialog(listSprite);
			}
		}

		static private Dictionary<string,Sprite> LinkedObject()
		{
			return null;
		}

		static public void AutoBuildAtlas (List<Texture2D> listTexture,string texturePath,TrimType trimType,int pading)
		{

			List<SpriteElement> listSprite=new List<SpriteElement>();
			for(int i=0;i<listTexture.Count;i++)
			{
				Object obj=listTexture[i];
				if(obj is Texture2D)
				{
					Texture2D tex=(Texture2D)obj;
			
					SpriteElement element=new SpriteElement(tex);
					if(trimType==TrimType.Trim2nTexture||trimType==TrimType.TrimMinimum)
					{
						element.TrimTexture();
					}
					else
					{
						element.CloneFromOriginTexture();
					}
					listSprite.Add(element);
				}

			}
			if(listSprite.Count>0)
			{
				BuildAtlas(trimType,listSprite,texturePath,pading);
				// GC memory
				for(int i=0;i<listSprite.Count;i++)
				{
					GameObject.DestroyImmediate(listSprite[i].texture);
					listSprite[i]=null;
				}
			}
		}

		static public void AutoBuildAtlasFromSpriteRaw (List<SpriteRawData> listSpriteRaw,string texturePath,TrimType trimType,int pading,Texture2D mainTexure)
		{
			
			List<SpriteElement> listSprite=new List<SpriteElement>();
			for(int i=0;i<listSpriteRaw.Count;i++)
			{
				SpriteRawData spriteRaw=listSpriteRaw[i];
				if(spriteRaw.sprite!=null)
				{
					SpriteElement element=new SpriteElement(spriteRaw,trimType);
					listSprite.Add(element);
				}
				
			}
			if(mainTexure!=null)
			{
				if(File.Exists(texturePath))
				{
					File.Delete(texturePath);
					//AssetDatabase.Refresh();
				}
			}
			if(listSprite.Count>0)
			{
				BuildAtlas(trimType,listSprite,texturePath,pading);
				// GC memory
				for(int i=0;i<listSprite.Count;i++)
				{
					GameObject.DestroyImmediate(listSprite[i].texture);
					listSprite[i]=null;
				}
			}
		}
		static public bool UpdateAtlasSpriteInfo(string pathOutput,float scale)
		{
			TextureImporter ti = AssetImporter.GetAtPath(pathOutput) as TextureImporter;
			TextureImporterSettings settings = new TextureImporterSettings();
			ti.ReadTextureSettings(settings);
			SpriteMetaData[] lstMetaSprite=ti.spritesheet;
			Dictionary<string,SpriteMetaData> dicSpriteMeta=new Dictionary<string,SpriteMetaData>();
			bool haveNew=true;
			for(int i=0;i<lstMetaSprite.Length;i++)
			{
				SpriteMetaData metaSprite=lstMetaSprite[i];
				if(Mathf.Abs(scale-1)>Mathf.Epsilon)
				{
					Rect rect=metaSprite.rect;
					rect.x=rect.x*scale;
					rect.y=rect.y*scale;
					rect.width=rect.width*scale;
					rect.height=rect.height*scale;
					metaSprite.rect=rect;
					haveNew=true;
				}
				dicSpriteMeta[lstMetaSprite[i].name]=metaSprite;
			}
			if(haveNew)
			{
				Texture2D mainTexture=AssetDatabase.LoadAssetAtPath(pathOutput,typeof(Texture2D)) as Texture2D;
				lstMetaSprite=new SpriteMetaData[dicSpriteMeta.Count];
				int count=0;
				foreach(KeyValuePair<string,SpriteMetaData> pair in dicSpriteMeta)
				{
					lstMetaSprite[count]=pair.Value;
					count++;
				}
				ti.isReadable=true;
				ti.mipmapEnabled=false;
				ti.spritesheet=lstMetaSprite;
				ti.textureType=TextureImporterType.Sprite;
				ti.spriteImportMode=SpriteImportMode.Multiple;
				ti.spritePixelsPerUnit=100;
				settings.textureFormat = TextureImporterFormat.ARGB32;
				settings.npotScale = TextureImporterNPOTScale.None;
				settings.alphaIsTransparency = true;
				ti.SetTextureSettings(settings);
				ti.maxTextureSize=4096;
				ti.mipmapEnabled=false;
				ti.spriteImportMode=SpriteImportMode.Multiple;
				AssetDatabase.ImportAsset(pathOutput);
				EditorUtility.SetDirty(mainTexture);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
				AssetDatabase.ImportAsset(pathOutput);
			}
			return true;
		}
		static public void BuildAtlas(TrimType trimType,List<SpriteElement> listSprite,string texturePath,int padingSize,bool append=false)
		{
			bool checkAppend=append;
			float prog =0.2f;
			EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
			Texture2D[] textArray=new Texture2D[listSprite.Count];
			for(int i=0;i<textArray.Length;i++)
			{
				textArray[i]=listSprite[i].texture;
			}
			Texture2D mainTexture=new Texture2D(8192,8192);
				Rect[] rects = mainTexture.PackTextures(textArray,padingSize, 8192,false);
			mainTexture.Apply();
			//ImportTextureUtil.MaxImportSettings(mainTexture);
			int xmin =0;
			int ymin =0;
			int cacheWidth=mainTexture.width;
			int cacheHeight=mainTexture.height;

			int optimizeWidth=cacheWidth;
			int optimizeHeight=cacheHeight;
			Texture2D mainTexture2=null;
			prog =0.4f;
			EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
			#region Trim to minimum Texture
			if(trimType==TrimType.TrimMinimum&&rects.Length>0)
			{
				float rectMinX=rects[0].xMin;
				float rectMinY=rects[0].yMin;
				float rectMaxX=rects[0].xMax;
				float rectMaxY=rects[0].yMax;
				for(int i=1;i<rects.Length;i++)
				{
					if(rects[i].xMin<rectMinX)
					{
						rectMinX=rects[i].xMin;
					}
					if(rects[i].yMin<rectMinY)
					{
						rectMinY=rects[i].yMin;
					}
					if(rects[i].xMax<rectMaxX)
					{
						rectMaxX=rects[i].xMax;
					}
					if(rects[i].yMax<rectMaxY)
					{
						rectMaxY=rects[i].yMax;
					}
				}
				int intRectMinX=(int)(rectMinX*cacheWidth);
				int intRectMinY=(int)(rectMinY*cacheHeight);
				int intRectMaxX=(int)(rectMaxX*cacheWidth);
				int intRectMaxY=(int)(rectMaxY*cacheHeight);

				Color32[] pixels = mainTexture.GetPixels32();
				xmin = mainTexture.width;
				int xmax = 0;
				ymin = mainTexture.height;
				int ymax = 0;
				int oldWidth = mainTexture.width;
				int oldHeight = mainTexture.height;
				
				// Trim solid pixels
				for (int y = 0, yw = oldHeight; y < yw; ++y)
				{
					for (int x = 0, xw = oldWidth; x < xw; ++x)
					{
						Color32 c = pixels[y * xw + x];
						
						if (c.a != 0)
						{
							if (y < ymin) ymin = y;
							if (y > ymax-1) ymax = y+1;
							if (x < xmin) xmin = x;
							if (x > xmax-1) xmax = x+1;
						}
					}
				}
				if(xmin>intRectMinX)
				{
					xmin=intRectMinX;
				}
				if(ymin>intRectMinY)
				{
					ymin=intRectMinY;
				}
				if(xmax<intRectMaxX)
				{
					xmax=intRectMaxX;
				}
				if(ymax<intRectMaxY)
				{
					ymax=intRectMaxY;
				}
				if(xmax-xmin>0&&ymax-ymin>0)
				{
					optimizeWidth=xmax-xmin;
					optimizeHeight=ymax-ymin;
					mainTexture2=new Texture2D(xmax-xmin,ymax-ymin);
					mainTexture2.SetPixels(mainTexture.GetPixels(xmin,ymin,xmax-xmin,ymax-ymin));
					mainTexture2.Apply();
					GameObject.DestroyImmediate(mainTexture);
					mainTexture=mainTexture2;
				}
			}
		
			#endregion

			prog =0.5f;
			EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
			#region Write New File
			byte[] byt = mainTexture.EncodeToPNG();
			if (texturePath != "") 
			{
				System.IO.File.WriteAllBytes(texturePath, byt);
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);         
			}
			AssetDatabase.ImportAsset(texturePath);
			#endregion
			EditorUtility.ClearProgressBar();
			prog =0.6f;
			EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
			mainTexture=AssetDatabase.LoadAssetAtPath(texturePath,typeof(Texture2D)) as Texture2D;
			TextureImporter ti = AssetImporter.GetAtPath(texturePath) as TextureImporter;
			TextureImporterSettings settings = new TextureImporterSettings();
			ti.ReadTextureSettings(settings);
			SpriteMetaData[] lstMetaSprite=new SpriteMetaData[listSprite.Count];
			if(append)
			{
				if(ti.spritesheet!=null&&ti.spritesheet.Length>0)
				{
					append=true;
					lstMetaSprite=ti.spritesheet;
				}
				else
				{
					append=false;
				}
			}
			for(int i=0;i<lstMetaSprite.Length;i++)
			{
				if(i<rects.Length)
				{
					SpriteMetaData metaSprite=new SpriteMetaData();
					if(append)
					{
						metaSprite=lstMetaSprite[i];
					}
					metaSprite.name=listSprite[i].name;
					Rect rectInfo=listSprite[i].GetSpriteRect();
				
					Rect rect=new Rect(rects[i].x*cacheWidth-xmin,rects[i].y*cacheHeight-ymin,rectInfo.width,rectInfo.height);
					if(rect.x+rect.width>optimizeWidth)
					{
						rect.width=optimizeWidth-rect.x;
					}
					if(rect.y+rect.height>optimizeHeight)
					{
						rect.height=optimizeHeight-rect.y;
					}
					metaSprite.rect=rect;
					int oWidth=listSprite[i].originalRect.width;
					int oHeight=listSprite[i].originalRect.height;
					if(oWidth<1)
						oWidth=1;
					if(oHeight<1)
						oHeight=1;
					int xLeft=listSprite[i].startX;
					int yTop=listSprite[i].startY;

					if(listSprite[i].IsOptimize())
					{
						float pivotX=listSprite[i].pivot.x*listSprite[i].originalRect.width;
						float pivotY=listSprite[i].pivot.y*listSprite[i].originalRect.height;
						pivotX=pivotX-xLeft;
						pivotY=pivotY-yTop;
						pivotX=pivotX/listSprite[i].optimizeRect.width;
						pivotY=pivotY/listSprite[i].optimizeRect.height;

						listSprite[i].SetPivot(new Vector2(pivotX,pivotY));
						metaSprite.pivot=new Vector2(pivotX,pivotY);
						metaSprite.alignment=listSprite[i].alignment;
					}
					else
					{
						metaSprite.pivot=listSprite[i].pivot;
						metaSprite.alignment=listSprite[i].alignment;
					}
					metaSprite.border=listSprite[i].border;
					lstMetaSprite[i]=metaSprite;
				}
			}
			prog =0.7f;
			EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
			ti.isReadable=true;
			ti.mipmapEnabled=false;
			ti.spritesheet=lstMetaSprite;
			ti.textureType=TextureImporterType.Sprite;
			ti.spriteImportMode=SpriteImportMode.Multiple;
			ti.spritePixelsPerUnit=100;
			settings.textureFormat = TextureImporterFormat.ARGB32;
			settings.npotScale = TextureImporterNPOTScale.None;
			settings.alphaIsTransparency = true;
			ti.SetTextureSettings(settings);
			ti.maxTextureSize=4096;
			ti.mipmapEnabled=false;
			ti.spriteImportMode=SpriteImportMode.Multiple;
			AssetDatabase.ImportAsset(texturePath);
			EditorUtility.SetDirty(mainTexture);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			AssetDatabase.ImportAsset(texturePath);
			prog =1.0f;
			EditorUtility.DisplayCancelableProgressBar("Creating Spritesheet", "Auto Build Atlas Sprites", prog);
			EditorUtility.ClearProgressBar();

			for(int i=0;i<listSprite.Count;i++)
			{
				listSprite[i].FreeMemory();
				System.GC.Collect();
			}
			return;
		}
	}
}
