using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GlobalSwitcher : MonoBehaviour {
	public GameObject oldCharacter;
	public GameObject youngCharacter;
	public event Action Switch;

	[Button("Switch")]
	public void CallSwitch() {
		Switch?.Invoke();
	}


	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Debug.Log($"Escape Pressed");
		}
#if DEBUG
		if (Input.GetKeyDown(KeyCode.F1)) {
			CallSwitch();
		}
#endif
	}
}