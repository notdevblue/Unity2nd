using UnityEngine;

public class MyGizimo : MonoBehaviour
{
    public Color color = Color.red;
    public float radius = 0.3f;


    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
