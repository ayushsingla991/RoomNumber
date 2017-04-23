using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour {

	[SerializeField] private GameObject loadingPanel;
	[SerializeField] private float minAlpha, maxAlpha;
	private float panelAlpha;

	private bool changeScene;

	void Start(){
		loadingPanel.SetActive(true);
		panelAlpha = maxAlpha;
	}

	void Update(){
		if(!changeScene){
			if(panelAlpha > minAlpha){
				loadingPanel.GetComponent<CanvasRenderer>().SetAlpha(panelAlpha);
				panelAlpha -= Time.deltaTime;
			}else{
				loadingPanel.SetActive(false);
			}
		}else{
			loadingPanel.SetActive(true);
			if(panelAlpha < maxAlpha){
				loadingPanel.GetComponent<CanvasRenderer>().SetAlpha(panelAlpha);
				panelAlpha += Time.deltaTime;
			}else{
				SceneManager.LoadScene("MainMenu");
			}
		}
	}

	public void Menu(){
		changeScene = true;
	}

	public void OpenUrl(string url){
		Application.OpenURL(url);
	}

}
