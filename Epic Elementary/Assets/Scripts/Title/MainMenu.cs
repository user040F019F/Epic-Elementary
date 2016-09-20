using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public string startLevel;

    public string instructions;

    public string mainMenu;

    public string credits;

    public void NewGame() {
        Application.LoadLevel(startLevel);
        Destroy(GameObject.FindWithTag("MusicManager"));
    }

    public void Instructions() {
        Application.LoadLevel(instructions);
    }

    public void Menu() {
        Application.LoadLevel(mainMenu);
    }

}
