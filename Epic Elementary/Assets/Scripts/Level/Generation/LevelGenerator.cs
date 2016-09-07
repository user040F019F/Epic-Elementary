using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelGenerator : MonoBehaviour {

    // Notifier
    public static bool isComplete = false;

    // To be calculated off of level.
    public float LevelLength;
    public int nMaxPlatforms;
    public int nMinPlatforms;

    // To be calculated by character jump distance.
    public int minDistance;
    public int maxDistance;

    // Generated Prefabs
    [SerializeField]
    private GameObject Prefab;

    //Static GameObjects
    [SerializeField]
    private GameObject Sky;
    [SerializeField]
    private GameObject Underground;

    // Generator Options
    [SerializeField]
    private int pitDepth;
    [SerializeField]
    private Vector3 Location;

    // Management
    [HideInInspector]
    public static volatile List<GameObject> Platforms = new List<GameObject>();
    private int cPlatform = 0;

    // External Data
    public static volatile int PlayerBound;

    // Calculation tools
    private static float offset;
    private System.Random rnd;
    private Vector3 cOrigin;
    float[] pSizes, gSizes;
    private Vector3 cViewport;

    // Use this for initialization
    void Start () {
        // Set base helpers
        rnd = new System.Random();
        pitDepth = Math.Abs(pitDepth);
        cOrigin = Camera.main.ViewportToWorldPoint(Location);
        offset = Camera.main.GetComponent<FollowPlayer>().Offset.x;

        // Calculate random platform and gap sizes based on parameters
        pSizes = getPlatformSizes();
        gSizes = getGapSizes();

        // Adjust level length to include gaps and offset
        LevelLength = pSizes.Sum() + gSizes.Sum();
        RenderUnderground();
        RenderSky();
        Generate();
        isComplete = true;
	}

    // Generate ground container
    private void RenderUnderground()
    {
        Underground.transform.position = cOrigin;
        Underground.transform.localScale = new Vector3(LevelLength, pitDepth, Location.z);
    }

    // Generate backdrop
    private void RenderSky()
    {
        Vector3 cTViewport = Camera.main.ViewportToWorldPoint(new Vector3(Location.x, 1, Location.z));
        Sky.transform.position = cOrigin;
        Sky.transform.localScale = new Vector3(LevelLength, cTViewport.y - cOrigin.y, Location.z);
    }

    // Generate new platform
    private void RenderPlatform()
    {
        GameObject Platform = Instantiate(Prefab);

        // Set transforms
        Platform.transform.localScale = new Vector3(pSizes[cPlatform], pitDepth, Location.z);
        Platform.transform.position = cOrigin;
        Platform.transform.parent = GameObject.Find("Level").transform;
        Platforms.Add(Platform);

        // Adjust origin for next platform
		cOrigin.x += pSizes [cPlatform];
		if (cPlatform < gSizes.Length)
			cOrigin.x += gSizes [cPlatform];
        cPlatform++;
    }

    private void Update()
    {
        // Get new viewport coordinates
        cViewport = Camera.main.ViewportToWorldPoint(new Vector3(0, Location.y, Location.z));
        Vector3 cRViewport = Camera.main.ViewportToWorldPoint(new Vector3(1, Location.y, Location.z));

        // Avoid indexing errors
        if (Platforms.Count > 0)
        {
            // Create new platforms
            if (cRViewport.x > cOrigin.x)
                Generate();

            // Destroy old platforms
            GameObject Platform = Platforms.First();
            if (cViewport.x > Platform.transform.position.x + Platform.transform.localScale.x)
            {
                Platforms.Remove(Platform);
                Destroy(Platform);
            }
        }
    }

    // Check to see if final platform has already been rendered
    private void Generate()
    {
        if (cPlatform < pSizes.Length)
        {
            RenderPlatform();
        }
    }

    // Set random gap sizes based on player jump distance
    private float[] getGapSizes()
    {
        float[] gSizes = new float[pSizes.Length - 1];
        for (int i = 0; i < gSizes.Length; i++)
            gSizes[i] = rnd.Next(minDistance, maxDistance);
        return gSizes;
    }

    // Set random platform sizes to add up to level length
    private float[] getPlatformSizes()
    {
        List<float> pSizes = new List<float>();
        int nPlatforms = rnd.Next(nMinPlatforms, nMaxPlatforms);
        for (int i = 0; i < nPlatforms; i++)
            pSizes.Add(rnd.Next(1, (int)LevelLength));
        float denom = pSizes.Sum() / LevelLength;
        for (int i = 0; i < pSizes.Count; i++)
            pSizes[i] /= denom;
        // Adjust for Camera offset
        pSizes[0] += offset;
        pSizes[pSizes.Count-1] += offset;
        return pSizes.ToArray();
    }
}
