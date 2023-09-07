using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : PlayerBaseComponent
{
    private int m_id = 0;
    private KeyData data;

    public float skill1Delay { private set; get; } = 8f;
    public float skill2Delay { private set; get; } = 10f;
    private float tempSkill1Delay = 0f;
    private float tempSkill2Delay = 0f;

    private void Start()
    {
        m_id = player.GetPlayerId();

        tempSkill1Delay = skill1Delay;
        tempSkill2Delay = skill2Delay;
    }

    private void Update()
    {
        data = InputManager.Instance.GetKeyData(m_id);

        if (data.isDead)
            return;

        tempSkill1Delay += Time.deltaTime;
        tempSkill2Delay += Time.deltaTime;

        if (tempSkill1Delay > skill1Delay && data.inputSkill1)
        {
            tempSkill1Delay = 0f;
            Bullet bullet = BulletPool.Instance.Pop(transform.position);
            bullet.SetPlayerID(m_id);
            bullet.SetBulletAttribute();
            if(player.GetIsMyClient())
                player.playerUI.ReduceSkillDelay(1, skill1Delay);
        }
        if (tempSkill2Delay > skill2Delay && data.inputSkill2)
        {
            tempSkill2Delay = 0f;
            Bullet bullet = BulletPool.Instance.Pop(transform.position);
            bullet.SetPlayerID(m_id);
            bullet.SetBulletAttribute();
            if (player.GetIsMyClient())
                player.playerUI.ReduceSkillDelay(2, skill2Delay);
        }
    }
}
