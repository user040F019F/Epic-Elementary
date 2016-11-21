using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {

	private static float rad, dia;
    public static float radius {
		get {
			return rad;
		}
		set {
			rad = value;
			dia = value * 2;
		}
	}
	public static float diameter {
		get {
			return dia;
		}
		set {
			rad = value / 2;
			dia = value;
		}
	}

	public bool isWalkable = true, isJumpable = false;
	public Vector3 worldPosition;
	public int gCost;
	public int hCost;
	public Point Position;
	public Node Parent;
	private int index;

	// Set vars
	public Node (Vector3 worldPos, Point Index, bool walkable, bool jumpable) {
		this.isWalkable = walkable;
		this.worldPosition = worldPos;
		this.Position = Index;
		this.isJumpable = jumpable;
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
