using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SynchronizedObject))]
public abstract class SynchronizedElement : MonoBehaviour{

	protected SynchronizedObject synchronizedObject;

	protected virtual void Awake() {
		synchronizedObject = GetComponent<SynchronizedObject>();
	}

	public abstract ServerData SynchronizeFromServer();

	public abstract ClientData SynchronizeFromClient();
}
