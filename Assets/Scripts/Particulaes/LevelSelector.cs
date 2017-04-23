using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

	[SerializeField] private GameObject loadingPanel;
	[SerializeField] private float minAlpha, maxAlpha;
	[SerializeField] private GameObject[] selectors;
	[SerializeField] private Text[] levels;

	private float panelAlpha;

	private bool changeScene;

	string[] scenes = {"Scene01", "Scene02", "Scene03", "Scene04", "Scene05", "Scene06", "Scene07", "Scene08"};
	string selectedScene;

	private bool back;

	void Start(){
		loadingPanel.SetActive(true);
		panelAlpha = maxAlpha;

		for(int i=0; i<selectors.Length; i++){
			if(i >= PlayerPrefs.GetInt(Utils.LEVELS_UNLOCKED, 1)){
				selectors[i].GetComponent<Button>().interactable = false;
				levels[i].color = selectors[i].GetComponent<Button>().colors.disabledColor;
			}
		}

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
				if(back){
					SceneManager.LoadScene("MainMenu");
				}else{
					SceneManager.LoadScene(selectedScene);
				}
			}
		}
	}

	public void ChangeScene(int index){
		if(index < PlayerPrefs.GetInt(Utils.LEVELS_UNLOCKED, 1)){
			changeScene = true;
			selectedScene = scenes[index];
		}
	}

	public void Back(){
		changeScene = true;
		back = true;
	}

}
