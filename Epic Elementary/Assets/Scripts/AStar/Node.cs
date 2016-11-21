using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {

	public bool isWalkable = true;
	public Vector3 worldPosition;
	public int gCost;
	public int hCost;
	public Grid.Point Position;
	public Node Parent;
	private int index;

	// Set vars
	public Node (bool walkable, Vector3 worldPos, Grid.Point Index) {
		this.isWalkable = walkable;
		this.worldPosition = worldPos;
		this.Position = Index;
	}

	// Determine fcost
	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int CompareTo(Node node) {
		return -(fCost.CompareTo (node.fCost) == 0 ? hCost.CompareTo (node.hCost) : fCost.CompareTo (node.fCost));
	}

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
		}
	}
}
