using UnityEngine;

public abstract class Entity : MonoBehaviour {

    protected GameObject m_piecePrefab;

    public void SetPiecePrefab( GameObject prefab ) {
        m_piecePrefab = prefab; 
    }

    public abstract void DoTurn();
    protected abstract void GeneratePiece();

}
