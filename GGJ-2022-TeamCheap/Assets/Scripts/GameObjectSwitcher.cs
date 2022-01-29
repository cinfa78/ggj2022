using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour {
	public GameObject youngSide;
	public GameObject oldSide;
	private GlobalSwitcher globalSwitcher;

	private void Start() {
		globalSwitcher = FindObjectOfType<GlobalSwitcher>();
		globalSwitcher.Switch += Switch;
		youngSide.SetActive(false);
	}

	private void Switch() {
		youngSide.SetActive(!youngSide.activeSelf);
		oldSide.SetActive(!oldSide.activeSelf);
	}
}