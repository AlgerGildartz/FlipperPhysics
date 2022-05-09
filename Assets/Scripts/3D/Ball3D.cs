using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball3D : PhysicObject
{
    [SerializeField]
    float radius;

    [SerializeField]
    private GameObject ballLaunch;

    private bool hasPhysics;

    public Vector3 Speed;
    public Vector3 Acceleration;
    public Vector3 NextPos;
    public Vector3 LastSpeed;
    public float MaxSpeed = 200;
    public bool FirstCollision = true;

    // Start is called before the first frame update
    void Start()
    {
        Acceleration += MyCollisionManager.Gravity;
        ResetBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPhysics)
        {
            Speed += Acceleration * Time.deltaTime;
            if (Speed.magnitude > MaxSpeed)
            {
                Speed = Vector3.Normalize(Speed) * MaxSpeed;
            }
            NextPos = transform.position + Speed * Time.deltaTime;
            FirstCollision = true;
            if (!MyCollisionManager.Instance.CheckCollision(this))
            {
                transform.position = NextPos;
            }
        }
    }

    public void ResetBall()
    {
        // Reset ball position
        if (ballLaunch)
            transform.position = ballLaunch.transform.position;
        // and desactivate ball physics
        hasPhysics = false;
        // and reset speed
        Speed = Vector3.zero;
    }

    public void EnablePhysics()
    {
        hasPhysics = true;
    }


    public override void CollisionEvent(PhysicObject collider, int remainStep)
    {
        Vector3 perp = Vector3.zero;
        Vector3 obj2Ball = transform.position - collider.transform.position;

        //////////////////////////////////////////////////////////
        ////////////////////// RECTANGLE /////////////////////////
        //////////////////////////////////////////////////////////
        if (collider.GetTypeObstacle() == TypeObstacle.Rectangle) {

            ///////////////////////////////////////////////////////////////////////


            Vector3 obj2BallXYProjection = collider.transform.up * Vector3.Dot(obj2Ball, collider.transform.up)
                +collider.transform.right * Vector3.Dot(obj2Ball, collider.transform.right);
            Vector3 obj2BallXZProjection = collider.transform.right * Vector3.Dot(obj2Ball, collider.transform.right) 
                + collider.transform.forward * Vector3.Dot(obj2Ball, collider.transform.forward);
            Vector3 obj2BallYZProjection = collider.transform.up * Vector3.Dot(obj2Ball, collider.transform.up) 
                + collider.transform.forward * Vector3.Dot(obj2Ball, collider.transform.forward);
            
            ///Certainement améliorable
            float angleUpXY = Vector3.Angle(obj2BallXYProjection, collider.transform.up);
            float angleUpYZ = Vector3.Angle(obj2BallYZProjection, collider.transform.up);
            float angleDownXY = Vector3.Angle(obj2BallXYProjection, -collider.transform.up);
            float angleDownYZ = Vector3.Angle(obj2BallYZProjection, -collider.transform.up);

            float angleRightXY = Vector3.Angle(obj2BallXYProjection, collider.transform.right);
            float angleRightXZ = Vector3.Angle(obj2BallXZProjection, collider.transform.right);
            float angleLeftXY = Vector3.Angle(obj2BallXYProjection, -collider.transform.right);
            float angleLeftXZ = Vector3.Angle(obj2BallXZProjection, -collider.transform.right);

            float angleForwardXZ = Vector3.Angle(obj2BallXZProjection, collider.transform.forward);
            float angleForwardYZ = Vector3.Angle(obj2BallYZProjection, collider.transform.forward);
            float angleBackWardXZ = Vector3.Angle(obj2BallXZProjection, -collider.transform.forward);
            float angleBackWardYZ = Vector3.Angle(obj2BallYZProjection, -collider.transform.forward);

            /////////////////////////////////////////////////////////////////////////////////////////////////////
            if ((angleUpXY < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.y) * Mathf.Rad2Deg )
                && angleUpYZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.y) * Mathf.Rad2Deg)) 
                ||
                (angleDownXY < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.y)) * Mathf.Rad2Deg
                && angleDownYZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.y)) * Mathf.Rad2Deg))
            {
                perp = Vector3.Normalize(collider.transform.up) * Vector3.Dot(Speed, collider.transform.up);
            }
            else if ( (angleRightXY < Mathf.Abs(Mathf.Atan(collider.transform.localScale.y / collider.transform.localScale.x) * Mathf.Rad2Deg) 
                && angleRightXZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.y / collider.transform.localScale.x) * Mathf.Rad2Deg))
                ||
                (angleLeftXY < Mathf.Abs(Mathf.Atan(collider.transform.localScale.y / collider.transform.localScale.x) * Mathf.Rad2Deg)
                && angleLeftXZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.y / collider.transform.localScale.x) * Mathf.Rad2Deg)) )
            {
                perp = Vector3.Normalize(collider.transform.right) * Vector3.Dot(Speed, collider.transform.right);

            }

            else if ( ( angleForwardXZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.z) * Mathf.Rad2Deg)
                && angleForwardYZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.z) * Mathf.Rad2Deg) )
                || 
                ( angleBackWardXZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.z) * Mathf.Rad2Deg)
                && angleBackWardYZ < Mathf.Abs(Mathf.Atan(collider.transform.localScale.x / collider.transform.localScale.z) * Mathf.Rad2Deg) ) )
                
            {
                perp = Vector3.Normalize(collider.transform.forward) * Vector3.Dot(Speed, collider.transform.forward);
            }

        }

        //////////////////////////////////////////////////////////
        //////////////////////// SPHERE //////////////////////////
        //////////////////////////////////////////////////////////
        else if (collider.GetTypeObstacle() == TypeObstacle.Sphere)
        {
            Debug.Log("collision avec une sphere");



            perp = Vector3.Normalize(obj2Ball) * Vector3.Dot(Speed, obj2Ball);
        }

        //////////////////////////////////////////////////////////
        /////////////////////// CYLINDER /////////////////////////
        //////////////////////////////////////////////////////////
        else if (collider.GetTypeObstacle() == TypeObstacle.Cylinder)
        {
            Vector3 normalPara = Vector3.Normalize(collider.transform.up) * Vector3.Dot(obj2Ball, collider.transform.up);
            Vector3 normal = obj2Ball - normalPara;

            perp = Vector3.Normalize(normal) * Vector3.Dot(Speed, normal);

            ScoreManager.Instance.AddScore();
            
        }
        Speed =  (Speed - perp) - perp * collider.GetAbsorbtion();
        NextPos = transform.position + Speed / MyCollisionManager.Instance.NbSteps * remainStep * Time.deltaTime;
        if (!MyCollisionManager.Instance.CheckCollision(this) || MyCollisionManager.Instance.GetLoop() > 100)
        {
            MyCollisionManager.Instance.ResetLoop();
            transform.position = NextPos;
            return;
        }

    }

    #region Getter/Setter
    public float GetRadius() { return radius; }
    #endregion
}
