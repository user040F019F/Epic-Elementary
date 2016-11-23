using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour {

    private LevelGenerator LevelGen;
    private LevelData Level;
    private List<GameObject> Obstacles;

	// Use this for initialization
	void Start () {
        LevelGen = GameObject.FindGameObjectWithTag("Generator").GetComponent<LevelGenerator>();
        Level = transform.parent.GetComponent<LevelData>();
        System.Random Rnd = new System.Random();
        // Generate Obstacles
        Obstacles = new List<GameObject>(
            Mathf.RoundToInt(
                ((float)Rnd.NextDouble() * transform.localScale.x)
            )
        );
        StartCoroutine(LateStart());
	}

    IEnumerator LateStart () {
        yield return new WaitForFixedUpdate();
        Debug.Log(Random.Range(0, transform.localScale.x));
        System.Random Rnd = new System.Random();
        for (int i = 0; i < Obstacles.Capacity; i++) {
            Obstacles.Add(Instantiate(
                    LevelGen.Package.Obstacles[Rnd.Next(0, LevelGen.Package.Obstacles.Length)]
                ));
            Obstacles[i].transform.parent = this.transform;
            Obstacles[i].transform.localPosition = new Vector3((float)Rnd.NextDouble(), 0, -(float)Rnd.NextDouble());
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
