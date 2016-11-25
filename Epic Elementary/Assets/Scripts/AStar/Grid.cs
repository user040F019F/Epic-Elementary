using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Grid : MonoBehaviour {

    private Node[,] grid; // Grid Nodes
	private Vector2 gridSize; // Gird Node Size as vector
	private Point Size; // Node Counts
	[SerializeField]
	private LayerMask PlatformMask, ObstacleMask; // Detection Masks
	private Vector3 Position; // Location of left front corner of grid

	private Vector2 Dimensions;
	public LayerMask[] UnwalkableMasks;

	public int maxSize { 
		get {
			return Size.x * Size.y;
		}
	}


	// TEMP
	public GameObject Player;

	// Draw grid in debugger
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, -gridSize.y));
		if (grid != null) {
			foreach (Node node in grid) {
				Gizmos.color = node.isWalkable ? node.isJumpable ? Color.green : Color.gray : Color.red;
				Gizmos.DrawCube (node.worldPosition, Vector3.one * (Node.diameter - .1f));
			}
			Gizmos.color = Color.green;
			Node PlayerNode = NodeFromWorldPoint (Player.transform.position);
			Gizmos.DrawCube (PlayerNode.worldPosition, Vector3.one * (Node.diameter - .1f));
			Gizmos.color = Color.blue;
			foreach (Node node in GetNeighbors(PlayerNode)) {
				Gizmos.DrawCube (node.worldPosition, Vector3.one * (Node.diameter - .1f));
			}
		}
	}

	// Return node neighbors
	public Node[] GetNeighbors(Node node) {
		List<Node> Neighbors = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;
				int X = node.Position.x + x;
				int Y = node.Position.y + y;
				if (X >= 0 && X < Size.x && Y >= 0 && Y < Size.y)
					Neighbors.Add (grid [X, Y].Clone());
			}
		}
		return Neighbors.ToArray();
	}

	void Start() {
		Node.radius = Player.transform.lossyScale.x;
	}

	void Update () {
		Regenerate ();
	}

	// Regenerate the grid
	public void Regenerate() {
		this.gridSize = new Vector2(transform.localScale.x, transform.localScale.z);
		this.Size = new Point (this.gridSize.x/Node.diameter, this.gridSize.y/Node.diameter);
		this.Position = new Vector3 (
			transform.parent.position.x,
			transform.position.y - (transform.localScale.y - (transform.localScale.y/2)),
			transform.parent.position.z - transform.localScale.z
		);
		CreateGrid ();
	}

	// Generate grid
	public Grid CreateGrid() {
		grid = new Node[Size.x, Size.y];
		for (int x = 0; x < Size.x; x++) {
			for (int y = 0; y < Size.y; y++) {
				Vector3 CurrentPoint = new Vector3 (
					this.Position.x + (x * Node.diameter + Node.radius),
					transform.position.y,
					this.Position.z + (y * Node.diameter + Node.radius)
                );
				bool Walkable = true, Jumpable = false;
				// Platform && Obstacle Mask Calculations
				Walkable = (Physics.CheckSphere(CurrentPoint, Node.radius, PlatformMask) && !Physics.CheckSphere(CurrentPoint, Node.radius, ObstacleMask));
				// Jumpable Calculations
				Jumpable = (Walkable
					&& (!Physics.CheckSphere (new Vector3 (CurrentPoint.x - Node.diameter, CurrentPoint.y, CurrentPoint.z), Node.radius, PlatformMask)
						|| !Physics.CheckSphere(new Vector3(CurrentPoint.x + Node.diameter, CurrentPoint.y, CurrentPoint.z), Node.radius, PlatformMask)
					)
				);
				grid [x, y] = new Node (CurrentPoint, new Point (x, y), Walkable, Jumpable);
			}
		}
        return this;
	}

	// Get index from a world point
	private Point IndexFromWorldPoint (Vector3 Location) {
            Vector3 LocalLocation = Location - this.Position;
            Vector2 LocationPercentage = new Vector2(
                                             Mathf.Clamp01(LocalLocation.x / transform.localScale.x),
                                             Mathf.Clamp01(LocalLocation.z / transform.localScale.z)
                                         );
            return new Point(
                (Size.x - 1) * LocationPercentage.x,
                (Size.y - 1) * LocationPercentage.y
                             );
	}

    // Get node from a world point
    public Node NodeFromWorldPoint(Vector3 Position) {
        try {
            Point Location = IndexFromWorldPoint(Position);
            return grid[Location.x, Location.y];
        } catch {
            return null;
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
		return (1000 * distance.x + 1000 * distance.y);
	}
}
