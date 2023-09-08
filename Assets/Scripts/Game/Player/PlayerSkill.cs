using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : PlayerBaseComponent
{
    private int m_id = 0;
    private KeyData data;

    private float fireDelay = 0.2f;
    private float tempFireDelay = 0f;

    private float skill1Delay  = 8f;
    private float skill2Delay  = 10f;
    private float tempSkill1Delay = 0f;
    private float tempSkill2Delay = 0f;

    [SerializeField]
    private GameObject skillObj;
    private SkillBase[] skills;

    [SerializeField]
    private GameObject bulletPrefab = null;

    private void Start()
    {
        m_id = player.GetPlayerId();
        SetSkills();
    }

    private void SetSkills()
    {
        skills = skillObj.GetComponentsInChildren<SkillBase>();
        foreach(var skill in skills)
        {
            skill.SetPlayer(player);
        }
    }

    private void Update()
    {
        data = InputManager.Instance.GetKeyData(m_id);

        if (data.isDead)
            return;

        tempFireDelay -= Time.deltaTime;
        tempSkill1Delay -= Time.deltaTime;
        tempSkill2Delay -= Time.deltaTime;

        if(tempFireDelay <= 0 && data.inputSpace)
        {
            tempFireDelay = fireDelay;

            Bullet bullet = PoolManager.Get(bulletPrefab.GetComponent<Bullet>());
            bullet.transform.position = player.transform.position;
            bullet.SetPlayerID(player.GetPlayerId());
            bullet.SetBulletAttribute(player.GetIsMyClient());

            if (m_id == 0)
                bullet.transform.right = Vector3.up;
            else
                bullet.transform.right = Vector3.down;
        }
        if (tempSkill1Delay <= 0 && data.inputSkill1)
        {
            tempSkill1Delay = skill1Delay;

            skills[0].Fire();

            if(player.GetIsMyClient())
                player.playerUI.ReduceSkillDelay(1, skill1Delay, this);
        }
        if (tempSkill2Delay <= 0 && data.inputSkill2)
        {
            tempSkill2Delay = skill2Delay;

            skills[1].Fire();

            if (player.GetIsMyClient())
                player.playerUI.ReduceSkillDelay(2, skill2Delay, this);
        }
    }

    public float GetSkillDelay(int num)
    {
        return num == 1 ? tempSkill1Delay : tempSkill2Delay;
    }
}
