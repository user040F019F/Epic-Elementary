using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	private AStar ai;
    private ActorController Actor;

	// Use this for initialization
	void Start () {
		ai = GameObject.FindGameObjectWithTag ("AI").GetComponent<AStar>();
        Actor = gameObject.GetComponent<ActorController>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        try {
			Vector3 Direction = AStar.PlayerNode.worldPosition - transform.position;
			if (Direction.magnitude <= ai.DetectionRadius) {
				Vector3[] Path = ai.PathToPlayer(transform.position);
				Vector3 PathDirection = Path[0] - transform.position;
				if (Direction.magnitude > Actor.ThrowRadius) {
		            Actor.MoveTo(Path[0]);
				} else if (Direction.magnitude > Actor.MinThrowRadius) {
					Actor.Throw(AStar.PlayerNode.worldPosition);
					Actor.Move(Vector3.zero);
					transform.LookAt(AStar.PlayerNode.worldPosition);
				} else {
					Actor.Move(-PathDirection);
				}
			}
		} catch {
			Actor.Move(Vector3.zero);
			transform.LookAt(AStar.PlayerNode.worldPosition);
        }
        //Debug.DrawLine(transform.position, Path[0]);
		//Debug.Log (Path);
		//Debug.DrawLine (transform.position, Path [0]);
	}
}
