using UnityEngine;

public class PlayerController : Entity {

    [SerializeField] private Transform m_spawnPosition;

    public override void DoTurn() {
        GeneratePiece();
    }

    protected override void GeneratePiece() {
        Instantiate(m_piecePrefab, m_spawnPosition.position, Quaternion.identity);
    }
}
