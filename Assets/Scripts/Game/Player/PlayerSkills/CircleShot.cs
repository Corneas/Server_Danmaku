using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShot : SkillBase
{
    private float angle = 10f;
    private float fireAngle = 0f;

    private int bulletAmount = 36;
    private int fireAmount = 2;

    private float fireInterval = 0.5f;
    private WaitForSeconds waitForFireInterval;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        waitForFireInterval = new WaitForSeconds(fireInterval);
    }

    public override void Fire()
    {
        StartCoroutine(IECircleShot());
    }

    public IEnumerator IECircleShot()
    {
        Vector3 dir;

        for(int i = 0; i < fireAmount; ++i)
        {
            fireAngle += angle / 2;

            for(int j = 0; j < bulletAmount; ++j)
            {
                Bullet bullet = PoolManager.Get(bulletPrefab.GetComponent<Bullet>());
                bullet.transform.position = player.transform.position;
                bullet.SetPlayerID(player.GetPlayerId());
                bullet.SetBulletAttribute(player.GetIsMyClient());

                dir = new Vector3(Mathf.Cos(fireAngle * Mathf.Deg2Rad), Mathf.Sin(fireAngle * Mathf.Deg2Rad)).normalized;
                bullet.transform.right = dir;

                fireAngle += angle;

                if (fireAngle >= 360)
                    fireAngle -= 360;
            }
            yield return waitForFireInterval;
        }
    }
}
