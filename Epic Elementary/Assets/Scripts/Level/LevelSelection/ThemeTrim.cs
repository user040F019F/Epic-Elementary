using UnityEngine;
using System.Collections;

public class ThemeTrim : MonoBehaviour {


    void Start() {
        this.GetComponent<AudioSource>().time = 2.5f;
        this.GetComponent<AudioSource>().Play();
    }

}
