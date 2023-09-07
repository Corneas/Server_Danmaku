using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    private int m_id = 0;
    private KeyData data;

    private bool isDamaged = false;

    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        m_id = GameManager.Instance.player.GetPlayerId();
        data = InputManager.Instance.GetKeyData(m_id);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (data.isDamaged)
        {
            StartCoroutine(IEDamaged());
        }
    }

    public void Damaged()
    {
        if (data.isDamaged || data.isDead)
            return;

        if (GameManager.Instance.player.Hp <= 0)
            Dead();

        data.isDamaged = true;
        GameManager.Instance.player.playerUI.UpdateUI();
        StartCoroutine(IEDamaged());
    }

    private IEnumerator IEDamaged()
    {
        if (isDamaged)
            yield break;

        GameManager.Instance.player.Hp--;
        isDamaged = true;
        for (int i = 0; i < 3; ++i)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        data.isDamaged = false;
        isDamaged = false;
    }

    private void Dead()
    {
        data.isDead = true;
        Debug.Log($"{gameObject.name} Dead");
    }
}
