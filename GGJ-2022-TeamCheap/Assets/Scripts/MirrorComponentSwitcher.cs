using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorComponentSwitcher : MonoBehaviour {
	public MirrorTransform young;
	public MirrorTransform old;
	private GlobalSwitcher globalSwitcher;

	private void Start() {
		globalSwitcher = FindObjectOfType<GlobalSwitcher>();
		globalSwitcher.Switch += Switch;
		young.enabled = true;
		old.enabled = false;
	}

	private void Switch() {
		young.enabled = !young.enabled;
		old.enabled = !old.enabled;
	}
}