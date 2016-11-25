using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	private AStar ai;

	// Use this for initialization
	void Start () {
		ai = GameObject.FindGameObjectWithTag ("AI").GetComponent<AStar>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        try {
            Vector3[] Path = ai.PathToPlayer(transform.position);
        } catch {

        }
        //Debug.DrawLine(transform.position, Path[0]);
		//Debug.Log (Path);
		//Debug.DrawLine (transform.position, Path [0]);
	}
}
