using UnityEngine;
using System.Collections;
using UnityEditor;

public class Pivoter : MonoBehaviour {

    public enum YOrientation
    {
        Bottom,
        Middle,
        Top
    }

    public enum HOrientation
    {
        Left,
        Middle,
        Right
    }

    public enum ZOrientation
    {
        Back,
        Middle,
        Front
    }

    public YOrientation yAlign;
    public HOrientation hAlign;
    public ZOrientation zAlign;

    void Awake()
    {
        Align();
    }

	// Update is called once per frame
	void Update () {
        if (gameObject.transform.hasChanged)
            Align();
	}

    // Align to chosen axis
    private void Align()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag != "Obstacle" && child.tag != "Enemy") {
                child.localPosition = Vector3.zero;
                Vector3 Halfed = child.localScale / 2;
                Vector3 finalLocation = Vector3.zero;
                switch (yAlign) {
                    case YOrientation.Bottom:
                        finalLocation.y = Halfed.y;
                        break;
                    case YOrientation.Top:
                        finalLocation.y = -Halfed.y;
                        break;
                }
                switch (hAlign) {
                    case HOrientation.Left:
                        finalLocation.x = Halfed.x;
                        break;
                    case HOrientation.Right:
                        finalLocation.x = -Halfed.x;
                        break;
                }
                switch (zAlign) {
                    case ZOrientation.Back:
                        finalLocation.z = Halfed.z;
                        break;
                    case ZOrientation.Front:
                        finalLocation.z = -Halfed.z;
                        break;
                }
                child.localPosition += finalLocation;
            }        
        }
    }
}
