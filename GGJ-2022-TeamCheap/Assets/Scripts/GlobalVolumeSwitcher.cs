using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeSwitcher : MonoBehaviour {
	public Volume young;
	public Volume old;
	private GlobalSwitcher globalSwitcher;

	private void Start() {
		globalSwitcher = FindObjectOfType<GlobalSwitcher>();
		globalSwitcher.Switch += Switch;
		young.weight = 0;
	}

	private void Switch() {
		Debug.Log($"Switcho volumi");
		young.weight = 1 - young.weight;
		old.weight = 1 - old.weight;
	}
}