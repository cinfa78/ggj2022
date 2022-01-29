using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FlashOnSwitch : MonoBehaviour {
	public CanvasGroup canvasGroup;
	public float duration;
	private GlobalSwitcher globalSwitcher;

	private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Start() {
		globalSwitcher = FindObjectOfType<GlobalSwitcher>();
		globalSwitcher.Switch += Switch;
	}

	private void Switch() {
		StopAllCoroutines();
		StartCoroutine(fadingOut());
	}

	private IEnumerator fadingOut() {
		canvasGroup.alpha = 1;
		float timer = duration;
		while (timer > 0) {
			float t = timer / duration;
			canvasGroup.alpha = Mathf.Lerp(0, 1, t * t);
			timer -= Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 0;
	}

	// Update is called once per frame
	void Update() { }
}