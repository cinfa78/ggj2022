using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
	private bool isLoading = false;
	public CanvasGroup canvasGroup;

	private void Awake() {
		
		DontDestroyOnLoad(this);
	}

	public void LoadGameScene() {
		if (!isLoading) {
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