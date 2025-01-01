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

    private void OnEnable() {
        GameManager.Instance.changeTurn += ConfigureTurn;
        GameManager.Instance.changeStats += ConfigureStats;
    }

    private void OnDisable() {
        GameManager.Instance.changeTurn -= ConfigureTurn;
        GameManager.Instance.changeStats -= ConfigureStats;
    }

    private void ConfigureStats() {
        foreach( var stats in m_playerStatistics.GetComponentsInChildren<PlayerStatsSlider>() ) {
            stats.UpdateStats();
        }
    }

    private void ConfigureTurn() {
        if (GameManager.Instance.currentTurn != EntityType.BOT) {
            m_playerTurnStatus.color = m_activeColor;
            m_botTurnStatus.color = m_deactiveColor;
        } else {
            m_playerTurnStatus.color = m_deactiveColor;
            m_botTurnStatus.color = m_activeColor;
        }
    }

}
