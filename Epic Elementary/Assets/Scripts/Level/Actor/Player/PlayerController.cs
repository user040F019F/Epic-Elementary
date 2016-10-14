using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private ActorController ActorController;

    public float CameraPadding = .1f;

    public bool disableAttack = false;



	// Use this for initialization
	void Start () {
		ActorController = gameObject.GetComponent<ActorController>();
	}
	
	// Update is called once per frame
	void Update () {

		//ActorController.toRagdoll ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			ActorController.Jump ();
		}

		Vector3 Movement = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")); // Get WASD
        
        
		if (Input.GetKey (KeyCode.LeftShift)) { // Left Shift: Run
			ActorController.Run (Movement);
		} else {
			ActorController.Move (Movement); // Continuously update movement
        }

        // Get character's coordinate relative to camera
        Vector3 ViewportLocation = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        if (ViewportLocation.x <= CameraPadding)
        {
            gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(CameraPadding, ViewportLocation.y, ViewportLocation.z));
        }

        if (Input.GetMouseButtonDown(0)) // Left click: Throw
        {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 100)) {
				ActorController.Throw (hit.point);
			}
        }
    }
}
