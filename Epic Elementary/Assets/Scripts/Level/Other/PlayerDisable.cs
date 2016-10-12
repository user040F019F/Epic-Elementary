using UnityEngine;
using System.Collections;

public class PlayerDisable : MonoBehaviour {

    GameObject player;

    public bool disableAttack;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Actor");

        if (disableAttack == true) {
            player.GetComponent<PlayerController>().disableAttack = true;
        }

	}

}
