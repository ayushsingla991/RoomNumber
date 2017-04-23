using UnityEngine;

public class Sound : MonoBehaviour {

	void Awake(){
		DontDestroyOnLoad(gameObject);
	}
}
