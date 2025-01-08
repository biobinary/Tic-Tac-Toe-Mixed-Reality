using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using GlobalType;

public class CellManager : MonoBehaviour {

    [SerializeField] private List<SnapCell> m_cells = new List<SnapCell>();
    [SerializeField] private GameObject m_highlightMesh;
    [SerializeField] Mesh m_circleMesh;
    [SerializeField] Mesh m_crossMesh;

    private Manager m_manager;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void Start() {

        foreach (var cell in m_cells) {
            cell.SetManager(this);
        }

        m_manager.onGameStart += OnGameStart;
        m_highlightMesh.SetActive(false);

    }

    private void OnGameStart() {

        MeshFilter filter = m_highlightMesh.GetComponent<MeshFilter>();

        if (m_manager.playerPiece == PieceType.CROSS)
            filter.mesh = m_crossMesh;
        else
            filter.mesh = m_circleMesh;

    }

    public void EnableHighlight(Vector3 position) {
        m_highlightMesh.transform.localPosition = position;
        m_highlightMesh.SetActive(true);
    }

    public void DisableHighlight() {
        m_highlightMesh.SetActive(false);
    }

    public void HandlePiecePlaced(SnapCell cell, EntityType entityType) {
        m_manager.InsertPiece(m_cells.IndexOf(cell), entityType);
    }

    public void InjectPiece(Piece piece, int indexPosition) {
        
        SnapCell cell = m_cells[indexPosition];
        SnapInteractable snapInteractable = cell.GetSnapInteractableComponent();

        piece.transform.position = cell.transform.position;
        piece.InjectInteractable(snapInteractable);

    }

}
