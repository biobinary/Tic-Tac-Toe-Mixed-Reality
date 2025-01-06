using UnityEngine;

public class CellTriggerArea : MonoBehaviour
{

    [SerializeField] private Cell m_cell;
    [SerializeField] private string m_tagFilter;

    private void OnTriggerEnter(Collider other) {
        
        if (!other.CompareTag(m_tagFilter)) return;
        
        Piece piece = other.GetComponent<Piece>();
        if (!piece.isSelected) return;

         m_cell.OnFocusEnter(piece);
    
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag(m_tagFilter)) return;
        m_cell.OnFocusExit();
    }

}
