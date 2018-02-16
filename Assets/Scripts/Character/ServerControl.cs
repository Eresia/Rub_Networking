using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerControl : MonoBehaviour {

	public float moveSpeed;
	public float cameraHorizontalSpeed;
	public float cameraVerticalSpeed;

	[SerializeField]
	private Transform selfCamera;

	private Transform selfTranform;

	void Start()
	{
		selfTranform = GetComponent<Transform>();
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		Move(Input.GetAxis("Horizontal"), Input.GetAxis("Height"), Input.GetAxis("Vertical"), Input.GetButton("SpeedUp"));
		Rotate(Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
	}

	public void Move(float x, float y, float z, bool accelerate){
		float speed = moveSpeed;
		if(accelerate){
			speed *= 2;
		}

		Vector3 xSpeed = selfTranform.right * x * speed * Time.deltaTime;
		Vector3 ySpeed = selfTranform.up * y * speed * Time.deltaTime;
		Vector3 zSpeed = selfCamera.forward * z * speed * Time.deltaTime;
		selfTranform.position += xSpeed + ySpeed + zSpeed;
	}

	public void Rotate(float vertical, float horizontal){
		selfTranform.Rotate(0, cameraHorizontalSpeed * vertical, 0);
		selfCamera.Rotate(- cameraVerticalSpeed * horizontal, 0, 0);
	}
}
