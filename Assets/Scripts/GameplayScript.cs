using UnityEngine;

public class GameplayScript : MonoBehaviour
{
    public float radiusForTakeGrenades = 1;//подбираем гранату при подходе к ней ближе чем на N
    public float timeForRespawnGrenadesAndEnemies = 5;//время до появления новой гранаты и новых врагов через M секунд
    public float radiusOfExplosionGrenade = 5;//взрыв в радиусе K
    public float damageFromGrenade = 35;//Урон от гранаты X HP
}
