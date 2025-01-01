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
        GameManager.PIECE playerPiece;

        switch (m_pieceSelector.GetFirstActiveToggle().name.ToLower()) {
            case "cross":
                playerPiece = GameManager.PIECE.CROSS;
                break;
            case "circle":
                playerPiece = GameManager.PIECE.CIRCLE;
                break;
            default:
                playerPiece = Random.Range(0, 2) == 1 ? GameManager.PIECE.CROSS : GameManager.PIECE.CIRCLE;
                break;
        }

        // Configure Algorithm Used
        GameManager.ALGORITM algorithmUsed;

        switch (m_algorithmSelector.GetFirstActiveToggle().name.ToLower()) {
            case "minimax":
                algorithmUsed = GameManager.ALGORITM.MINIMAX;
                break;
            case "alphabeta":
                algorithmUsed = GameManager.ALGORITM.ALPHABETA;
                break;
            default:
                algorithmUsed = GameManager.ALGORITM.NONE;
                break;
        }

        GameManager.Instance.StartGame(playerPiece, algorithmUsed);

    }

}
