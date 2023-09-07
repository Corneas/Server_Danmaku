using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerBaseComponent
{
    private int m_id = 0;
    private float speed = 4f;
    private KeyData data;

    private Vector3 pos;

    private void Start()
    {
        m_id = player.GetPlayerId();
    }

    private void Update()
    {
        data = InputManager.Instance.GetKeyData(m_id);

        if (data.isDead)
            return;

        pos = transform.position;

        if (data.inputShift)
            speed = 3f;
        else
            speed = 4f;

        // 서버라면
        if (m_id == 0)
        {
            pos.x += data.horizontal * Time.deltaTime * speed;
            pos.x = Mathf.Clamp(pos.x, -3.8f, 3.8f);

            pos.y += data.vertical * Time.deltaTime * speed;
            pos.y = Mathf.Clamp(pos.y, -4.5f, -1f);
        }
        // 클라(2p)라면
        else if (m_id == 1)
        {
            pos.x -= data.horizontal * Time.deltaTime * speed;
            pos.x = Mathf.Clamp(pos.x, -3.5f, 3.5f);

            pos.y -= data.vertical * Time.deltaTime * speed;
            pos.y = Mathf.Clamp(pos.y, 1f, 4.5f);
        }

        transform.position = pos;
    }
}
