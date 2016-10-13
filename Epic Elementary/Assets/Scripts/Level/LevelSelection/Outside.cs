using UnityEngine;
using System.Collections;

public class Outside : MonoBehaviour {

    void OnTriggerEnter(Collider other) {

        if (other.tag == "Actor") {

            Debug.Log("Entering Outside");
            Application.LoadLevel("Level");


        }
    
    }

}
