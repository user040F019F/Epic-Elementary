using UnityEngine;
using System.Collections;

public class AnimationEventController : MonoBehaviour {

    private ActorController ActorController;

	// Use this for initialization
	void Start () {
        ActorController = gameObject.GetComponent<ActorController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ThrowObject()
    {
		//Throw object
    }
}
