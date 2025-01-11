using UnityEngine;
using GlobalType;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class BotController : Entity {

    public class GameState {

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

    [SerializeField]
    private CellManager m_cellManager;

    [Header("Minimax Properties")]
    [SerializeField] private int m_maxDepth = 10;

    private string[][] m_boardState;
    private int indexPicked = -1;
    private Coroutine m_currentOnGoingTask = null;

    private string m_player;
    private string m_bot;

    private void Start() {
        m_manager.onGameReset += OnGameReset;
    }

    private void OnGameReset() {
        if (m_currentOnGoingTask != null) {
            StopCoroutine(m_currentOnGoingTask);
            m_currentOnGoingTask = null;
        }
    }

    public override void DoTurn() {

        m_boardState = m_manager.GetCurrentBoardState();
        m_player = m_manager.playerPiece == PieceType.CROSS ? "X" : "O";
        m_bot = m_manager.playerPiece == PieceType.CROSS ? "O" : "X";

        switch (m_manager.algorithmUsed) {
            case AlgorithmType.MINIMAX:
                m_currentOnGoingTask = StartCoroutine(Minimax());
                break;
            case AlgorithmType.ALPHABETA:
                AlphaBeta();
                break;
            default:
                m_currentOnGoingTask = StartCoroutine(RandomMove());
                break;
        }

    }

    static string[][] DuplicateBoard(string[][] source) {

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

    IEnumerator RandomMove() {

        yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));

        List<int> emptyCell = new List<int>();
        
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (m_boardState[i][j] == "" )
                    emptyCell.Add(i * 3 + j);
            }
        }

        indexPicked = emptyCell[Random.Range(0, emptyCell.Count)];

        GeneratePiece();

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

    private bool IsTerminalState(string[][] board, out int score) {
        
        score = 0;
        
        for (int i = 0; i < 3; i++) {

            if (board[i][0] == board[i][1] &&
                board[i][1] == board[i][2]) {

                if (board[i][0] == m_bot ) {
                    score = 10;
                    return true;
                
                } else if (board[i][0] == m_player ) {
                    score = -10;
                    return true;

                }

            }

            if (board[0][i] == board[1][i] &&
                board[1][i] == board[2][i]) {

                if( board[0][i] == m_bot ) {
                    score = 10; 
                    return true;
                    
                } else if (board[0][i] == m_player) {
                    score = -10;
                    return true;

                }

            }

        }

        if (board[0][0] == board[1][1] &&
            board[1][1] == board[2][2]) {

            if (board[0][0] == m_bot) {
                score = 10;
                return true;

            } else if (board[0][0] == m_player) { 
                score = -10;
                return true;
            }

        }

        if (board[0][2] == board[1][1] &&
            board[1][1] == board[2][0]) {

            if (board[0][2] == m_bot) {
                score = 10;
                return true;

            } else if( board[0][2] == m_player) {
                score = -10;
                return true;

            }

        }

        if (IsDraw(board))
            return true;

        return false;

    }

    IEnumerator Minimax() {

        // FPS Blocking Prevention
        int iterationCounter = 0;

        string[][] board = DuplicateBoard(m_boardState);
        int bestVal = int.MinValue;
        int bestMove = -1;

        for(int i = 0; i < 3; i++) {
            for(int j = 0; j < 3; j++) {
                if (board[i][j] == "" ) {

                    string[][] newBoard = DuplicateBoard(board);
                    newBoard[i][j] = m_bot;
                    
                    GameState rootState = new GameState(newBoard, false, 1);
                    Stack<GameState> stack = new Stack<GameState>();
                    stack.Push(rootState);

                    while(stack.Count != 0) {

                        iterationCounter++;
                        if (iterationCounter >= 100) {
                            iterationCounter = 0;
                            yield return null;
                        }

                        GameState currentState = stack.Peek();

                        int score = 0;
                        if(IsTerminalState(currentState.board, out score) || currentState.depth >= m_maxDepth) {
                            currentState.value = score;
                            stack.Pop();
                            continue;
                        }

                        if( currentState.children == null ) {

                            currentState.children = new List<GameState>();

                            for(int k = 0; k < 3; k++) {
                                for(int l = 0; l < 3; l++) {

                                    if (currentState.board[k][l] == "" ) {

                                        string[][] currentStateNewBoard = DuplicateBoard(currentState.board);
                                        currentStateNewBoard[k][l] = currentState.isMaximazing ? m_bot : m_player;
                                        
                                        GameState childState = new GameState(
                                            currentStateNewBoard,
                                            !currentState.isMaximazing,
                                            currentState.depth + 1
                                        );

                                        currentState.children.Add( childState );

                                    }

                                }
                            }

                            if( currentState.children.Count == 0 ) {
                                currentState.value = score;
                                stack.Pop(); 
                                continue;
                            }

                            foreach(GameState childState in currentState.children.AsEnumerable().Reverse()) {
                                stack.Push( childState );
                            }

                            continue;

                        }

                        var allValuesNotNull = currentState.children.All(child => child.value.HasValue);
                        if (allValuesNotNull) {

                            if ( currentState.isMaximazing ) {
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
                    
                    if(rootState.value.HasValue && rootState.value > bestVal) {
                        bestVal = rootState.value.Value;
                        bestMove = i * 3 + j;
                    }

                }
            }
        }

        indexPicked = bestMove;
        GeneratePiece();

    }

    private void AlphaBeta() {
        ;
    }

    private void MinimaxResult() {
        Debug.Log(indexPicked);
    }

    protected override void GeneratePiece() {
        
        m_currentOnGoingTask = null;
        
        GameObject obj = Instantiate(m_piecePrefab);
        Piece piece = obj.GetComponent<Piece>();

        piece.isPlayer = false;
        m_cellManager.InjectPiece(piece, indexPicked);
    
    }

}
