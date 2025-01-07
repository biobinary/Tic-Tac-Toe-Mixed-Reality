using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GlobalType;

public class GameWindow : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI m_windowTitleLabel;

    [Space]

    // The Background Not The Gameobject
    [Header("Game Status")]
    [SerializeField] private Image m_playerTurnStatus;
    [SerializeField] private TextMeshProUGUI m_playerScoreIndicator;
    [SerializeField] private Image m_botTurnStatus;
    [SerializeField] private TextMeshProUGUI m_botScoreIndicator;

    private Color m_activeColor = new Color(0f, 0.3921569f, 0.8784314f);
    private Color m_deactiveColor = new Color(1.0f, 1.0f, 1.0f, 0.03921569f);

    [Header("Player Statistics")]
    [SerializeField] private GameObject m_playerStatistics;

    private Manager m_manager;

    private int m_playerCurrentScore = 0;
    private int m_botCurrentScore = 0;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void OnEnable() {

        m_playerCurrentScore = 0;
        m_botCurrentScore = 0;

        m_playerScoreIndicator.text = "0";
        m_botScoreIndicator.text = "0";

        m_manager.onChangeTurn += ConfigureTurn;
        m_manager.onChangeStats += ConfigureStats;
        m_manager.onGameEnd += OnGameEnd;

    }

    private void OnDisable() {
        m_manager.onChangeTurn -= ConfigureTurn;
        m_manager.onChangeStats -= ConfigureStats;
        m_manager.onGameEnd -= OnGameEnd;
    }

    private void ConfigureStats() {
        foreach( var stats in m_playerStatistics.GetComponentsInChildren<PlayerStatsSlider>() ) {
            stats.UpdateStats();
        }
    }

    private void ConfigureTurn() {

        if (m_manager.currentTurn != EntityType.BOT) {
            m_playerTurnStatus.color = m_activeColor;
            m_botTurnStatus.color = m_deactiveColor;

        } else {
            m_playerTurnStatus.color = m_deactiveColor;
            m_botTurnStatus.color = m_activeColor;
        
        }

    }

    private void OnGameEnd(GameResultType result) {

        m_playerTurnStatus.color = m_deactiveColor;
        m_botTurnStatus.color = m_deactiveColor;

        switch( result ) {
            case GameResultType.WIN:
                m_windowTitleLabel.text = "Player Win";
                m_playerCurrentScore++;
                m_playerScoreIndicator.text = m_playerCurrentScore.ToString();
                break;
            case GameResultType.LOSE:
                m_windowTitleLabel.text = "Bot Win";
                m_botCurrentScore++;
                m_botScoreIndicator.text = m_botCurrentScore.ToString();
                break;
            default:
                m_windowTitleLabel.text = "Draw";
                break;
        }

    }

    public void OnResetGameButtonPressed() {
        m_windowTitleLabel.text = "Playing";
        m_manager.ResetGame();
    }

    public void BackToConfiguration() {
        m_manager.ResetConfiguration();
    }

    public void ResetStatistics() {
        m_manager.ResetStatistics();
    }

}
