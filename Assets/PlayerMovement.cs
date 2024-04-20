using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerCircle;
    [SerializeField] private Rigidbody2D enemyCircle;
    
    public float PlayerVelocity;
    private float circlesDistance;
    private float dotProduct;
    private float angle;
    private float xInput;
    private float yInput;

    void Update()
    {
        LoadInput();
    }

    private void FixedUpdate()
    {
        InputMoveObject(enemyCircle);
        AutoMovePlayer();
    }

    private void InputMoveObject(Rigidbody2D givenObject)
    {
        givenObject.velocityX = xInput * PlayerVelocity * 5;
        givenObject.velocityY = yInput * PlayerVelocity * 5;
    }

    private void AutoMovePlayer()
    {
        Vector2 direction = (enemyCircle.position - playerCircle.position);
        float dot = GetDotProduct(playerCircle.transform.up, direction.normalized);
        
        //if (dot <= 0.9f || dot > 1 || direction.magnitude >= 25)
        //{
        //    return;
        //}
        
        float angle = GetAngle(playerCircle.transform.up, direction);
        Vector3 rotationAxis = GetCrossProduct(playerCircle.transform.up, direction);

        int clockwise = 1;
        if (rotationAxis.z < 0)
        {
            clockwise = -1;
        }

        playerCircle.transform.Rotate(0, 0, angle * clockwise);
        playerCircle.velocity = direction.normalized;
    }

    private void LoadInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    public float GetDistance()
    {
        circlesDistance = Vector2.Distance(playerCircle.position, enemyCircle.position);
        
        //Debug.Log("Distance: " + circlesDistance);
        return circlesDistance;
    }

    public float GetDotProduct(Vector2 V, Vector2 W)
    {
        //inaczej: Iloczyn skalarny
        dotProduct = V.x * W.x + V.y * W.y;
        
        return dotProduct;
    }

    public float GetAngle(Vector2 V, Vector2 W)
    {
        float playerMagnitude = V.magnitude;
        float enemyMagnitude = W.magnitude;

        if (playerMagnitude <= 0f || enemyMagnitude <= 0f)
        {
            return 0f;
        }

        float angleRad = GetDotProduct(V, W) / (playerMagnitude * enemyMagnitude);
        angle = Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(angleRad, -1f, 1f));
        
        return angle;
    }
    public Vector3 GetCrossProduct(Vector2 V, Vector2 W)
    {
        Vector3 playerV3 = new(V.x, V.y, 1);
        Vector3 enemyV3 = new(W.x, W.y, 1);

        float cross_x = playerV3.y * enemyV3.z - playerV3.z * enemyV3.y;
        float cross_y = playerV3.z * enemyV3.x - playerV3.x * enemyV3.z;
        float cross_z = playerV3.x * enemyV3.y - playerV3.y * enemyV3.x;

        Vector3 cross = new(cross_x, cross_y, cross_z);

        return cross;
    }
}
