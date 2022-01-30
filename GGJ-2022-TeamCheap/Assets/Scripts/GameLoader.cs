using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
	private bool isLoading = false;
	public CanvasGroup canvasGroup;
	public AudioClip loadAudioClip;

	private void Awake() {
		DontDestroyOnLoad(this);
	}

	public void LoadGameScene() {
		if (!isLoading) {
			AudioSource.PlayClipAtPoint(loadAudioClip, Vector3.zero);
			isLoading = true;
			StartCoroutine(LoadYourAsyncScene());
		}
	}

	private IEnumerator LoadYourAsyncScene() {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Principale");
		while (!asyncLoad.isDone) {
			canvasGroup.alpha += Time.deltaTime;
			yield return null;
		}
	}
}