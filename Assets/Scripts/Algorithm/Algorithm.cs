using UnityEngine;

public abstract class Algorithm : MonoBehaviour
{

    protected BotController m_controller;
    protected Coroutine m_onGoingTask;

    public void SetController(BotController controller) {
        
        if (m_controller == controller) {
            return;
        }
        
        if( m_controller != null )
            m_controller.stopOnGoingTask -= StopOnGoingTask;
        
        m_controller = controller;

        if (m_controller != null)
            m_controller.stopOnGoingTask += StopOnGoingTask;

    }

    public abstract void Calculate();

    public void StopOnGoingTask() {

        if (m_onGoingTask != null) {
            StopCoroutine(m_onGoingTask);
            m_onGoingTask = null;
        }

    }

}
