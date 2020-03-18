using UnityEditor;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(Entity))]
public class EntityEditor : OdinEditor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
	}
}
