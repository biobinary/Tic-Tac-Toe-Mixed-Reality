using Oculus.Interaction;
using UnityEngine;

public class PlayerController : Entity {

    [SerializeField] private Transform m_spawnPosition;
    [SerializeField] private string m_tagName = "PlayerPiece";

    public override void DoTurn() {
        GeneratePiece();
    }

    protected override void GeneratePiece() {
        GameObject obj = Instantiate(m_piecePrefab, m_spawnPosition.position, Quaternion.identity);
        obj.tag = m_tagName;
    }

}
