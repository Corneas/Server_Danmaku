using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : PlayerBaseComponent
{
    private int m_id = 0;
    private KeyData data;

    private bool isDamaged = false;

    private SpriteRenderer spriteRenderer = null;

    private Coroutine damagedCo = null;

    private void Start()
    {
        m_id = player.GetPlayerId();

        spriteRenderer = GetComponent<SpriteRenderer>();

        damagedCo = null;
    }

    private void Update()
    {
        data = InputManager.Instance.GetKeyData(m_id);

        if (data.isDamaged && damagedCo == null)
        {
            damagedCo = StartCoroutine(IEDamaged());
        }
    }

    // 데미지를 입었음을 서버에 보내기 위함
    public void Damaged()
    {
        if (data.isDamaged || data.isDead)
            return;

        data.isDamaged = true;
        InputManager.Instance.SetInputData(m_id, data);
    }

    // 클라에서 보이는 데미지를 입었을 경우
    private IEnumerator IEDamaged()
    {
        if (isDamaged)
            yield break;

        isDamaged = true;

        player.Hp--;

        if (player.GetIsMyClient())
            player.playerUI.UpdateUI();

        if (player.Hp <= 0)
        {
            Dead();

            data.isDamaged = false;
            isDamaged = false;
            damagedCo = null;
            InputManager.Instance.SetInputData(m_id, data);

            yield break;
        }

        for (int i = 0; i < 3; ++i)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        data.isDamaged = false;
        isDamaged = false;
        damagedCo = null;
        InputManager.Instance.SetInputData(m_id, data);
    }

    private void Dead()
    {
        data.isDead = true;
        InputManager.Instance.SetInputData(m_id, data);
        player.gameObject.SetActive(false);
    }
}
