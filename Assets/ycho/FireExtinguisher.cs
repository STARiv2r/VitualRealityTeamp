using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public GameObject spray;
    public float extinguishRange = 5f;
    public LayerMask fireLayerMask;
    bool isHolding = false;
    bool isPressing = false;

    private ParticleSystem.Particle[] particles;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            isPressing = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            isPressing = false;
        }
        if (isPressing && isHolding)
        {
            spray.SetActive(true);
            Ray ray = new Ray(spray.transform.position, spray.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, extinguishRange, fireLayerMask))
            {
                FireController fire = hit.collider.GetComponent<FireController>();
                if (fire != null)
                {
                    fire.Extinguish();
                }
            }
        }
        else
            spray.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            isHolding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            isHolding = false;
    }
}
