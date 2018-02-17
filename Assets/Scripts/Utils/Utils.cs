using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializableVector3{
	public float x;
	public float y;
	public float z;

	public SerializableVector3(float x, float y, float z){
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public SerializableVector3(Vector3 from){
		this.x = from.x;
		this.y = from.y;
		this.z = from.z;
	}

	public SerializableVector3(Quaternion from) : this(from.eulerAngles){ }

	public Vector3 ToVector3(){
		return new Vector3(x, y, z);
	}

	public Quaternion ToQuaternion(){
		return Quaternion.Euler(x, y, z);
	}
}

[System.Serializable]
public struct SerializableTransform{
	public SerializableVector3 position;
	public SerializableVector3 rotation;
	public SerializableVector3 scale;

	public SerializableTransform(Transform transform){
		position = new SerializableVector3(transform.position);
		rotation = new SerializableVector3(transform.rotation);
		scale = new SerializableVector3(transform.localScale);
	}
}