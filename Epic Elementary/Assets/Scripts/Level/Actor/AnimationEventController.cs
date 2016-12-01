using UnityEngine;
using System.Collections;

public class AnimationEventController : MonoBehaviour {

    private ActorController ActorController;
    private AudioSource clip;

	[SerializeField]
	private GameObject Weapon;

	[SerializeField]
	private GameObject RightHand;

	// Use this for initialization
	void Start () {
        ActorController = gameObject.GetComponent<ActorController>();
        clip = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ThrowObject()
    {
        try {
            GameObject Projectile = Instantiate(Weapon,
                RightHand.transform.position,
                Quaternion.identity) as GameObject;
                Projectile.GetComponent<WeaponManager>().isFriendly = gameObject.tag == "Actor";
                Projectile.GetComponent<Rigidbody>().velocity = BallisticVelocity(ActorController.ThrowDestination, Projectile.transform.position, ActorController.ThrowAngle);
            clip.Play();
        } catch { }
    }

	private Vector3 BallisticVelocity(Vector3 target, Vector3 origin, float angle)
	{
		Vector3 dir = target - origin; // get Target Direction
		float height = dir.y; // get height difference
		dir.y = 0; // retain only the horizontal difference
		float dist = dir.magnitude; // get horizontal direction
		float a = angle * Mathf.Deg2Rad; // Convert angle to radians
		dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
		dist += height / Mathf.Tan(a); // Correction for small height differences

		// Calculate the velocity magnitude
		float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
		return velocity * dir.normalized; // Return a normalized vector.
	}
}
