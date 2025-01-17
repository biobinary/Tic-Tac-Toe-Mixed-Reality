using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdversarialSearch : Algorithm {
    
    protected class GameState {

        public string[][] board { get; }
        public bool isMaximazing { get; }
        public int depth { get; }
        public int? value { get; set; }
        public List<GameState> children { get; set; }

        // Alpha Beta
        public int alpha { get; set; }
        public int beta { get; set; }
        public int childrenProcessed { get; set; }

        public GameState(string[][] board, bool isMaximazing = true, int depth = 0) {

            this.board = board;
            this.isMaximazing = isMaximazing;
            this.depth = depth;

            value = null;
            children = null;

        }

        public GameState(string[][] board, bool isMaximazing = true, int depth = 0, int alpha = int.MinValue, int beta = int.MaxValue) {

            this.board = board;
            this.isMaximazing = isMaximazing;
            this.depth = depth;

            value = null;
            children = null;

            this.alpha = alpha;
            this.beta = beta;
            childrenProcessed = 0;

        }

    }

    [SerializeField, Range(3, 9)] protected int m_maxDepth = 5;
    [SerializeField] protected int m_maxIterationCount = 100;
    [SerializeField] protected RandomMove m_randomMoveAlgorithm;

    protected string[][] DuplicateBoard(string[][] source) {

        var len = source.Length;
        var dest = new string[len][];

        for (var x = 0; x < len; x++) {
            var inner = source[x];
            var ilen = inner.Length;
            var newer = new string[ilen];
            System.Array.Copy(inner, newer, ilen);
            dest[x] = newer;
        }

        return dest;

    }

    protected bool IsDraw(string[][] board) {

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == "")
                    return false;
            }
        }

        return true;

    }

    protected bool IsTerminalState(string[][] board, out int score, int depth) {

        score = 0;

        for (int i = 0; i < 3; i++) {

            if (board[i][0] == board[i][1] &&
                board[i][1] == board[i][2]) {

                if (board[i][0] == m_controller.GetBot()) {
                    score = 10 - depth;
                    return true;

                } else if (board[i][0] == m_controller.GetPlayer()) {
                    score = -10 + depth;
                    return true;

                }

            }

            if (board[0][i] == board[1][i] &&
                board[1][i] == board[2][i]) {

                if (board[0][i] == m_controller.GetBot()) {
                    score = 10 - depth;
                    return true;

                } else if (board[0][i] == m_controller.GetBot()) {
                    score = -10 + depth;
                    return true;

                }

            }

        }

        if (board[0][0] == board[1][1] &&
            board[1][1] == board[2][2]) {

            if (board[0][0] == m_controller.GetBot()) {
                score = 10 - depth;
                return true;

            } else if (board[0][0] == m_controller.GetPlayer()) {
                score = -10 + depth;
                return true;
            }

        }

        if (board[0][2] == board[1][1] &&
            board[1][1] == board[2][0]) {

            if (board[0][2] == m_controller.GetBot()) {
                score = 10 - depth;
                return true;

            } else if (board[0][2] == m_controller.GetPlayer()) {
                score = -10 + depth;
                return true;

            }

        }

        if (IsDraw(board))
            return true;

        return false;

    }

    protected abstract IEnumerator CalculateAdversarial();

    public override void Calculate() {

        if (m_controller == null) {
            Debug.Log("Controller Is Not Defined");
            return;
        }

        bool isBoardEmpty = true;
        string[][] board = m_controller.GetBoardState();

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] != "") {
                    isBoardEmpty = false;
                    break;
                }
            }
        }

        if (isBoardEmpty && m_randomMoveAlgorithm != null) {
            m_randomMoveAlgorithm.Calculate();

        } else {
            StopOnGoingTask();
            m_onGoingTask = StartCoroutine(CalculateAdversarial());

        }

    }

}
