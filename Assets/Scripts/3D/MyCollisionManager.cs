using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCollisionManager : MonoBehaviour
{
    public static MyCollisionManager Instance;
    public static Vector3 Gravity = new Vector3(0, -9.81f, 0);

    public List<PhysicObject> StaticObjects = new List<PhysicObject>();
    public List<PhysicObject> BallsList = new List<PhysicObject>();
    public int NbSteps = 100;
    private int Loop = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
        }
        Instance = this;

        // Get all static and dynamic objects
        BallsList.Clear();
        StaticObjects.Clear();

        PhysicObject[] objects = FindObjectsOfType<PhysicObject>();
        foreach (PhysicObject obj in objects)
        {
            if ( obj.gameObject.name == "Floor") 
            {
                StaticObjects.Add(obj);
            }
        }
        foreach (PhysicObject obj in objects)
        {
            if (obj is Ball3D)
            {
                BallsList.Add(obj);
                StaticObjects.Add(obj);
            }
            else if (!StaticObjects.Contains(obj))
                StaticObjects.Add(obj);
        }
            
    }

    public void CheckFlipperCollision(List<PhysicObject> physicObjects)
    {
        foreach (PhysicObject ball in BallsList)
        {
            CheckCollision((Ball3D)ball, physicObjects);
        }
    }


    public bool CheckCollision(Ball3D _ball)
    {
        return CheckCollision(_ball, StaticObjects);
    }

    public bool CheckCollision(Ball3D _ball, List<PhysicObject> physicObjects)
    {
        Loop++;
        foreach (PhysicObject obj in physicObjects)
        {
            if (IsCollision(_ball, obj) && _ball.gameObject != obj.gameObject)
            {
                if (obj.GetTypeObstacle() == PhysicObject.TypeObstacle.Limit)
                {
                    _ball.ResetBall();
                    ScoreManager.Instance.ResetScore();
                }
                else
                {
                    _ball.CollisionEvent(obj, findColisionPoint(_ball, obj));
                }
                return true;
            }
        }
        return false;
    }

    static bool IsCollision(Ball3D _ball, PhysicObject _obj)
    {
        if (_obj.GetTypeObstacle() == PhysicObject.TypeObstacle.Rectangle || _obj.GetTypeObstacle() == PhysicObject.TypeObstacle.Limit)
        {
            // Collide with a rectangle
            Vector3 obj2Ball =  _ball.NextPos - _obj.transform.position;
            return Mathf.Abs(Vector3.Dot(obj2Ball, _obj.transform.up)) < _ball.GetRadius() + _obj.transform.localScale.y / 2
                && Mathf.Abs(Vector3.Dot(obj2Ball, _obj.transform.right)) < _ball.GetRadius() + _obj.transform.localScale.x / 2
                && Mathf.Abs(Vector3.Dot(obj2Ball, _obj.transform.forward)) < _ball.GetRadius() + _obj.transform.localScale.z / 2;

        } 
        else if(_obj.GetTypeObstacle() == PhysicObject.TypeObstacle.Sphere)
        {
            // Collide with a Sphere
            float dist = Vector3.Distance(_ball.NextPos, _obj.transform.position);
            return Mathf.Abs(dist) < _ball.GetRadius() + _obj.transform.localScale.x / 2;
        }
        else if (_obj.GetTypeObstacle() == PhysicObject.TypeObstacle.Cylinder)
        {
            // Collide with a Cylinder
            Vector3 obj2Ball = _ball.NextPos - _obj.transform.position;
            return Mathf.Abs(Vector3.Dot(obj2Ball, _obj.transform.up)) < _obj.transform.localScale.y + _ball.GetRadius()
                && Mathf.Abs(Vector3.Dot(obj2Ball, _obj.transform.right)) < _obj.transform.localScale.x / 2 + _ball.GetRadius()
                && Mathf.Abs(Vector3.Dot(obj2Ball, _obj.transform.forward)) < _obj.transform.localScale.z / 2 + _ball.GetRadius();

        }
        return false;

    }
    int findColisionPoint(Ball3D ball, PhysicObject collider)
    {
        Vector3 dir = ball.Speed;
        Vector3 step = dir / NbSteps;
        Vector3 tempPosition = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);
        for (int i = 1; i <= NbSteps; i++)
        {
            tempPosition += step;
            if(IsCollision(ball, collider))
            {
                ball.transform.position = tempPosition - step;
                return NbSteps - i;
            }
        }
        return 0;
    }


    public int GetLoop() { return Loop; }
    public void ResetLoop() { Loop = 0; }
    
}
