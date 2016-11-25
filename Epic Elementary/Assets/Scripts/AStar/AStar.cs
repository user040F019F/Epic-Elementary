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
		return PathTo (grid.NodeFromWorldPoint(Location).Clone(), grid.NodeFromWorldPoint(Player.transform.position).Clone());
	}

	public static Vector3[] PathTo (Node Location, Node Target) {
		MinHeap<Node> Opened = new MinHeap<Node> (grid.maxSize); // Nodes Found
		HashSet<Node> Closed = new HashSet<Node>(); // Nodes Explored
		Opened.Push(Location); // Open current location
        int i = 0;
		while (Opened.Count > 0 && i < 3) {
			// Move Current node into explored
			Node Current = Opened.Pop ();
            i++;
			Closed.Add (Current);
			// Stop if target node found
			if (Current == Target) {
				return ToWaypoints (ReversePath (Location, Current));
			}
			Node[] Neighbors = grid.GetNeighbors (Current);
			foreach (Node Neighbor in Neighbors) {
				if (Neighbor.isJumpable)
					return null;
				if (!Neighbor.isWalkable || Closed.Contains (Neighbor))
					continue;
				int Cost = Current.gCost + grid.GetDistance (Location, Target);
				if (!Opened.Contains (Neighbor) || Cost < Neighbor.hCost) {
                    Neighbor.Parent = Current;
                    Neighbor.gCost = Cost;
                    Neighbor.hCost = grid.GetHeuristic (Neighbor, Target);
					if (!Opened.Contains (Neighbor)) {
				    	Debug.DrawLine (Current.worldPosition, Neighbor.worldPosition);
						Opened.Push (Neighbor);
					}
				}
			}
		}
		return null;
	}

	private static List<Node> ReversePath ( Node Location, Node Target) {
		List<Node> Path = new List<Node> ();
		Node Current = Target;
		while (Current != Location) {
			Path.Add (Current);
         //   Debug.DrawLine(Current.worldPosition, Current.Parent.worldPosition);
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