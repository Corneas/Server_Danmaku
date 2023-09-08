using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedShot : SkillBase
{
    [SerializeField]
    private GameObject guidedShotBulletPrefab = null;

    private Transform targetTransform = null;

    [Header("미사일 기능 관련")]
    [SerializeField]
    private float speed = 1; // 미사일 속도.
    [Space(10f)]
    [SerializeField]
    private float distanceFromStart = 6.0f; // 시작 지점을 기준으로 얼마나 꺾일지.
    [SerializeField]
    private float distanceFromEnd = 3.0f; // 도착 지점을 기준으로 얼마나 꺾일지.
    [Space(10f)]
    [SerializeField]
    private int shotCount = 12; // 총 몇 개 발사할건지.
    [SerializeField]
    [Range(0, 1)] private float interval = 0.15f;
    [SerializeField]
    private int shotCountEveryInterval = 2; // 한번에 몇 개씩 발사할건지.

    public override void Fire()
    {
        StartCoroutine(CreateMissile());
    }

    IEnumerator CreateMissile()
    {
        int _shotCount = shotCount;
        while (_shotCount > 0)
        {
            for (int i = 0; i < shotCountEveryInterval; i++)
            {
                if (_shotCount > 0)
                {
                    BezierBullet bezierBullet = PoolManager.Get(guidedShotBulletPrefab.GetComponent<BezierBullet>());
                    bezierBullet.transform.position = player.transform.position;
                    bezierBullet.SetPlayerID(player.GetPlayerId());
                    bezierBullet.SetBulletAttribute(player.GetIsMyClient());

                    bezierBullet.Init(player.transform, targetTransform, speed, distanceFromStart, distanceFromEnd);

                    _shotCount--;
                }
            }
            yield return new WaitForSeconds(interval);
        }
        yield return null;
    }
}
