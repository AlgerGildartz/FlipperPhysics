using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour
{
    [SerializeField]
    private float Absorbtion = 1;

    [SerializeField]
    private TypeObstacle type;

    public enum TypeObstacle
    {
        None,
        Rectangle,
        Sphere,
        Cylinder,
        Limit
    }

    public virtual void CollisionEvent(PhysicObject collider, int remainStep)
    {

    }

    public virtual void SetAbsobtion(float _absorbtion) { Absorbtion = _absorbtion; }

    public virtual float GetAbsorbtion() { return Absorbtion; }

    public virtual TypeObstacle GetTypeObstacle() { return type; }

}
