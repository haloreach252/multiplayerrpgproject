using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(PlayerEntity))]
public class PlayerEntityEditor : OdinEditor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
	}
}
