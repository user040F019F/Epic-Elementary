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
		Vector3[] Path = ai.PathToPlayer (transform.position);
		//Debug.Log (Path);
		//Debug.DrawLine (transform.position, Path [0]);
	}
}
