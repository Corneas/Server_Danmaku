using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedShot : SkillBase
{
    [SerializeField]
    private GameObject guidedShotBulletPrefab = null;

    private Transform targetTransform = null;

    [Header("�̻��� ��� ����")]
    [SerializeField]
    private float speed = 1; // �̻��� �ӵ�.
    [Space(10f)]
    [SerializeField]
    private float distanceFromStart = 6.0f; // ���� ������ �������� �󸶳� ������.
    [SerializeField]
    private float distanceFromEnd = 3.0f; // ���� ������ �������� �󸶳� ������.
    [Space(10f)]
    [SerializeField]
    private int shotCount = 12; // �� �� �� �߻��Ұ���.
    [SerializeField]
    [Range(0, 1)] private float interval = 0.15f;
    [SerializeField]
    private int shotCountEveryInterval = 2; // �ѹ��� �� ���� �߻��Ұ���.

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
