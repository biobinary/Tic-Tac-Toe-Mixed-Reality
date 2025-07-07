using System.Collections.Generic;

public class MinimaxNode {

	public string[][] board { get; }
	public bool isMaximazing { get; }
	public int depth { get; }
	public int? value { get; set; }
	public List<MinimaxNode> children { get; set; }

	public MinimaxNode(string[][] board, bool isMaximazing = true, int depth = 0) {

            this.board = board;
            this.isMaximazing = isMaximazing;
            this.depth = depth;

            value = null;
            children = null;

    }

}