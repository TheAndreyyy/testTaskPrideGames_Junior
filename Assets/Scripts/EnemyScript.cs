using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public GameObject healthBar;
    public Slider slider;
    public GameplayScript gameplayScript;
    public EnemySpawner enemySpawner;

    void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void Update()
    {
        slider.transform.LookAt(transform.position + Camera.main.transform.forward);//улыбаемся в камеру
        if (health < maxHealth)
        {
            healthBar.SetActive(true);
        }
        slider.value = CalculateHealth();
        if (health <= 0)
        {
            Death();
            Destroy(gameObject);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void TakeDamage(int takenDamage)
    {
        health -= takenDamage;
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    public void Death()
    {
        enemySpawner.WaitingBeforeSpawn(transform.parent.gameObject);
    }
}
