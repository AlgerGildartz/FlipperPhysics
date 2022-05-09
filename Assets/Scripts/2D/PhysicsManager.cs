using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhysicsManager : MonoBehaviour
{
    public static PhysicsManager Instance;
    public float GravityForce;
    public float Teta;
    [SerializeField]
    public List<GameObject> Obstacles = new List<GameObject>();
    [SerializeField]
    List<Ball> Balls = new List<Ball>();

    [SerializeField]
    float rotateSpeed = 1;
    float leftRotation;

    public int score = 0;
    public Text scoreText;

    public GameObject LeftFlipper;
    public GameObject RightFlipper;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
        }
        Instance = this;
    }

    private void Update()
    {
        /*CheckCollision();*/
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if ( (LeftFlipper.transform.localRotation.eulerAngles.z >= 340 && LeftFlipper.transform.localRotation.eulerAngles.z <= 360) || (LeftFlipper.transform.localRotation.eulerAngles.z >= 0 && LeftFlipper.transform.localRotation.eulerAngles.z <= 20))
            {
                LeftFlipper.transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime);
                if (LeftFlipper.transform.localRotation.eulerAngles.z > 20)
                    LeftFlipper.transform.Rotate(transform.forward, 20 - LeftFlipper.transform.localRotation.eulerAngles.z);
            }
        }
        else
        {
            if (LeftFlipper.transform.localRotation.eulerAngles.z <  21 && LeftFlipper.transform.localRotation.eulerAngles.z >= 20)
                LeftFlipper.transform.Rotate(transform.forward, -40);
        }
        Debug.Log(RightFlipper.transform.localRotation.eulerAngles.z);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if ((RightFlipper.transform.localRotation.eulerAngles.z >= 340 && RightFlipper.transform.localRotation.eulerAngles.z <= 360) || (RightFlipper.transform.localRotation.eulerAngles.z >= 0 && RightFlipper.transform.localRotation.eulerAngles.z <= 20))
            {
                RightFlipper.transform.Rotate(transform.forward, -rotateSpeed * Time.deltaTime);
                if (RightFlipper.transform.localRotation.eulerAngles.z > 20)
                    RightFlipper.transform.Rotate(transform.forward, - (20 - RightFlipper.transform.localRotation.eulerAngles.z));
            }
            
        }
        else
        {
            if (RightFlipper.transform.localRotation.eulerAngles.z > 337 && RightFlipper.transform.localRotation.eulerAngles.z <= 341)
                RightFlipper.transform.Rotate(transform.forward, 39);
        }

        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene(0); }
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

    }

    public float VerticalSpeed(float speed)
    {
        return speed - GravityForce * Mathf.Abs(Mathf.Sin(Teta * Mathf.Deg2Rad)) * Time.deltaTime;
    }


    public bool Collision(Ball obj1, GameObject obj2)
    {
        if (obj1.GetComponent<SphereData>() && obj2.GetComponent<SphereData>())
        {
            float dist = Vector3.Distance(obj1.transform.position, obj2.transform.position);
            /*Debug.Log(dist);*/
            return Mathf.Abs(dist) < obj1.GetComponent<SphereData>().radius + obj2.GetComponent<SphereData>().radius;
        }

        return false;
    }
    public bool Collision(Vector3 pos, Ball obj1, GameObject obj2)
    {
        if (obj1.GetComponent<SphereData>() && obj2.GetComponent<SphereData>())
        {
            float dist = Mathf.Sqrt(Mathf.Pow((pos.x - obj2.transform.position.x), 2) + Mathf.Pow((pos.y - obj2.transform.position.y), 2));
            /*Debug.Log(dist);*/
            return dist < obj1.GetComponent<SphereData>().radius + obj2.GetComponent<SphereData>().radius;
        }

        if(obj1.GetComponent<SphereData>() && obj2.GetComponent<SquareData>())
        {
            Vector3 Ball2Rect = new Vector3(obj2.transform.position.x - obj1.transform.position.x, obj2.transform.position.y - obj1.transform.position.y);
            float distance = Mathf.Abs(Vector3.Distance(obj1.transform.position, obj2.transform.position));
            float Alpha = -Mathf.Atan(Ball2Rect.x / Ball2Rect.y) + Mathf.PI / 2;
            float Beta = -Mathf.Atan(obj2.transform.up.x / obj2.transform.up.y) + Mathf.PI / 2;
            float sigma = Mathf.PI - Alpha - (Mathf.PI / 2 - Beta);

            float radius = obj1.GetComponent<SphereData>().radius;

            return Mathf.Abs(distance * Mathf.Sin(sigma)) < obj2.GetComponent<SquareData>().height / 2 + radius
                && Mathf.Abs(distance * Mathf.Cos(sigma)) < obj2.GetComponent<SquareData>().width / 2 + radius;
        }

        return false;
    }

    public bool CheckCollsion(Vector3 pos, Ball ball)
    {
        foreach (GameObject obstacle in Obstacles)
        {
            if (Collision(pos, ball, obstacle))
            {
                int remainStep = findColisionPoint(ball, obstacle);
                ball.EnterCollision(obstacle, remainStep);
                return true;
            }
        }
        return false;
    }
    
    int findColisionPoint(Ball ball, GameObject collider)
    {
        Vector3 dir = ball.GetSpeed();
        Vector3 step = dir / 100;
        Vector3 tempPosition = new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z);
        for (int i = 1; i <= 100; i++)
        {
            tempPosition += step;
            if(Collision(tempPosition, ball, collider))
            {
                ball.transform.position = tempPosition - step;
                return 100 - i;
            }
        }
        return 0;
    }

    public void AddScore(int scoreAmout)
    {
        score += scoreAmout;
        scoreText.text = "Score : " + score;
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score : " + score;
    }

}
