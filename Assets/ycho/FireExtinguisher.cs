using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public ParticleSystem spray;
    public float extinguishRange = 5f;
    public LayerMask fireLayerMask;

    private ParticleSystem.Particle[] particles;

    private void Update()
    {
       if (spray.isEmitting)
        {
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
    }
}
