using UnityEngine;

public class ObstacleRemover : MonoBehaviour {

	[SerializeField] private Transform obstacle;
	[SerializeField] private AudioSource audioSource;

	private GameObject particles;

	void Start(){
		particles = transform.FindChild("ClickEffect").gameObject;
	}

	void OnMouseDown(){
		particles.SetActive(true);
		Invoke("DisableParticles", 0.5f);
		audioSource.Play();
		if(GameManager.instance.useObstacle && ((Mathf.Abs(transform.position.x - obstacle.position.x) <= obstacle.GetComponent<Obstacle>().GetMaxMoves() && transform.position.z == obstacle.position.z) || (Mathf.Abs(transform.position.z - obstacle.position.z) <= obstacle.GetComponent<Obstacle>().GetMaxMoves() && transform.position.x == obstacle.position.x))){
			Obstacle.instance.Move(transform);
		}
	}

	void DisableParticles(){
		particles.SetActive(false);
	}

}
