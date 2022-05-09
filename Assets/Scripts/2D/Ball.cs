using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 launchSpeed = Vector3.zero;
    private Vector3 Speed = Vector3.zero;


    public Transform launchPoint;

    public float YBoundaries = -10;


    public float TiltForce = 1;

    private bool move = false;


    public Vector3 GetSpeed() { return Speed; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !move) { 
            move = true;
            Speed = launchSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Q) && move)
        {
            LeftTilt();
            ScreenShakeManager.Instance.ScreenShake();
        }
        if (Input.GetKeyDown(KeyCode.D) && move)
        {
            RightTilt();
            ScreenShakeManager.Instance.ScreenShake();
        }

        if (move) { 
            Speed.y = PhysicsManager.Instance.VerticalSpeed(Speed.y);
            Vector3 NextPos = transform.position + Speed * Time.deltaTime;
            if (!PhysicsManager.Instance.CheckCollsion(NextPos, this))
                transform.position = NextPos;
        }


        if (transform.position.y < YBoundaries && launchPoint)
        {
            transform.position= launchPoint.position;
            move = false;
            PhysicsManager.Instance.ResetScore();
        }
    }


    private void RightTilt()
    {
        Speed.x += TiltForce;
    }
    private void LeftTilt()
    {
        Speed.x -= TiltForce;
    }

    public void EnterCollision(GameObject colObj, int remainStep)
    {
        if (colObj.GetComponent<SphereData>())
        {
            Vector3 Bump2Ball = new Vector3(transform.position.x - colObj.transform.position.x, transform.position.y - colObj.transform.position.y, transform.position.z - colObj.transform.position.z);
            float Beta = Mathf.Atan(Bump2Ball.x / Bump2Ball.y);
            float Alpha = Mathf.Atan(Speed.x / Speed.y);
            float Alpha2 = 2 * Beta - Alpha;
            if (Vector3.Angle(Speed, colObj.transform.up) < 90)
            {
                Alpha2 += Mathf.PI;
            }
            float realSpeed = Mathf.Sqrt(Mathf.Pow(Speed.x, 2) + Mathf.Pow(Speed.y, 2)) * colObj.GetComponent<SphereData>().absorbtion;
            float speedStep = realSpeed / 100;
            Speed.y = realSpeed * Mathf.Cos(Alpha2);
            Speed.x = realSpeed * Mathf.Sin(Alpha2);

            transform.position += new Vector3(speedStep * remainStep * Mathf.Sin(Alpha2), speedStep * remainStep * Mathf.Cos(Alpha2), Speed.z) * Time.deltaTime;

            PhysicsManager.Instance.AddScore(colObj.GetComponent<SphereData>().score);

            Debug.Log("la balle entre en collision avec cercle");
        }

        if (colObj.GetComponent<SquareData>())
        {

            //calcul des ratios en x et y dans le refrenciel du cube
            Vector3 B2R = new Vector3(colObj.transform.position.x - transform.position.x, colObj.transform.position.y - transform.position.y);
            float dist = B2R.magnitude;
            float B2RAngle = -Mathf.Atan(B2R.x / B2R.y) + Mathf.PI / 2;
            float angleUp = -Mathf.Atan(colObj.transform.up.x / colObj.transform.up.y) + Mathf.PI / 2;

            float sigma = Mathf.PI - B2RAngle - (Mathf.PI / 2 - angleUp);

            float distY = dist * Mathf.Sin(sigma) / colObj.GetComponent<SquareData>().height;
            float distX = dist * Mathf.Cos(sigma) / colObj.GetComponent<SquareData>().width;

            /////////////////////////////////////////////////////////////
            ///calcul de l'angle

            Vector3 R2B = new Vector3(transform.position.x - colObj.transform.position.x, transform.position.y - colObj.transform.position.y);

            float angleUnder = Vector3.Angle(R2B, colObj.transform.up);

            float vectorToUp = -Mathf.Atan(colObj.transform.up.x / colObj.transform.up.y) + Mathf.PI / 2;
            float angleToSpeed = -Mathf.Atan(Speed.x / Speed.y) + Mathf.PI / 2;
            bool swapangle = angleUnder > 90;//vérifie la position de la balle par rapport la position et l'orienttion du rectangle
                                             //
            if (Mathf.Abs(distY) < Mathf.Abs(distX))
            {
                vectorToUp = -Mathf.Atan(colObj.transform.right.x / colObj.transform.right.y) + Mathf.PI / 2;
                swapangle = false;
                if (Vector3.Angle(R2B, colObj.transform.right) < 90)
                    swapangle = true;
            }

            float newAngle = 2 * vectorToUp - angleToSpeed;
            //////////////////////////////////////////////////////////////////////////////
            if (swapangle)
                newAngle += Mathf.PI;


            float realSpeed = Mathf.Sqrt(Mathf.Pow(Speed.x, 2) + Mathf.Pow(Speed.y, 2)) * colObj.GetComponent<SquareData>().absorbtion;
            Speed.y = realSpeed * Mathf.Sin(newAngle);
            Speed.x = realSpeed * Mathf.Cos(newAngle);



            Debug.Log("Speed = " + Speed);
            float speedStep = realSpeed / 100;
            transform.position += new Vector3(speedStep * (remainStep + 1) * Mathf.Cos(newAngle), speedStep * remainStep * Mathf.Sin(newAngle), Speed.z) * Time.deltaTime;
        }
    }
}
