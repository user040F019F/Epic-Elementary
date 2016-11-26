using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour {

    public Vector3 ThrowDestination = Vector3.zero;
    private Animator Animator;
	private Rigidbody RB;

	//	Level Info
	public GameObject levelObject;
	[HideInInspector]
	public LevelData level;

    // Health tracker
    [HideInInspector]
    private float health;
    public float Health {
        get {
            return this.health;
        }
        set {
            this.health = Mathf.Clamp(value, 0, this.MaxHealth);
        }
    }
    [HideInInspector]
    public float MaxHealth = 100;

    // Throwing Angle
	[Range(0f,80f)]
	public float ThrowAngle = 45f;
    [Range(0f, 80f)]
    public float ThrowRange = 45f;
    [Range(0f, 10f)]
    public float ThrowRadius = 5f;
    [Range(0f, 10f)]
    public float MinThrowRadius = 1f;

    // Restrictions
    [SerializeField]
	private float MaxJoggingSpeed = 5f,
		RunningSpeed = 8f,
		Multiplier = 2f,
		MinSpeed = .01f,
		RotationSpeed = 1f,
        JumpHeight = 5f;

    // State monitors
	public bool Jumping, Dead;
    
	private Vector3 Velocity = Vector3.zero;

	GlobalControl globalControl;

	private void Awake() {
        this.Health = this.MaxHealth;
	}

	// Use this for initialization
	void Start () {
        this.Health = this.MaxHealth;
        Animator = gameObject.GetComponent<Animator>();
		RB = gameObject.GetComponent<Rigidbody> ();
		try {
			level = levelObject.GetComponent<LevelData>();
		} catch {
			// Will be defined by level generator
		}
		fromRagdoll ();
		globalControl = GameObject.FindGameObjectWithTag ("GlobalControl").GetComponent<GlobalControl> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (Health <= 0) {
            Die();
        }

	}

    void LateUpdate()
    {
        if (Jumping = Animator.GetBool("Jump"))
        {
            if (isGrounded())
            {
                Animator.SetBool("Jump", false);
                Jumping = false;
            }
		}
		if (gameObject.transform.position.z > level.zBoundBack) {
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, level.zBoundBack);
		} 
		if (gameObject.transform.position.z < level.zBoundFront) {
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, level.zBoundFront);
		}
    }

	public void Die() {
		Dead = true;
        Health = 0;

        Debug.Log("player death");

		this.toRagdoll ();
	}

	public bool isGrounded() {
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
		if (!Dead) {
			if (isGrounded ()) {
				RB.velocity += new Vector3 (0, JumpHeight, 0);
				Animator.SetBool ("Jump", true);
			}
		}
	}



	// Provide running abilities
	public void Run(Vector3 Movement) {
		if (!Dead) {
			Animator.SetBool ("Running", true);
			Velocity = Movement.normalized * RunningSpeed;
			Finalize (Movement);
		}
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

    public void MoveTo(Vector3 Point) {
        Move(Point - transform.position);
    }

	// Move player under regular circumstances
	public void Move (Vector3 Movement)
    {
		if (!Dead) {
			if (Movement.magnitude > MinSpeed) {
				if (!Jumping) {
					Velocity = Movement * Multiplier;
					if (Velocity.magnitude > MaxJoggingSpeed) {
						Velocity.Normalize ();
						Velocity *= MaxJoggingSpeed;
					}
				}
				Finalize (Movement);
			} else {
				Velocity = Vector3.zero;
				transform.rotation = Quaternion.Euler (0, transform.rotation.y, 0);
			}
			Animator.SetFloat ("Speed", Movement.magnitude);
			Animator.SetBool ("Running", false);
            
		} else {
			Animator.SetFloat ("Speed", 0);
			Animator.SetBool ("Running", false);
			Velocity = Vector3.zero;
		}
    }

	// Throw
    public void Throw (Vector3 Destination)
    {
		if (!Dead) {
                Vector3 Direction = Destination - transform.position;
                if (Vector3.Angle(transform.forward, Direction) <= ThrowRange) {
                    if (Direction.magnitude > ThrowRadius) {
                    Direction = Direction.normalized * ThrowRadius;
                } else if (Direction.magnitude < MinThrowRadius) {
                    Direction = Direction * MinThrowRadius;
                }
                    ThrowDestination = transform.position + Direction;
                    Animator.SetTrigger("Throw");
			}
		}
    }
}