using UnityEngine;

public class PokerGameController : MonoBehaviour
{
    private void Start()
    {
        GameStateMananger.Instance?.SwitchState(GameStateMananger.GameState.Shooter);
    }
}
