using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInteraction : Interactable {
	public CanvasGroup iconCanvasGroup;
	public float duration = 0.3f;
	private Animator animator;
	private Coroutine iconCoroutine;
	private bool animationOver;
	public AudioClip audioClip;

	private void Awake() {
		animator = GetComponent<Animator>();
		iconCanvasGroup.alpha = 0;
	}

	public override void Interact() {
		if (!animationOver) {
			animator.SetTrigger("start");
			animationOver = true;
			if (audioClip != null) {
				AudioSource.PlayClipAtPoint(audioClip, transform.position);
			}
		}
	}

	public override void ShowInteractionAvailable() {
		if (!animationOver) {
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