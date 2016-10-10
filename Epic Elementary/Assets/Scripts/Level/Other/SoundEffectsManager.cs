using UnityEngine;
using System.Collections;

public class SoundEffectsManager : MonoBehaviour {

    public AudioSource clip;

	// Use this for initialization
	void Start () {
        clip = this.GetComponent<AudioSource>();


	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) {

            clip.Play();

            Debug.Log("working");

        }

    }
}
