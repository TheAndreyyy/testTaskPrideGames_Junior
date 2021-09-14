using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    [SerializeField] GameObject explosionParticles;
    [SerializeField] public int colorOfThisGrenade;
    [SerializeField] bool isThrown = false;
    public GrenadeSpawner grenadeSpawner;
    public GameplayScript gameplayScript;
    GameObject spawnedParticles;

    [Header("Damage Popups Settings")]
    public GameObject damageText;

    public void Start()
    {
        gameplayScript = FindObjectOfType<GameplayScript>();
    }
    //так, это прям вот вообще отстой...я понимаю, что Find на порядки медленнее, чем остальной код
    //но почему-то все мои попытки прокинуть ссылки (и при спавне и через другой скрипт) обломались,
    //поэтому решил попробовать так, и прокатило...так что сильно не бейте :(
    //P.S. - Уверен, что есть более адекватные варианты, просто времени не хватило.
    private void OnTriggerEnter(Collider other)
    {
        if (isThrown)
        {
            if ((other.tag == "Ground" || other.tag == "Enemy"))
            {
                Explode();
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                grenadeSpawner.WaitingBeforeSpawn(transform.parent.gameObject);
                PlayerController playerController = other.GetComponent<PlayerController>();
                switch (colorOfThisGrenade)
                {
                    case 0:
                        playerController.grenadeCount0++;
                        playerController.grenade0Text.text = playerController.grenadeCount0.ToString();
                        break;
                    case 1:
                        playerController.grenadeCount1++;
                        playerController.grenade1Text.text = playerController.grenadeCount1.ToString();
                        break;
                    case 2:
                        playerController.grenadeCount2++;
                        playerController.grenade2Text.text = playerController.grenadeCount2.ToString();
                        break;
                }
                Destroy(gameObject);
            }
        }
    }

    private void Explode()
    {
        ParticleSystem particleOfExplosion = explosionParticles.GetComponent<ParticleSystem>();
        var main = particleOfExplosion.main;
        switch (colorOfThisGrenade)
        {
            case 0:
                main.startColor = Color.red;
                break;
            case 1:
                main.startColor = Color.green;
                break;
            case 2:
                main.startColor = Color.blue;
                break;
        }
        spawnedParticles = Instantiate(explosionParticles, transform.position, transform.rotation);
        Collider[] collidersToDamage = Physics.OverlapSphere(transform.position, gameplayScript.radiusOfExplosionGrenade);
        foreach (Collider nearbyObject in collidersToDamage)
        {
            if (nearbyObject.tag == "Enemy")
            {
                EnemyScript enemy = nearbyObject.GetComponent<EnemyScript>();
                enemy.TakeDamage((int)gameplayScript.damageFromGrenade);
                FirstDamagePopup indicator = Instantiate(damageText, nearbyObject.transform.position, Quaternion.identity).GetComponent<FirstDamagePopup>();
                indicator.SetDamageText((int)gameplayScript.damageFromGrenade);
            }
        }
        Destroy(gameObject);
        Destroy(spawnedParticles.gameObject, 2);
    }
}
