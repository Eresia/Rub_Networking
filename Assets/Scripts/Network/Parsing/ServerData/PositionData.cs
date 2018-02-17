using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData : ServerData {

	private int id;

	private SerializableTransform transform;

	public PositionData(int id, SerializableTransform transform){
		this.id = id;
		this.transform = transform;
	}

	protected override bool Validate(){
		return false;
	}

	protected override bool Execute(){
		return false;
	}

	public override void ExecuteOnMainThread(){
		
	}
}
