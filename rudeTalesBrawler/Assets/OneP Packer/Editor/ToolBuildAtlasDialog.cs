/// <summary>
/// version 1.3
/// 2015-08-10
/// Copyright OnePStudio
/// mail: onepstudio@gmail.com
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEditor;
using OnePStudio.OptimizeSprite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*public enum TypeAutoLink
{
	NONE,
	ReMappingCurrentScene,
	ReMappingAllInProject
}*/

public class ToolBuildAtlasDialog : EditorWindow {
	internal static ToolBuildAtlasDialog instance;
	
	public TrimType trimType=TrimType.Trim2nTexture;
	public int paddingSize=2;

	public bool showTextureFile=true;
	//public List<Texture2D> listTexture=new List<Texture2D>();

	public Texture2D mainTexture;

	public bool autoSprite=false;
	public bool autoUGUIImage=false;

	//public TypeAutoLink typeAutoLink;
	private Texture2D oldMainTexture;
	private List<SpriteRawData> listShowUI=new List<SpriteRawData>();
	private List<SpriteRawData> listSpriteChoose=new List<SpriteRawData>();
	private List<SpriteRawData> listCurrentInMain=new List<SpriteRawData>();
	public Vector2 mScroll=Vector2.zero;
	[MenuItem("Window/OneP Packer/Build Atlas Sprite")]
	public static void CreateWindow()
	{
		instance = GetWindow<ToolBuildAtlasDialog>();
		instance.title = "\u25A0 Build Atlas";
		instance.minSize = new Vector2(450, 500);
		instance.Show();
		instance.ShowUtility();
		instance.autoRepaintOnSceneChange = false;
		instance.wantsMouseMove = false;
		ResetUIData();
	}
	static private void ResetUIData()
	{
		instance.mainTexture=null;
		instance.oldMainTexture=null;
		instance.listSpriteChoose=new List<SpriteRawData>();
		instance.listCurrentInMain=new List<SpriteRawData>();
		instance.listShowUI=new List<SpriteRawData>();
	}
	static private void RebuildListShowUI()
	{
		instance.listShowUI=new List<SpriteRawData>();
		instance.listShowUI.AddRange(instance.listCurrentInMain);
		for(int i=0;i<instance.listSpriteChoose.Count;i++)
		{
			SpriteRawData now=instance.listSpriteChoose[i];
			List<SpriteRawData> checkList = instance.listShowUI
				.Where(m => m.name == now.name ).ToList();
			if(checkList.Count>0)// same name
			{
				SpriteRawData old=checkList[0];
				now.spriteStaus=SpriteBuildStatus.UPDATE;
				int index=instance.listShowUI.IndexOf(old);
				instance.listShowUI[index]=now;
			}
			else// new
			{
				now.spriteStaus=SpriteBuildStatus.NEW;
				instance.listShowUI.Add(now);
			}
		}
	}
	public static void ShowBuildAtlasDialog(List<SpriteRawData> _listChoose)
	{
		if (instance == null) CreateWindow();
		ResetUIData();
		instance.listSpriteChoose=_listChoose;
		instance.listCurrentInMain=new List<SpriteRawData>();
		instance.mainTexture=null;
		instance.oldMainTexture=null;
		RebuildListShowUI();
		instance.Focus();
		instance.ShowPopup();
	}

	public static void ShowEditAtlasDialog(Texture2D _maintexture)
	{
		if (instance == null) CreateWindow();
		ResetUIData();
		instance.listSpriteChoose=new List<SpriteRawData>();;
		instance.listCurrentInMain=new List<SpriteRawData>();
		instance.oldMainTexture=null;
		instance.mainTexture=_maintexture;
		RebuildListShowUI();
		instance.Focus();
		instance.ShowPopup();
	}

	void OnGUI()
	{
		if (instance == null) CreateWindow();
		DrawToolbar();
	}
	static public void DrawHeader (string text)
	{
		GUILayout.Space(3f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(3f);
		
		GUI.changed = false;

		text = "<b><size=11>" + text + "</size></b>";

		text = "\u25BC " + text;
		GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f));
		GUILayout.Space(2f);
		GUILayout.EndHorizontal();
	}
	static public void BeginContents ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	static public void BeginContentsMaxHeight ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(4f);
		EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MaxHeight(20000f));
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	static public void EndContents ()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(3f);
		GUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}
	void ChooseMainTexture()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Choose An Atlas You want to Append If Creating new atlas, let't it is 'None(Object)'", MessageType.Info);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		{
			if(mainTexture!=null)
			{
				string path=AssetDatabase.GetAssetPath(mainTexture);
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if(ti.textureType!=TextureImporterType.Sprite)
				{
					EditorUtility.DisplayDialog("Texture Input Warning","Please choose Texture in Sprite mode","OK");
					mainTexture=null;
				}
			}
			EditorGUILayout.LabelField("Choose Atlas",GUILayout.Width(100));

			if(mainTexture==null)
			{
				GUI.color=Color.yellow;
			}
			else
			{
				GUI.color=Color.white;
			}
			mainTexture = EditorGUILayout.ObjectField("",mainTexture, typeof(Object),false,GUILayout.ExpandWidth(true)) as Texture2D;
			if(oldMainTexture!=mainTexture)
			{
				oldMainTexture=mainTexture;
				if(mainTexture==null)
				{
					listCurrentInMain=new List<SpriteRawData>();
				}
				else
				{
					string path=AssetDatabase.GetAssetPath(mainTexture);
					TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
					if(ti.textureType==TextureImporterType.Sprite)
					{
						listCurrentInMain=new List<SpriteRawData>();
						OptimizeSprite.SetSpriteRawFromTexture(ref listCurrentInMain,mainTexture);
						RebuildListShowUI();
					}
				}
			}
			GUI.color=Color.white;
			if(GUILayout.Button("Create New Atlas", new GUILayoutOption[]{GUILayout.Height(20f),GUILayout.Width(100.0f)}))
			{
				mainTexture=null;
				listCurrentInMain=new List<SpriteRawData>();
				if(mainTexture!=null)
				{
					OptimizeSprite.SetSpriteRawFromTexture(ref listCurrentInMain,mainTexture);
				}
				RebuildListShowUI();
			}
		}
		
		EditorGUILayout.EndHorizontal();
	}
	void DrawToolbar()
	{
		EditorGUILayout.BeginVertical();
		#region Build Atlas Tool
		GUI.color = Color.cyan;  
		DrawHeader("BUILD ATLAS SPRITE");
		GUI.color = Color.white; 
		BeginContents();
		ChooseMainTexture();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		{
			GUI.color = Color.white;
			EditorGUILayout.LabelField("Choose Trimming Type:",new GUILayoutOption[]{GUILayout.Width(150f)});
			trimType= (TrimType)EditorGUILayout.EnumPopup(trimType, "DropDown", new GUILayoutOption[]{GUILayout.MaxWidth(1000f),GUILayout.Height(20)});
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		{
			GUI.color = Color.white;
			EditorGUILayout.LabelField("Padding Size:",new GUILayoutOption[]{GUILayout.Width(150f)});
			paddingSize= EditorGUILayout.IntSlider(paddingSize,0,8,new GUILayoutOption[]{GUILayout.MaxWidth(1000f),GUILayout.Height(20)});
		}
		EditorGUILayout.EndHorizontal();
		//EditorGUILayout.BeginHorizontal();
		{
			GUI.color = Color.white;

			EditorGUILayout.LabelField("Auto mapping in current scene",new GUILayoutOption[]{GUILayout.Width(205f)});
			
			autoSprite= EditorGUILayout.Toggle("\t \u25AB SpriteRenderer  ",autoSprite,new GUILayoutOption[]{GUILayout.MaxWidth(1000f),GUILayout.Height(20)});
			autoUGUIImage= EditorGUILayout.Toggle("\t \u25AB UGUI Image  ",autoUGUIImage,new GUILayoutOption[]{GUILayout.MaxWidth(1000f),GUILayout.Height(20)});

			//typeAutoLink=(TypeAutoLink)EditorGUILayout.EnumPopup(typeAutoLink,"DropDown",new GUILayoutOption[]{GUILayout.MaxWidth(1000f),GUILayout.Height(20)});
		}
		//EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("If you want to re-mapping sprite (from input) in current scene with current altas, please press Auto Link is true", MessageType.Info);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		{
			GUI.color = Color.green; 
			if(GUILayout.Button("BUILD", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				List<SpriteRawData> lstFinal=new List<SpriteRawData>();
				for(int i=0;i<listShowUI.Count;i++)
				{
					if(listShowUI[i].isAttact)
					{
						lstFinal.Add(listShowUI[i]);
					}
				}
				if(lstFinal.Count<1)
				{
					EditorUtility.DisplayDialog("Error","Please choose atleast one image to build","OK");
					return;  
				}
				else
				{
					if(mainTexture==null)
					{
						listCurrentInMain=new List<SpriteRawData>();
						RebuildListShowUI();
					}
					string texturePath ="";
					if(mainTexture==null)
					{
						texturePath = EditorUtility.SaveFilePanelInProject("Save As", "New Atlas", "png", "Save atlas as...");
						if(texturePath.Length==0)
							return;
					}
					else
					{
						texturePath=AssetDatabase.GetAssetPath(mainTexture);
						ImportTextureUtil.MaxImportSettings(mainTexture);
						ImportTextureUtil.ReadAndUnScale(mainTexture);
					}
					TexturePacker.AutoBuildAtlasFromSpriteRaw(lstFinal,texturePath,trimType,paddingSize,mainTexture);
					mainTexture=AssetDatabase.LoadAssetAtPath(texturePath,typeof(Texture2D)) as Texture2D;

					#region auto link
					if(autoSprite||autoUGUIImage)
					{
						List<Sprite> spriteTemp = AssetDatabase.LoadAllAssetsAtPath(texturePath)
							.OfType<Sprite>().ToList();
						Dictionary<string,Sprite> dicSprites=new Dictionary<string, Sprite>();
						for(int i=0;i<lstFinal.Count;i++)
						{
							SpriteRawData raw=lstFinal[i];
							List<Sprite> checkList = spriteTemp
								.Where(m => m.name == raw.name).ToList();
							if(checkList.Count>0)
							{
								if(raw.spriteStaus!=SpriteBuildStatus.OLD)
								{
									dicSprites[raw.fullPath]=checkList[0];
									//Debug.LogError("them zo ne:"+checkList[0].name);
								}
							}
						}
						List<GameObject> list=new List<GameObject>();
						GameObject[] gameObjs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
						for(int i=0;i<gameObjs.Length;i++)
						{
							if(gameObjs[i].transform.parent==null)
							{
								list.Add(gameObjs[i]);
								//Debug.Log(gameObjs[i].name);
							}
						}
						for(int i=0;i<list.Count;i++)
						{
							if(autoSprite)
							{
								SpriteRenderer[] listSprite=list[i].GetComponentsInChildren<SpriteRenderer>(true);
								for(int x=0;x<listSprite.Length;x++)
								{
									SpriteRenderer spriteRender=listSprite[x];
									if(spriteRender.sprite!=null)
									{
										Sprite _spriteNew=null;
										dicSprites.TryGetValue(ImportTextureUtil.GetFullPath(spriteRender.sprite),out _spriteNew);
										if(_spriteNew!=null)
										{
											//Debug.LogError("spriteRender:"+spriteRender.name+" is "+ _spriteNew.name);
											spriteRender.sprite=_spriteNew;
										}
									}
								}
							}
							if(autoUGUIImage)
							{

								UnityEngine.UI.Image[] listImage=list[i].GetComponentsInChildren<UnityEngine.UI.Image>(true);
								for(int x=0;x<listImage.Length;x++)
								{
									UnityEngine.UI.Image image=listImage[x];
									if(image.sprite!=null)
									{
										Sprite _spriteNew=null;
										dicSprites.TryGetValue(ImportTextureUtil.GetFullPath(image.sprite),out _spriteNew);
										if(_spriteNew!=null)
										{
											image.sprite=_spriteNew;
										}
									}
								}
							}
						}
					}
					#endregion
					#region refresh UI
					listCurrentInMain=new List<SpriteRawData>();
					if(mainTexture!=null)
					{
						OptimizeSprite.SetSpriteRawFromTexture(ref listCurrentInMain,mainTexture);
					}
					listSpriteChoose.Clear();
					
					RebuildListShowUI();
					for(int i=0;i<listShowUI.Count;i++)
					{
						listShowUI[i].spriteStaus=SpriteBuildStatus.OLD;
						listShowUI[i].isAttact=true;
					}
					#endregion
				}

			}
		}
		EditorGUILayout.EndHorizontal();
		EndContents();
		#endregion

		GUI.color = Color.cyan;  
		DrawHeader("Sprites");
		GUI.color = Color.white; 

		BeginContentsMaxHeight();
		mScroll = GUILayout.BeginScrollView(mScroll);

		EditorGUILayout.BeginHorizontal();
		{
			showTextureFile=EditorGUILayout.Foldout(showTextureFile,"List Texture Files ("+listShowUI.Count+")");
		}
		EditorGUILayout.EndHorizontal();
		
		if(showTextureFile)
		{
			for(int i=0;i<listShowUI.Count;i++)
			{
				EditorGUILayout.BeginHorizontal();
				string name="null ";
				if(listShowUI[i]!=null)
				{
					name=listShowUI[i].name;
					if(name.Length>20)
					{
						name=name.Substring(0,18)+"...";
					}
				}
				EditorGUILayout.LabelField("\t"+(i+1).ToString()+" : "+name,EditorStyles.boldLabel);
				EditorGUILayout.ObjectField(listShowUI[i].sprite, typeof(Object),false,GUILayout.MaxWidth(1000f));
				if(!listShowUI[i].isAttact)
				{
					GUI.color=Color.red;
					EditorGUILayout.LabelField("Removed",GUILayout.Width(60));
					GUI.color=Color.green;
					if(GUILayout.Button("+", new GUILayoutOption[]{GUILayout.Width(30.0f)}))
					{
						listShowUI[i].isAttact=true;
					}
					GUI.color=Color.white;
				}
				else
				{
					if(listShowUI[i].spriteStaus==SpriteBuildStatus.UPDATE)
					{
						GUI.color=Color.yellow;
						EditorGUILayout.LabelField("Update",GUILayout.Width(45));
						GUI.color=Color.white;
					}
					else if(listShowUI[i].spriteStaus==SpriteBuildStatus.NEW)
					{
						GUI.color=Color.green;
						EditorGUILayout.LabelField("New",GUILayout.Width(60));
						GUI.color=Color.red;
						if(GUILayout.Button("x", new GUILayoutOption[]{GUILayout.Width(30.0f)}))
						{
							listShowUI[i].isAttact=false;
						}
						GUI.color=Color.white;
					}
					else
					{
						GUI.color=Color.red;
						if(GUILayout.Button("-", new GUILayoutOption[]{GUILayout.Width(20.0f)}))
						{
							listShowUI[i].isAttact=false;
						}
						GUI.color=Color.white;
					}
				}

				EditorGUILayout.EndHorizontal();
			}
		}
		
		GUILayout.EndScrollView();
		EndContents();

		EditorGUILayout.EndVertical();

	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
