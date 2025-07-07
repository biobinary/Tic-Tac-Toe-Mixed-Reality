using System.Collections.Generic;

public class AlphaBetaNode : MinimaxNode {

	public int alpha { get; set; }
	public int beta { get; set; }
	public int childrenProcessed { get; set; }
	public new List<AlphaBetaNode> children { get; set; }

	public AlphaBetaNode(string[][] board, bool isMaximazing = true, int depth = 0, int alpha = int.MinValue, int beta = int.MaxValue) : base(board, isMaximazing, depth) {

		this.alpha = alpha;
		this.beta = beta;
		this.children = null;
		childrenProcessed = 0;

	}

}