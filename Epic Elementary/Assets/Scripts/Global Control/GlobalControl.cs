using UnityEngine;
using System.Collections;

public class GlobalControl : MonoBehaviour {

    public static GlobalControl Instance;

    // data to save
    public float health;
    public int currentLevel;
    public int achivementOne;
    public int achivementTwo;
    public int classroomEnemiesKilled;
    public int outsideEmemiesKilled;

    void Awake() {

        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

    }
}