using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarImage : MonoBehaviour
{

    private Image healthBar;
    public float currentHealth;
    private float maxHealth = 100f;


    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = GameManager.Instance.playerHP;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
