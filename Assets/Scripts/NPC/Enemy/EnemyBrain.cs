using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBrain : MonoBehaviour
{
    private enum State { patrol, shooting };
    Visibility visibility;
    State enemyState = State.patrol;
    Rigidbody2D rb2d;

    [SerializeField]
    float movespeed;

    [SerializeField]
    float minDistToPoint = 0.01f;

    [SerializeField]
    Vector3[] points;

    private float standTimer;

    private EnemyMovement enemyMovement;
    int i = 0;
    int step = 1;

    void Start()
    {
        visibility = GetComponent<Visibility>();
        rb2d = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
        if (points.Length > 0)
        {
            if ((transform.position - points[0]).magnitude >= minDistToPoint)
            {
                Vector3 direction = points[0] - transform.position;
                rb2d.velocity = new Vector2(direction.x, direction.y).normalized * movespeed;
            }
        }
    }

    void Update()
    {
        if (visibility.isVisible)
        {
            enemyState = State.shooting;
        }
        else
        {
            if (enemyState == State.shooting) { standTimer = 0f; }
            else { standTimer += Time.deltaTime; }
            enemyState = State.patrol;
        }
    }

    void FixedUpdate()
    {
        if (points.Length < 2)
            return;
        switch (enemyState)
        {
            case State.patrol:
                if (standTimer > 2f)
                {
                    if ((transform.position - points[i]).magnitude < minDistToPoint)
                    {
                        i += step;
                        if ((i == points.Length - 1) || (i == 0))
                        {
                            step = -step;
                        }
                    }
                    Vector3 direction = points[i] - transform.position;
                    Quaternion lookRotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - enemyMovement.angleDifference - 2f, Vector3.forward);
                    rb2d.velocity = new Vector2(direction.x, direction.y).normalized * movespeed;
                    transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 18f);
                }
                break;
            case State.shooting:
                rb2d.velocity = new Vector2(0, 0);
                break;
        }
    }
}