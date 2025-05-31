using UnityEngine;
/// <summary>
/// version 1.3
/// 2015-08-10
/// Copyright OnePStudio
/// mail: onepstudio@gmail.com
/// </summary>
/// 
using System.Collections;
using UnityEditor;
using OnePStudio.OptimizeSprite;

public class ToolSpriteDialog : EditorWindow {
	internal static ToolSpriteDialog instance;

	public Texture2D mainTexture=null;
	public TrimType trimType=TrimType.Trim2nTexture;
	public int paddingSize=0;
	public bool isOverride=true;

	[MenuItem("Window/OneP Packer/Optimize Sprite Atlas")]
	public static void CreateWindow()
	{
		instance = GetWindow<ToolSpriteDialog>();
		instance.title = "\u25A0 Optimize Sprite Atlas";
		instance.minSize = new Vector2(200, 200);
		instance.maxSize = new Vector2(300, 260);
		instance.Show();
		instance.ShowUtility();
		instance.autoRepaintOnSceneChange = false;
		instance.wantsMouseMove = false;
	}
	public static void ShowOptimizeAtlasDialog(Texture2D _texture)
	{
		if (instance == null) CreateWindow();
		instance.Focus();
		instance.ShowPopup();
		instance.mainTexture=_texture;
	}

	void OnGUI()
	{
		if (instance == null) CreateWindow();
		DrawToolbar();
	}
	void DrawToolbar()
	{
		EditorGUILayout.BeginVertical();
		GUI.color = Color.green;  
		GUILayout.Label("OPTIMIZE ATLAS SPRITE",EditorStyles.boldLabel);
		GUI.color = Color.white; 
		EditorGUILayout.BeginHorizontal();
		{
			if(mainTexture!=null)
			{
				string path=AssetDatabase.GetAssetPath(mainTexture);
				TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
				if(ti.textureType!=TextureImporterType.Sprite|| ti.spriteImportMode!=SpriteImportMode.Multiple)
				{
					EditorUtility.DisplayDialog("Texture Input Warning","Please choose Texture Sprite in multiple mode","OK");
					mainTexture=null;
				}
			}
			
			GUI.color = Color.white;
			if(mainTexture==null)
			{
				EditorGUILayout.HelpBox("Atlas Texture Sprite:\n(only allow Texture Sprite in Sprite Mode Multiple)\n", MessageType.Warning);
			}
			else
			{
				EditorGUILayout.HelpBox("Atlas Texture Sprite:\n(only allow Texture Sprite in Sprite Mode Multiple)\n", MessageType.Info);
			}
			if(mainTexture==null)
			{
				GUI.color=Color.red;
			}
			else
			{
				GUI.color=Color.white;
			}
			mainTexture = EditorGUILayout.ObjectField("",mainTexture, typeof(Texture2D),true,new GUILayoutOption[]{GUILayout.Width(70.0f),GUILayout.Height(70.0f)}) as Texture2D;
		}
		EditorGUILayout.EndHorizontal();
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
		EditorGUILayout.BeginHorizontal();
		{
			GUI.color = Color.white;
			EditorGUILayout.LabelField("Override Current Atlas:",new GUILayoutOption[]{GUILayout.Width(150f)});
			isOverride= EditorGUILayout.Toggle(isOverride,new GUILayoutOption[]{GUILayout.Width(20f),GUILayout.Height(20)});
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		{
			GUI.color = Color.green; 
			if(GUILayout.Button("OPTIMIZE", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				if(mainTexture==null)
				{
					EditorUtility.DisplayDialog("Texture Input Warning","Please choose Texture Sprite in multiple mode","OK");
				}
				else
				{
					OptimizeSprite.OnProcessOptimizeSprite(mainTexture,trimType,paddingSize,isOverride);
				}
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
