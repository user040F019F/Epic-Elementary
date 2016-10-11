using UnityEngine;
using System.Collections;

public class AnimationEventController : MonoBehaviour {

    private ActorController ActorController;
    private AudioSource clip;

	// Use this for initialization
	void Start () {
        ActorController = gameObject.GetComponent<ActorController>();
        clip = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ThrowObject()
    {
        clip.Play();
    }
}
