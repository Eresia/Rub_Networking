using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public float speed;

	private Transform selfTranform;

	[SerializeField]
	private Transform selfCamera;

	void Awake()
	{
		selfTranform = GetComponent<Transform>();
	}

	void FixedUpdate()
	{
		Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	public void Move(float x, float z){
		// Debug.Log(x + " " + z);
		Vector3 newPosition = selfTranform.position;
		newPosition.x += x * speed * Time.deltaTime;
		newPosition.z += z * speed * Time.deltaTime;
		selfTranform.position = newPosition;
	}
}
