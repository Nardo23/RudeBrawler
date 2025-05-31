/// <summary>
/// version 1.3
/// 2015-08-10
/// Copyright OnePStudio
/// mail: onepstudio@gmail.com
/// </summary>
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OnePStudio.OptimizeSprite
{
	public class OptimizeSprite
	{
		[MenuItem("Assets/Tools/OneP Packer/Optimize Sprite Atlas")]
		static public void AutoOptimizeSprite()
		{
			Object[] selection = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
			if(selection.Length>1)
			{
				EditorUtility.DisplayDialog("Error","Please just choose only one image texture Sprite","OK");
				return;  
			}
			Object obj=selection[0];
			if(obj is Texture2D)
			{
				Texture2D texture=(Texture2D)obj;
				string path=AssetDatabase.GetAssetPath(texture);

				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if(ti.textureType==TextureImporterType.Sprite&&
				   ti.spriteImportMode==SpriteImportMode.Multiple)
				{
					ToolSpriteDialog.ShowOptimizeAtlasDialog(texture);
				}
				else
				{
					EditorUtility.DisplayDialog("Error","Feature just optimize for Texture Sprite with mode Multiple, please choose correct type","OK");
				}

			}
			else
			{
				EditorUtility.DisplayDialog("Error","Please choose Texture type Sprite","OK");
				return;
			}
		}


		[MenuItem("Assets/Tools/OneP Packer/Edit Sprite Atlas")]
		static public void EditSpriteAtlas()
		{
			Object[] selection = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
			if(selection.Length>1)
			{
				EditorUtility.DisplayDialog("Error","Please just choose only one image texture Sprite","OK");
				return;  
			}
			Object obj=selection[0];
			if(obj is Texture2D)
			{
				Texture2D texture=(Texture2D)obj;
				string path=AssetDatabase.GetAssetPath(texture);
				
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if(ti.textureType==TextureImporterType.Sprite&&
				   ti.spriteImportMode==SpriteImportMode.Multiple)
				{
					ToolBuildAtlasDialog.ShowEditAtlasDialog(texture);
				}
				else
				{
					EditorUtility.DisplayDialog("Error","Feature just optimize for Texture Sprite with mode Multiple, please choose correct type","OK");
				}
				
			}
			else
			{
				EditorUtility.DisplayDialog("Error","Please choose Texture type Sprite","OK");
				return;
			}
		}

		[MenuItem("Assets/Tools/OneP Packer/Fix Resize Sprite Atlas")]
		static public void FixResizeAtlas()
		{
			Object[] selection = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
			if(selection.Length>1)
			{
				EditorUtility.DisplayDialog("Error","Please just choose only one image texture Sprite","OK");
				return;  
			}
			Object obj=selection[0];
			if(obj is Texture2D)
			{
				Texture2D texture=(Texture2D)obj;
				string path=AssetDatabase.GetAssetPath(texture);
				
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if(ti.textureType==TextureImporterType.Sprite&&
				   ti.spriteImportMode==SpriteImportMode.Multiple)
				{
					ToolAtlasScaleDialog.ShowFixResizeAtlasDialog(texture);
				}
				else
				{
					EditorUtility.DisplayDialog("Error","Feature just optimize for Texture Sprite with mode Multiple, please choose correct type","OK");
				}
				
			}
			else
			{
				EditorUtility.DisplayDialog("Error","Please choose Texture type Sprite","OK");
				return;
			}
		}
		public static void OnProcessOptimizeSprite(Texture2D texture,TrimType trimType,int padding,bool isOverride)
		{
			string path=AssetDatabase.GetAssetPath(texture);
			List<SpriteElement> listSprite=GetListSpriteFromTexture(texture);
			if(isOverride==true)
			{
				File.Delete(path);
				//AssetDatabase.Refresh();
				TexturePacker.BuildAtlas(trimType,listSprite,path,padding,true);
			}
			else
			{
				string path2=path.Replace(".png","_copy.png");
				TexturePacker.BuildAtlas(trimType,listSprite,path2,padding);
			}
			for(int i=0;i<listSprite.Count;i++)
			{
				GameObject.DestroyImmediate(listSprite[i].texture);
				listSprite[i]=null;
			}
		}

		public static List<SpriteElement> GetListSpriteFromTexture(Texture2D texture)
		{
			List<SpriteElement> listSprite=new List<SpriteElement>();
			string path=AssetDatabase.GetAssetPath(texture);
			ImportTextureUtil.MaxImportSettings(texture);
			ImportTextureUtil.ReadAndUnScale(texture);
			TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
			if(ti.spritesheet!=null)
			{
				
				ti.isReadable=true;
				for(int i=0;i<ti.spritesheet.Length;i++)
				{
					SpriteMetaData spriteMeta=ti.spritesheet[i];
					Rect rect=spriteMeta.rect;
					if(rect.x<0)
						rect.x=0;
					if(rect.y<0)
						rect.y=0;
					if(rect.x+rect.width>=texture.width)
					{
						rect.width=texture.width-rect.x;
					}
					
					if(rect.y+rect.height>=texture.height)
					{
						rect.height=texture.height-rect.y;
					}
					if(rect.width<1)
						rect.width=1;
					if(rect.height<1)
						rect.height=1;
					Texture2D texSprite=new Texture2D((int)rect.width,(int)rect.height);
					texSprite.SetPixels(texture.GetPixels((int)rect.x,(int)rect.y,(int)rect.width,(int)rect.height));
					texSprite.Apply();
					SpriteElement spriteElement=new SpriteElement(texSprite,false);
					spriteElement.TrimTexture();
					spriteElement.SetPivot(ImportTextureUtil.GetPivotFromMetaSprite(spriteMeta));
					spriteElement.name=spriteMeta.name;
					listSprite.Add(spriteElement);
					
				}
			}
			return listSprite;
		}


		public static void SetSpriteRawFromTexture(ref List<SpriteRawData> listSprite,Texture2D texture)
		{
			string path=AssetDatabase.GetAssetPath(texture);
			ImportTextureUtil.MaxImportSettings(texture);
			ImportTextureUtil.ReadAndUnScale(texture);
			TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
			Sprite[] spriteTemp = AssetDatabase.LoadAllAssetsAtPath(path)
				.OfType<Sprite>().ToArray();
			Dictionary<string,Sprite> dicSprites=new Dictionary<string,Sprite>();
			//Debug.LogError(ti.spriteImportMode+","+spriteTemp.Length);
			for(int i=0;i<spriteTemp.Length;i++)
			{
				dicSprites[spriteTemp[i].name]=spriteTemp[i];
			}
			if(ti.spriteImportMode==SpriteImportMode.Multiple)
			{
				if(ti.spritesheet!=null)
				{
					ti.isReadable=true;
					for(int i=0;i<ti.spritesheet.Length;i++)
					{
						SpriteMetaData spriteMeta=ti.spritesheet[i];
						Sprite sprite=null;
						dicSprites.TryGetValue(spriteMeta.name,out sprite);

						if(sprite!=null)
						{
							string name=sprite.name;
							while(true)
							{
								List<SpriteRawData> checkList = listSprite
									.Where(m => m.name == name).ToList();
								if(checkList.Count>0)
								{
									if(!name.Contains("_copy_"))
									{
										name=name+"_copy_1";
									}
									else
									{
										name+="1";
									}
								}
								else
								{
									break;
								}
							}
							Vector2 pivot=ImportTextureUtil.GetPivotFromMetaSprite(spriteMeta);
							int alignment=ImportTextureUtil.GetAlignment(pivot);
							SpriteRawData spriteElement=new SpriteRawData(sprite,name,pivot,alignment);
							listSprite.Add(spriteElement);
						}
					}
				}
			}
			else if(ti.spriteImportMode==SpriteImportMode.Single)
			{
				if(spriteTemp.Length>0)
				{
					Sprite sprite=spriteTemp[0];
					string name=sprite.name;
					while(true)
					{
						List<SpriteRawData> checkList = listSprite
							.Where(m => m.name == name).ToList();
						if(checkList.Count>0)
						{
							if(!name.Contains("_copy_"))
							{
								name=name+"_copy_1";
							}
							else
							{
								name+="1";
							}
						}
						else
						{
							break;
						}
					}
					Vector2 pivot=ti.spritePivot;
					int alignment=ImportTextureUtil.GetAlignment(pivot);
					SpriteRawData spriteElement=new SpriteRawData(sprite,name,pivot,alignment);
					listSprite.Add(spriteElement);
				}
			}
		}

	}

}