using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using GlobalType;

public class CellManager : MonoBehaviour {

	[SerializeField] private Manager m_manager;
	[SerializeField] private List<SnapInteractable> m_snapInteractables = new();
	
	private SnapCellInteractableListener m_listener;

    private void Awake() {
		m_listener = new(m_snapInteractables);
		m_listener.OnInteractorViewAdded += HandleInteractorAdded;
	}

	private void HandleInteractorAdded(int cellIndex, IInteractorView view) {

		GameObject obj = (view.Data as MonoBehaviour).gameObject;
		GameObject parentObj = obj.transform.parent.gameObject;

		Piece piece = parentObj.GetComponent<Piece>();
		piece.DisableInteraction();

		if (piece.isPlayer)
			m_manager.InsertPiece(cellIndex, EntityType.PLAYER);
		else
			m_manager.InsertPiece(cellIndex, EntityType.BOT);

	}

    public void InjectPiece(Piece piece, int indexPosition) {
        SnapInteractable snapInteractable = m_snapInteractables[indexPosition];
		piece.transform.position = snapInteractable.transform.position;
        piece.InjectInteractable(snapInteractable);
    }

}
