using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour {

    private AudioSource clip;
    private ActorController actor;

    public string effect;

    // for "play once only" effects
    private bool hasPlayed = false;


	void Start () {
        clip = this.GetComponent<AudioSource>();
        actor = GameObject.FindGameObjectWithTag("Actor").GetComponent<ActorController>();
	}
	
	
    // check name of string (effect) and play clip
	void Update () {

        if (effect == "attack") {

            if (Input.GetMouseButtonDown(0)) {

                clip.Play();

            }
        }

        if (effect == "jump") {
                                      // check is grounded to ensure no jump sound on random "spacebar press" || cooldown
            if (Input.GetKeyDown(KeyCode.Space) && actor.isGrounded() == true) {

                clip.Play();

            }
        }

        if (effect == "death") {

            if (actor.Dead == true && hasPlayed == false) {

                clip.Play();

                hasPlayed = true;

            }

        }

    }
}
