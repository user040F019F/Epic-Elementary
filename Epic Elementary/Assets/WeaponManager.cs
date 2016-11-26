using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {
    public int Damage = 5;
    void OnCollisionEnter (Collision other) {
        if (other.gameObject.CompareTag("Actor") || other.gameObject.CompareTag("Enemy")) {
            other.gameObject.GetComponent<ActorController>().Health -= Damage;
         //   Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Platform")) {
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            Destroy(gameObject, 2000);
        }
    }
}
