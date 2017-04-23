using UnityEngine;

public class FootSteps : MonoBehaviour {

	[SerializeField] private AudioSource footSteps;

	public void PlaySound(){
		footSteps.Play();
	}

}
