using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchInteraction : Interactable {
	public CanvasGroup iconCanvasGroup;
	public float duration = 0.5f;
	private GlobalSwitcher gameObjectSwitcher;
	private CharacterSwitcher characterSwitcher;
	private Coroutine iconCoroutine;

	private void Awake() {
		gameObjectSwitcher = FindObjectOfType<GlobalSwitcher>();
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
		iconCanvasGroup.alpha = 0;
	}

	public override void Interact() {
		gameObjectSwitcher.CallSwitch();
	}

	public override void ShowInteractionAvailable() {
		//if (!characterSwitcher.IsHoldingObject) 
		{
			iconCanvasGroup.alpha = 1;
			if (iconCoroutine != null) StopCoroutine(iconCoroutine);
			iconCoroutine = StartCoroutine(FadeOutIcon());
		}
	}

	private IEnumerator FadeOutIcon() {
		float timer = duration;
		while (timer > 0) {
			float t = timer / duration;
			iconCanvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		iconCanvasGroup.alpha = 0;
	}
}