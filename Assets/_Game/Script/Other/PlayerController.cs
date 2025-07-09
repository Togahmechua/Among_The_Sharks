using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;  // tốc độ tăng tốc
    public float deceleration = 10f;  // tốc độ giảm tốc

    [SerializeField] private GameObject model;

    private Rigidbody rb;
    private Vector3 inputDirection;
    private Vector3 targetVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Xoay model theo hướng input nếu đang di chuyển
        if (inputDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        Vector3 currentVelocity = rb.velocity;

        // Giữ lại vận tốc Y để không ảnh hưởng gravity
        targetVelocity = inputDirection * moveSpeed;
        targetVelocity.y = currentVelocity.y;

        // Tăng tốc hoặc giảm tốc mượt
        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity,
            (inputDirection.magnitude > 0 ? acceleration : deceleration));

        rb.velocity = newVelocity;
    }


    public void SetInputDirection(Vector3 direction)
    {
        inputDirection = direction.normalized;
    }

}
