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
	public int nMinPlatformSize;

    // To be calculated by character jump distance.
    public int minDistance;
    public int maxDistance;
	public float startFall = .1f;
	public float offset;
	public float EndOffset = 10f;
	public float playerBoundBack = 3f;
	public float playerBoundFront = 3f;
	public static float BackBound = 3f, FrontBound = 3f;

    // Generated Prefabs
    [SerializeField]
    private GameObject Prefab;

    //Static GameObjects
    [SerializeField]
    private GameObject Sky;
    [SerializeField]
	private GameObject Underground;
	[SerializeField]
	private GameObject FallCollider;
	[SerializeField]
	private GameObject Player;

    // Static Materials
    [SerializeField]
    private Material[] GroundTextures;
    [SerializeField]
    private Material[] UndergroundTextures;
    [SerializeField]
    private Material[] BackdropTextures;

    // Generator Options
    [SerializeField]
    private int pitDepth;
    [SerializeField]
    private Vector3 Location;

    // Management
    [HideInInspector]
	public static volatile List<GameObject> Platforms = new List<GameObject>();
	[HideInInspector]
	public Rect PlayerBounds;
    private int cPlatform = 0;

    // External Data
    public static volatile int PlayerBound;

    // Calculation tools
    private System.Random rnd;
    private Vector3 cOrigin;
    float[] pSizes, gSizes;
    private Vector3 cViewport;
	private float EndScroll = 10f;

    // Use this for initialization
    void Start () {
        // Set base helpers
        rnd = new System.Random();
        pitDepth = Math.Abs(pitDepth);
        cOrigin = Camera.main.ViewportToWorldPoint(Location);

        // Calculate random platform and gap sizes based on parameters
        pSizes = getPlatformSizes();
        gSizes = getGapSizes();

        // Adjust level length to include gaps and offset
        LevelLength = pSizes.Sum() + gSizes.Sum();
        RenderUnderground();
        RenderSky();
		RenderFallCollider ();
		Generate();
		setPlayerBounds ();
		setPlayer ();
		EndScroll = cOrigin.x + LevelLength - EndOffset;
		isComplete = true;

	}

	private void Update()
	{
		// Get new viewport coordinates
		cViewport = Camera.main.ViewportToWorldPoint(new Vector3(0, Location.y, Location.z));
		Vector3 cRViewport = Camera.main.ViewportToWorldPoint(new Vector3(1, Location.y, Location.z));

		if (cRViewport.x > EndScroll) {
			Camera.main.GetComponent<FollowPlayer> ().enabled = false;
		}

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

	private void setPlayerBounds() {
		BackBound = -playerBoundBack;
		FrontBound = -Location.z + playerBoundFront;
	}

	// Set Player
	private void setPlayer() {
		Player.transform.position = new Vector3 (cOrigin.x - (offset ), cOrigin.y, -Location.z / 2);
	}

    // Generate ground container
    private void RenderUnderground()
    {
        MeshRenderer Back = Underground.transform.Find("ExpandablePlane/Back").gameObject.GetComponent<MeshRenderer>();
        MeshRenderer Bottom = Underground.transform.Find("ExpandablePlane/Bottom").gameObject.GetComponent<MeshRenderer>();
        Back.material = Bottom.material = UndergroundTextures[0];
        Back.material.mainTextureScale = Bottom.material.mainTextureScale = new Vector2(100, .5f);

        Underground.transform.position = cOrigin;
        Underground.transform.localScale = new Vector3(LevelLength, pitDepth, Location.z);
    }

    // Generate backdrop
    private void RenderSky()
    {
        Vector3 cTViewport = Camera.main.ViewportToWorldPoint(new Vector3(Location.x, 1, Location.z));

        MeshRenderer Back = Sky.transform.Find("ExpandablePlane/Back").gameObject.GetComponent<MeshRenderer>();
        Back.material = BackdropTextures[0];
        Back.material.mainTextureScale = new Vector2(20, 10);

        Sky.transform.position = cOrigin;
        Sky.transform.localScale = new Vector3(LevelLength, cTViewport.y - cOrigin.y, Location.z);
    }

	private void RenderFallCollider() {
		FallCollider.transform.position = new Vector3(cOrigin.x, cOrigin.y - startFall, cOrigin.z);
		FallCollider.transform.localScale = new Vector3(LevelLength, pitDepth - startFall, Location.z);
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
        
        MeshRenderer Top = Platform.transform.Find("ExpandablePlane/Top").gameObject.GetComponent<MeshRenderer>();
        MeshRenderer Left = Platform.transform.Find("ExpandablePlane/Left").gameObject.GetComponent<MeshRenderer>();
        MeshRenderer Right = Platform.transform.Find("ExpandablePlane/Right").gameObject.GetComponent<MeshRenderer>();
        Top.material = GroundTextures[0];
        Left.material = Right.material = UndergroundTextures[0];
        Top.material.mainTextureScale = Left.material.mainTextureScale = Right.material.mainTextureScale = new Vector2(10, 10);

        // Adjust origin for next platform
        cOrigin.x += pSizes [cPlatform];
		if (cPlatform < gSizes.Length)
			cOrigin.x += gSizes [cPlatform];
        cPlatform++;
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
		for (int i = 0; i < pSizes.Count; i++) {
			pSizes [i] /= denom;
			if (pSizes [i] <= nMinPlatformSize) {
				pSizes [i] = nMinPlatformSize;
			}
		}
        // Adjust for Camera offset
        pSizes[0] = offset;
        pSizes[pSizes.Count-1] = offset;
        return pSizes.ToArray();
    }
}
