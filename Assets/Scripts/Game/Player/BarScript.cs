using UnityEngine;
using System.Collections;

public class BarScript : MonoBehaviour {

    private int hp = 3;
    private bool isDamaged = false;
    public float skill1Delay { private set; get; } = 8f;
    public float skill2Delay { private set; get; } = 10f;
    private float tempSkill1Delay = 0f;
    private float tempSkill2Delay = 0f;

    [SerializeField]
    private GameObject bulletPrefab = null;

    //public GameObject m_ballPrefab; //발사할 수 있는 볼.
    //float m_shotTime;           //발사한 시간.
    //bool m_shotEnable;          //true이면 총알을 쏴도 ok.
    int m_id = 0;               //서버・클라이언트 판정용.
    private float speed = 4f;
    private bool isHost = false;

    InputManager inputManager = null;
    KeyData data;

    private PlayerUI playerUI = null;
    private SpriteRenderer spriteRenderer = null;

	void Start()
	{
        //m_shotTime = 1.0f;
        //m_shotEnable = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        inputManager = FindObjectOfType<InputManager>();
        playerUI = FindObjectOfType<PlayerUI>();

        tempSkill1Delay = skill1Delay;
        tempSkill2Delay = skill2Delay;
        data = inputManager.GetKeyData(m_id);
    }


	//void FixedUpdate () {
    void Update()
    {
        // ID에 대응한 입력값을 얻습니다.
        // GameObject manager = GameObject.Find("InputManager");
        // KeyData data = inputManager.GetComponent<InputManager>().GetKeyData(m_id);

        // 이동시킵니다.
        Vector3 pos = transform.position;

        if (data.inputShift)
            speed = 3f;
        else
            speed = 4f;

        // 서버라면
        if(m_id == 0)
        {
            pos.x += data.horizontal * Time.deltaTime * speed;
            pos.x = Mathf.Clamp(pos.x, -3.8f, 3.8f);

            pos.y += data.vertical * Time.deltaTime * speed;
            pos.y = Mathf.Clamp(pos.y, -4.5f, -1f);
        }
        // 클라(2p)라면
        else if(m_id == 1)
        {
            pos.x -= data.horizontal * Time.deltaTime * speed;
            pos.x = Mathf.Clamp(pos.x, -3.5f, 3.5f);

            pos.y -= data.vertical * Time.deltaTime * speed;
            pos.y = Mathf.Clamp(pos.y, 1f, 4.5f);
        }


        // 양쪽 벽 사이만큼 이동하도록 제한합니다.


        //if (pos.x < -3.5f) {
        //	pos.x = -3.5f;
        //} else if (pos.x > 3.5f) {
        //	pos.x = 3.5f;
        //}

        // 이동 후의 위치를 재설정한다.
        transform.position = pos;

        tempSkill1Delay += Time.deltaTime;
        tempSkill2Delay += Time.deltaTime;

        if(tempSkill1Delay > skill1Delay && data.inputSkill1)
        {
            tempSkill1Delay = 0f;
            Bullet bullet = BulletPool.Instance.Pop(pos);
            bullet.SetPlayerID(m_id);
            //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //bullet.GetComponent<Bullet>().SetPlayerID(m_id);
            playerUI.ReduceSkillDelay(1, skill1Delay);
        }
        if(tempSkill2Delay > skill2Delay && data.inputSkill2)
        {
            tempSkill2Delay = 0f;
            Bullet bullet = BulletPool.Instance.Pop(pos);
            bullet.SetPlayerID(m_id);
            //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //bullet.GetComponent<Bullet>().SetPlayerID(m_id);
            playerUI.ReduceSkillDelay(2, skill2Delay);
        }

        if (data.isDamaged)
        {
            StartCoroutine(IEDamaged());
        }

        // 버튼을 눌러 볼 발사.
        //m_shotTime += Time.fixedDeltaTime;
        //if (m_shotEnable && data.inputSkill1)
        //{
        //    if (m_shotTime > 1.0f)
        //    {
        //        GameObject ball = Instantiate(m_ballPrefab, transform.position + transform.up * 0.8f, transform.rotation) as GameObject;
        //        ball.GetComponent<BallScript>().SetPlayerId(m_id);
        //        m_shotTime = 0;
        //    }
        //}
    }
	
	//
    public int GetBarId() {
        return m_id;
    }
    public void SetBarId(int id) {
        m_id = id;
    }

    //총알을 발사할 수 있는지 조정.
    //public void SetShotEnable(bool enable){
    //    m_shotEnable = enable;
    //}

    public void SetHost(bool _isHost)
    {
        isHost = _isHost;
    }

    public void Damaged()
    {
        if (data.isDamaged)
            return;

        data = inputManager.GetKeyData(m_id);
        data.isDamaged = true;
        playerUI.UpdateUI();
        StartCoroutine(IEDamaged());
    }

    private IEnumerator IEDamaged()
    {
        if (isDamaged)
            yield break;

        hp--;
        isDamaged = true;
        for(int i = 0; i < 3; ++i)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        data = inputManager.GetKeyData(m_id);
        data.isDamaged = false;
        isDamaged = false;
    }

    private void Dead()
    {

    }
}
