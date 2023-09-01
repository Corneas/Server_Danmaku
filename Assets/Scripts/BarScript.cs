﻿using UnityEngine;
using System.Collections;

public class BarScript : MonoBehaviour {
    public GameObject m_ballPrefab; //발사할 수 있는 볼.
    float m_shotTime;           //발사한 시간.
    bool m_shotEnable;          //true이면 총알을 쏴도 ok.
    int m_id = 0;               //서버・클라이언트 판정용.
    private float speed = 2f;
    private bool isHost = false;

	// Use this for initialization
	void Start()
	{
        m_shotTime = 1.0f;
        m_shotEnable = false;
	}


	void FixedUpdate () {
        // ID에 대응한 입력값을 얻습니다.
		GameObject manager = GameObject.Find("InputManager");
        KeyData data = manager.GetComponent<InputManager>().GetKeyData(m_id);

        // 이동시킵니다.
        Vector3 pos = transform.position;

        // 서버라면
        if(m_id == 0)
        {
            pos.x += data.horizontal * Time.deltaTime * speed;
            pos.y += data.vertical * Time.deltaTime * speed;
        }
        // 클라(2p)라면
        else if(m_id == 1)
        {
            pos.x -= data.horizontal * Time.deltaTime * speed;
            pos.y -= data.vertical * Time.deltaTime * speed;
        }


		// 양쪽 벽 사이만큼 이동하도록 제한합니다.
		if (pos.x < -3.5f) {
			pos.x = -3.5f;
		} else if (pos.x > 3.5f) {
			pos.x = 3.5f;
		}

		// 이동 후의 위치를 재설정한다.
		transform.position = pos;

        // 버튼을 눌러 볼 발사.
        m_shotTime += Time.fixedDeltaTime;
        if(m_shotEnable && data.inputSkill1){
            if(m_shotTime > 1.0f){
                GameObject ball = Instantiate(m_ballPrefab, transform.position + transform.up*0.8f, transform.rotation) as GameObject;
                ball.GetComponent<BallScript>().SetPlayerId(m_id);
                m_shotTime = 0;
            }
        }
	}
	
	//
    public int GetBarId() {
        return m_id;
    }
    public void SetBarId(int id) {
        m_id = id;
    }

    //총알을 발사할 수 있는지 조정.
    public void SetShotEnable(bool enable){
        m_shotEnable = enable;
    }

    public void SetHost(bool _isHost)
    {
        isHost = _isHost;
    }

}