﻿using UnityEngine;
using System.Collections;

public class AchivementOne : MonoBehaviour {

    public GameObject text;

	void Start () {
	
        if (GlobalControl.Instance.achivementOne == 1) {

            this.GetComponent<MeshRenderer>().enabled = true;
            text.SetActive(true);


        }

	}

}
