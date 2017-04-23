using UnityEngine;

public class Gate : MonoBehaviour {

	public static Gate instance;
	[SerializeField] private Animator animator;

	void Start(){
		instance = this;
	}

	public void OpenGate(){
		animator.SetTrigger("Open");
	}

	public void OnGateOpen(){
		Debug.Log("gate open");
		GameManager.instance.CompleteLevel();
	}

}
