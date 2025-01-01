using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum ENTITIES {
        PLAYER,
        BOT
    }

    public enum PIECE {
        CROSS,
        CIRCLE
    }

    public enum ALGORITM {
        MINIMAX,
        ALPHABETA,
        NONE
    }

    public static GameManager Instance;

    // Event Callback
    public System.Action gameStart;
    public System.Action gameReset;
    public System.Action changeTurn;
    public System.Action changeStats;

    // Game Properties
    public PIECE playerPiece {  get; private set; }
    public ALGORITM algorithmUsed {  get; private set; }
    public ENTITIES currentTurn { get; private set; }

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
    public void StartGame(PIECE playerPiece, ALGORITM algorithmUsed) {

        this.playerPiece = playerPiece;
        this.algorithmUsed = algorithmUsed;
        
        gameStart?.Invoke();
        changeStats?.Invoke();

        ChangeTurn();

    }

    public void ChangeTurn() {

        if (m_isFirstTurn) {
            currentTurn = (Random.Range(0, 2) == 1) ? ENTITIES.PLAYER : ENTITIES.BOT;
            m_isFirstTurn = false;
        } else {
            currentTurn = currentTurn == ENTITIES.PLAYER ? ENTITIES.BOT : ENTITIES.PLAYER;
        }

        changeTurn?.Invoke();

    }

}
