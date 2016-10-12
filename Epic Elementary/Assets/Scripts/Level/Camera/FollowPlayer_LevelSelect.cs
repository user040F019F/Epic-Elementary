using UnityEngine;
using System.Collections;

public class FollowPlayer_LevelSelect : MonoBehaviour {

    public Transform target;
    public float distance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(target.position.x-1, target.position.y+4, target.position.z - distance);

    }
}
