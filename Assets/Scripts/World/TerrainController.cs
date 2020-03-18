using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.IO;
using Newtonsoft.Json;
using Sirenix.Serialization;

[ExecuteInEditMode, System.Serializable, ShowOdinSerializedPropertiesInInspector]
public class TerrainController : SerializedMonoBehaviour {
	float unit;
	private Terrain[] terrains;

	#region Splat Mapping
	#region Settings

	[TabGroup("Splat Mapping", GroupID = "TerrainSettings")]
	public TerrainLayer roadLayer;
	[TabGroup("Splat Mapping", GroupID = "TerrainSettings")]
	public CustomTerrainLayer[] customLayers;
	[TabGroup("Splat Mapping", GroupID = "TerrainSettings")]
	public Dictionary<string, TerrainLayer> terrainLayers;

	[Space(4)]

	[Header("Generation Settings"), TabGroup("Splat Mapping", GroupID = "TerrainSettings")]
	public bool scaleToTerrain = false;

	#endregion

	[Button("Set Splatmap"), TabGroup("Splat Mapping", GroupID = "TerrainSettings")]
	private void AssignMap() {
		terrains = GetComponentsInChildren<Terrain>();

		foreach (Terrain terrain in terrains) {
			TerrainData terrainData = terrain.terrainData;
			unit = 1f / (terrainData.size.x - 1);

			TerrainLayer[] tempLayers = new TerrainLayer[customLayers.Length + 2];

			for (int i = 0; i < customLayers.Length; i++) {
				tempLayers[i] = terrainLayers[customLayers[i].layer.ToLower()];
			}
			tempLayers[tempLayers.Length - 1] = roadLayer;

			terrainData.terrainLayers = tempLayers;

			float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

			for (int y = 0; y < terrainData.alphamapHeight; y++) {
				for (int x = 0; x < terrainData.alphamapWidth; x++) {
					float y_01 = (float)y / (float)terrainData.alphamapHeight;
					float x_01 = (float)x / (float)terrainData.alphamapWidth;

					float[] splatWeights = new float[terrainData.alphamapLayers - 1];

					float height = GetHeightAtPoint(x_01 * terrainData.size.x, y_01 * terrainData.size.z, terrainData);
					float slope = GetSlopeAtPoint(x_01 * terrainData.size.x, y_01 * terrainData.size.z, terrainData);

					for (int i = 0; i < customLayers.Length; i++) {
						splatWeights[i] = customLayers[i].GetWeight(height, slope, terrainData);
					}

					float z = splatWeights.Sum();

					for (int i = 0; i < terrainData.alphamapLayers - 1; i++) {
						splatWeights[i] /= z;

						splatmapData[y, x, i] = splatWeights[i];
					}
				}
			}

			terrainData.SetAlphamaps(0, 0, splatmapData);
		}

		Debug.Log("Finished setting splatmaps");
	}

	#region Non tabbed stuff
	private float GetSlopeAtPoint(float pointX, float pointZ, TerrainData terrainData) {
		return terrainData.GetSteepness(unit * pointX, unit * pointZ) / 90.0f;
	}

	private float GetHeightAtPoint(float pointX, float pointZ, TerrainData terrainData) {
		float height = terrainData.GetInterpolatedHeight(unit * pointX, unit * pointZ);

		if (scaleToTerrain) {
			return height / terrainData.size.y;
		} else {
			return height;
		}
	}
	#endregion
	#endregion

	#region Update terrain
	[TabGroup("Update Terrain Settings", GroupID = "TerrainSettings", Order = 1)]
	#region Settings

	#endregion

	[Button("Update Terrain Settings")]
	public void UpdateTerrains() {
		terrains = GetComponentsInChildren<Terrain>();
	}
	#endregion

	[TabGroup("Save Load", GroupID = "TerrainSettings", Order = 2)]
	[FolderPath]
	public string saveFilePath;
	[TabGroup("Save Load", GroupID = "TerrainSettings", Order = 2)]
	public string saveFileName;
	[TabGroup("Save Load", GroupID = "TerrainSettings", Order = 2)]
	[FilePath]
	public string loadFilePath;

	[TabGroup("Save Load", GroupID = "TerrainSettings", Order = 2)]
	[Button("Save Terrain Layers")]
	private void SaveTerrainLayers() {
		string jsonString = JsonConvert.SerializeObject(customLayers);
		string path = saveFilePath + "/" + saveFileName + ".json";
		File.WriteAllText(path, jsonString);
	}

	[TabGroup("Save Load", GroupID = "TerrainSettings", Order = 2)]
	[Button("Load Terrain Layer")]
	private void LoadTerrainLayers() {
		string inString = File.ReadAllText(loadFilePath);
		customLayers = JsonConvert.DeserializeObject<CustomTerrainLayer[]>(inString);
	}
}