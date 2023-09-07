using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : PlayerBaseComponent
{
    private int m_id = 0;
    private KeyData data;

    private float skill1Delay  = 8f;
    private float skill2Delay  = 10f;
    private float tempSkill1Delay = 0f;
    private float tempSkill2Delay = 0f;

    private bool useSkill1 = false;
    private bool useSkill2 = false;

    private void Start()
    {
        m_id = player.GetPlayerId();
    }

    private void Update()
    {
        data = InputManager.Instance.GetKeyData(m_id);


        if (data.isDead)
            return;

        tempSkill1Delay -= Time.deltaTime;
        tempSkill2Delay -= Time.deltaTime;

        Debug.Log("data inputSkill1 : " + data.inputSkill1);
        Debug.Log("data inputSkill2 : " + data.inputSkill2);

        if (tempSkill1Delay <= 0 && data.inputSkill1)
        {
            tempSkill1Delay = skill1Delay;
            Bullet bullet = BulletPool.Instance.Pop(transform.position);
            bullet.SetPlayerID(m_id);
            bullet.SetBulletAttribute(player.GetIsMyClient());
            if(player.GetIsMyClient())
                player.playerUI.ReduceSkillDelay(1, skill1Delay, this);
        }
        if (tempSkill2Delay <= 0 && data.inputSkill2)
        {
            tempSkill2Delay = skill2Delay;
            Bullet bullet = BulletPool.Instance.Pop(transform.position);
            bullet.SetPlayerID(m_id);
            bullet.SetBulletAttribute(player.GetIsMyClient());
            if (player.GetIsMyClient())
                player.playerUI.ReduceSkillDelay(2, skill2Delay, this);
        }
    }

    public float GetSkillDelay(int num)
    {
        return num == 1 ? tempSkill1Delay : tempSkill2Delay;
    }
}
