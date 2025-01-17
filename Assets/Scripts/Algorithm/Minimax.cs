using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Minimax : AdversarialSearch {

    protected override IEnumerator CalculateAdversarial() {

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
