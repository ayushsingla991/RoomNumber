using UnityEngine;

public class Obstacle : MonoBehaviour {

	public static Obstacle instance;
	[HideInInspector] public Transform currentTile;

	[SerializeField] private float maxMove;
	[SerializeField] private float originalMoveSpeed;
	[SerializeField] private Transform tiles;

	private Transform nextTile;
	private float xUnitsToMove;
	private float zUnitsToMove;
	private float xOld;
	private float zOld;
	private float moveSpeed;

	private bool hasNext;

	void Start () {
		instance = this;
		xUnitsToMove = 0;
		xUnitsToMove = 0;
		moveSpeed = originalMoveSpeed;
	}

	void Update(){
		if(!hasNext && currentTile != null && (xUnitsToMove != 0f || zUnitsToMove != 0f)){
			transform.position = new Vector3(currentTile.position.x, transform.position.y, currentTile.position.z);
			ResetValues();
		}

		if(xUnitsToMove != 0f){
			transform.position = new Vector3(transform.position.x + (Time.deltaTime * moveSpeed), transform.position.y, transform.position.z);

			if(Mathf.Abs(transform.position.x - (xOld + xUnitsToMove)) <= 0.2f){
				transform.position = new Vector3(xOld + xUnitsToMove, transform.position.y, transform.position.z);
				ResetValues();
			}
		}

		if(zUnitsToMove != 0f){
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (Time.deltaTime * moveSpeed));

			if(Mathf.Abs(transform.position.z - (zOld + zUnitsToMove)) <= 0.2f){
				transform.position = new Vector3(transform.position.x, transform.position.y, zOld + zUnitsToMove);
				ResetValues();
			}
		}
	}

	void ResetValues(){
		moveSpeed = originalMoveSpeed;
		xUnitsToMove = 0f;
		zUnitsToMove = 0f;
	}

	public void Move(Transform source){
		float xDistance = transform.position.x - source.position.x;
		float zDistance = transform.position.z - source.position.z;

		if(xDistance != 0f && zDistance != 0f){
			return;
		}

		if(xDistance != 0){
			if(xDistance > 0){ 
				xUnitsToMove = maxMove - Mathf.Abs(xDistance) + 1;
			}else if(xDistance < 0){
				xUnitsToMove = -(maxMove - Mathf.Abs(xDistance) + 1);
				moveSpeed = -moveSpeed;
			}
			xOld = transform.position.x;
			zUnitsToMove = 0f;
		}

		if(zDistance != 0){
			if(zDistance > 0){
				zUnitsToMove = maxMove - Mathf.Abs(zDistance) + 1;
			}else if(zDistance < 0){
				zUnitsToMove = -(maxMove - Mathf.Abs(zDistance) + 1);
				moveSpeed = -moveSpeed;
			}
			zOld = transform.position.z;
			xUnitsToMove = 0f;
		}

		if(xDistance != 0 || zDistance != 0){
			SetCurrentTile(currentTile);
		}
	}

	public void SetCurrentTile(Transform _tile){
		currentTile = _tile;


		hasNext = false;
		for(int i=0; i<tiles.childCount; i++){
			if(xUnitsToMove < 0){
				if(currentTile.position.x - tiles.GetChild(i).position.x == 1f && currentTile.position.z == tiles.GetChild(i).position.z){
					hasNext = true;
					break;
				}
			}else if(xUnitsToMove > 0){
				if(currentTile.position.x - tiles.GetChild(i).position.x == -1f && currentTile.position.z == tiles.GetChild(i).position.z){
					hasNext = true;
					break;
				}
			}

			if(zUnitsToMove < 0){
				// Debug.Log(tiles.GetChild(i).name);
				if(currentTile.position.z - tiles.GetChild(i).position.z == 1f && currentTile.position.x == tiles.GetChild(i).position.x){
					hasNext = true;
					break;
				}
			}else if(zUnitsToMove > 0){
				if(currentTile.position.z - tiles.GetChild(i).position.z == -1f && currentTile.position.x == tiles.GetChild(i).position.x){
					hasNext = true;
					break;
				}
			}
		}

		if(!hasNext){
			transform.position = new Vector3(currentTile.position.x, transform.position.y, currentTile.position.z);
			ResetValues();
		}

	}

	public float GetMaxMoves(){
		return maxMove;
	}

}
