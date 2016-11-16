using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

    // player stats
    public float health;
	public ActorController actorStat;

    //At start, load data from GlobalControl.
    void Start () {
        health = GlobalControl.Instance.health;
    }

    //Save data to global control   
    public void SavePlayer() {
        health = actorStat.health.currentVal;
        GlobalControl.Instance.health = health;
    }
}
