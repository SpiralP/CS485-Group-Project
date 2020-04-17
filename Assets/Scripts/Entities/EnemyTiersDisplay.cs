using UnityEngine;

public class EnemyTiersDisplay : MonoBehaviour
{
    public Texture tier1, tier2, tier3;
    Renderer renderer;
    float t;
    int i;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        i = 1;
    }

    void Update()
    {
        t += Time.deltaTime;

        if (isActiveAndEnabled)
        {
            if (Mathf.Floor(t) % 3.0 == 0.0 && t > 1.0)
            {
                if (i == 1) 
                { 
                    renderer.material.SetTexture("_MainTex", tier2); 
                    t = 0f;
                    i++; 
                }
                else if (i == 2) 
                { 
                    renderer.material.SetTexture("_MainTex", tier3); 
                    t = 0f;
                    i++;
                }
                else 
                { 
                    renderer.material.SetTexture("_MainTex", tier1); 
                    t = 0f;
                    i = 1;
                }
            }
        }
    }
}
