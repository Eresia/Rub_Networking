using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Linq;

public class SynchronizedObjectGestion {

	private object requireLock;

	public ConcurrentDictionary<int, SynchronizedObject> synchronizedObjects {get; private set;}

	private List<int> requiredObject;

	public SynchronizedObjectGestion(object requireLock){
		synchronizedObjects = new ConcurrentDictionary<int, SynchronizedObject>();
		requiredObject = new List<int>();
		this.requireLock = requireLock;
	}

	public bool Create(int i, SynchronizedObject obj){
		if(Has(i)){
			return false;
		}

		synchronizedObjects.TryAdd(i, obj);
		return true;
	}

	public bool Has(int i){
		return synchronizedObjects.ContainsKey(i);
	}

	public SynchronizedObject Get(int i){
		if(Has(i)){
			return synchronizedObjects[i];
		}

		return null;
	}

	public int[] GetExistantIds(){
		return synchronizedObjects.Keys.ToArray();
	}

	public bool Timeout(int i, float timeout){
		return (!Has(i) || (synchronizedObjects[i].timeout > timeout));
	}

	public void IncrementTime(int i, float deltaTime){
		if(Has(i)){
			synchronizedObjects[i].timeout += deltaTime;
		}
	}

	public void ResetTime(int i){
		if(Has(i)){
			synchronizedObjects[i].timeout = 0;
		}
	}

	public void Remove(int i){
		if(Has(i)){
			SynchronizedObject obj;
			synchronizedObjects.TryRemove(i, out obj);
		}
	}

	public void RemoveAndDestroy(int i){
		if(Has(i)){
			SynchronizedObject obj;
			synchronizedObjects.TryRemove(i, out obj);
			GameObject.Destroy(obj.gameObject);
		}
	}

	public void RequireObject(int i){
		lock(requireLock){
			if(!requiredObject.Contains(i)){
				requiredObject.Add(i);
			}
		}
	}

	public bool HasRequiredObject(int i){
		lock(requireLock){
			return requiredObject.Contains(i);
		}
	}

	public void EndRequireObject(int i){
		lock(requireLock){
			if(requiredObject.Contains(i)){
				requiredObject.Remove(i);
			}
		}
	}
}
