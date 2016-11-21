using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	private Vector2 Dimensions;
	public LayerMask unwalkableMask;
	public float NodeDiameter, NodeRadius;
	public static Node[,] grid;
	public int maxSize { 
		get {
			return Size.x * Size.y;
		}
	}

	public struct Point {
		public int x,y;
		public Point (int x, int y) {
			this.x = x;
			this.y = y;
		}
		public Point (Vector2 Position) {
			this.x = (int)Position.x;
			this.y = (int)Position.y;
		}
	}

	Point Size;

	public GameObject player;
	public GameObject leader;

	public Vector3 Position, Corner, Percent, Percentages;

	// Return node neighbors
	public List<Node> GetNeighbors(Node node) {
		List<Node> Neighbors = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (grid [node.Position.x + x, node.Position.y + y] == node)
					continue;
				try {
					Neighbors.Add(grid[node.Position.x + x, node.Position.y + y]);
				} catch {
				}
			}
		}
		return Neighbors;
	}

	// Use this for initialization
	void Start () {
		Dimensions = new Vector2 (transform.localScale.x, transform.localScale.z);
		Corner = transform.position - transform.localScale / 2;
		NodeDiameter = player.transform.localScale.x;
		NodeRadius = NodeDiameter / 2;
		Size = new Point (Dimensions / NodeDiameter);
		grid = new Node[Size.x, Size.y];
		Regenerate ();
	}

	// Get node from a world point
	public Node NodefromWorld(Vector3 Position) {
		Point position = NodeIndexfromWorld (Position);
		return grid [position.x, position.y];
	}

	// Get node position from world point
	public Point NodeIndexfromWorld (Vector3 Position) {
		this.Position = Position - Corner;
		Percent = new Vector2(Mathf.Clamp01(this.Position.x / transform.localScale.x), Mathf.Clamp01(this.Position.z / transform.localScale.z));
		Point position = new Point ();
		position.x = (int)((Dimensions.x - 1) * Percent.x);
		position.y = (int)((Dimensions.y - 1) * Percent.y);
		return position;
	}

	// Regenerate grid
	public void Regenerate() {
		for (int x = 0; x < Size.x; x++) {
			for (int z = 0; z < Size.y; z++) {
				Vector3 WorldPoint = Corner + (Vector3.right * (x * NodeDiameter + NodeRadius)) + (Vector3.forward * (z * NodeDiameter + NodeRadius));
				bool isWalkable = !Physics.CheckSphere (WorldPoint, NodeRadius, unwalkableMask);
				grid [x, z] = new Node (isWalkable, WorldPoint, new Point(x,z));
			}
		}
	}

	// Get distance allowing diagonals
	public int GetDistance (Node start, Node target) {
		Point distance = new Point (Mathf.Abs (start.Position.x - target.Position.x), Mathf.Abs (start.Position.y - target.Position.y));
		if (distance.x > distance.y)
			return 10 * distance.y + 5 * (distance.x - distance.y);
		else
			return 10 * distance.x + 5 * (distance.y - distance.x);
	}

	// Get distance without diagonals
	public int GetHeuristic (Node start, Node target) {
		Point distance = new Point (Mathf.Abs (start.Position.x - target.Position.x), Mathf.Abs (start.Position.y - target.Position.y));
		return (10 * distance.x + 10 * distance.y);
	}

	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetMouseButtonDown(1))
		{
			Regenerate ();
		}
	}
}
