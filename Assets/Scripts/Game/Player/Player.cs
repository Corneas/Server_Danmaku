using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private int hp;
    public int Hp
    {
        set
        {
            hp = value;
            if (hp > 3)
                hp = 3;
            else if (hp <= 0)
                hp = 0;
        }
        get
        {
            return hp;
        }
    }

    int m_id = 0;               //서버・클라이언트 판정용.
    private bool isHost = false;

    private KeyData data;
    private float speed = 4f;

    private Vector3 pos;

    public PlayerUI playerUI { private set; get; } = null;

    public PlayerDamaged playerDamaged { private set; get; } = null;

	void Start()
	{
        Debug.Log(gameObject.name);
        Debug.Log("isHost : " + isHost);
        Debug.Log("m_id : " + m_id);

        playerUI = FindObjectOfType<PlayerUI>();
        playerDamaged = GetComponent<PlayerDamaged>();
        data = InputManager.Instance.GetKeyData(m_id);

    }

    private void Update()
    {
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


    public int GetPlayerId() {
        return m_id;
    }

    public void SetPlayerId(int id) {
        m_id = id;
    }

    public void SetHost(bool _isHost)
    {
        isHost = _isHost;
    }
}
