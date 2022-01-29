using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ShowTextOnInteract : Interactable {
	public Canvas canvasText;
	public CanvasGroup iconCanvasGroup;
	[TextArea] public string message;
	private CanvasGroup textCanvasGroup;
	private TMP_Text label;
	private int textLength;
	public float duration = 0.3f;
	public float minTimeout;
	public float perLetterTime = 0.05f;
	public float fadeDuration;
	private Collider collider;
	public GameObject[] objectsToActivate;
	public GameObject[] objectsToDeactivate;
	private Coroutine iconCoroutine;
	private Coroutine textCoroutine;
	private bool skip;

	public bool showTextMultipleTimes;
	[ReadOnly] public bool storyAdvanced;

	private void Awake() {
		textCanvasGroup = canvasText.GetComponent<CanvasGroup>();
		textCanvasGroup.alpha = 0;
		label = canvasText.GetComponentInChildren<TMP_Text>();
		textLength = label.text.Length;
		iconCanvasGroup.alpha = 0;
		foreach (GameObject o in objectsToActivate) {
			o.SetActive(false);
		}
		collider = GetComponent<Collider>();
	}

	private void Start() {
		label.text = message;
	}

	public override void Interact() {
		if (showTextMultipleTimes) {
			if (textCoroutine != null) StopCoroutine(textCoroutine);
			textCoroutine = StartCoroutine(FadeOutText());
		}
		else {
			if (!storyAdvanced) {
				if (textCoroutine != null) StopCoroutine(textCoroutine);
				textCoroutine = StartCoroutine(FadeOutTextOnce());
				storyAdvanced = true;
				Destroy(collider);
			}
		}
	}

	public override void SecondaryInteract() {
		skip = true;
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
		while (timer > 0 && !skip) {
			float t = timer / fadeDuration;
			textCanvasGroup.alpha = 1 - t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 1;
		float timeout = minTimeout - fadeDuration / 2f + textLength * perLetterTime;
		timer = timeout;
		while (timer > 0 && !skip) {
			timer -= Time.deltaTime;
			yield return null;
		}
		timer = fadeDuration;
		while (timer > 0 && !skip) {
			float t = timer / fadeDuration;
			textCanvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 0;
		skip = false;
	}

	private IEnumerator FadeOutTextOnce() {
		textCanvasGroup.alpha = 0;
		float timer = fadeDuration / 2;
		while (timer > 0 && !skip) {
			float t = timer / fadeDuration;
			textCanvasGroup.alpha = 1 - t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 1;
		float timeout = minTimeout - fadeDuration / 2 + textLength * 0.1f;
		timer = timeout;
		while (timer > 0 && !skip) {
			timer -= Time.deltaTime;
			yield return null;
		}
		timer = fadeDuration;
		while (timer > 0 && !skip) {
			float t = timer / fadeDuration;
			textCanvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 0;
		foreach (GameObject o in objectsToActivate) {
			o.SetActive(true);
		}
		foreach (GameObject o in objectsToDeactivate) {
			o.SetActive(false);
		}
		skip = false;
	}
}