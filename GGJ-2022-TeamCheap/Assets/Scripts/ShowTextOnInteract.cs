using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ShowTextOnInteract : Interactable {
	public Canvas canvasText;
	public CanvasGroup iconCanvasGroup;
	private CanvasGroup textCanvasGroup;
	private TMP_Text label;
	private int textLength;
	public float duration = 0.3f;
	 public float minTimeout;
	public float fadeDuration;
	private Coroutine iconCoroutine;
	private Coroutine textCoroutine;

	private void Awake() {
		textCanvasGroup = canvasText.GetComponent<CanvasGroup>();
		textCanvasGroup.alpha = 0;
		label = canvasText.GetComponentInChildren<TMP_Text>();
		textLength = label.text.Length;
		iconCanvasGroup.alpha = 0;
	}

	public override void Interact() {
		if (textCoroutine != null) StopCoroutine(textCoroutine);
		textCoroutine = StartCoroutine(FadeOutText());
	}

	public override void ShowInteractionAvailable() {
		iconCanvasGroup.alpha = 1;
		if (iconCoroutine != null) StopCoroutine(iconCoroutine);
		iconCoroutine = StartCoroutine(FadeOutIcon());
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

	private IEnumerator FadeOutText() {
		textCanvasGroup.alpha = 0;
		float timer = fadeDuration / 2;
		while (timer > 0) {
			float t = timer / fadeDuration;
			textCanvasGroup.alpha = 1 - t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 1;
		float timeout = minTimeout - fadeDuration / 2 + textLength * 0.1f;
		yield return new WaitForSeconds(timeout);
		timer = fadeDuration;
		while (timer > 0) {
			float t = timer / fadeDuration;
			textCanvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 0;
	}
}