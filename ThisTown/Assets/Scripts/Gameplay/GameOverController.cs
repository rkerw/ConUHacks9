using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text winText;

    private void Start()
    {
        int winner = HackyMemory.GetWinner();
        winText.text = $"Player {winner + 1} Wins!!";
    }
}
