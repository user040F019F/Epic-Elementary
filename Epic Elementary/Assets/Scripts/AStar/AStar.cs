using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

	private static Grid grid; // Keep track of Grid
	[SerializeField]
	private GameObject Player;
	[HideInInspector]
	public static Node PlayerNode; // Keep track of player location

	void Start () {
		grid = GetComponentInChildren<Grid> ();
	}

	void Update () {
		PlayerNode = grid.NodeFromWorldPoint (Player.transform.position);
	}

	public Vector3[] PathToPlayer (Vector3 Location) {
		return PathTo (grid.NodeFromWorldPoint(Location), PlayerNode);
	}

	public static Vector3[] PathTo (Node Location, Node Target) {
		MinHeap<Node> Opened = new MinHeap<Node> (grid.maxSize); // Nodes Found
		HashSet<Node> Closed = new HashSet<Node>(); // Nodes Explored
		Opened.Push(Location); // Open current location
		Node Current = null;
		while (Opened.Count > 0) {
			// Move Current node into explored
			Current = Opened.Pop ();
			Closed.Add (Current);
			// Stop if target node found
			if (Current.Position.x == Target.Position.x && Current.Position.y == Target.Position.y) {
				return ToWaypoints (ReversePath (Location, Target));
			}
			Node[] Neighbors = grid.GetNeighbors (Current);
			foreach (Node Neighbor in Neighbors) {
				if (Neighbor.isJumpable)
					return null;
				if (!Neighbor.isWalkable || Closed.Contains (Neighbor))
					continue;
				int Cost = Current.gCost + grid.GetDistance (Location, Target);
				if (!Opened.Contains (Neighbor) || Cost < Neighbor.hCost) {
					Neighbor.hCost = grid.GetHeuristic (Neighbor, Target);
					Neighbor.gCost = Cost;
					Neighbor.Parent = Current;
					if (!Opened.Contains (Neighbor)) {
					//	Debug.DrawLine (Current.worldPosition, Neighbor.worldPosition);
						Debug.DrawLine (Current.worldPosition, Neighbor.worldPosition);
						Opened.Push (Neighbor);
					}
				}
			}
		}
		Debug.Log ("Ending at: " + Current.Position.x + ", " + Current.Position.y);
		return null;
	}

	private static List<Node> ReversePath ( Node Location, Node Target) {
		List<Node> Path = new List<Node> ();
		Node Current = Target;
		while (Current != Location) {
			Path.Add (Current);
			Current = Current.Parent;
		}
		Path.Reverse();
		return Path;
	}

	private static Vector3[] ToWaypoints (List<Node> Nodes) {
		List<Vector3> Waypoints = new List<Vector3> ();
		Vector2 OldDirection = Vector2.zero;
		Node OldNode = Nodes [0];
		Nodes.RemoveAt (0);
		foreach (Node Node in Nodes) {
			Vector2 NewDirection = new Vector2 (OldNode.Position.x - Node.Position.x, OldNode.Position.y - Node.Position.y);
			if (NewDirection != OldDirection)
				Waypoints.Add (Node.worldPosition);
			OldDirection = NewDirection;
			OldNode = Node;
		}
		return Waypoints.ToArray ();
	}
}