using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlphaBeta : AdversarialSearch {

    protected override IEnumerator CalculateAdversarial() {

        int iterationCounter = 0;

        string[][] board = DuplicateBoard(m_controller.GetBoardState());
        int bestValue = int.MinValue;
        int bestMove = -1;
        int alpha = int.MinValue;
        int beta = int.MaxValue;

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (board[i][j] == "") {

                    string[][] newBoard = DuplicateBoard(board);
                    newBoard[i][j] = m_controller.GetBot();

                    GameState rootState = new GameState(newBoard, false, 1, alpha, beta);
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
                                            currentState.depth + 1,
                                            currentState.alpha,
                                            currentState.beta
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

                            currentState.childrenProcessed = 0;
                            stack.Push(currentState.children[0]);
                            continue;

                        }

                        if (currentState.childrenProcessed < currentState.children.Count) {

                            GameState processedChildState = currentState.children[currentState.childrenProcessed];

                            if (processedChildState.value.HasValue) {

                                if (currentState.isMaximazing) {
                                    currentState.alpha = Mathf.Max(currentState.alpha, processedChildState.value.Value);

                                } else {
                                    currentState.beta = Mathf.Min(currentState.beta, processedChildState.value.Value);

                                }

                                currentState.childrenProcessed++;

                                if (currentState.beta <= currentState.alpha) {
                                    currentState.value = currentState.isMaximazing ? currentState.alpha : currentState.beta;
                                    stack.Pop();
                                    continue;

                                }

                                if (currentState.childrenProcessed < currentState.children.Count) {
                                    stack.Push(currentState.children[currentState.childrenProcessed]);
                                }

                            }

                            continue;

                        }

                        if(currentState.children?.Any() == true) {
                            currentState.value = currentState.isMaximazing ?
                                currentState.children.Max(child => child.value ?? int.MinValue) :
                                currentState.children.Min(child => child.value ?? int.MinValue);
                        }

                        stack.Pop();

                    }

                    if (rootState.value.HasValue && rootState.value > bestValue) {
                        bestValue = rootState.value.Value;
                        bestMove = i * 3 + j;
                    }

                    alpha = Mathf.Max(alpha, rootState.value ?? int.MinValue);

                }

            }
        }

        m_controller.GeneratePieceAtIndex(bestMove);

    }

}
