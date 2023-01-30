using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBrain : MonoBehaviour
{
    private enum State { patrol, shooting };
    private Visibility visibility;
    private State enemyState = State.patrol;
    private Rigidbody2D rb2d;

    [SerializeField]
    private float speed = 0.001f;

    [SerializeField]
    private float minDistToPoint = 0.01f;

    [SerializeField]
    private Vector3[] points;

    private int i = 0;
    private int step = 1;

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
        switch(enemyState)
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
                Vector3 lookDirection = points[i] - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                break;
            case State.shooting:
                rb2d.velocity = new Vector2(0, 0);
                break;
        }
    }
}
