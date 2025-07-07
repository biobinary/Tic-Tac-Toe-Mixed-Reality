using UnityEngine;
using UnityEngine.UI;

public class ConfigurationWindow : MonoBehaviour
{

    [Header("Game Configuration")]
    [SerializeField] private ToggleGroup m_pieceSelector;
    [SerializeField] private ToggleGroup m_algorithmSelector;
    [SerializeField] private Toggle m_continueButton;

    private Manager m_manager;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void Start() {
        m_continueButton.onValueChanged.AddListener(delegate { OnButtonContinue(m_continueButton); });
    }

    private void OnButtonContinue(Toggle change) {

        // Configure Player Piece
        GlobalType.PieceType playerPiece;

        switch (m_pieceSelector.GetFirstActiveToggle().name.ToLower()) {
            case "cross":
                playerPiece = GlobalType.PieceType.CROSS;
                break;
            case "circle":
                playerPiece = GlobalType.PieceType.CIRCLE;
                break;
            default:
                playerPiece = Random.Range(0, 2) == 1 ? GlobalType.PieceType.CROSS : GlobalType.PieceType.CIRCLE;
                break;
        }

        // Configure Algorithm Used
        IAlgorithm algorithmUsed;

        switch (m_algorithmSelector.GetFirstActiveToggle().name.ToLower()) {
            case "minimax":
                algorithmUsed = new MinimaxAlgorithm();
                break;
            case "alphabeta":
                algorithmUsed = new AlphaBetaAlgorithm();
                break;
            default:
                algorithmUsed = new RandomMoveAlgorithm();
                break;
        }

        m_manager.StartGame(playerPiece, algorithmUsed);

    }

}
