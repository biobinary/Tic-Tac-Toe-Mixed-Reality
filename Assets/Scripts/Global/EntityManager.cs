using UnityEngine;

public class EntityManager : MonoBehaviour
{

    [SerializeField] private GameObject m_crossPiece;
    [SerializeField] private GameObject m_circlePiece;

    private PlayerController m_playerController;
    private BotController m_botController;

    private void Awake() {
        m_playerController = FindAnyObjectByType<PlayerController>();
        m_botController = FindAnyObjectByType<BotController>();
    }

    void Start()
    {
        GameManager.Instance.gameStart += OnGameStart;
        GameManager.Instance.changeTurn += OnChangeTurn;
    }

    private void OnGameStart() {

        if( GameManager.Instance.playerPiece == PieceType.CROSS ) {
            m_playerController.SetPiecePrefab(m_crossPiece);
            m_botController.SetPiecePrefab(m_circlePiece);

        } else {
            m_playerController.SetPiecePrefab(m_circlePiece);
            m_botController.SetPiecePrefab(m_crossPiece);

        }

    }

    private void OnChangeTurn() {

        if (GameManager.Instance.currentTurn == EntityType.PLAYER )
            m_playerController.DoTurn();
        else
            m_botController.DoTurn();

    }

}
