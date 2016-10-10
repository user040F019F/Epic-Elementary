using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    // Left lead from player
    public Vector3 Offset;
	public Vector3 EndOffset;

    // Required Player Object
    [SerializeField]
    public GameObject Player;

	public bool enabled = true;

    // Helpers
    private Vector3 CtpLocation;
	
	// Update is called once per frame
	void Update () {
        if(LevelGenerator.isComplete)
        {
            if (transform.position.x < this.Player.transform.position.x + Offset.x)
			{
				if (enabled) {
	                transform.position = new Vector3(this.Player.transform.position.x + Offset.x, transform.position.y, transform.position.z);
				}
			}
        }
	}
}
