using GlobalType;
using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;

public class CellVisualHightlighter : MonoBehaviour {

	[SerializeField] private List<SnapInteractable> m_snapInteractables = new();
    [SerializeField] private GameObject m_highlightMesh;
	[SerializeField] Mesh m_circleMesh;
	[SerializeField] Mesh m_crossMesh;

	private SnapCellInteractableListener m_cellInteractableListener;
	private Manager m_manager;

	private void Awake() {
		m_manager = FindAnyObjectByType<Manager>();
		m_cellInteractableListener = new(m_snapInteractables);
		m_cellInteractableListener.OnStateChange += HandleOnStateChange;
	}

	private void Start() {
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

	private void HandleOnStateChange(SnapInteractable interactable, InteractableStateChangeArgs args) {

		if (args.NewState == InteractableState.Hover) {
			EnableHighlight(interactable.transform.localPosition);

		} else if (args.PreviousState == InteractableState.Hover) {
			DisableHighlight();

		}

	}

}
