using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;

public class DropperManager : MonoBehaviour
{

    [SerializeField] private List<DropArea> m_dropAreas;

    private PointableElement m_currentActivePointable = null;
    private DropArea m_currentActiveDropArea = null;
    
    private void Start() {

        foreach (var dropArea in m_dropAreas) {
            dropArea.manager = this;
        }

        GameManager.Instance.playerPieceGenerated += OnNewPlayerPieceGenerated;

    }

    private void OnNewPlayerPieceGenerated(PointableElement element) {
        m_currentActivePointable = element;
    }

    public void OnAreaFocusEnter(DropArea currentFocusArea) {

        if (!currentFocusArea.gameObject.activeSelf) return;

        currentFocusArea.InjectPointable(m_currentActivePointable);
        m_currentActiveDropArea = currentFocusArea;

        foreach(var dropArea in m_dropAreas) {
            if (dropArea != currentFocusArea)
                dropArea.gameObject.SetActive(false);
        }

    }

    public void OnAreaFocusExit(DropArea currentFocusArea) {

        currentFocusArea.DisinjectPointable();
        m_currentActiveDropArea = null;
        
        foreach (var dropArea in m_dropAreas) {
            if (!dropArea.gameObject.activeSelf) {
                dropArea.gameObject.SetActive(true);
            }
        }

    }

}
