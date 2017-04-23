using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;
	[HideInInspector] public bool isWalking;
	[HideInInspector] public Transform currentTile;
	[HideInInspector] public bool isAnimating;

	private Transform target;
	private float myX, myZ;
	private float correctRotation;
	private bool doneRotation;
	private float rotY;
	private bool obstacleStop;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotateSpeed;
	[SerializeField] private Animator animator;
	[SerializeField] private Transform obstacle;
	[SerializeField] private Transform tiles;

	void Awake(){
		instance = this;
		isWalking = false;
		myX = transform.position.x;
		myZ = transform.position.z;
	}

	void Update(){
		if(isAnimating){
			return;
		}

		if(target != null && !doneRotation){
			if(transform.rotation.eulerAngles.y < correctRotation){
				rotY += Time.deltaTime * rotateSpeed;
			}else if(transform.rotation.eulerAngles.y > correctRotation){
				rotY -= Time.deltaTime * rotateSpeed;
			}
			transform.rotation = Quaternion.Euler(0, rotY, 0);

			if(Mathf.Abs(transform.rotation.eulerAngles.y - correctRotation) <= 8f || Mathf.Abs(transform.rotation.eulerAngles.y - (correctRotation + 360)) <= 8f){
				transform.rotation = Quaternion.Euler(0, correctRotation, 0);
				animator.SetTrigger("Walk");
				doneRotation = true;
			}
			// Debug.Log("my rotation: "+transform.rotation.eulerAngles.y +", target roation: "+correctRotation);
		}

		if(target != null && doneRotation){
			if(target.position.x == transform.position.x){
				// move z
				myX = target.position.x;
				if(target.position.z < transform.position.z){
					myZ -= Time.deltaTime * moveSpeed;
					if(obstacle != null){
						if(obstacle.position.z < transform.position.z && obstacle.position.z > target.position.z && transform.position.x == obstacle.position.x){
							obstacleStop = true;
						}
					}
				}
				if(target.position.z > transform.position.z){
					myZ += Time.deltaTime * moveSpeed;
					if(obstacle != null){
						if(obstacle.position.z > transform.position.z && obstacle.position.z < target.position.z && transform.position.x == obstacle.position.x){
							obstacleStop = true;
						}
					}
				}
			}else if(target.position.z == transform.position.z){
				// move x
				myZ = target.position.z;
				if(target.position.x < transform.position.x){
					myX -= Time.deltaTime * moveSpeed;
					if(obstacle != null){
						if(obstacle.position.x < transform.position.x && obstacle.position.x > target.position.x && transform.position.z == obstacle.position.z){
							obstacleStop = true;
						}
					}
				}
				if(target.position.x > transform.position.x){
					myX += Time.deltaTime * moveSpeed;
					if(obstacle != null){
						if(obstacle.position.x > transform.position.x && obstacle.position.x < target.position.x && transform.position.z == obstacle.position.z){
							obstacleStop = true;
						}
					}
				}
			}
			transform.position = new Vector3(myX, transform.position.y, myZ);

			if((Mathf.Abs(target.position.x - transform.position.x) <= 0.2f && Mathf.Abs(target.position.z - transform.position.z) <= 0.2f) || obstacleStop){
				if(!obstacleStop){
					transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
				}else{
					transform.position = new Vector3(currentTile.position.x, transform.position.y, currentTile.position.z);
				}
				target = null;
				animator.SetTrigger("Idle");
				isWalking = false;
			}
		}
	}

	public void UpdateTarget(Transform _target){
		if(_target.position.x == transform.position.x || _target.position.z == transform.position.z){
			target = _target;

			if(target.position.z < transform.position.z){
				correctRotation = 90;
			}
			if(target.position.z > transform.position.z){
				correctRotation = -90;
			}
			if(target.position.x < transform.position.x){
				correctRotation = 180;
			}
			if(target.position.x > transform.position.x){
				correctRotation = 0;
			}

			if(Mathf.Abs(transform.rotation.eulerAngles.y - (correctRotation+360)) < 180){
				correctRotation += 360;
			}

			if(target.tag != "LevelComplete"){
				if(!hasNext()){
					target = null;
					return;
				}
			}

			myX = transform.position.x;
			myZ = transform.position.z;

			obstacleStop = false;
			doneRotation = false;
			isWalking = true;
		}
	}

	public Transform GetTarget(){
		return target;
	}

	public bool hasNext(){
		bool hasNext = false;

		float xMove = 0;
		float zMove = 0;

		// Debug.Log("target x: "+ target.position.x +", myx: "+transform.position.x);
		// Debug.Log("target z: "+ target.position.z +", myz: "+transform.position.z);
		if(target.position.x == transform.position.x){
			// moving in z direction
			// Debug.Log("z diff: "+(transform.position.z - target.position.z));
			if(transform.position.z - target.position.z > 0){
				zMove = 1f;
			}else if(transform.position.z - target.position.z < 0){
				zMove = -1f;
			}
		}else if(target.position.z == transform.position.z){
			// moving in x direction
			// Debug.Log("x diff: "+(transform.position.x - target.position.x));
			if(transform.position.x - target.position.x > 0){
				xMove = 1f;
			}else if(transform.position.x - target.position.x < 0){
				xMove = -1f;
			}
		}

		// Debug.Log("xMove: "+zMove);
		// Debug.Log("zMove: "+zMove);

		if(xMove == 0 && zMove == 0){
			return false;
		}

		for(int i=0; i<tiles.childCount; i++){
			// Debug.Log(tiles.GetChild(i).name +", diff: "+(transform.position.z - tiles.GetChild(i).position.z));
			if(xMove != 0 && transform.position.x - tiles.GetChild(i).position.x == xMove && transform.position.z == tiles.GetChild(i).position.z){
				hasNext = true;
				break;
			}else if(zMove != 0 && transform.position.z - tiles.GetChild(i).position.z == zMove && transform.position.x == tiles.GetChild(i).position.x){
				hasNext = true;
				break;
			}
		}

		return hasNext;
	}

}
