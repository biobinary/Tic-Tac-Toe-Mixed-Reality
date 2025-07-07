public class AlgorithmUtilities {

	public string[][] DuplicateBoard(string[][] source) {

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

	public bool IsDraw(string[][] board) {

		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				if (board[i][j] == "")
					return false;
			}
		}

		return true;

	}

	public bool IsTerminalState(BotController controller, string[][] board, out int score, int depth) {

		score = 0;

		for (int i = 0; i < 3; i++) {

			if (board[i][0] == board[i][1] &&
				board[i][1] == board[i][2]) {

				if (board[i][0] == controller.GetBot()) {
					score = 10 - depth;
					return true;

				} else if (board[i][0] == controller.GetPlayer()) {
					score = -10 + depth;
					return true;

				}

			}

			if (board[0][i] == board[1][i] &&
				board[1][i] == board[2][i]) {

				if (board[0][i] == controller.GetBot()) {
					score = 10 - depth;
					return true;

				} else if (board[0][i] == controller.GetBot()) {
					score = -10 + depth;
					return true;

				}

			}

		}

		if (board[0][0] == board[1][1] &&
			board[1][1] == board[2][2]) {

			if (board[0][0] == controller.GetBot()) {
				score = 10 - depth;
				return true;

			} else if (board[0][0] == controller.GetPlayer()) {
				score = -10 + depth;
				return true;
			}

		}

		if (board[0][2] == board[1][1] &&
			board[1][1] == board[2][0]) {

			if (board[0][2] == controller.GetBot()) {
				score = 10 - depth;
				return true;

			} else if (board[0][2] == controller.GetPlayer()) {
				score = -10 + depth;
				return true;

			}

		}

		if (IsDraw(board))
			return true;

		return false;

	}

	public bool IsBoardEmpty(string[][] board) {
		foreach (var row in board)
			foreach (var cell in row)
				if (!string.IsNullOrEmpty(cell)) return false;

		return true;
	}


}
