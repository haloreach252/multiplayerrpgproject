using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Newtonsoft.Json;

public class BuildEditor : OdinEditorWindow {

	[SerializeField]
	private int buildKey = 0;
	public string buildPath;
	public string[] scenes;

	private BuildPlayerOptions options = new BuildPlayerOptions();

	[MenuItem("Miniverse/Build Editor")]
	public static void ShowWindow() {
		GetWindow<BuildEditor>();
	}

	protected override void OnEnable() {
		base.OnEnable();
		buildKey = EditorPrefs.GetInt("BuildKey");
		buildPath = EditorPrefs.GetString("BuildPath");
		scenes = JsonConvert.DeserializeObject<string[]>(EditorPrefs.GetString("BuildScenes"));
	}

	protected override void OnDestroy() {
		EditorPrefs.SetInt("BuildKey", buildKey);
		EditorPrefs.SetString("BuildPath", buildPath);
		EditorPrefs.SetString("BuildScenes", JsonConvert.SerializeObject(scenes));
		base.OnDestroy();
	}

	[Button("Create Build")]
	public void CreateBuild() {
		string[] tempScenes = new string[scenes.Length];
		for (int i = 0; i < tempScenes.Length; i++) {
			tempScenes[i] = "Assets/Scenes/" + scenes[i] + ".unity";
		}
		buildKey++;
		MainMenuController.buildNumberText.text = "Build number: " + buildKey;
		options.options = BuildOptions.Development;
		options.target = BuildTarget.StandaloneWindows64;
		options.targetGroup = BuildTargetGroup.Standalone;
		options.locationPathName = buildPath + "/RPGProject.exe";
		options.scenes = tempScenes;
		EditorPrefs.SetInt("BuildKey", buildKey);
		EditorPrefs.SetString("BuildPath", buildPath);
		EditorPrefs.SetString("BuildScenes", JsonConvert.SerializeObject(scenes));
		BuildPipeline.BuildPlayer(options);
	}

}