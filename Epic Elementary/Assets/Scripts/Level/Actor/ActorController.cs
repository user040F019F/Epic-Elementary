using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour {

    public static Vector3 ThrowDestination = Vector3.zero;
    private static Animator Animator;
	private CharacterController Character;
	private Rigidbody RB;

	// Restrictions
	[SerializeField]
	private float MaxSpeed = 5f,
		Multiplier = 2f,
		MinSpeed;

	private Vector3 Velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        Animator = gameObject.GetComponent<Animator>();
		RB = gameObject.GetComponent<Rigidbody> ();
		Character = gameObject.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Jump () {

	}
		
	public void Move (Vector3 Move) {
		Velocity = Move * Multiplier;
		if (Velocity.magnitude > MinSpeed) {
			if (Velocity.magnitude > MaxSpeed) {
				Velocity.Normalize ();
				Velocity *= MaxSpeed;
			}
			transform.Translate (Velocity * Time.deltaTime, Space.World);
			transform.rotation = Quaternion.LookRotation (Velocity);
		}
		Animator.SetFloat ("Speed", Velocity.magnitude);
	}

    public void Throw (Vector3 Destination)
    {
        ThrowDestination = Destination;
        Animator.SetTrigger("Throw");
    }
}
