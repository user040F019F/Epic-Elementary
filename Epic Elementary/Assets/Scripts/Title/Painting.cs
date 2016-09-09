using UnityEngine;
using System.Collections;

public class Painting : MonoBehaviour {

    [SerializeField]
    public GameObject paintBall;
    public Animator paintBallAnim;

    public GameObject splatter;

	void Start () {

        paintBallAnim = paintBall.GetComponent<Animator>();

        splatter = this.gameObject;

	}
	
	// Update is called once per frame
	void Update () {

        
        if (paintBallAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 ) {

            splatter.GetComponent<SpriteRenderer>().enabled = true;
            paintBall.GetComponentInChildren<MeshRenderer>().enabled = false;

        }
	}


}
