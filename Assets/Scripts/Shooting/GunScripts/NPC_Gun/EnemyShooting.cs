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
        if (visibility.isVisible && !GetComponent<Enemy>().IsDead && visibility.ReactionTimer > 0.25f) 
        {
            Vector3 lookDirection = visibility.GetPlayerAxis.position - transform.position;
            Quaternion lookRotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - enemyMovement.angleDifference - 2f, Vector3.forward);
            if (Quaternion.Angle(lookRotation, transform.rotation) < 12f)
            {
                enemyGun.EnemyShoot();
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 12f);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 6f);
        }
    }
}
