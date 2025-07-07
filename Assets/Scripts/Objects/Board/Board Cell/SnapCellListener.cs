using Oculus.Interaction;
using UnityEngine;

public class SnapCellListener : MonoBehaviour {

	public SnapInteractable interactable;
	public event System.Action<SnapCellListener, IInteractorView> OnSelectingInteractorViewAdded;

	private void OnEnable() {
		
		if( interactable != null ) {
			interactable.WhenSelectingInteractorViewAdded += OnHandleInteractorViewAdded;
		}

	}

	private void OnDisable() {
		
		if (interactable != null) {
			interactable.WhenSelectingInteractorViewAdded -= OnHandleInteractorViewAdded;
		}

	}

	private void OnHandleInteractorViewAdded(IInteractorView view) {
		OnSelectingInteractorViewAdded?.Invoke(this, view);
	}

}
