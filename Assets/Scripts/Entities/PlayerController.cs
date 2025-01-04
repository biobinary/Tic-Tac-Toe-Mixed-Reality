using Oculus.Interaction;
using UnityEngine;

public class PlayerController : Entity {

    [SerializeField] private Transform m_spawnPosition;

    public override void DoTurn() {
        GeneratePiece();
    }

    protected override void GeneratePiece() {
        GameObject obj = Instantiate(m_piecePrefab, m_spawnPosition.position, Quaternion.identity);
        PointableElement pointableElement = obj.GetComponentInChildren<Grabbable>();
        GameManager.Instance.playerPieceGenerated?.Invoke(pointableElement);
    }
}
