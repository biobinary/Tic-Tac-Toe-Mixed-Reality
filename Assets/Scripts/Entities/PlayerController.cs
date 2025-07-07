using UnityEngine;

public class PlayerController : MonoBehaviour, IEntity {

    [SerializeField] private Transform m_spawnPosition;
    [SerializeField] private string m_tagName = "PlayerPiece";

	private GameObject m_piecePrefab;

	public void DoTurn() {
        GeneratePiece();
    }

    public void GeneratePiece() {
        GameObject obj = Instantiate(m_piecePrefab, m_spawnPosition.position, Quaternion.identity);
        obj.tag = m_tagName;
    }

	public void SetPiecePrefab(GameObject prefab) {
		m_piecePrefab = prefab;
	}

}
