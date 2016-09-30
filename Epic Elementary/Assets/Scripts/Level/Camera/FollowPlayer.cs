using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    // Left lead from player
    public Vector3 Offset;

    // Required Player Object
    [SerializeField]
    public GameObject Player;

    // Allow updates after start
    private static bool enabled = false;

    // Helpers
    private Vector3 CtpLocation;
	
	// Update is called once per frame
	void Update () {
        if(LevelGenerator.isComplete)
        {
            /*
            if (transform.position.x < this.Player.transform.position.x + Offset.x)
                transform.position = new Vector3(this.Player.transform.position.x + Offset.x, transform.position.y, transform.position.z);
                */
            /*
            CtpLocation = gameObject.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, Player.transform.position.y, Player.transform.position.z));
            if (Player.transform.position.x - CtpLocation.x > Offset)
            {
                gameObject.transform.position = new Vector3(Player.transform.position.x - Offset, gameObject.transform.position.y, gameObject.transform.position.z);
            }
            */
        }
	}
}
