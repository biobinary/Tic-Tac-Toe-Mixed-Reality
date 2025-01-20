using UnityEngine;
using GlobalType;

namespace GlobalType {

    public enum EntityType {
        PLAYER, BOT
    }

    public enum PieceType {
        CROSS, CIRCLE
    }

    public enum AlgorithmType {
        MINIMAX, ALPHABETA, NONE
    }

    public enum GameResultType {
        WIN, LOSE, DRAW
    }

}

public class Manager : MonoBehaviour
{

    private string[][] m_currentBoardState = {
        new string[] { "", "", "" },
        new string[] { "", "", "" },
        new string[] { "", "", "" }
    };

    // Base Event Callback
    public event System.Action onGameStart;
    public event System.Action<GameResultType> onGameEnd;
    public event System.Action onGameReset;
    public event System.Action onResetConfiguration;
    public event System.Action onChangeTurn;
    public event System.Action onChangeStats;

    // Game Properties
    public PieceType playerPiece { get; private set; }
    public AlgorithmType algorithmUsed { get; private set; }
    public EntityType currentTurn { get; private set; }

    // Player Statistics
    public int winAmount { get; private set; } = 0;
    public int loseAmount { get; private set; } = 0;
    public int drawAmount { get; private set; } = 0;

    private bool m_isFirstTurn = true;

    public void StartGame(PieceType playerPiece, AlgorithmType algorithmUsed) {

        this.playerPiece = playerPiece;
        this.algorithmUsed = algorithmUsed;

        onGameStart?.Invoke();
        onChangeStats?.Invoke();

        ChangeTurn();

    }

    public void ChangeTurn() {

        if (m_isFirstTurn) {
            currentTurn = (Random.Range(0, 2) == 1) ? EntityType.PLAYER : EntityType.BOT;
            m_isFirstTurn = false;

        } else {
            currentTurn = currentTurn == EntityType.PLAYER ? EntityType.BOT : EntityType.PLAYER;
        }

        onChangeTurn?.Invoke();

    }

    public void GameEnd(GameResultType gameResult) {
        onChangeStats?.Invoke();
        onGameEnd?.Invoke(gameResult);
    }

    public void ResetGame() {
        ClearBoard();
        onGameReset?.Invoke();
        ChangeTurn();
    }

    public void ResetConfiguration() {
        
        ClearBoard();
        m_isFirstTurn = true;

        onGameReset?.Invoke();
        onResetConfiguration?.Invoke();

    }

    public void ResetStatistics() {
        winAmount = 0;
        loseAmount = 0;
        drawAmount = 0;
        onChangeStats?.Invoke();
    }

    public void InsertPiece(int indexPosition, EntityType entityType) {

        int column = indexPosition % 3;
        int row = indexPosition / 3;

        string player = playerPiece == PieceType.CROSS ? "X" : "O";
        string bot = playerPiece == PieceType.CROSS ? "O" : "X";

        if (entityType == EntityType.PLAYER)
            m_currentBoardState[row][column] = player;
        else
            m_currentBoardState[row][column] = bot;

        string winResult = "";
        if( CheckWin(out winResult) ) {

            if (winResult == player) {
                winAmount += 1;
                GameEnd(GameResultType.WIN);

            } else {
                loseAmount += 1;
                GameEnd(GameResultType.LOSE);

            }

            return;

        } else if( CheckDraw() ) {
            drawAmount += 1;
            GameEnd(GameResultType.DRAW);
            return;
        
        }

        ChangeTurn();

    }

    public string[][] GetCurrentBoardState() {
        return m_currentBoardState;
    }

    private void ClearBoard() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                m_currentBoardState[i][j] = "";
            }
        }
    }

    private bool CheckWin(out string winResult) {

        for(int i = 0; i < 3; i++) {
            
            if (m_currentBoardState[i][0] == m_currentBoardState[i][1] && 
                m_currentBoardState[i][1] == m_currentBoardState[i][2] && 
                m_currentBoardState[i][0] != "") {
                
                winResult = m_currentBoardState[i][0];
                return true;
            
            }

            if (m_currentBoardState[0][i] == m_currentBoardState[1][i] &&
                m_currentBoardState[1][i] == m_currentBoardState[2][i] &&
                m_currentBoardState[0][i] != "") {

                winResult = m_currentBoardState[0][i];
                return true;

            }

        }

        if (m_currentBoardState[0][0] == m_currentBoardState[1][1] &&
            m_currentBoardState[1][1] == m_currentBoardState[2][2] &&
            m_currentBoardState[0][0] != "") {

            winResult = m_currentBoardState[0][0];
            return true; 
        
        }

        if (m_currentBoardState[0][2] == m_currentBoardState[1][1] &&
            m_currentBoardState[1][1] == m_currentBoardState[2][0] &&
            m_currentBoardState[0][2] != "") {

            winResult = m_currentBoardState[0][2];
            return true;

        }

        winResult = "";
        return false;
    
    }

    private bool CheckDraw() {

        for(int i = 0; i < 3; i++) {
            for(int j = 0; j < 3; j++) {
                if (m_currentBoardState[i][j] == "")
                    return false;
            }
        }

        return true;
    }

}
