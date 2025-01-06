using UnityEngine;

public abstract class Entity : MonoBehaviour {

    protected GameObject m_piecePrefab;
    protected Manager m_manager;

    protected void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    public void SetPiecePrefab( GameObject prefab ) {
        m_piecePrefab = prefab; 
    }

    public abstract void DoTurn();

    protected abstract void GeneratePiece();

}
