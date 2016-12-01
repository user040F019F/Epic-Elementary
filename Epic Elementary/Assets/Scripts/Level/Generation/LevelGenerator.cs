using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject Level;
	private LevelData levelData;

	[SerializeField]
	private GameObject AI;
	[SerializeField]
	private GameObject AIGrid;

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

    // Texture Packs
    [SerializeField]
    private TexturePack[] Packages;
    public TexturePack Package {
        get {
            return Packages[GlobalControl.Instance.currentLevel];
        }
    }

    //Static GameObjects
    [SerializeField]
    private GameObject Sky;
    [SerializeField]
	private GameObject Underground;
	[SerializeField]
	private GameObject FallCollider;
	[SerializeField]
	private GameObject Player;
    
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
            try {
                if (cViewport.x > Platform.transform.position.x + Platform.transform.localScale.x) {
                    Platforms.Remove(Platform);
                    Destroy(Platform);
                }
            } catch { }

		}
		SetAI ();
	}
    
    public void MoveEnemy(GameObject Enemy) {
        try {
			if (!Enemy.GetComponent<ActorController>().Dead)
            	Platforms.Last().GetComponent<Platform>().SpawnEnemy();
		} catch { }
		Destroy(Enemy);
    }

    private void SetAI() {
		int LastIndex = Platforms.Count - 1;
		if (LastIndex > -1) {
			AIGrid.transform.localScale = new Vector3 (
				((Platforms [LastIndex].transform.position.x + Platforms [LastIndex].transform.localScale.x) - AI.transform.position.x),
				1,
				Math.Abs(levelData.zBoundBack - levelData.zBoundFront)
			);
            try {
                AI.transform.position = new Vector3(
                    Platforms[0].transform.position.x,
                    Platforms[0].transform.position.y,
                    levelData.zBoundBack
                );
            } catch { }
		}
	}

	private void setPlayerBounds() {
		levelData = Level.GetComponent<LevelData> ();
		levelData.zBoundBack = -playerBoundBack;
		levelData.zBoundFront = -Location.z + playerBoundFront;
	}

	// Set Player
	private void setPlayer() {
		Player.GetComponent<ActorController> ().level = this.levelData;
		Player.transform.position = new Vector3 (cOrigin.x - (offset ), cOrigin.y, -Location.z / 2);
	}

    // Generate ground container
    private void RenderUnderground()
    {
        MeshRenderer Back = Underground.transform.Find("ExpandablePlane/Back").gameObject.GetComponent<MeshRenderer>();
        MeshRenderer Bottom = Underground.transform.Find("ExpandablePlane/Bottom").gameObject.GetComponent<MeshRenderer>();
        Back.material = Bottom.material = Package.Underground;

        Underground.transform.position = cOrigin;
        Underground.transform.localScale = new Vector3(LevelLength, pitDepth, Location.z);
        Back.material.mainTextureScale = Underground.transform.localScale;
        Bottom.material.mainTextureScale = new Vector2(Underground.transform.localScale.x, Underground.transform.localScale.z);
    }

    // Generate backdrop
    private void RenderSky()
    {
        Vector3 cTViewport = Camera.main.ViewportToWorldPoint(new Vector3(Location.x, 1, Location.z));

        Sky.transform.position = cOrigin;
        Sky.transform.localScale = new Vector3(LevelLength, cTViewport.y - cOrigin.y, Location.z);

        MeshRenderer Back = Sky.transform.Find("ExpandablePlane/Back").gameObject.GetComponent<MeshRenderer>();
        Back.material = Package.Backdrop;
        Back.material.mainTextureScale = Sky.transform.localScale;
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
        Top.material = Package.Platform;
        Left.material = Right.material = Package.Sides;
        Top.material.mainTextureScale = new Vector2(Platform.transform.localScale.x, Platform.transform.localScale.z);
        Left.material.mainTextureScale = Right.material.mainTextureScale = new Vector2(Platform.transform.localScale.y, Platform.transform.localScale.z);

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
