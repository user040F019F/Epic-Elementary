using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects; // to access imported standard effects

public class UImanager : MonoBehaviour {

    GameObject[] pauseObjects;
    public Camera mainCamera;


    // Use this for initialization
    void Start() {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update() {

        //uses the esc button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape)) {



            if (Time.timeScale == 1) {
                Time.timeScale = 0;
                mainCamera.GetComponent<BlurOptimized>().enabled = true;
                showPaused();
            }
            else if (Time.timeScale == 0) {
                Time.timeScale = 1;
                mainCamera.GetComponent<BlurOptimized>().enabled = false;
                hidePaused();
            }

        }
    }

    //controls the pausing of the scene
    public void pauseControl() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0) {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused() {
        foreach (GameObject g in pauseObjects) {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hidePaused() {
        foreach (GameObject g in pauseObjects) {
            g.SetActive(false);
        }
    }

}
