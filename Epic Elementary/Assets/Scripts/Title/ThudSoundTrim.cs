using UnityEngine;
using System.Collections;

public class ThudSoundTrim : MonoBehaviour {

    [SerializeField]
    public int seconds;

    void Start() {
        StartCoroutine(Trim());
    }

    IEnumerator Trim() {

        yield return new WaitForSeconds(seconds);
        this.GetComponent<AudioSource>().Play();
    }

}
