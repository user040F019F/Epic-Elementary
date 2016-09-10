using UnityEngine;
using System.Collections;

public class ChalkSound : MonoBehaviour {

    [SerializeField]
    public int seconds;

    void Start() {
        StartCoroutine(Trim());
    }

    IEnumerator Trim() {
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(seconds);
        this.GetComponent<AudioSource>().Stop();
    }

}
