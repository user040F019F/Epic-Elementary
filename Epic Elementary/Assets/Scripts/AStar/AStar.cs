using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

	private static Grid grid;

	void Start () {
		grid = GetComponent<Grid> ();
	}

	// Find a path
	public static Vector3[] getPath (Vector3 position, Vector3 target) {
		Node startNode = grid.NodefromWorld (position);
		Node targetNode = grid.NodefromWorld (target);

		MinHeap<Node> Open = new MinHeap<Node> (grid.maxSize);
		HashSet<Node> Closed = new HashSet<Node> ();

		Open.Push(startNode);

		while (Open.Count > 0) {
			Node Current = Open.Pop ();
			Closed.Add (Current);

			if (Current == targetNode) {
				return ReversePath(startNode, targetNode);
			}
			List<Node> test = grid.GetNeighbors (Current);
			foreach (Node Neighbor in grid.GetNeighbors(Current)) {
				if (!Neighbor.isWalkable || Closed.Contains (Neighbor)) {
					continue;
				} 
				int CostToNeighbor = Current.gCost + grid.GetDistance (Current, Neighbor);
				if (!Open.Contains (Neighbor) || CostToNeighbor < Neighbor.gCost) {
					Neighbor.Parent = Current;
					Neighbor.gCost = CostToNeighbor;
					Neighbor.hCost = grid.GetHeuristic (Neighbor, targetNode);
					if (!Open.Contains(Neighbor)) {
						Open.Push (Neighbor);
					} 
				}
			}
		}
		return null;
	}

	// Get corrected path
	private static Vector3[] ReversePath(Node startNode, Node endNode) {
		List<Node> Path = new List<Node>();
		Node CurrentNode = endNode;
		while (CurrentNode != startNode) {
			Path.Add (CurrentNode);
			CurrentNode = CurrentNode.Parent;
		}
		return getWaypoints (Path);
	}

	// Generate waypoints for movement
	private static Vector3[] getWaypoints (List<Node> Path) {
		List<Vector3> Waypoints = new List<Vector3> ();
		Vector2 dirOld = Vector2.zero;
		Node oldNode = Path [0];
		Path.RemoveAt (0);
		foreach (Node newNode in Path) {
			Vector2 dirNew = new Vector2 (oldNode.Position.x - newNode.Position.x, oldNode.Position.y - newNode.Position.y);
			if (dirNew != dirOld)
				Waypoints.Add (newNode.worldPosition);
			dirOld = dirNew;
			oldNode = newNode;
		}
		Waypoints.Reverse ();
		return Waypoints.ToArray();
	}

}