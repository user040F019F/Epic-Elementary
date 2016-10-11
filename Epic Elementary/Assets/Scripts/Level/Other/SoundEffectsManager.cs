using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour {

    private AudioSource clip;
    private ActorController actor;
    private Animator playerAnimator;

    public string effect;

    // for "play once only" effects
    private bool hasPlayed = false;
    // 1 sec cooldown
    private float coolDownInSeconds = 1f;
    private float timeStamp;


	void Start () {
        clip = this.GetComponent<AudioSource>();
        actor = GameObject.FindGameObjectWithTag("Actor").GetComponent<ActorController>();
        playerAnimator = GameObject.FindGameObjectWithTag("Actor").GetComponent<Animator>();
	}
	
	
    // check name of string (effect) and play clip
	void Update () {


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
