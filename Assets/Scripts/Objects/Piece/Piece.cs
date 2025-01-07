using Oculus.Interaction;
using UnityEngine;

public class Piece : MonoBehaviour
{
    
    [SerializeField] private GameObject m_interactableGameObject;
    [SerializeField] private GameObject m_visualGameObject;
    [SerializeField] private Vector3 m_stashOffset = Vector3.zero;

    [HideInInspector] public bool isSelected = false;

    // Event
    public System.Action onUnselect;

    private Manager m_manager;

    private void Awake() {
        m_manager = FindAnyObjectByType<Manager>();
    }

    private void Start() {
        m_manager.onGameReset += OnReset;
    }

    private void OnReset() {
        m_manager.onGameReset -= OnReset;
        Destroy(gameObject);
    }

    public void Stash(Transform newParent) {

        m_interactableGameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;

        gameObject.transform.parent = newParent;
        gameObject.transform.localPosition = Vector3.zero + m_stashOffset;
        gameObject.transform.localRotation = Quaternion.identity;

    }

    public void OnSelected(PointerEvent @event) {
        isSelected = true;
    }

    public void OnUnselect(PointerEvent @event) { 
        isSelected = false;
        onUnselect?.Invoke();
    }

}
