using GlobalType;
using Oculus.Interaction;
using UnityEngine;

public class CellVisualHighlighter : MonoBehaviour {

	[SerializeField] private Manager m_manager;
	[SerializeField] private SnapInteractable m_snapInteractable;
    [SerializeField] private GameObject m_highlightMesh;
	[SerializeField] Mesh m_circleMesh;
	[SerializeField] Mesh m_crossMesh;

	private void Start() {

		if (m_manager != null)
			m_manager.onGameStart += OnGameStart;

		if (m_snapInteractable != null)
			m_snapInteractable.WhenStateChanged += HandleOnStateChange;

		m_highlightMesh.SetActive(false);

	}

	private void OnGameStart() {

		MeshFilter filter = m_highlightMesh.GetComponent<MeshFilter>();

		if (m_manager.playerPiece == PieceType.CROSS)
			filter.mesh = m_crossMesh;
		else
			filter.mesh = m_circleMesh;

	}

	private void OnDisable() {
		
		if(m_manager != null)
			m_manager.onGameStart -= OnGameStart;

		if (m_snapInteractable != null)
			m_snapInteractable.WhenStateChanged -= HandleOnStateChange;

	}

	public void EnableHighlight(Vector3 position) {
		m_highlightMesh.transform.localPosition = position;
		m_highlightMesh.SetActive(true);
	}

	public void DisableHighlight() {
		m_highlightMesh.SetActive(false);
	}

	private void HandleOnStateChange(InteractableStateChangeArgs args) {

		if (args.NewState == InteractableState.Hover) {
			EnableHighlight(transform.localPosition);

		} else if (args.PreviousState == InteractableState.Hover) {
			DisableHighlight();

		}

	}

}
