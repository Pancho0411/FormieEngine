using UnityEngine;

public class FormieCollision
{
    public Vector3 point;
    public Vector3 normal;

    public float distance;

    public Collider collider;

    public FormieCollision(Vector3 point, Vector3 normal, float distance, Collider collider)
    {
        this.point = point;
        this.normal = normal;
        this.distance = distance;
        this.collider = collider;
    }
}
