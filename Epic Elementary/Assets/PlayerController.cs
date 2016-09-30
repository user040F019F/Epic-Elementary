using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private ActorController ActorController;

	// Use this for initialization
	void Start () {
        ActorController = gameObject.GetComponent<ActorController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) // Left click: Throw
        {
            System.Random rnd = new System.Random();
            ActorController.Throw(new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
        }
    }
}
