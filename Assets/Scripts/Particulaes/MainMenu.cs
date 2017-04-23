using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	[SerializeField] private GameObject loadingPanel;
	[SerializeField] private float minAlpha, maxAlpha;
	private float panelAlpha;

	private bool showLevels;

	void Start(){
		loadingPanel.SetActive(true);
		loadingPanel.GetComponent<CanvasRenderer>().SetAlpha(maxAlpha);
		panelAlpha = maxAlpha;
	}

	void Update(){
		if(!showLevels){
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
				SceneManager.LoadScene("Levels");
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

	}

	public void LoadLevels(){
		showLevels = true;
	}

	public void OpenUrl(string url){
		Application.OpenURL(url);
	}

}
