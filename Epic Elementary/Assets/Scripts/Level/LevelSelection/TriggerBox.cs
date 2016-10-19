using UnityEngine;
using System.Collections;

public class TriggerBox : MonoBehaviour {

    public SpriteRenderer sprite;
    public float fadeSpeed;
    private float tempAlphaColor;

    private bool fade = true;
    
	
	void Start () {
        tempAlphaColor = sprite.color.a;
    }
	
    void OnTriggerEnter (Collider other) {

        if (other.tag == "Actor") {         

            fade = false;

        }

    }

    void OnTriggerExit(Collider other) {

        if (other.tag == "Actor") {
           
            fade = true;

        }

    }

    void Update () {

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, tempAlphaColor);

        if (fade == false) {
            IncreaseAlpha();
        }

        if (fade == true) {
            DecreaseAlpha();
        }

	}



    void IncreaseAlpha () {

        if (tempAlphaColor != 2) {
            tempAlphaColor += fadeSpeed;
        }

        // cap the alpha value
        if (tempAlphaColor > 1) {
            tempAlphaColor = 1;
        }
    }

    void DecreaseAlpha () {

        if (tempAlphaColor != 0) {
            tempAlphaColor -= fadeSpeed;
        }

        // cap the alpha value
        if (tempAlphaColor < -1) {
            tempAlphaColor = -1;
        }
    }


}
