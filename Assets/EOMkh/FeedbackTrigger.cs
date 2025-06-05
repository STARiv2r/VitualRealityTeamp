using UnityEngine;

public class FeedbackTrigger : MonoBehaviour
{
    //public FeedbackType type;
    public float intensity = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            //FindObjectOfType<FeedbackManager>().TriggerFeedback(type, intensity);
        }
    }

    
}
