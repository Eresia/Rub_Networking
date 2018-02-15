using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MonoBehaviour {

	[System.Serializable]
	public struct Neighbours{
		public MapBlock up;
		public MapBlock down;
		public MapBlock front;
		public MapBlock back;
		public MapBlock left;
		public MapBlock right;

		public Neighbours(MapBlock me, MapBlock up, MapBlock down, MapBlock front, MapBlock back, MapBlock left, MapBlock right){
			this.up = up;
			if(up != null){
				up.neighbours.down = me;
				up.CalculateVisibility();
			}

			this.down = down;
			if(down != null){
				down.neighbours.up = me;
				down.CalculateVisibility();
			}

			this.front = front;
			if(front != null){
				front.neighbours.back = me;
				front.CalculateVisibility();
			}

			this.back = back;
			if(back != null){
				back.neighbours.front = me;
				back.CalculateVisibility();
			}

			this.left = left;
			if(left != null){
				left.neighbours.right = me;
				left.CalculateVisibility();
			}

			this.right = right;
			if(right != null){
				right.neighbours.left = me;
				right.CalculateVisibility();
			}
		}

		public bool IsNotArounded(){
			return (up == null) || (down == null) || (front == null) || (back == null) || (left == null) || (right == null);
		}
	}

	public Neighbours neighbours; 

	[HideInInspector]
	public Renderer selfRenderer;

	void Awake()
	{
		selfRenderer = GetComponent<Renderer>();
	}

	public void CalculateVisibility(){
		selfRenderer.gameObject.SetActive(neighbours.IsNotArounded());
		// selfRenderer.enabled = true;
	}
}
