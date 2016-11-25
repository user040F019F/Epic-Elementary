using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour {

    private LevelGenerator LevelGen;
    private LevelData Level;
    private List<GameObject> Obstacles;
    private List<GameObject> Enemies;
    private Transform Player;
    [SerializeField]
    private GameObject Enemy; 
    [SerializeField]
    private LayerMask ObstacleMask;
    private float padding = 2f;

	// Use this for initialization
	void Start () {
        LevelGen = GameObject.FindGameObjectWithTag("Generator").GetComponent<LevelGenerator>();
        Player = GameObject.FindGameObjectWithTag("Actor").transform;
        Transform Container = GameObject.FindGameObjectWithTag("Container").transform;
        Level = transform.parent.GetComponent<LevelData>();
        System.Random Rnd = new System.Random();
        // Generate Obstacles
        Obstacles = new List<GameObject>(
            Mathf.RoundToInt(
                ((float)Rnd.NextDouble() * transform.localScale.x)
            )
        );
        Debug.Log(transform.position.x + ", " + transform.lossyScale.x);
        Debug.Log(transform.position.x + transform.lossyScale.x);
        Debug.Log(Rnd.Next((int)transform.position.x, (int)(transform.position.x + transform.lossyScale.x)));
        for (int i = 0; i < Obstacles.Capacity; i++) {
            try {
                Obstacles.Add(Instantiate(
                        LevelGen.Package.Obstacles[Rnd.Next(0, LevelGen.Package.Obstacles.Length)]
                    ));
                Obstacles[i].transform.parent = Container;
                Obstacles[i].transform.position = new Vector3(
                    Rnd.Next((int)(transform.position.x + padding), (int)(transform.lossyScale.x + transform.position.x - padding)),
                    transform.position.y,
                    Rnd.Next((int)(transform.position.z - transform.lossyScale.z +2 ), (int)transform.position.z - 2));
                //    Obstacles[i].transform.localPosition = new Vector3(Mathf.Clamp((float)Rnd.NextDouble(), 0+padding, 1- padding), 0, -(float)Rnd.NextDouble());
                Obstacles[i].transform.localRotation = Quaternion.Euler(new Vector3(0, (float)Rnd.Next(0,360), 0));
                Obstacles[i].transform.localScale *= Mathf.Clamp((float)Rnd.NextDouble(), .6f, 1f);
                //Obstacles[i].transform.parent = Container;
                foreach (Collider collider in Physics.OverlapSphere(Obstacles[i].transform.position, Obstacles[i].transform.lossyScale.x / 2 + Player.lossyScale.x, ObstacleMask)) {
                    if (collider.transform != Obstacles[i].transform) {
                        Destroy(Obstacles[i]);
                    }
                }
            } catch { }
        }

        // Generate enemies
        
        Enemies = new List<GameObject>(
            Mathf.RoundToInt((float)Rnd.NextDouble() * (transform.localScale.x / 2))
            );
        for (int i = 0; i < Enemies.Capacity; i++) {
            try {
                Enemies.Add(Instantiate(Enemy));
                Enemies[i].GetComponent<ActorController>().level = Level;
                Enemies[i].transform.parent = Container;
                Enemies[i].GetComponentInChildren<SkinnedMeshRenderer>().material = LevelGen.Package.Enemies[Rnd.Next(0, LevelGen.Package.Enemies.Length)];
                Enemies[i].transform.position = new Vector3(
                    Rnd.Next((int)(transform.position.x + padding), (int)(transform.lossyScale.x + transform.position.x - padding)),
                    transform.position.y,
                    Rnd.Next((int)Level.zBoundFront, (int)Level.zBoundFront));
                foreach (Collider collider in Physics.OverlapSphere(Enemies[i].transform.position, Enemies[i].transform.lossyScale.x, ObstacleMask)) {
                    Destroy(Enemies[i]);
                }
            } catch { }
        }
        

    }

    void OnDestroy () {
        foreach (GameObject Obstacle in Obstacles) {
            Destroy(Obstacle);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
