using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {
	public void ChangeScene(){
		SceneManager.LoadScene("MainMenu");
	}
}
