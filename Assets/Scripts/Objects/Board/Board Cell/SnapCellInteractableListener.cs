using Oculus.Interaction;
using System.Collections.Generic;

public class SnapCellInteractableListener {

	public event System.Action<SnapInteractable, InteractableStateChangeArgs> OnStateChange;
	public event System.Action<int, IInteractorView> OnInteractorViewAdded;

	private List<EventWrapper> m_eventWrappers = new();

	private class EventWrapper {

		public SnapInteractable snapInteractable;
		public int index;

		public event System.Action<SnapInteractable, InteractableStateChangeArgs> OnStateChange;
		public event System.Action<int, IInteractorView> OnInteractorViewAdded;

		public EventWrapper(SnapInteractable snapInteractable, int index) {
			
			this.snapInteractable = snapInteractable;
			this.index = index;
			
			this.snapInteractable.WhenStateChanged += OnHandleStateChange;
			this.snapInteractable.WhenSelectingInteractorViewAdded += OnHandleInteractorViewAdded;

		}

		private void OnHandleStateChange(InteractableStateChangeArgs args) {
			OnStateChange?.Invoke(snapInteractable, args);
		}

		private void OnHandleInteractorViewAdded(IInteractorView view) {
			OnInteractorViewAdded?.Invoke(index, view);
		}

	}

	public SnapCellInteractableListener(List<SnapInteractable> snapInteractables) {
		for (int i = 0; i < snapInteractables.Count; i++) {
			EventWrapper eventWrapper = new(snapInteractables[i], i);
			eventWrapper.OnStateChange += OnHandleStateChange;
			eventWrapper.OnInteractorViewAdded += OnHandleInteractorViewAdded;
			m_eventWrappers.Add(eventWrapper);
		}
	}

	private void OnHandleStateChange(SnapInteractable snapInteractable, InteractableStateChangeArgs args) {
		OnStateChange?.Invoke( snapInteractable, args );
	}

	private void OnHandleInteractorViewAdded(int index, IInteractorView view) {
		OnInteractorViewAdded?.Invoke( index, view );
	}

}
