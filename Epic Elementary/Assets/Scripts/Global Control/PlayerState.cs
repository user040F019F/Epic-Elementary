using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

    // player stats
    public float health;
    public PlayerController playerStat;

    //At start, load data from GlobalControl.
    void Start () {
        health = GlobalControl.Instance.health;
    }

    //Save data to global control   
    public void SavePlayer() {
        health = playerStat.health.currentVal;
        GlobalControl.Instance.health = health;
    }
}
