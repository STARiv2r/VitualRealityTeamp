using UnityEngine;
using UnityEngine.AI; //J1 Modify


public class FireController : MonoBehaviour
{
    public ParticleSystem[] fireParticleList;
    public float extinguishTime = 3f;
    private float extinguishProgress = 0f; 

    
    private bool extinguishFlag;

    private NavMeshObstacle navObstacle; // J1 Modify

    private void Update()
    {
        if (extinguishFlag)
        {
            extinguishProgress += Time.deltaTime;
            if (extinguishProgress < extinguishTime)
                UpdateVFX(1f - (extinguishProgress / extinguishTime));
            else
            {
                //J1 Modify
                if (navObstacle != null)
                {
                    navObstacle.enabled = false;
                }

                Destroy(gameObject);
            }
        }

        extinguishFlag = false;
    }

    private void UpdateVFX(float strength)
    {
        foreach (var particle in fireParticleList)
        {
            var color = particle.colorOverLifetime;
            color.enabled = true;

            Gradient originalGrad = color.color.gradient;

            GradientColorKey[] existingColorKeys = originalGrad.colorKeys;

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] {
            new GradientAlphaKey(strength, 0.0f),
            new GradientAlphaKey(0f, 1.0f) };

            Gradient newGrad = new Gradient();
            newGrad.SetKeys(existingColorKeys, alphaKeys);
            color.color = new ParticleSystem.MinMaxGradient(newGrad);
        }
    }

    public void Extinguish()
    {
        Debug.Log("fire Extinguish on");
        extinguishFlag = true;
    }
}
