using UnityEngine;
using System.Collections;

public class OnFall : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Actor")) {
			ActorController Actor = other.gameObject.GetComponent<ActorController> ();
			Actor.Die ();
		}
	}
}
