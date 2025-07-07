using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MinimaxAlgorithm : IAlgorithm {

	protected BotController m_controller;
	private CancellationTokenSource m_cancellationTokenSource = new();
	private readonly AlgorithmUtilities m_algorithmUtilities = new();

	~MinimaxAlgorithm() {
		StopOnGoingTask();
	}

	private int SearchForBestMove(CancellationToken token) {

		string[][] board = m_algorithmUtilities.DuplicateBoard(m_controller.GetBoardState());
		int bestVal = int.MinValue;
		int bestMove = -1;

		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {

				token.ThrowIfCancellationRequested();

				if (board[i][j] == "") {

					string[][] newBoard = m_algorithmUtilities.DuplicateBoard(board);
					newBoard[i][j] = m_controller.GetBot();

					MinimaxNode rootState = new MinimaxNode(newBoard, false, 1);
					Stack<MinimaxNode> stack = new Stack<MinimaxNode>();
					stack.Push(rootState);

					while (stack.Count != 0) {

						token.ThrowIfCancellationRequested();

						MinimaxNode currentState = stack.Peek();

						int score = 0;
						if (
							m_algorithmUtilities.IsTerminalState(m_controller, currentState.board, out score, currentState.depth) || 
							currentState.depth >= 5) {
							
							currentState.value = score;
							stack.Pop();
							continue;
						
						}

						if (currentState.children == null) {

							currentState.children = new List<MinimaxNode>();

							for (int k = 0; k < 3; k++) {
								for (int l = 0; l < 3; l++) {

									if (currentState.board[k][l] == "") {

										string[][] currentStateNewBoard = m_algorithmUtilities.DuplicateBoard(currentState.board);
										currentStateNewBoard[k][l] = currentState.isMaximazing ? m_controller.GetBot() : m_controller.GetPlayer();

										MinimaxNode childState = new MinimaxNode(
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

							foreach (MinimaxNode childState in currentState.children.AsEnumerable().Reverse()) {
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

		if (m_controller == controller) {
			return;
		}

		if (m_controller != null)
			m_controller.stopOnGoingTask -= StopOnGoingTask;

		m_controller = controller;

		if (m_controller != null)
			m_controller.stopOnGoingTask += StopOnGoingTask;

	}

	public void StopOnGoingTask() {
		if (m_cancellationTokenSource != null) {
			m_cancellationTokenSource.Cancel();
			m_cancellationTokenSource.Dispose();
			m_cancellationTokenSource = null;
		}
	}

}
