using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player player = null;

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
            player = PingPong.Instance.serverBar;
        else
            player = PingPong.Instance.clientBar;
    }

    public void SetSpriteColor()
    {
        // 참여자
        if (id == 1)
            spriteRenderer.color = Color.white;
        // 호스트
        else
            spriteRenderer.color = Color.red;
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
        if(Vector2.Distance(transform.position, player.transform.position) < colDis)
        {
            player.playerDamaged.Damaged();
        }
    }

    public void Pool()
    {
        BulletPool.Instance.Push(this);
    }
}
