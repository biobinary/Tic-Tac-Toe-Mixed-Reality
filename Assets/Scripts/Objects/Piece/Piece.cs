using Oculus.Interaction;
using UnityEngine;

public class Piece : MonoBehaviour
{
    
    [SerializeField] private GameObject m_interactableGameObject;
    [SerializeField] private GameObject m_visualGameObject;

    [HideInInspector] public bool isSelected = false;

    // Event
    public System.Action onUnselect;

    public void Stash(Transform newParent) {

        m_interactableGameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;

        gameObject.transform.parent = newParent;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;

    }

    public void OnSelected(PointerEvent @event) {
        isSelected = true;
    }

    public void OnUnselect(PointerEvent @event) { 
        isSelected = false;
        onUnselect?.Invoke();
    }

}
