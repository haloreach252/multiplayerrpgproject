using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;

public class TestSave : SerializedMonoBehaviour {
	public CustomTerrainLayer testLayer;

	[FolderPath]
	public string saveFilePath;
	public string saveFileName;
	[FilePath]
	public string loadFilePath;

	[Button("Save Terrain Layers")]
	private void SaveTerrainLayers() {
		string path = saveFilePath + "/" + saveFileName + ".ctl";
		byte[] bytes = SerializationUtility.SerializeValue(testLayer, DataFormat.Binary);
		File.WriteAllBytes(path, bytes);
	}

	[Button("Load Terrain Layer")]
	private void LoadTerrainLayers() {
		string path = loadFilePath;
		if (!File.Exists(path)) return;

		byte[] bytes = File.ReadAllBytes(path);
		testLayer = SerializationUtility.DeserializeValue<CustomTerrainLayer>(bytes, DataFormat.Binary);
	}
}