using UnityEngine; 
using UnityEngine.UIElements;
using System.Collections;

public class Move : MonoBehaviour
{
    private float Target = 900f;
    
	void Update()
	{
        //Target += Time.deltaTime / 125;
    
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, Target), 0.03f);

        if (transform.position.z > -380f)
        {
            Target = -900f;
        }

        if (transform.position.z < -500f)
        {
            Target = 900f;
        }

	}
}