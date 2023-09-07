using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Transform hpTransform = null;
    [SerializeField]
    private List<Image> hearts = new List<Image>();

    [SerializeField]
    private GameObject[] skillPanels = null;
    private Image[] skillPanelSpriteRenderer = null;

    [SerializeField]
    private TextMeshProUGUI[] skillDelayTmp = null;

    private WaitForEndOfFrame waitForEndOfFrame = null;

    int damagedCount = 0;

    private void Awake()
    {
        Init();

        waitForEndOfFrame = new WaitForEndOfFrame();
    }

    private void Init()
    {
        for(int i = 0; i < skillDelayTmp.Length; ++i)
        {
            skillDelayTmp[i].gameObject.SetActive(false);
        }

        skillPanelSpriteRenderer = new Image[skillPanels.Length];
        for (int i = 0; i < skillPanels.Length; ++i)
        {
            skillPanelSpriteRenderer[i] = skillPanels[i].GetComponent<Image>();
            skillPanelSpriteRenderer[i].gameObject.SetActive(false);
        }

        SetHeart();
    }

    public void SetHeart()
    {
        for (int i = 0; i < hpTransform.childCount; ++i)
        {
            hearts.Add(hpTransform.GetChild(i).GetComponent<Image>());
            hearts[i].gameObject.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        UpdateHeart();
    }

    public void UpdateHeart()
    {
        if (damagedCount >= hpTransform.childCount)
            return;

        hearts[damagedCount].gameObject.SetActive(false);
        damagedCount++;
    }

    public void ReduceSkillDelay(int skillNum, float skillCool, PlayerSkill playerSkill)
    {
        StartCoroutine(IEReduceSkillDelay(skillNum, skillCool, playerSkill));
    }

    public IEnumerator IEReduceSkillDelay(int skillNum, float skillCool, PlayerSkill playerSkill)
    {
        int num = skillNum - 1;
        skillDelayTmp[num].gameObject.SetActive(true);
        skillPanelSpriteRenderer[num].gameObject.SetActive(true);

        while (playerSkill.GetSkillDelay(skillNum) > 0)
        {
            skillDelayTmp[num].SetText(Mathf.RoundToInt(playerSkill.GetSkillDelay(skillNum)).ToString());

            skillPanelSpriteRenderer[num].fillAmount = playerSkill.GetSkillDelay(skillNum) / skillCool;

            yield return waitForEndOfFrame;
        }

        skillDelayTmp[num].gameObject.SetActive(false);
        skillPanelSpriteRenderer[num].gameObject.SetActive(false);
        yield return null;
    }
}
