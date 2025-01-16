using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Minimax : Algorithm {

    private class GameState {

        public string[][] board { get; }
        public bool isMaximazing { get; }
        public int depth { get; }

        // Will Dynamically Change
        public int? value { get; set; }
        public List<GameState> children { get; set; }

        public GameState(string[][] board, bool isMaximazing = true, int depth = 0) {

            this.board = board;
            this.isMaximazing = isMaximazing;
            this.depth = depth;

            value = null;
            children = null;

        }

    }


    [SerializeField, Range(3, 9)] private int m_maxDepth = 5;
    [SerializeField] private int m_maxIterationCount = 100;
    [SerializeField] private RandomMove m_randomMoveAlgorithm;

    public void SetRandomMoveAlgorithm(RandomMove randomMoveAlgorithm) {
        m_randomMoveAlgorithm = randomMoveAlgorithm;
    }

    private string[][] DuplicateBoard(string[][] source) {

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

    private bool IsDraw(string[][] board) {

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == "")
                    return false;
            }
        }

        return true;

    }

    private bool IsTerminalState(string[][] board, out int score, int depth) {

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
            m_onGoingTask = StartCoroutine(CalculateMinimax());

        }

    }

    private IEnumerator CalculateMinimax() {

        int iterationCounter = 0;

        string[][] board = DuplicateBoard(m_controller.GetBoardState());
        int bestVal = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == "") {

                    string[][] newBoard = DuplicateBoard(board);
                    newBoard[i][j] = m_controller.GetBot();

                    GameState rootState = new GameState(newBoard, false, 1);
                    Stack<GameState> stack = new Stack<GameState>();
                    stack.Push(rootState);

                    while (stack.Count != 0) {

                        iterationCounter++;
                        if (iterationCounter >= m_maxIterationCount) {
                            iterationCounter = 0;
                            yield return null;
                        }

                        GameState currentState = stack.Peek();

                        int score = 0;
                        if (IsTerminalState(currentState.board, out score, currentState.depth) || currentState.depth >= m_maxDepth) {
                            currentState.value = score;
                            stack.Pop();
                            continue;
                        }

                        if (currentState.children == null) {

                            currentState.children = new List<GameState>();

                            for (int k = 0; k < 3; k++) {
                                for (int l = 0; l < 3; l++) {

                                    if (currentState.board[k][l] == "") {

                                        string[][] currentStateNewBoard = DuplicateBoard(currentState.board);
                                        currentStateNewBoard[k][l] = currentState.isMaximazing ? m_controller.GetBot() : m_controller.GetPlayer();

                                        GameState childState = new GameState(
                                            currentStateNewBoard,
                                            !currentState.isMaximazing,
                                            currentState.depth + 1
                                        );

                                        currentState.children.Add(childState);

                                    }

                                }
                            }

                            if (currentState.children.Count == 0) {
                                currentState.value = score;
                                stack.Pop();
                                continue;
                            }

                            foreach (GameState childState in currentState.children.AsEnumerable().Reverse()) {
                                stack.Push(childState);
                            }

                            continue;

                        }

                        var allValuesNotNull = currentState.children.All(child => child.value.HasValue);
                        if (allValuesNotNull) {

                            if (currentState.isMaximazing) {
                                var maxValue = currentState.children.Max(child => child.value);
                                currentState.value = maxValue;

                            } else {
                                var minValue = currentState.children.Min(child => child.value);
                                currentState.value = minValue;

                            }

                            stack.Pop();
                            continue;

                        }

                    }

                    if (rootState.value.HasValue && rootState.value > bestVal) {
                        bestVal = rootState.value.Value;
                        bestMove = i * 3 + j;
                    }

                }
            }
        }

        m_controller.GeneratePieceAtIndex(bestMove);

    }

}
