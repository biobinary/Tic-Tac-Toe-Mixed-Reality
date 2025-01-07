using System.Collections.Generic;
using UnityEngine;
using GlobalType;

public class CellManager : MonoBehaviour
{

    [Header("Highlight Helper")]
    [SerializeField] private GameObject m_highlightMesh;
    [SerializeField] Mesh m_circleMesh;
    [SerializeField] Mesh m_crossMesh;
    [SerializeField] private Vector3 m_highlightOffset;

    [Space]

    [Header("Cells")]
    [SerializeField] private List<Cell> m_cells = new List<Cell>();
    private List<Cell> m_usedCells = new List<Cell>();

    private Manager m_manager;
    private Cell m_currentFocusCell = null;
    private Piece m_currentActivePlayerPiece = null;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void Start() {
        
        foreach (var cell in m_cells) { 
            cell.SetCellManager(this);
        }

        m_manager.onGameStart += OnGameStart;
        m_manager.onGameReset += OnGameReset;
        m_highlightMesh.SetActive(false);

    }

    private void OnGameReset() {
        
        m_usedCells.Clear();
        foreach (var cell in m_cells) {
            cell.EnableTrigger();
        }

    }

    private void OnGameStart() {

        MeshFilter filter = m_highlightMesh.GetComponent<MeshFilter>();

        if( m_manager.playerPiece == PieceType.CROSS )
            filter.mesh = m_crossMesh;    
        else
            filter.mesh = m_circleMesh;

    }

    public void SetCellFocusEnter(Cell focusCell, Piece piece) {

        m_currentFocusCell = focusCell;
        m_currentActivePlayerPiece = piece;
        m_currentActivePlayerPiece.onUnselect += OnPiecePlacedOnCell;

        m_highlightMesh.transform.localPosition = focusCell.transform.localPosition + m_highlightOffset;
        m_highlightMesh.SetActive(true);

        foreach (var cell in m_cells) {

            if (cell == focusCell || m_usedCells.Contains(cell))
                continue;

            cell.DisableTrigger();

        }

    }

    public void SetCellFocusExit(Cell focusCell) {

        m_currentFocusCell = null;
        m_highlightMesh.SetActive(false);

        if (m_currentActivePlayerPiece != null) {
            m_currentActivePlayerPiece.onUnselect -= OnPiecePlacedOnCell;
            m_currentActivePlayerPiece = null;
        }

        foreach (var cell in m_cells) {

            if (cell == focusCell || m_usedCells.Contains(cell))
                continue;

            cell.EnableTrigger();

        }

    }

    public void OnPiecePlacedOnCell() {
        
        int pieceIndexPosition = m_cells.IndexOf(m_currentFocusCell);
        Piece pieceToPlaced = m_currentActivePlayerPiece;

        SetCellFocusExit(m_currentFocusCell);
        StashPiece(pieceIndexPosition, pieceToPlaced, EntityType.PLAYER);

    }

    public void StashPiece(int indexPosition, Piece piece, EntityType entityType) {

        Cell cell = m_cells[indexPosition];
        cell.DisableTrigger();
        m_usedCells.Add(cell);

        piece.Stash(cell.transform);
        m_manager.InsertPiece(indexPosition, entityType);

    }

}
