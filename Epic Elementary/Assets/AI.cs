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
            Vector3[] Path = ai.PathToPlayer(transform.position);
            Debug.DrawLine(transform.position, Path[0], Color.green);
            Actor.MoveTo(Path[0]);
        } catch {

        }
        //Debug.DrawLine(transform.position, Path[0]);
		//Debug.Log (Path);
		//Debug.DrawLine (transform.position, Path [0]);
	}
}
