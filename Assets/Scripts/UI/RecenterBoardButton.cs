using UnityEngine;
using UnityEngine.UI;

public class RecenterBoardButton : MonoBehaviour
{

    [SerializeField] private BoardGeneralManager m_board; 
    private Toggle m_toggle;
    
    private void Awake() {
        m_toggle = GetComponent<Toggle>();
    }

    private void Start() {
        m_toggle.onValueChanged.AddListener(delegate { OnButtonPressed(m_toggle); });
    }

    private void OnButtonPressed(Toggle change) {
        m_board.RecenterBoard();
    }

}
