using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects; // to access imported standard effects

public class UImanager : MonoBehaviour {

    GameObject[] pauseObjects;
    GameObject[] deathObjects;
    public Camera mainCamera;

    public ActorController playerController;


    // Use this for initialization
    void Start() {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        deathObjects = GameObject.FindGameObjectsWithTag("ShowOnDeath");
        hidePaused();

        
    }

    // Update is called once per frame
    void Update() {

        if (playerController.Health != null && playerController.Health == 0) {
            mainCamera.GetComponent<BlurOptimized>().enabled = true;
            showDeath();
        } else {
            mainCamera.GetComponent<BlurOptimized>().enabled = false;
            hideDeath();
        }

        //uses the esc button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (Time.timeScale == 1) {
                Time.timeScale = 0;
                mainCamera.GetComponent<Blur>().enabled = true;
                showPaused();
            }
            else if (Time.timeScale == 0) {
                Time.timeScale = 1;
                mainCamera.GetComponent<Blur>().enabled = false;
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

    public void showDeath() {
        foreach (GameObject g in deathObjects) {
            g.SetActive(true);
        }
    }

    public void hideDeath() {
        foreach (GameObject g in deathObjects) {
            g.SetActive(false);
        }
    }

    public void restartLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void backToLevelSelect() {
        Application.LoadLevel("LevelSelect");
    }
}
