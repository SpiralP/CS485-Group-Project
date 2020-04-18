using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class StarfieldMove : MonoBehaviour
{
  private float Target;
  public float minDist;
  public float maxDist;
  public float speed;

  void Start()
  {
    Target = minDist;
  }

  void Update()
  {
    //Target += Time.deltaTime / 125;

    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, Target), speed);

    if (transform.position.z >= maxDist)
    {
      Target = minDist;
    }

    if (transform.position.z <= minDist)
    {
      Target = maxDist;
    }

  }
}