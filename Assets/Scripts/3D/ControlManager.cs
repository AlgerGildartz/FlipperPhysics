using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    [SerializeField]
    private Ball3D ball;

    // Update is called once per frame
    void Update()
    {
        
        // Launch ball
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Activate ball physics
            ball.EnablePhysics();
        }

        // Reset Ball
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reset ball position     
            ball.ResetBall();
            ScoreManager.Instance.ResetScore();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
