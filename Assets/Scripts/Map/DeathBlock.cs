﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlock : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Loose");
	}
}
