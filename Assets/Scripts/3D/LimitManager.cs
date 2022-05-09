using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitManager : MonoBehaviour
{
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private Ball3D ball;
    [SerializeField]
    private Transform center;


    // Update is called once per frame
    void Update()
    {
        float dist = Mathf.Abs(Vector3.Distance(ball.transform.position, center.position));
        if (dist > maxDistance)
            ball.ResetBall();
    }
}
