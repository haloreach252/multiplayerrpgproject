using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class HouseEditor : OdinEditorWindow {

	[MenuItem("Miniverse/House Editor")]
	private static void OpenWindow() {
		GetWindow<HouseEditor>().Show();
	}

	protected override void OnEnable() {
		base.OnEnable();
		floors = new List<HouseFloor>();
	}

	public HouseCreatorSettings settings;
	private float[] rotations = new float[] {
		0f,90f,180f,270f
	};

	public string floorName = "floora";
	public int floorWidth = 4;
	public int floorHeight = 4;

	public Vector3 houseSpawnPosition = new Vector3();

	public List<HouseFloor> floors;

	[Button("Create floor")]
	private void CreateFloor() {
		HouseFloor newFloor = CreateInstance<HouseFloor>();
		AssetDatabase.CreateAsset(newFloor, "Assets/Scripts/World/HouseFloors/" + floorName + ".asset");
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		newFloor.Init(floorWidth, floorHeight, settings.housePrefabPreviews, settings.rotationPreviews);
		floors.Add(newFloor);
		currentFloor = newFloor;
	}

	[InlineEditor(InlineEditorObjectFieldModes.Boxed)]
	public HouseFloor currentFloor;

	[Button("Create House")]
	private void CreateHouse() {
		GameObject houseParent = new GameObject("House Parent");
		houseParent.transform.position = houseSpawnPosition;
		for (int i = 0; i < currentFloor.tiles.GetLength(0); i++) {
			for (int k = 0; k < currentFloor.tiles.GetLength(1); k++) {
				if(currentFloor.tiles[i,k] == 0) {
					continue;
				} else {
					GameObject piece = Instantiate(settings.housePrefabs[currentFloor.tiles[i, k] - 1], houseParent.transform);
					piece.transform.rotation = Quaternion.Euler(0, rotations[currentFloor.tileRots[i, k]], 0);
					piece.transform.position += new Vector3(i * 3, 0, k * 3);
				}
			}
		}
	}
}

[CreateAssetMenu()]
public class HouseCreatorSettings : SerializedScriptableObject {
	public GameObject[] housePrefabs;
	public Texture[] housePrefabPreviews;
	public Texture[] rotationPreviews;
}

[System.Serializable]
public class HouseFloor : SerializedScriptableObject {
	public int floorWidth;
	public int floorHeight;

	public static Texture[] housePrefabPreviews;
	public static Texture[] rotationPreviews;

	[TableMatrix(DrawElementMethod = "DrawColoredCell", SquareCells = true, RowHeight = 20, ResizableColumns = false), SerializeField]
	public int[,] tiles;

	[TableMatrix(DrawElementMethod = "DrawColoredRotCell", SquareCells = true, RowHeight = 20, ResizableColumns = false), SerializeField]
	public int[,] tileRots;

	public void Init(int width, int height) {
		floorWidth = width;
		floorHeight = height;
		tiles = new int[width, height];
		tileRots = new int[width, height];
	}

	public void Init(int width, int height, Texture[] inTextures) {
		floorWidth = width;
		floorHeight = height;
		tiles = new int[width, height];
		tileRots = new int[width, height];
		housePrefabPreviews = inTextures;
	}

	public void Init(int width, int height, Texture[] inTexturesA, Texture[] inTexturesB) {
		floorWidth = width;
		floorHeight = height;
		tiles = new int[width, height];
		tileRots = new int[width, height];
		housePrefabPreviews = inTexturesA;
		rotationPreviews = inTexturesB;
	}

	private static int DrawColoredCell(Rect rect, int value) {
		if(Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
			value++;
			if (value > 2) value = 0;
			GUI.changed = true;
			Event.current.Use();
		}

		EditorGUI.DrawPreviewTexture(rect, housePrefabPreviews[value]);
		return value;
	}

	private static int DrawColoredRotCell(Rect rect, int value) {
		if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
			value++;
			if (value > 3) value = 0;
			GUI.changed = true;
			Event.current.Use();
		}

		Debug.Log(rect.x + " " + rect.y);

		EditorGUI.DrawPreviewTexture(rect, rotationPreviews[value]);
		return value;
	}
}