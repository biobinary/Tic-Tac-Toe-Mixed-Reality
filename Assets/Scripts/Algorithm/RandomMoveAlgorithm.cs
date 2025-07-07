using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class RandomMoveAlgorithm : IAlgorithm {

	protected BotController m_controller;
	private CancellationTokenSource m_cancellationTokenSource = new();

	~RandomMoveAlgorithm() { 
		StopOnGoingTask();
	}

	public async void Calculate() {

		if (m_controller == null) {
			Debug.LogError("Controller Is Not Defined");
			return;
		}

		StopOnGoingTask();
		m_cancellationTokenSource = new CancellationTokenSource();

		try {

			int delay = Random.Range(2000, 6000);
			await Task.Delay(delay, m_cancellationTokenSource.Token);

			if (m_cancellationTokenSource.IsCancellationRequested)
				return;

			string[][] board = m_controller.GetBoardState();
			List<int> emptyCells = new List<int>();

			for (int i = 0; i < board.Length; i++) {
				for (int j = 0; j < board[i].Length; j++) {
					if (string.IsNullOrEmpty(board[i][j])) {
						emptyCells.Add(i * 3 + j);
					}
				}
			}

			if (emptyCells.Count > 0) {
				int randomIndex = Random.Range(0, emptyCells.Count);
				m_controller.GeneratePieceAtIndex(emptyCells[randomIndex]);
			}
		
		} catch (TaskCanceledException) {
			// Silent fail: Task dibatalkan secara normal.
		
		} catch (System.Exception ex) {
			Debug.LogError($"RandomMoveAlgorithm Error: {ex.Message}");
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
