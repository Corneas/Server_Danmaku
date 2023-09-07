using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player target = null;

    private float speed = 5f;

    private int id = 0;

    private float colDis = 0.5f;

    private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPlayerID(int _id)
    {
        id = _id;
    }

    public void SetBulletAttribute()
    {
        SetTarget();
        SetSpriteColor();
    }

    public void SetTarget()
    {
        if (id == 1)
            target = PingPong.Instance.serverBar;
        else
            target = PingPong.Instance.clientBar;
    }

    public void SetSpriteColor()
    {

    }

    private void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * speed;

        if(transform.position.x > 5f)
            Pool();
        if(transform.position.x < -5f)
            Pool();
        if(transform.position.y > 5f)
            Pool();
        if(transform.position.y < -5f)
            Pool();
    }

    public void CollisionCheck()
    {
        if(Vector2.Distance(transform.position, target.transform.position) < colDis)
        {
            target.playerDamaged.Damaged();
        }
    }

    public void Pool()
    {
        BulletPool.Instance.Push(this);
    }
}
