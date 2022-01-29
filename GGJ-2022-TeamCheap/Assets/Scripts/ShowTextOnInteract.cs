using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ShowTextOnInteract : Interactable {
	public Canvas canvas;
	private CanvasGroup canvasGroup;
	private TMP_Text label;
	private int textLength;
	[FormerlySerializedAs("timeout")] public float minTimeout;
	public float fadeDuration;

	private void Awake() {
		canvasGroup = canvas.GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0;
		label = canvas.GetComponentInChildren<TMP_Text>();
		textLength = label.text.Length;
	}

	public override void Interact() {
		StopAllCoroutines();
		StartCoroutine(FadingOut());
	}

	private IEnumerator FadingOut() {
		canvasGroup.alpha = 0;
		float timer = fadeDuration / 2;
		while (timer > 0) {
			float t = timer / fadeDuration;
			canvasGroup.alpha = 1 - t;
			timer -= Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 1;
		float timeout = minTimeout - fadeDuration / 2 + textLength * 0.1f;
		yield return new WaitForSeconds(timeout);
		timer = fadeDuration;
		while (timer > 0) {
			float t = timer / fadeDuration;
			canvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 0;
	}
}