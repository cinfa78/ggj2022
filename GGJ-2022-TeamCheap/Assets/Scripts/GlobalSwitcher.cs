using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSwitcher : MonoBehaviour {
	public event Action Switch;

	private void CallSwitch() {
		Switch?.Invoke();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F1)) {
			CallSwitch();
		}
	}
}