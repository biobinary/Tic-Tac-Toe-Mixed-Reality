using Oculus.Interaction;
using UnityEngine;
using GlobalType;

public class SnapCell : MonoBehaviour
{

    [SerializeField] private SnapInteractable m_snapInteractable;

    private CellManager m_cellManager;

    public void SetManager(CellManager manager) {
        m_cellManager = manager;
    }

    protected virtual void Awake() {
        m_snapInteractable.WhenStateChanged += HandleOnStateChange;
        m_snapInteractable.WhenSelectingInteractorViewAdded += HandleInteractorAdded;
    }

    private void HandleOnStateChange(InteractableStateChangeArgs args) {
        
        if (args.NewState == InteractableState.Hover) {
            m_cellManager.EnableHighlight(transform.localPosition);

        } else if (args.PreviousState == InteractableState.Hover) {
            m_cellManager.DisableHighlight();
        }

    }

    private void HandleInteractorAdded(IInteractorView interactor) {

        GameObject obj = (interactor.Data as MonoBehaviour).gameObject;
        GameObject parentObj = obj.transform.parent.gameObject;
        
        Piece piece = parentObj.GetComponent<Piece>();
        piece.DisableInteraction();

        Debug.Log(piece.isPlayer);

        if (piece.isPlayer)
            m_cellManager.HandlePiecePlaced(this, EntityType.PLAYER);
        else
            m_cellManager.HandlePiecePlaced(this, EntityType.BOT);

    }

    public SnapInteractable GetSnapInteractableComponent() {
        return m_snapInteractable;
    }

}
