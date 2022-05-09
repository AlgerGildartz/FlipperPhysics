using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public enum Side {
        Left,
        Right
    }
    [SerializeField]
    private Side side;

    [SerializeField]
    private float speed = 240f;

    [SerializeField]
    private List<PhysicObject> children;

    private bool isMoving = false;
    private float actualRotation = 0f;
    private float maxRotation = 60f;
    private bool go = true;
    private Quaternion baseRotation;
    private int direction;

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            children.Add(child.GetComponent<PhysicObject>());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        baseRotation = transform.rotation;
    }

    void ChangeAbsorbtion(bool reset, float increment = 0.2f)
    {
        float value = 1;
        foreach (PhysicObject obj in children)
        {
            obj.SetAbsobtion(value);
            if(!reset)
                value += increment;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (side == Side.Left && !isMoving && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isMoving = true;
            actualRotation = 0;
            direction = 1;
            ChangeAbsorbtion(false);
        } else if(side == Side.Right && !isMoving && Input.GetKeyDown(KeyCode.RightArrow))
        {
            isMoving = true;
            actualRotation = 0;
            direction = -1;
            ChangeAbsorbtion(false);
        }

        if (isMoving)
            Movement();
   
    }

    void Movement()
    {
        float move = speed * Time.deltaTime;

        // handle the go and the return
        if (go)
            actualRotation += move ;
        else
            actualRotation -= move ;
        
        if (go)
            transform.Rotate(transform.right, move * -direction, Space.World);
        else
            transform.Rotate(transform.right, move * direction, Space.World);
        MyCollisionManager.Instance.CheckFlipperCollision(children);

        if (go && actualRotation >= maxRotation)
            go = false;

        else if (!go && actualRotation <= 0)
        {
            StopMovement();
        }
    }

    void StopMovement()
    {
        isMoving = false;
        go = true;
        // Avoid the flipper to move slightly
        transform.rotation = baseRotation;
        ChangeAbsorbtion(true);
    }
}
