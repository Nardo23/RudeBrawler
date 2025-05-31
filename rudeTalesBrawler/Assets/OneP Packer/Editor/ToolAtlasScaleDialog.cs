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

public class ToolAtlasScaleDialog : EditorWindow {
	internal static ToolAtlasScaleDialog instance;
	
	public Texture2D mainTexture=null;

	public float scale=1;
	[MenuItem("Window/OneP Packer/Fix Resize Atlas")]
	public static void CreateWindow()
	{
		instance = GetWindow<ToolAtlasScaleDialog>();
		instance.title = "\u25A0 Fix Resize";
		instance.minSize = new Vector2(200, 200);
		instance.Show();
		instance.ShowUtility();
		instance.autoRepaintOnSceneChange = false;
		instance.wantsMouseMove = false;
	}
	public static void ShowFixResizeAtlasDialog(Texture2D _texture)
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
		GUILayout.Label("FIX RESIZE ATLAS SPRITE",EditorStyles.boldLabel);
		GUI.color = Color.white; 
		
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
			EditorGUILayout.LabelField("Scale Ratio :",new GUILayoutOption[]{GUILayout.Width(150f)});
			scale= EditorGUILayout.Slider(scale,0.01f,4,new GUILayoutOption[]{GUILayout.MaxWidth(1000f),GUILayout.Height(20)});
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			GUI.color = Color.green; 
			if(GUILayout.Button("FIX RESIZE ATLAS", new GUILayoutOption[]{GUILayout.Height(40f),GUILayout.MaxWidth(2000.0f)}))
			{
				if(mainTexture==null)
				{
					EditorUtility.DisplayDialog("Texture Input Warning","Please choose Texture Sprite in multiple mode","OK");
				}
				else
				{
					string path=AssetDatabase.GetAssetPath(mainTexture);
					TexturePacker.UpdateAtlasSpriteInfo(path,scale);
					scale=1;
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
