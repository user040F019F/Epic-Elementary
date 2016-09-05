using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject Prefab;

    [SerializeField]
    private GameObject Sky;

    [SerializeField]
    private GameObject Underground;

    [SerializeField]
    private GameObject FrontDecor;

    public float LevelLength;
    public int nMaxPlatforms;
    public int nMinPlatforms;
    public int minDistance;
    public int maxDistance;
    float[] pSizes, gSizes;

    private Vector3 ViewportLocation;

    [HideInInspector]
    public static volatile List<GameObject> Platforms = new List<GameObject>();

    GameObject sky;

    private int cPlatform = 0;
    private Vector3 cOrigin;

	// Use this for initialization
	void Start () {
        cOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0,.5f,10f));
        pSizes = getPlatformSizes();
        gSizes = getGapSizes(pSizes.Length);
        LevelLength += gSizes.Sum();
        RenderSky();
        Generate();
	}

    private void RenderSky()
    {
        Vector3 YScale = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10f));
        sky = GameObject.Find("Level/Backdrop");
        sky.transform.position = cOrigin;
        sky.transform.localScale = new Vector3(LevelLength, YScale.y-cOrigin.y,1);
    }

    private void RenderPlatform()
    {
            GameObject Platform = Instantiate(Prefab);
            Platform.transform.localScale = new Vector3(pSizes[cPlatform], 1, 1);
            Platform.transform.position = cOrigin;
            Platform.transform.parent = GameObject.Find("Level").transform;
            Platforms.Add(Platform);
			cOrigin.x += pSizes [cPlatform];
			if (cPlatform < gSizes.Length)
				cOrigin.x += gSizes [cPlatform];
            cPlatform++;
    }

    private void Update()
    {
        ViewportLocation = Camera.main.ViewportToWorldPoint(new Vector3(0, .5f, 10f));
        Vector3 RightViewportLocation = Camera.main.ViewportToWorldPoint(new Vector3(1, .5f, 10f));
        if (Platforms.Count > 0)
        {
            // Create new platforms
            if (RightViewportLocation.x > cOrigin.x)
                Generate();

            // Destroy old platforms
            GameObject Platform = Platforms.First();
            if (ViewportLocation.x > Platform.transform.position.x + Platform.transform.localScale.x)
            {
                Platforms.Remove(Platform);
                Destroy(Platform);
            }
        }
    }

    private void Generate()
    {
        if (cPlatform < pSizes.Length)
        {
            RenderPlatform();
        }
    }

    private float[] getGapSizes(int pSizes)
    {
        float[] gSizes = new float[pSizes - 1];
        System.Random rnd = new System.Random();
        for (int i = 0; i < gSizes.Length; i++)
            gSizes[i] = rnd.Next(minDistance, maxDistance);
        return gSizes;
    }

    private float[] getPlatformSizes()
    {
        List<float> pSizes = new List<float>();
        System.Random rnd = new System.Random();
        int nPlatforms = rnd.Next(nMinPlatforms, nMaxPlatforms);
        for (int i = 0; i < nPlatforms; i++)
            pSizes.Add(rnd.Next(1, (int)LevelLength));
        float denom = pSizes.Sum() / LevelLength;
        for (int i = 0; i < pSizes.Count; i++)
            pSizes[i] /= denom;
        return pSizes.ToArray();
    }
}
