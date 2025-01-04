using Oculus.Interaction;
using UnityEngine;

public enum EntityType {
    PLAYER, BOT
}

public enum PieceType {
    CROSS, CIRCLE
}

public enum AlgorithmType {
    MINIMAX, ALPHABETA, NONE
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    // Base Event Callback
    public System.Action gameStart;
    public System.Action gameReset;
    public System.Action changeTurn;
    public System.Action changeStats;

    // Custom Event Callback
    public System.Action<PointableElement> playerPieceGenerated;

    // Game Properties
    public PieceType playerPiece {  get; private set; }
    public AlgorithmType algorithmUsed {  get; private set; }
    public EntityType currentTurn { get; private set; }

    // Player Statistics
    public int winAmount { get; private set; } = 0;
    public int loseAmount { get; private set; } = 0;
    public int drawAmount { get; private set; } = 0;

    private bool m_isFirstTurn = true;

    private void Awake() {
        
        if (Instance == null) {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    
    }

    // Configure the game first before starting it
    public void StartGame(PieceType playerPiece, AlgorithmType algorithmUsed) {

        this.playerPiece = playerPiece;
        this.algorithmUsed = algorithmUsed;
        
        gameStart?.Invoke();
        changeStats?.Invoke();

        ChangeTurn();

    }

    public void ChangeTurn() {

        if (m_isFirstTurn) {
            currentTurn = (Random.Range(0, 2) == 1) ? EntityType.PLAYER : EntityType.BOT;
            m_isFirstTurn = false;
        } else {
            currentTurn = currentTurn == EntityType.PLAYER ? EntityType.BOT : EntityType.PLAYER;
        }

        changeTurn?.Invoke();

    }

}
