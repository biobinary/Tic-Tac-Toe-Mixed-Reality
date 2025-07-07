using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AlphaBetaAlgorithm : IAlgorithm {

	private BotController m_controller;
	private CancellationTokenSource m_cancellationTokenSource = new();
	private readonly AlgorithmUtilities m_algorithmUtilities = new();

	~AlphaBetaAlgorithm() {
		StopOnGoingTask();
	}

	private int SearchForBestMove(CancellationToken token) {

		string[][] board = m_algorithmUtilities.DuplicateBoard(m_controller.GetBoardState());
		int bestValue = int.MinValue;
		int bestMove = -1;
		int alpha = int.MinValue;
		int beta = int.MaxValue;

		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {

				token.ThrowIfCancellationRequested();

				if (board[i][j] == "") {

					string[][] newBoard = m_algorithmUtilities.DuplicateBoard(board);
					newBoard[i][j] = m_controller.GetBot();

					AlphaBetaNode rootState = new AlphaBetaNode(newBoard, false, 1, alpha, beta);
					Stack<AlphaBetaNode> stack = new Stack<AlphaBetaNode>();
					stack.Push(rootState);

					while (stack.Count != 0) {

						AlphaBetaNode currentState = stack.Peek();

						int score = 0;

						if (
							m_algorithmUtilities.IsTerminalState(m_controller, currentState.board, out score, currentState.depth) || 
							currentState.depth >= 5) {
							
							currentState.value = score;
							stack.Pop();
							continue;

						}

						if (currentState.children == null) {

							currentState.children = new List<AlphaBetaNode>();

							for (int k = 0; k < 3; k++) {
								for (int l = 0; l < 3; l++) {
									if (currentState.board[k][l] == "") {

										string[][] currentStateNewBoard = m_algorithmUtilities.DuplicateBoard(currentState.board);
										currentStateNewBoard[k][l] = currentState.isMaximazing ? m_controller.GetBot() : m_controller.GetPlayer();

										AlphaBetaNode childState = new AlphaBetaNode(
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

							AlphaBetaNode processedChildState = currentState.children[currentState.childrenProcessed];

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

						if (currentState.children?.Any() == true) {
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

		return bestMove;

	}

	public async void Calculate() {

		if (m_controller == null) {
			Debug.LogError("Controller Is Not Defined");
			return;
		}

		string[][] board = m_controller.GetBoardState();
		if (m_algorithmUtilities.IsBoardEmpty(board)) {
			var randomMove = new RandomMoveAlgorithm();
			randomMove.SetController(m_controller);
			randomMove.Calculate();
			return;
		}

		StopOnGoingTask();
		m_cancellationTokenSource = new CancellationTokenSource();

		try {

			int bestMove = await Task.Run(() => SearchForBestMove(m_cancellationTokenSource.Token),
				m_cancellationTokenSource.Token);

			if (!m_cancellationTokenSource.IsCancellationRequested)
				m_controller.GeneratePieceAtIndex(bestMove);

		} catch (OperationCanceledException) {
			Debug.Log("Minimax task was cancelled.");

		} catch (Exception ex) {
			Debug.LogError($"Unexpected error in Minimax: {ex}");

		}

	}

	public void SetController(BotController controller) {
		
		if (m_controller == controller) return;

		if (m_controller != null)
			m_controller.stopOnGoingTask -= StopOnGoingTask;

		m_controller = controller;

		if (m_controller != null)
			m_controller.stopOnGoingTask += StopOnGoingTask;

	}

	public void StopOnGoingTask() {
		if( m_cancellationTokenSource != null ) {
			m_cancellationTokenSource.Cancel();
			m_cancellationTokenSource.Dispose();
			m_cancellationTokenSource = null;
		}
	}

}
