using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class GlobalSwitcher : MonoBehaviour {
	public event Action Switch;

	[Button("Switch")]
	public void CallSwitch() {
		Switch?.Invoke();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log($"Escape Pressed");
#if !UNITY_EDITOR
Application.Quit();
#endif
		}
#if DEBUG
		if (Input.GetKeyDown(KeyCode.F1)) {
			CallSwitch();
		}
#endif
	}
}