using UnityEngine;
using UnityEngine.UI;

public class GameWindow : MonoBehaviour
{

    // The Background Not The Gameobject
    [Header("Game Status")]
    [SerializeField] private Image m_playerTurnStatus;
    [SerializeField] private Image m_botTurnStatus;

    private Color m_activeColor = new Color(0f, 0.3921569f, 0.8784314f);
    private Color m_deactiveColor = new Color(1.0f, 1.0f, 1.0f, 0.03921569f);

    [Header("Player Statistics")]
    [SerializeField] private GameObject m_playerStatistics;

    private Manager m_manager;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void OnEnable() {
        m_manager.onChangeTurn += ConfigureTurn;
        m_manager.onChangeStats += ConfigureStats;
    }

    private void OnDisable() {
        m_manager.onChangeTurn -= ConfigureTurn;
        m_manager.onChangeStats -= ConfigureStats;
    }

    private void ConfigureStats() {
        foreach( var stats in m_playerStatistics.GetComponentsInChildren<PlayerStatsSlider>() ) {
            stats.UpdateStats();
        }
    }

    private void ConfigureTurn() {

        if (m_manager.currentTurn != GlobalType.EntityType.BOT) {
            m_playerTurnStatus.color = m_activeColor;
            m_botTurnStatus.color = m_deactiveColor;

        } else {
            m_playerTurnStatus.color = m_deactiveColor;
            m_botTurnStatus.color = m_activeColor;
        
        }

    }

}
