using UnityEngine;
using UnityEngine.UI;

public class ConfigurationWindow : MonoBehaviour
{

    [SerializeField] private GameManager m_gameManager;

    [Header("Game Configuration")]
    [SerializeField] private ToggleGroup m_pieceSelector;
    [SerializeField] private ToggleGroup m_algorithmSelector;
    [SerializeField] private Toggle m_continueButton;

    private void Start() {
        m_continueButton.onValueChanged.AddListener(delegate { OnButtonContinue(m_continueButton); });
    }

    private void OnButtonContinue(Toggle change) {

        // Configure Player Piece
        PieceType playerPiece;

        switch (m_pieceSelector.GetFirstActiveToggle().name.ToLower()) {
            case "cross":
                playerPiece = PieceType.CROSS;
                break;
            case "circle":
                playerPiece = PieceType.CIRCLE;
                break;
            default:
                playerPiece = Random.Range(0, 2) == 1 ? PieceType.CROSS : PieceType.CIRCLE;
                break;
        }

        // Configure Algorithm Used
        AlgorithmType algorithmUsed;

        switch (m_algorithmSelector.GetFirstActiveToggle().name.ToLower()) {
            case "minimax":
                algorithmUsed = AlgorithmType.MINIMAX;
                break;
            case "alphabeta":
                algorithmUsed = AlgorithmType.ALPHABETA;
                break;
            default:
                algorithmUsed = AlgorithmType.NONE;
                break;
        }

        GameManager.Instance.StartGame(playerPiece, algorithmUsed);

    }

}
