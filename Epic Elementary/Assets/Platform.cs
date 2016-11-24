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
    private float padding = .1f;

	// Use this for initialization
	void Start () {
        LevelGen = GameObject.FindGameObjectWithTag("Generator").GetComponent<LevelGenerator>();
        Player = GameObject.FindGameObjectWithTag("Actor").transform;
        Level = transform.parent.GetComponent<LevelData>();
        System.Random Rnd = new System.Random();
        // Generate Obstacles
        Obstacles = new List<GameObject>(
            Mathf.RoundToInt(
                ((float)Rnd.NextDouble() * transform.localScale.x)
            )
        );
        for (int i = 0; i < Obstacles.Capacity; i++) {
            Obstacles.Add(Instantiate(
                    LevelGen.Package.Obstacles[Rnd.Next(0, LevelGen.Package.Obstacles.Length)]
                ));
            Obstacles[i].transform.parent = this.transform;
            Obstacles[i].transform.localPosition = new Vector3(Mathf.Clamp((float)Rnd.NextDouble(), 0+padding, 1- padding), 0, -(float)Rnd.NextDouble());
            Obstacles[i].transform.localRotation = Quaternion.Euler(new Vector3(0, (float)Rnd.NextDouble(), 0));
            Obstacles[i].transform.localScale *= Mathf.Clamp((float)Rnd.NextDouble(), .6f, 1f);
            foreach (Collider collider in Physics.OverlapSphere(Obstacles[i].transform.position, Obstacles[i].transform.lossyScale.x / 2 + Player.lossyScale.x, ObstacleMask)) {
                if (collider.transform != Obstacles[i].transform) {
                    Destroy(Obstacles[i]);
                }
            }
        }

        // Generate enemies
        Enemies = new List<GameObject>(
            Mathf.RoundToInt((float)Rnd.NextDouble() * (transform.localScale.x / 2))
            );
        for (int i = 0; i < Enemies.Capacity; i++) {
            Enemies.Add(Instantiate(Enemy));
            Enemies[i].GetComponent<ActorController>().level = Level;
            Enemies[i].transform.parent = transform;
            Enemies[i].GetComponentInChildren<SkinnedMeshRenderer>().material = LevelGen.Package.Enemies[Rnd.Next(0, LevelGen.Package.Enemies.Length)];
            Enemies[i].transform.localPosition = new Vector3(Mathf.Clamp((float)Rnd.NextDouble(), 0 + padding, 1 - padding), 0, -Mathf.Clamp((float)Rnd.NextDouble(), .2f, .5f));
            Enemies[i].transform.localRotation = Quaternion.Euler(new Vector3(0, (float)Rnd.Next(0, 360), 0));
        }

    }

    void Destroy () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
