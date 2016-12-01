using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {
    public int Damage = 5;
    public bool isFriendly = true;
    void OnCollisionEnter (Collision other) {
        if ((other.gameObject.CompareTag("Actor") && !isFriendly) || (other.gameObject.CompareTag("Enemy") && isFriendly)) {
            other.gameObject.GetComponent<ActorController>().Health -= Damage;
            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("Platform")) {
            Destroy(gameObject);
        }
    }
}
