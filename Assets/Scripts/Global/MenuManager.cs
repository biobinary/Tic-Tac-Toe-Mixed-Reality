using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [Header("Window To Manage")]    
    [SerializeField] private ConfigurationWindow m_configurationWindow;
    [SerializeField] private GameWindow m_gameWindow;

    private void Start() {
        GameManager.Instance.gameStart += OnGameStart;
        GameManager.Instance.gameReset += OnGameReset;
        OnGameReset();
    }

    private void OnGameStart() {
        m_configurationWindow.gameObject.SetActive(false);
        m_gameWindow.gameObject.SetActive(true);
    }

    private void OnGameReset() {
        m_configurationWindow.gameObject.SetActive(true);
        m_gameWindow.gameObject.SetActive(false);
    }

}
