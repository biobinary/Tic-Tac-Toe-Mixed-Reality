using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : Algorithm {

    IEnumerator CalculateRandomMove() {

        yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));

        List<int> emptyCell = new List<int>();
        string[][] board = m_controller.GetBoardState();

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == "")
                    emptyCell.Add(i * 3 + j);
            }
        }

        m_controller.GeneratePieceAtIndex(emptyCell[Random.Range(0, emptyCell.Count)]);
    }

    public override void Calculate() {
        if (m_controller == null) { Debug.Log("Controller Is Not Defined"); return; }
        m_onGoingTask = StartCoroutine(CalculateRandomMove());
    }

}
