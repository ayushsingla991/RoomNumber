using UnityEngine;

public class Portal : MonoBehaviour {

	[SerializeField] private Transform destination;
	[SerializeField] private Transform player;

	private float yPlayer;
	[HideInInspector] public bool portalUsed;

	void Start () {
		yPlayer = player.position.y;
	}

	void Update () {
		if(portalUsed){
			return;
		}
		if(PlayerController.instance.GetTarget() != null){
			return;
		}
		if(Mathf.Abs(player.position.x - transform.position.x) <= 0.2f && Mathf.Abs(player.position.z - transform.position.z) <= 0.2f){
			// player.position = new Vector3(destination.position.x, player.position.y, destination.position.z);
			// portalUsed = true;
			// destination.GetComponent<Portal>().portalUsed = true;
			PlayerController.instance.isAnimating = true;
			destination.GetComponent<Portal>().portalUsed = true;
			if(player.position.y >= -0.7f){
				player.position = new Vector3(player.position.x, player.position.y - Time.deltaTime, player.position.z);
			}else if(player.position.y <= -0.7f){
				player.position = new Vector3(destination.position.x, player.position.y, destination.position.z);
			}
		}else if(Mathf.Abs(destination.position.x - player.position.x) <= 0.2f && Mathf.Abs(destination.position.z - player.position.z) <= 0.2f){
			if(player.position.y <= yPlayer){
				player.position = new Vector3(player.position.x, player.position.y + Time.deltaTime, player.position.z);
			}
			if(Mathf.Abs(player.position.y - yPlayer) <= 0.2f){
				portalUsed = true;
				player.position = new Vector3(player.position.x, yPlayer, player.position.z);
				PlayerController.instance.currentTile = destination;
				PlayerController.instance.isAnimating = false;
				PlayerController.instance.isWalking = false;
			}
		}
	}
}
