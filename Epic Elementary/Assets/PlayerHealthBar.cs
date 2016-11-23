using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthBar : MonoBehaviour {

    // Player Variables
    [SerializeField]
    private GameObject Player;
    private ActorController Actor;

    // Bar Variables
    [SerializeField]
    private Image HealthBar;
    [SerializeField]
    private Text HealthText;

    [SerializeField]
    [Range(0, 10)]
    private int LerpSpeed;

	// Use this for initialization
	void Start () {
        // Collect components
        Actor = Player.GetComponent<ActorController>();
	}
	
	// Update is called once per frame
	void Update () {
        float PlayerHealth = Actor.Health / Actor.MaxHealth;
	    if (HealthBar.fillAmount != PlayerHealth) {
            HealthBar.fillAmount = Mathf.Lerp(HealthBar.fillAmount, PlayerHealth, Time.deltaTime * LerpSpeed);
        }
        HealthText.text = "Health: " + Actor.Health.ToString();
	}
}
