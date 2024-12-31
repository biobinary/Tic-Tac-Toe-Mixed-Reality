using UnityEngine;

public class BoardGeneralManager : MonoBehaviour
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
        transform.rotation = Quaternion.LookRotation(transform.position - m_cameraRig.transform.position) * Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        
        Vector3 rotatedOffset = Quaternion.Euler(-90.0f, 0.0f, 0.0f) * m_recenterOffset;
        transform.position = transform.position + transform.TransformDirection(rotatedOffset);

    }

    private void Update() {

        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LHand))
            Debug.Log("Pressed");

    }

}
