using UnityEditor.VersionControl;
using UnityEngine;

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

}

public class Manager : MonoBehaviour
{

    private string[][] m_currentBoardState = {
        new string[] { "", "", ""},
        new string[] { "", "", "" },
        new string[] { "", "", "" }
    };

    // Base Event Callback
    public System.Action onGameStart;
    public System.Action onGameReset;
    public System.Action onChangeTurn;
    public System.Action onChangeStats;

    // Game Properties
    public GlobalType.PieceType playerPiece { get; private set; }
    public GlobalType.AlgorithmType algorithmUsed { get; private set; }
    public GlobalType.EntityType currentTurn { get; private set; }

    // Player Statistics
    public int winAmount { get; private set; } = 0;
    public int loseAmount { get; private set; } = 0;
    public int drawAmount { get; private set; } = 0;

    private bool m_isFirstTurn = true;

    public void StartGame(GlobalType.PieceType playerPiece, GlobalType.AlgorithmType algorithmUsed) {

        this.playerPiece = playerPiece;
        this.algorithmUsed = algorithmUsed;

        onGameStart?.Invoke();
        onChangeStats?.Invoke();

        ChangeTurn();

    }

    public void ChangeTurn() {

        if (m_isFirstTurn) {
            currentTurn = GlobalType.EntityType.PLAYER; // (Random.Range(0, 2) == 1) ? EntityType.PLAYER : EntityType.BOT;
            m_isFirstTurn = false;
        } else {
            currentTurn = currentTurn == GlobalType.EntityType.PLAYER ? GlobalType.EntityType.BOT : GlobalType.EntityType.PLAYER;
        }

        onChangeTurn?.Invoke();

    }

    public void InsertPiece(int indexPosition, GlobalType.EntityType entityType) {

        int column = indexPosition % 3;
        int row = indexPosition / 3;

        if (entityType == GlobalType.EntityType.PLAYER)
            m_currentBoardState[row][column] = playerPiece == GlobalType.PieceType.CROSS ? "X" : "O";
        else
            m_currentBoardState[row][column] = playerPiece == GlobalType.PieceType.CROSS ? "O" : "X";

        ChangeTurn();

    }

    public string[][] GetCurrentBoardState() {
        return m_currentBoardState;
    }

}
