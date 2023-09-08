using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Player target = null;
    protected float speed = 5f;
    protected int id = 0;
    protected float colDis = 0.25f;

    protected SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.color = Color.red;
    }

    public void SetPlayerID(int _id)
    {
        id = _id;
    }

    public void SetBulletAttribute(bool isMyClient)
    {
        SetTarget();
        SetSpriteColor(isMyClient);
    }

    public void SetTarget()
    {
        if (id == 1)
            target = PingPong.Instance.serverBar;
        else
            target = PingPong.Instance.clientBar;
    }

    public void SetSpriteColor(bool isMyClient)
    {
        if (isMyClient)
            spriteRenderer.color = Color.white;
    }

    protected virtual void Update()
    {
        Move();

        DeadLineCheck();

        CollisionCheck();
    }

    protected virtual void Move()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    public void DeadLineCheck()
    {
        if (transform.position.x > 5f)
            Pool();
        if (transform.position.x < -5f)
            Pool();
        if (transform.position.y > 5f)
            Pool();
        if (transform.position.y < -5f)
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
        PoolManager.Release(this);
    }
}
