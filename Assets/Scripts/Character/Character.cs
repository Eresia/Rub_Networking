using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float cameraHorizontalSpeed;

	[SerializeField]
	private float cameraVerticalSpeed;

	[SerializeField]
	private float minAngleRotation;

	[SerializeField]
	private float maxAngleRotation;

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private float jumpRaycastLength;

	[SerializeField]
	private float pushCooldown;

	[SerializeField]
	private float pushDistance;

	[SerializeField]
	private float pushForce;

	[SerializeField]
	private LayerMask pushLayer;

	[SerializeField]
	private float pushYDirection;

	[Space]

	[SerializeField]
	private LayerMask cantJumpLayers;

	[Space]

	public Transform head;

	public Transform selfCamera;

	public Transform selfTranform {get ; private set;}
	
	public Rigidbody selfRigidbody {get ; private set;}

	private bool canPush;
	private float actualPush;

	void Awake()
	{
		selfTranform = GetComponent<Transform>();
		selfRigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
		canPush = true;
		StartCoroutine(PushCooldownCoroutine());
	}

	public void Move(float x, float z){
		selfTranform.position += selfTranform.forward * z * moveSpeed * Time.deltaTime + selfTranform.right * x * moveSpeed * Time.deltaTime;
	}

	public void Jump(){
		bool onGround = Physics.Raycast(GetJumpPoint(1, 1), Vector3.down, jumpRaycastLength, ~cantJumpLayers);
		onGround |= Physics.Raycast(GetJumpPoint(-1, 1), Vector3.down, jumpRaycastLength, ~cantJumpLayers);
		onGround |= Physics.Raycast(GetJumpPoint(1, -1), Vector3.down, jumpRaycastLength, ~cantJumpLayers);
		onGround |= Physics.Raycast(GetJumpPoint(-1, -1), Vector3.down, jumpRaycastLength, ~cantJumpLayers);
		if(onGround){
			selfRigidbody.velocity = Vector3.up * jumpForce;
		}
	}

	public void Push(){
		if(canPush){
			RaycastHit hit;

			if(Physics.Raycast(selfTranform.position, head.forward, out hit, pushDistance, pushLayer)){
				Character other = hit.collider.GetComponent<Character>();
				if(other != null){
					Vector3 direction = head.forward;
					direction.y = pushYDirection;
					other.selfRigidbody.velocity = direction * pushForce;
				}
			}
			else{

			}

			actualPush = 0f;
			canPush = false;
		}
	}

	private IEnumerator PushCooldownCoroutine(){
		while(true){
			yield return new WaitWhile(() => canPush);
			do{
				yield return null;
				actualPush += Time.deltaTime;
			}while(actualPush < pushCooldown);

			actualPush = pushCooldown;
			
			canPush = true;
		}
	}

	public float GetPushProgession(){
		return actualPush / pushCooldown;
	}

	private Vector3 GetJumpPoint(int x, int z){
		return selfTranform.position + new Vector3((x * selfTranform.localScale.x) / 2, 0, (z * selfTranform.localScale.z) / 2);
	}

	public void Rotate(float vertical, float horizontal){
		selfTranform.Rotate(0, cameraVerticalSpeed * vertical, 0);
		float yaw = head.localRotation.eulerAngles.x - cameraHorizontalSpeed * horizontal;
		if(yaw > 180){
			yaw -= 360f;
		}
		yaw = Mathf.Clamp(yaw, minAngleRotation, maxAngleRotation);
		head.localRotation = Quaternion.Euler(new Vector3(yaw, 0, 0));
	}

	public void Kill(){
		Vector3 newPosition = GameManager.instance.map.GetRandomSpawnPosition();
		newPosition.y += selfTranform.localScale.y;
		selfTranform.position = newPosition;
		selfRigidbody.velocity = Vector3.zero;
	}
}
