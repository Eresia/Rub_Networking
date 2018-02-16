using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public float moveSpeed;
	public float cameraHorizontalSpeed;
	public float cameraVerticalSpeed;

	public float minAngleRotation;
	public float maxAngleRotation;

	private Transform selfTranform;
	private Rigidbody selfRigidbody;

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private Transform selfCamera;

	void Awake()
	{
		selfTranform = GetComponent<Transform>();
		selfRigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}

	void FixedUpdate()
	{
		Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		Rotate(Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
		if(Input.GetButtonDown ("Jump")){
			Jump();
		}
	}

	public void Move(float x, float z){
		selfTranform.position += selfTranform.forward * z * moveSpeed * Time.deltaTime + selfTranform.right * x * moveSpeed * Time.deltaTime;
	}

	public void Jump(){
		bool onGround = Physics.Raycast(GetJumpPoint(1, 1), Vector3.down, 1.5f);
		onGround |= Physics.Raycast(GetJumpPoint(-1, 1), Vector3.down, 1.5f);
		onGround |= Physics.Raycast(GetJumpPoint(1, -1), Vector3.down, 1.5f);
		onGround |= Physics.Raycast(GetJumpPoint(-1, -1), Vector3.down, 1.5f);
		if(onGround){
			selfRigidbody.velocity = Vector3.up * jumpForce;
		}
	}

	private Vector3 GetJumpPoint(int x, int z){
		return selfTranform.position + new Vector3((x * selfTranform.localScale.x) / 2, 0, (z * selfTranform.localScale.z) / 2);
	}

	public void Rotate(float vertical, float horizontal){
		selfTranform.Rotate(0, cameraVerticalSpeed * vertical, 0);
		float yaw = selfCamera.localRotation.eulerAngles.x - cameraHorizontalSpeed * horizontal;
		if(yaw > 180){
			yaw -= 360f;
		}
		yaw = Mathf.Clamp(yaw, minAngleRotation, maxAngleRotation);
		selfCamera.localRotation = Quaternion.Euler(new Vector3(yaw, 0, 0));
	}
}
