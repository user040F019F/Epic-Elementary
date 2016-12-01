using UnityEngine;
using System.Collections;

public class Classroom : MonoBehaviour {

    public PlayerState playerState;


    void OnTriggerEnter(Collider other) {

        if (other.tag == "Actor") {

            Debug.Log("Load Classroom");

            GlobalControl.Instance.currentLevel = 0;

            playerState.SavePlayer();

            Application.LoadLevel("Level");


        }

    }
}
