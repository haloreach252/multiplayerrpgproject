using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using System.Text;
using Sirenix.Serialization;

[System.Serializable]
public class CustomTerrainLayer {
	[HorizontalGroup("Toggles", LabelWidth = 70)]
	public string layer;
	[HorizontalGroup("Toggles", LabelWidth = 70)]
	public bool slopeMod = false;

	[ShowIfGroup("SH", MemberName = "slopeMod")]
	[BoxGroup("SH/Slope Settings")]
	[HorizontalGroup("SH/Slope Settings/Settings")]
	public float addToWeight = 0.1f;
	[HorizontalGroup("SH/Slope Settings/Settings")]
	public bool increaseWithSlope = true;
	
	[HideIfGroup("GS", MemberName = "slopeMod")]
	[HorizontalGroup("GS/General Settings", LabelWidth = 75)]
	[BoxGroup("GS/General Settings/Height")]
	public float minHeight = 0.1f;
	[BoxGroup("GS/General Settings/Height")]
	public float maxHeight = 0.5f;
	[BoxGroup("GS/General Settings/Slope")]
	public float minSlope = 0f;
	[BoxGroup("GS/General Settings/Slope")]
	public float maxSlope = 0.5f;
	[BoxGroup("GS/General Settings/Slope")]
	public float weight = 1.0f;

	public float GetWeight(float height, float slope, TerrainData data) {
		bool sendWeight = false;
		if (slopeMod) {
			if (increaseWithSlope) {
				weight = slope + addToWeight;
			} else {
				weight = 1 + addToWeight - slope;
			}
			return weight;
		} else {
			bool belowMaxSlope = slope <= maxSlope;
			bool aboveMinSlope = slope >= minSlope;
			bool belowMaxHeight = height <= (maxHeight * data.heightmapResolution);
			bool aboveMinHeight = height >= (minHeight * data.size.y);
			sendWeight = (aboveMinHeight && aboveMinSlope && belowMaxHeight && belowMaxSlope);
			if (sendWeight) {
				return weight;
			} else {
				float newWeight = 0f;
				if (!belowMaxSlope) {
					newWeight = (maxSlope + 0.05f) - slope;
					if(newWeight < 0) {
						newWeight = 0;
					}
				}
				if (!aboveMinSlope) {
					newWeight = 0.05f;
				}

				return newWeight;
			}
		}
	}
}
