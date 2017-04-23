using UnityEngine;

public class Tile : MonoBehaviour {

	private Transform player;
	private Transform obstacle;

	[SerializeField] private Texture hoverTexture;
	[SerializeField] private Color startColor;
	[SerializeField] private Color finalColor;
	[SerializeField] private Color crossedColor;
	private Color currentColor;
	private bool clicked;

	private bool playerCrossed;
	private int crossCount = 0;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player").transform;
		if(GameManager.instance.useObstacle){
			obstacle = GameObject.FindGameObjectWithTag("Obstacle").transform;
		}
	}

	void Update(){
		float xDistancePlayer = Mathf.Abs(transform.position.x - player.position.x);
		float zDistancePlayer = Mathf.Abs(transform.position.z - player.position.z);

		if(xDistancePlayer <= 0.3f && zDistancePlayer <= 0.3f && PlayerController.instance.currentTile != transform){
			// Debug.Log("current tile: "+name);
			crossCount++;
			PlayerController.instance.currentTile = transform;
			GameManager.instance.IncreaseTileCrossed();
		}

		if(PlayerController.instance.isAnimating){
			return;
		}
		if(obstacle != null){
			float xDistanceObstacle = Mathf.Abs(transform.position.x - obstacle.position.x);
			float zDistanceObstacle = Mathf.Abs(transform.position.z - obstacle.position.z);
			if(xDistanceObstacle <= 0.3f && zDistanceObstacle <= 0.3f && Obstacle.instance.currentTile != transform){
				crossCount++;
				// Obstacle.instance.currentTile = transform;
				Obstacle.instance.SetCurrentTile(transform);
				GameManager.instance.IncreaseTileCrossed();
				PlayerCrossed();
			}
		}

		if(crossCount > 1 && xDistancePlayer <= 0.3f && zDistancePlayer <= 0.3f){
			GameManager.instance.GameOver();
			return;
		}

		if(xDistancePlayer <= 0.3f && zDistancePlayer <= 0.3f){
			// Debug.Log("crossed: "+name);
			PlayerCrossed();
			return;
		}

		if(playerCrossed){
			return;
		}
		currentColor = Color.Lerp(currentColor, finalColor, Time.deltaTime);
		GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", currentColor);
	}

	void OnMouseDown(){
		if(PlayerController.instance.isAnimating){
			return;
		}
		if(playerCrossed){
			return;
		}
		if(PlayerController.instance.isWalking){
			return;
		}
		clicked = true;
		// PlayerController.instance.gameStarted = true;
		currentColor = startColor;
		PlayerController.instance.UpdateTarget(transform);
		GetComponent<MeshRenderer>().material.SetTexture ("_EmissionMap", null);
		GetComponent<MeshRenderer>().material.SetColor ("_EmissionColor", currentColor);
	}

	void OnMouseEnter(){
		if(PlayerController.instance.isAnimating){
			return;
		}
		if(playerCrossed){
			return;
		}
		// Debug.Log(name+", walking: "+PlayerController.instance.isWalking);
		if(PlayerController.instance.isWalking){
			return;
		}
		clicked = false;
		currentColor = Color.white;
		GetComponent<MeshRenderer>().material.SetTexture ("_EmissionMap", hoverTexture);
		GetComponent<MeshRenderer>().material.SetColor ("_EmissionColor", currentColor);
	}

	void OnMouseExit(){
		if(playerCrossed){
			return;
		}
		if(PlayerController.instance.isWalking){
			return;
		}
		if(!clicked){
			currentColor = Color.black;
			GetComponent<MeshRenderer>().material.SetColor ("_EmissionColor", currentColor);
		}
	}

	void PlayerCrossed(){
		playerCrossed = true;
		currentColor = crossedColor;
		GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", currentColor);
		GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", null);
	}

}
