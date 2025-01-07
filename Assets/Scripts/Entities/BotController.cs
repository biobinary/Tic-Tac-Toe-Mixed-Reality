using UnityEngine;
using GlobalType;
using System.Collections.Generic;
using System.Collections;

public class BotController : Entity {

    [SerializeField]
    private CellManager m_cellManager;

    private string[][] m_boardState;
    private int indexPicked = -1;
    private Coroutine m_currentOnGoingTask = null;

    private void Start() {
        m_manager.onGameReset += OnGameReset;
    }

    private void OnGameReset() {
        if (m_currentOnGoingTask != null) {
            StopCoroutine(m_currentOnGoingTask);
            m_currentOnGoingTask = null;
        }
    }

    public override void DoTurn() {

        m_boardState = m_manager.GetCurrentBoardState();

        switch (m_manager.algorithmUsed) {
            case AlgorithmType.MINIMAX:
                Minimax();
                break;
            case AlgorithmType.ALPHABETA:
                AlphaBeta();
                break;
            default:
                m_currentOnGoingTask = StartCoroutine(RandomMove());
                break;
        }

    }

    IEnumerator RandomMove() {

        yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));

        List<int> emptyCell = new List<int>();
        
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (m_boardState[i][j] == "" )
                    emptyCell.Add(i * 3 + j);
            }
        }

        indexPicked = emptyCell[Random.Range(0, emptyCell.Count)];

        GeneratePiece();

    }

    private void Minimax() {
        ;
    }

    private void AlphaBeta() {
        ;
    }

    protected override void GeneratePiece() {
        m_currentOnGoingTask = null;
        GameObject obj = Instantiate(m_piecePrefab);
        m_cellManager.StashPiece(indexPicked, obj.GetComponent<Piece>(), EntityType.BOT);
    }

}
