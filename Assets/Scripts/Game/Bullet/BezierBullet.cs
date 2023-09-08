using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierBullet : Bullet
{
    Vector3[] m_points = new Vector3[4];

    private float m_timerMax = 0;
    private float m_timerCurrent = 0;
    private float m_speed;

    private Vector3 endPos;

    private void OnEnable()
    {
        m_timerCurrent = 0f;
    }

    public void Init(Transform _startTr, Transform _endTr, float _speed, float _newPointDistanceFromStartTr, float _newPointDistanceFromEndTr)
    {
        _endTr = target.transform;
        endPos = _endTr.transform.position;

        m_speed = _speed;

        // ���� ������ �ð��� �������� ��.
        m_timerMax = Random.Range(0.8f, 1.0f);

        // ���� ����.
        m_points[0] = _startTr.position;

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[1] = _startTr.position +
            (_newPointDistanceFromStartTr * Random.Range(-1.0f, 1.0f) * _startTr.right) + // X (��, �� ��ü)
            (_newPointDistanceFromStartTr * Random.Range(-0.15f, 1.0f) * _startTr.up); // Y (�Ʒ��� ����, ���� ��ü)

        // ���� ������ �������� ���� ����Ʈ ����.
        m_points[2] = endPos +
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.right) + // X (��, �� ��ü)
            (_newPointDistanceFromEndTr * Random.Range(-1.0f, 1.0f) * _endTr.up); // Y (��, �Ʒ� ��ü)

        // ���� ����.
        m_points[3] = endPos;

        transform.position = _startTr.position;
    }

    protected override void Update()
    {
        base.Update();

        if (m_timerCurrent > m_timerMax)
        {
            PoolManager.Release(this);
            return;
        }

        // ��� �ð� ���.
        m_timerCurrent += Time.deltaTime * m_speed;

        // ������ ����� X,Y,Z ��ǥ ���.
        transform.position = new Vector3(
            CubicBezierCurve(m_points[0].x, m_points[1].x, m_points[2].x, m_points[3].x),
            CubicBezierCurve(m_points[0].y, m_points[1].y, m_points[2].y, m_points[3].y)
        );
    }

    /// <summary>
    /// 3�� ������ �.
    /// </summary>
    /// <param name="a">���� ��ġ</param>
    /// <param name="b">���� ��ġ���� �󸶳� ���� �� ���ϴ� ��ġ</param>
    /// <param name="c">���� ��ġ���� �󸶳� ���� �� ���ϴ� ��ġ</param>
    /// <param name="d">���� ��ġ</param>
    /// <returns></returns>
    private float CubicBezierCurve(float a, float b, float c, float d)
    {
        // (0~1)�� ���� ���� ������ � ���� ���ϱ� ������, ������ ���� �ð��� ���ߴ�.
        float t = m_timerCurrent / m_timerMax; // (���� ��� �ð� / �ִ� �ð�)

        // ������.
        /*
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * 3 * t * b
            + Mathf.Pow(t, 2) * 3 * (1 - t) * c
            + Mathf.Pow(t, 3) * d;
        */

        // �����Ѵ�� ���ϰ� ����.
        float ab = Mathf.Lerp(a, b, t);
        float bc = Mathf.Lerp(b, c, t);
        float cd = Mathf.Lerp(c, d, t);

        float abbc = Mathf.Lerp(ab, bc, t);
        float bccd = Mathf.Lerp(bc, cd, t);

        return Mathf.Lerp(abbc, bccd, t);
    }

    protected override void Move()
    {

    }
}
