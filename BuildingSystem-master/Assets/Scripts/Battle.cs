using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    [SerializeField] BuildingsGrid Town;
    [SerializeField] GameObject WinText;
    [SerializeField] GameObject LoseText;
    [SerializeField] GameObject BattlesCanvas;

    private void Awake()
    {
        WinText.SetActive(false);
        LoseText.SetActive(false);
        BattlesCanvas.SetActive(false);
    }
    public void BattleOutcome(int EnemyArmyPower)
    {
        if (Town.ArmyPower >= EnemyArmyPower) 
        {
            Town.ArmyPower += EnemyArmyPower / 2;
            BattlesCanvas.SetActive(false);
            WinText.SetActive(true);
            StartCoroutine(FadeTextToZeroAlpha(1f, WinText.GetComponent<Text>()));
        }
        else
        {
            Town.ArmyPower = 0;
            BattlesCanvas.SetActive(false);
            LoseText.SetActive(true);
            StartCoroutine(FadeTextToZeroAlpha(1f, LoseText.GetComponent<Text>()));
        }
    }

    public void OpenBattles()
    {
        BattlesCanvas.SetActive(true);
    }

    public void CloseBattles()
    {
        BattlesCanvas.SetActive(false);
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        yield return new WaitForSeconds(2f);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        WinText.SetActive(false);
    }
}
