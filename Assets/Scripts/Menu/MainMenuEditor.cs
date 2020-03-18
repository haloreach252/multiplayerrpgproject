using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class MainMenuEditor : MonoBehaviour {

	public Font selectedFont;

	[Button("Set all text to selected font")]
	private void SetFonts() {
		Text[] allTexts = GetComponentsInChildren<Text>(true);
		foreach (Text text in allTexts) {
			text.font = selectedFont;
		}
	}

	public int fontSizeModifier;

	[Button("Modify font size based on the fontSizeModifier")]
	private void ModifyFontSize() {
		Text[] allTexts = GetComponentsInChildren<Text>(true);
		foreach (Text text in allTexts) {
			text.fontSize += fontSizeModifier;
		}
	}

	public Color fontColor;

	[Button("Set all text color to fontColor")]
	private void SetColor() {
		Text[] allTexts = GetComponentsInChildren<Text>(true);
		foreach(Text text in allTexts) {
			text.color = fontColor;
			if (text.name == "Placeholder") {
				text.color = new Color(fontColor.r, fontColor.g, fontColor.b, 0.65f);
			}
		}
	}

}
