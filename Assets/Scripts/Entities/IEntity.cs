using UnityEngine;

public interface IEntity {

    public void SetPiecePrefab(GameObject prefab);
    public void DoTurn();
    public void GeneratePiece();

}
