using UnityEngine;
using System.Collections;

public class Outside : MonoBehaviour {

    public PlayerState playerState;


    void OnTriggerEnter(Collider other) {

        if (other.tag == "Actor") {

            Debug.Log("Entering Outside");

            playerState.SavePlayer();

            Application.LoadLevel("Level");


        }
    
    }

}
