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
    private bool isMyClient = false;

    private KeyData data;
    private float speed = 4f;

    private Vector3 pos;

    public PlayerUI playerUI { private set; get; } = null;

    public PlayerDamaged playerDamaged { private set; get; } = null;

	void Start()
	{
        playerUI = FindObjectOfType<PlayerUI>();
        playerDamaged = GetComponent<PlayerDamaged>();
        data = InputManager.Instance.GetKeyData(m_id);

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

    public bool GetHost()
    {
        return isHost;
    }

    public void SetIsMyClient(bool _isMyClient)
    {
        isMyClient = _isMyClient;
    }
    public bool GetIsMyClient()
    {
        return isMyClient;
    }
}