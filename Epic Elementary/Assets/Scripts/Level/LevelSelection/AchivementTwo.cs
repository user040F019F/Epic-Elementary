using UnityEngine;
using System.Collections;

public class AchivementTwo : MonoBehaviour {

    public GameObject text;

    void Start() {

        if (GlobalControl.Instance.achivementTwo == 1) {

            this.GetComponent<MeshRenderer>().enabled = true;
            text.SetActive(true);
        }

    }
}
