using UnityEngine;

public class Cell : MonoBehaviour
{

    [SerializeField] private GameObject m_cellTrigger;

    private CellManager m_cellManager;
    
    public void SetCellManager(CellManager cellManager) {
        m_cellManager = cellManager;
    }

    public void OnFocusEnter(Piece piece) {
        if (m_cellManager == null) return;
        if (!m_cellTrigger.activeSelf) return;
        m_cellManager.SetCellFocusEnter(this, piece);
    }

    public void OnFocusExit() {
        if( m_cellManager == null) return;
        m_cellManager.SetCellFocusExit(this);
    }

    public void EnableTrigger() {
        m_cellTrigger.SetActive(true);
    }

    public void DisableTrigger() {
        m_cellTrigger.SetActive(false);
    }

}
