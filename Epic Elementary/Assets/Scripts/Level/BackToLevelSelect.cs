using UnityEngine;
using System.Collections;

public class BackToLevelSelect : MonoBehaviour {


    void OnTriggerEnter(Collider other) {

        if (other.tag == "Actor") {

            if (GlobalControl.Instance.currentLevel == 1) {

                GlobalControl.Instance.achivementOne = 1;

            }

            if (GlobalControl.Instance.currentLevel == 0) {

                GlobalControl.Instance.achivementTwo = 1;

            }

            Debug.Log("Loading LevelSelect");

            GlobalControl.Instance.currentLevel = -1;

            Application.LoadLevel("LevelSelect");


        }

    }
}
