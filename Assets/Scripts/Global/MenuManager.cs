using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [Header("Window To Manage")]    
    [SerializeField] private ConfigurationWindow m_configurationWindow;
    [SerializeField] private GameWindow m_gameWindow;

    private Manager m_manager;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void Start() {
        m_manager.onGameStart += OnGameStart;
        m_manager.onGameReset += OnGameReset;
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
