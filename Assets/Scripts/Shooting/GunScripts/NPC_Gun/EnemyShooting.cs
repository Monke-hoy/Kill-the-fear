using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    private EnemyGun enemyGun;
    private EnemySound enemySound;
    private EnemyMovement enemyMovement;
    Visibility visibility;

    void Start()
    {
        enemySound = GetComponent<EnemySound>();    
        enemyGun = GetComponent<EnemyGun>();
        enemyMovement = GetComponent<EnemyMovement>();
        visibility = GetComponent<Visibility>();
        enemyGun.ChangeGun(enemyGun.GetNumOfGun);
        enemySound.ChangeGunSound(enemyGun.GetNumOfGun);
    }

    void Update()
    {
        if (visibility.isVisible && !GetComponent<Enemy>().IsDead)
        {

            Vector3 lookDirection = visibility.GetPlayerAxis.position - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - enemyMovement.angleDifference - 2f, Vector3.forward);
            enemyGun.EnemyShoot();
        }
    }
}
