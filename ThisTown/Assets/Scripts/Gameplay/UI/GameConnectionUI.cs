using UnityEngine;
using UnityEngine.UI;

public class GameConnectionUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField ipAddrField;
    [SerializeField] private Button serverStart;
    [SerializeField] private Button clientStart;

    [SerializeField] private GameObject serverWaitMsg;
    [SerializeField] private GameObject clientWaitMsg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clientStart.onClick.AddListener(() =>
        {
            GameStateMananger.Instance.StartGameAsClient(ipAddrField.text);
            clientWaitMsg.gameObject.SetActive(true);
        });

        serverStart.onClick.AddListener(() =>
        {
            GameStateMananger.Instance.StartGameAsServer(ipAddrField.text);
            serverWaitMsg.gameObject.SetActive(true);
            
        });
    }
}
