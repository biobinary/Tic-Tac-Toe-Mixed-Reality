using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsSlider : MonoBehaviour
{

    enum SLIDER_TYPE {
        WIN, LOSE, DRAW
    }

    [Header("Type")]
    [SerializeField] private SLIDER_TYPE sliderType;

    [Header("Slider Properties")]
    [SerializeField] private TextMeshProUGUI m_title;
    [SerializeField] private Slider m_slider;
    [SerializeField] private TextMeshProUGUI m_firstLabel;
    [SerializeField] private TextMeshProUGUI m_lastLabel;

    private Manager m_manager;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void Start() {

        m_slider.value = 0;
        m_slider.maxValue = 1;

        string sliderName = sliderType.ToString().ToLower();
        gameObject.name = sliderName;
        m_title.text = sliderName;

    }

    public void UpdateStats() {

        int totalGame = m_manager.winAmount + m_manager.loseAmount + m_manager.drawAmount;
        m_slider.maxValue = totalGame;
        m_lastLabel.text = totalGame.ToString();

        switch( sliderType ) {
            case SLIDER_TYPE.WIN:
                m_slider.value = m_manager.winAmount;
                m_firstLabel.text = m_manager.winAmount.ToString();
                break;
            case SLIDER_TYPE.LOSE:
                m_slider.value = m_manager.loseAmount;
                m_firstLabel.text = m_manager.loseAmount.ToString();
                break;
            case SLIDER_TYPE.DRAW:
                m_slider.value = m_manager.drawAmount;
                m_firstLabel.text = m_manager.drawAmount.ToString();
                break;
        }

    }

}
