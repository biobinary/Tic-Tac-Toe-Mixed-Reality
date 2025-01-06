using UnityEngine;

public class UtilityManager : MonoBehaviour
{

    [SerializeField] private GameObject m_cameraRig; // XR Camera, not the origin rig
    [SerializeField] private GameObject m_interactableComponent;

    [Header("Recenter Properties")]
    [SerializeField] private float m_recenterDistance = 1.0f;
    [SerializeField] private Vector3 m_recenterOffset = Vector3.zero;

    public void OnBoardInteractableChange(bool isActive) {
        m_interactableComponent.SetActive(isActive);
    }

    public void RecenterBoard() {
        
        transform.position = m_cameraRig.transform.position + m_cameraRig.transform.forward * m_recenterDistance;
        transform.rotation = Quaternion.LookRotation(transform.position - m_cameraRig.transform.position);
        transform.position = transform.position + transform.TransformDirection(m_recenterOffset);

    }

    private void Update() {

        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LHand))
            Debug.Log("Pressed");

    }

}
