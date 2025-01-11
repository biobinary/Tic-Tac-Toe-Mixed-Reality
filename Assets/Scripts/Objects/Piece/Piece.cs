using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Piece : MonoBehaviour
{
    
    [SerializeField] private GameObject m_interactableGameObject;
    [SerializeField] private SnapInteractor m_snapInteractor;
    [SerializeField] private GameObject m_visualGameObject;

    [HideInInspector] public bool isPlayer = true;

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

    public void InjectInteractable(SnapInteractable interactable) {
        m_snapInteractor.InjectOptionalTimeOutInteractable(interactable);
    }

    public void DisableInteraction() {
        m_interactableGameObject.GetComponent<HandGrabInteractable>().enabled = false;
        m_interactableGameObject.GetComponent<GrabInteractable>().enabled = false;
    }

    public void EnableInteraction() {
        m_interactableGameObject.GetComponent<HandGrabInteractable>().enabled = true;
        m_interactableGameObject.GetComponent<GrabInteractable>().enabled = true;
    }

}
