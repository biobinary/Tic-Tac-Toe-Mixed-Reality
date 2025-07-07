using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using GlobalType;

public class CellManager : MonoBehaviour {

	[SerializeField] private Manager m_manager;
	[SerializeField] private List<SnapCellListener> m_snapCellListener = new();

	private void OnEnable() {

		if (m_snapCellListener.Count > 0) {
			foreach (SnapCellListener listener in m_snapCellListener) {
				listener.OnSelectingInteractorViewAdded += HandleInteractorAdded;
			}
		}

	}

	private void OnDisable() {
		if (m_snapCellListener.Count > 0) {
			foreach (SnapCellListener listener in m_snapCellListener) {
				listener.OnSelectingInteractorViewAdded -= HandleInteractorAdded;
			}
		}
	}

	public void HandleInteractorAdded(SnapCellListener listener, IInteractorView view) {

		GameObject obj = (view.Data as MonoBehaviour).gameObject;
		GameObject parentObj = obj.transform.parent.gameObject;

		Piece piece = parentObj.GetComponent<Piece>();
		piece.DisableInteraction();

		if (piece.isPlayer)
			m_manager.InsertPiece(m_snapCellListener.IndexOf(listener), EntityType.PLAYER);
		else
			m_manager.InsertPiece(m_snapCellListener.IndexOf(listener), EntityType.BOT);

	}

    public void InjectPiece(Piece piece, int indexPosition) {
        SnapInteractable snapInteractable = m_snapCellListener[indexPosition].interactable;
		piece.transform.position = snapInteractable.transform.position;
        piece.InjectInteractable(snapInteractable);
    }

}
