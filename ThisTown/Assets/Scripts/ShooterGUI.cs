using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShooterGUI : MonoBehaviour
{

    [SerializeField] Image healthBar;
    [SerializeField] Image roundBanner;
    [SerializeField] TMPro.TMP_Text roundText;
    [SerializeField] Animator animator;

    private static ShooterGUI instance;
    public static ShooterGUI Instance
    {
        get
        {
            if(instance == null)
                instance = FindFirstObjectByType<ShooterGUI>();

            return instance;
        }
    }

    public void UpdateHealth(float valuePerc)
    {
        healthBar.fillAmount = valuePerc;
    }

    public void ShowRound(int roundNumber)
    {
        roundText.text = $"Round {roundNumber}!!";
        animator.SetTrigger("ShowRound");
    }
}
