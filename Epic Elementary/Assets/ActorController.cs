using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour {

    public static Vector3 ThrowDestination = Vector3.zero;
    private static Animator Animator;

	// Use this for initialization
	void Start () {
        Animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void Throw (Vector3 Destination)
    {
        ThrowDestination = Destination;
        Animator.SetTrigger("Throw");
    }
}
