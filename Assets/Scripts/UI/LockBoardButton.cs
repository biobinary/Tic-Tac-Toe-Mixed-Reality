using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockBoardButton : MonoBehaviour
{

    [SerializeField] private BoardGeneralManager m_board;

    [Header("Icon Properties")]
    [SerializeField] private Image m_icon;
    [SerializeField] private Sprite m_lockBoardImage;
    [SerializeField] private Sprite m_unlockBoardImage;

    [Header("Label")]
    [SerializeField] TextMeshProUGUI m_primaryLabel;
    [SerializeField] TextMeshProUGUI m_secondaryLabel;

    [Header("On Unlocked Text")]
    [SerializeField, TextArea] private string m_primaryLabelUnlockedText;
    [SerializeField, TextArea] private string m_secondaryLabelUnlockedText;

    [Header("On Locked Text")]
    [SerializeField, TextArea] private string m_primaryLabelLockedText;
    [SerializeField, TextArea] private string m_secondaryLabelLockedText;

    private Toggle m_toggle;

    private void Awake() {
        m_toggle = GetComponent<Toggle>();
    }

    private void Start() {
        m_toggle.onValueChanged.AddListener(delegate { OnButtonSelected(m_toggle); });
    }

    public void OnButtonSelected(Toggle change) {

        if( change.isOn ) {
            m_icon.sprite = m_lockBoardImage;
            m_primaryLabel.text = m_primaryLabelLockedText;
            m_secondaryLabel.text = m_secondaryLabelLockedText;

        } else {
            m_icon.sprite = m_unlockBoardImage;
            m_primaryLabel.text = m_primaryLabelUnlockedText;
            m_secondaryLabel.text = m_secondaryLabelUnlockedText;

        }

        m_board.OnBoardInteractableChange(!m_toggle.isOn);

    }

}