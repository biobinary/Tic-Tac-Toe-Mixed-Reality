using Oculus.Interaction;
using UnityEngine;

public class DropArea : MonoBehaviour
{

    private PointableUnityEventWrapper m_eventWrapper;
    public DropperManager manager;

    private void Awake() {
        m_eventWrapper = GetComponent<PointableUnityEventWrapper>();
        m_eventWrapper.enabled = false;
    }

    public void InjectPointable(PointableElement element) {
        m_eventWrapper.InjectPointable(element);
        m_eventWrapper.enabled = true;
    }

    public void DisinjectPointable() {
        m_eventWrapper.enabled = false;
        m_eventWrapper.InjectPointable(null);
    }

    private void OnTriggerEnter(Collider other) {

        if (manager == null) return;
        if (!other.CompareTag("PlayerPiece")) return;
        
        manager.OnAreaFocusEnter(this);
    
    }

    private void OnTriggerExit(Collider other) {
        
        if (manager == null) return;
        if (!other.CompareTag("PlayerPiece")) return;
        
        manager.OnAreaFocusExit(this);
    
    }

    public void OnPieceDrop(PointerEvent @event) {
        ;
    }

}
