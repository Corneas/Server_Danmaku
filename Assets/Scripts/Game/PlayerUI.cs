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
        for (int i = 0; i < skillPanelSpriteRenderer.Length; ++i)
        {
            skillPanelSpriteRenderer[i] = skillPanels[i].GetComponent<Image>();
        }

        SetHeart();

        waitForEndOfFrame = new WaitForEndOfFrame();
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

    public void ReduceSkillDelay(int skillNum, float skillCool)
    {
        StartCoroutine(IEReduceSkillDelay(skillNum, skillCool));
    }

    public IEnumerator IEReduceSkillDelay(int skillNum, float skillCool)
    {
        int num = skillNum - 1;
        skillDelayTmp[num].gameObject.SetActive(true);

        while(skillCool > 0)
        {
            skillCool -= Time.deltaTime;

            skillDelayTmp[num].SetText(Mathf.RoundToInt(skillCool).ToString());

            skillPanelSpriteRenderer[num].fillAmount = 1 / skillCool;

            yield return waitForEndOfFrame;
        }

        skillDelayTmp[num].gameObject.SetActive(false);
        yield return null;
    }
}
