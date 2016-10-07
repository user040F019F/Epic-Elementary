using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour {

    public Vector3 ThrowDestination = Vector3.zero;
    private Animator Animator;
	private CharacterController Character;
	private Rigidbody RB;

	// Restrictions
	[SerializeField]
	private float MaxJoggingSpeed = 5f,
		RunningSpeed = 8f,
		Multiplier = 2f,
		MinSpeed = .01f,
		RotationSpeed = 1f,
        JumpHeight = 5f;

	private bool Jumping = false, JumpNow = false;

	private Vector3 Velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        Animator = gameObject.GetComponent<Animator>();
		RB = gameObject.GetComponent<Rigidbody> ();
		Character = gameObject.GetComponent<CharacterController> ();
		fromRagdoll ();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void LateUpdate()
    {
        if (Jumping = Animator.GetBool("Jump"));
        {
            if (isGrounded())
            {
                Animator.SetBool("Jump", false);
                Jumping = false;
            }
        }
    }

	private bool isGrounded() {
		RaycastHit hit;
		float checkDistance = gameObject.GetComponent<Collider> ().bounds.extents.y + .1f;
		if(Physics.Raycast (gameObject.transform.position, Vector3.down, out hit, checkDistance)) {
			if (hit.transform.CompareTag("Platform")) {
				return true;
			}
		}
		return false;
	}

    // Set jump variable for update to pick up
	public void Jump () {
        if (isGrounded())
        {
            RB.velocity += new Vector3(0, JumpHeight, 0);
            Animator.SetBool("Jump", true);
        }
	}

	// Provide running abilities
	public void Run(Vector3 Movement) {
		Animator.SetBool ("Running", true);
		Velocity = Movement.normalized * RunningSpeed;
		Finalize (Movement);

	}

	public void toRagdoll () {
		foreach (Rigidbody RigidBody in gameObject.GetComponentsInChildren<Rigidbody>()) {
			RigidBody.isKinematic = false;
			RigidBody.detectCollisions = true;
		}
		gameObject.GetComponent<Rigidbody> ().detectCollisions = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		Animator.enabled = false;
	}

	public void fromRagdoll () {
		foreach (Rigidbody RigidBody in gameObject.GetComponentsInChildren<Rigidbody>()) {
			RigidBody.isKinematic = true;
			RigidBody.detectCollisions = false;
		}
		gameObject.GetComponent<Rigidbody> ().detectCollisions = true;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		Animator.enabled = true;
	}

	// Helper function for running finalization
	private void Finalize (Vector3 Movement) {
		Finalize (Movement, Velocity);
	}

	// Finalize movement
	private void Finalize (Vector3 Movement, Vector3 Velocity) {
		transform.Translate (Velocity * Time.deltaTime, Space.World);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (Velocity.normalized), RotationSpeed * Time.deltaTime);
	}

	// Move player under regular circumstances
	public void Move (Vector3 Movement)
    {
        if (Movement.magnitude > MinSpeed)
        {
            if (!Jumping)
            {
                Velocity = Movement * Multiplier;
				if (Velocity.magnitude > MaxJoggingSpeed) {
					Velocity.Normalize ();
					Velocity *= MaxJoggingSpeed;
				}
			}
			Finalize (Movement);
        }
        Animator.SetFloat("Speed", Movement.magnitude);
        Animator.SetBool("Running", false);
    }

	// Throw
    public void Throw (Vector3 Destination)
    {
		if (!Jumping) {
			ThrowDestination = Destination;
			Animator.SetTrigger ("Throw");
		}
    }
}
