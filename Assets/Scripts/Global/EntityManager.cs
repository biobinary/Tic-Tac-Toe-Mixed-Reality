using UnityEngine;

public class EntityManager : MonoBehaviour
{

    [Header("Piece")]
    [SerializeField] private GameObject m_crossPiece;
    [SerializeField] private GameObject m_circlePiece;

    [Header("Controller")]
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private BotController m_botController;

    private Manager m_manager;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
        m_botController.SetManager(m_manager);
    }

    private void Start() {
        m_manager.onGameStart += OnGameStart;
        m_manager.onChangeTurn += OnChangeTurn;
    }

    private void OnGameStart() {

        m_botController.SetAlgorithm(m_manager.algorithmUsed);

        if (m_manager.playerPiece == GlobalType.PieceType.CROSS) {
            m_playerController.SetPiecePrefab(m_crossPiece);
            m_botController.SetPiecePrefab(m_circlePiece);

        } else {
            m_playerController.SetPiecePrefab(m_circlePiece);
            m_botController.SetPiecePrefab(m_crossPiece);

        }

    }

    private void OnChangeTurn() {

        if (m_manager.currentTurn == GlobalType.EntityType.PLAYER)
            m_playerController.DoTurn();
        else
            m_botController.DoTurn();

    }

}
