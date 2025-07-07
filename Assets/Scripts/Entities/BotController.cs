using UnityEngine;
using GlobalType;

public class BotController : MonoBehaviour, IEntity {

    [SerializeField]
    private CellManager m_cellManager;

	private GameObject m_piecePrefab;
	private Manager m_manager;
    private IAlgorithm m_algorithm;

	private string[][] m_boardState;
    private int m_indexPicked = -1;

    private string m_player;
    private string m_bot;

    public System.Action stopOnGoingTask;

    public void SetManager(Manager manager) {
        if (manager == null || manager == m_manager) return;
        m_manager = manager;
        m_manager.onGameReset += OnGameReset;
    }

    public void SetAlgorithm(IAlgorithm algorithm) {
        if( algorithm == null || algorithm == m_algorithm) return;
        m_algorithm = algorithm;
        m_algorithm.SetController(this);
    }

    private void OnGameReset() {
        stopOnGoingTask?.Invoke();
    }

    public void DoTurn() {

        m_boardState = m_manager.GetCurrentBoardState();
        m_player = m_manager.playerPiece == PieceType.CROSS ? "X" : "O";
        m_bot = m_manager.playerPiece == PieceType.CROSS ? "O" : "X";

        if (m_algorithm != null)
            m_algorithm.Calculate();

    }

    public string[][] GetBoardState() {
        return m_boardState;
    }

    public string GetPlayer() {
        return m_player;
    }

    public string GetBot() {
        return m_bot;
    }

    public void GeneratePieceAtIndex(int index) {
        m_indexPicked = index;
        GeneratePiece();
    }

    public void GeneratePiece() {

        GameObject obj = Instantiate(m_piecePrefab);
        Piece piece = obj.GetComponent<Piece>();

        piece.isPlayer = false;
        m_cellManager.InjectPiece(piece, m_indexPicked);
    
    }

	public void SetPiecePrefab(GameObject prefab) {
		m_piecePrefab = prefab;
	}

}
