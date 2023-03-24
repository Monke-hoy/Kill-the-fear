using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField]
    private int numOfGun = 2;

    Visibility visibility;
    public EnemyGun enemyGun;



    void Start()
    {
        visibility = GetComponent<Visibility>();
        enemyGun.ChangeGun(numOfGun);
    }

    void Update()
    {
        if (visibility.isVisible)
        {
            Vector3 lookDirection = visibility.player.position - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg, Vector3.forward);
            enemyGun.EnemyShoot();
        }
    }
}
