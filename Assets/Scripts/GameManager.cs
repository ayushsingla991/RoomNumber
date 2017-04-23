using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	[SerializeField] private Text insText;
	[SerializeField] private GameObject instructionPanel;
	[SerializeField] private Transform tiles;
	[SerializeField] private Transform player;
	[SerializeField] private bool gateOnX;
	[SerializeField] private GameObject loadingPanel;
	[SerializeField] private float minAlpha, maxAlpha;
	public bool useObstacle = true;

	private float panelAlpha;

	private int totalTiles;
	private int crossedTiles;
	private bool levelComplete;

	private string instruction;
	private char[] instructionChars;
	private int textIndex = 0;

	private bool gameOver;
	private bool changeScene;
	private bool menu;
	private bool reload;

	private bool gateOpened;
	private bool movePlayer;

	void Awake(){
		instance = this;
		loadingPanel.SetActive(true);
		loadingPanel.GetComponent<CanvasRenderer>().SetAlpha(maxAlpha);
		panelAlpha = maxAlpha;
	}

	void Start(){
		crossedTiles = 0;
		totalTiles = tiles.childCount;

		if(SceneManager.GetActiveScene().name == "Scene01" || SceneManager.GetActiveScene().name == "Scene02"){
			if(insText != null){
				instruction = insText.text;
				insText.text = "";
				instructionChars = instruction.ToCharArray();
				StartCoroutine(ShowText());
			}
		}

		int sceneIndex = SceneManager.GetActiveScene().buildIndex - 4;
		if(sceneIndex > PlayerPrefs.GetInt(Utils.LEVELS_UNLOCKED, 1)){
			PlayerPrefs.SetInt(Utils.LEVELS_UNLOCKED, sceneIndex);
		}

	}

	public void IncreaseTileCrossed(){
		crossedTiles++;
		Debug.Log("totalTiles: "+totalTiles+ ", crossed tiles: "+crossedTiles);
		// Debug.Log("gatex: "+Gate.instance.transform.position.x + ", playerx: "+player.position.x+", gatez: "+Gate.instance.transform.position.z + ", playerz: "+player.position.z);
		if(crossedTiles >= totalTiles){
			movePlayer = true;
		}
		if(crossedTiles >= totalTiles && ((Mathf.Abs(Gate.instance.transform.position.x - player.transform.position.x) <= 1.5f && Gate.instance.transform.position.z == player.transform.position.z) || (Mathf.Abs(Gate.instance.transform.position.z - player.transform.position.z) <= 1.5f && Gate.instance.transform.position.x == player.transform.position.x))){
			gateOpened = true;
			Debug.Log("Open Gate");
			Gate.instance.OpenGate();
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			menu = true;
			ChangeScene();
		}

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
				if(levelComplete){
					ChangeLevel();
				}else if(gameOver){
					ShowGameOver();
				}else if(menu){
					SceneManager.LoadScene("Levels");
				}else if(reload){
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
			}
		}

		if(levelComplete){
			if(!gateOnX){
				player.position = new Vector3(player.position.x, player.position.y, player.position.z + Time.deltaTime);
			}else{
				player.position = new Vector3(player.position.x + Time.deltaTime, player.position.y, player.position.z);
			}
			Invoke("ChangeScene", 2f);
		}

		if(movePlayer && !gateOpened){
			if(crossedTiles >= totalTiles && ((Mathf.Abs(Gate.instance.transform.position.x - player.transform.position.x) <= 1.5f && Gate.instance.transform.position.z == player.transform.position.z) || (Mathf.Abs(Gate.instance.transform.position.z - player.transform.position.z) <= 1.5f && Gate.instance.transform.position.x == player.transform.position.x))){
				gateOpened = true;
				Debug.Log("Open Gate");
				Gate.instance.OpenGate();
			}
		}

	}

	public void CompleteLevel(){
		levelComplete = true;
	}

	void ChangeScene(){
		changeScene = true;
	}

	void ChangeLevel(){
		int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
		// Debug.Log("nextScene: "+nextScene + ", sceneCount: "+SceneManager.sceneCountInBuildSettings);
		if(nextScene >= SceneManager.sceneCountInBuildSettings){
			SceneManager.LoadScene("GameComplete");
		}else{
			SceneManager.LoadScene(nextScene);
		}
	}

	public void GameOver(){
		gameOver = true;
		ChangeScene();
	}

	void ShowGameOver(){
		SceneManager.LoadScene("GameOver");
	}

	public void Reload(){
		reload = true;
		ChangeScene();
	}

	IEnumerator ShowText(){
		yield return new WaitForSeconds(0.03f);
		if(textIndex < instructionChars.Length){
			insText.text += instructionChars[textIndex];
			textIndex++;
			StartCoroutine(ShowText());
		}else{
			yield return new WaitForSeconds(4f);
			CloseIns();
		}
	}

	public void CloseIns(){
		instructionPanel.GetComponent<Animator>().SetTrigger("Close");
		Invoke("DisableIns", 0.5f);
	}

	void DisableIns(){
		instructionPanel.SetActive(false);
	}

}
