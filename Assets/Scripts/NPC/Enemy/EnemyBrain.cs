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
    float speed = 0.001f;

    [SerializeField]
    float minDistToPoint = 0.01f;

    [SerializeField]
    Vector3[] points;

    int i = 0;
    int step = 1;

    void Start()
    {
        visibility = GetComponent<Visibility>();
        rb2d = GetComponent<Rigidbody2D>();
        if ((transform.position - points[0]).magnitude >= minDistToPoint)
        {
            Vector3 direction = points[0] - transform.position;
            rb2d.velocity = new Vector2(direction.x, direction.y).normalized * speed;
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
            enemyState = State.patrol;
        }
    }

    void FixedUpdate()
    {
        switch (enemyState)
        {
            case State.patrol:
                if ((transform.position - points[i]).magnitude < minDistToPoint)
                {
                    i += step;
                    if ((i == points.Length - 1) || (i == 0))
                    {
                        step = -step;
                    }
                    Vector3 direction = points[i] - transform.position;
                    rb2d.velocity = new Vector2(direction.x, direction.y).normalized * speed;
                }
                else
                {
                    Vector3 direction = points[i] - transform.position;
                    rb2d.velocity = new Vector2(direction.x, direction.y).normalized * speed;
                }
                break;
            case State.shooting:
                rb2d.velocity = new Vector2(0, 0);
                break;
        }
    }
}