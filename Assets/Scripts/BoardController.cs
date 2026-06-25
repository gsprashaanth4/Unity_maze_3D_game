using UnityEngine;

public class BoardController : MonoBehaviour
{
    [Header("Speed")]
    public float AngularRate = 90.0f;                      // variable specifying the multiplier for the rotation input

    private float updateInterval = 0.5f;                  // the time interval to update the FPS measurement
    private float accumTime = 0;                          // support variable for FPS measurement
    private int framesCount = 0;                          // support variable for FPS measurement
    private float fps;                                    // variable for storing FPS measurement

    // Accumulators for input offsets per frame
    private float app_pitch;
    private float app_roll;

    // Define your global reference axes
    private readonly Vector3 rightGlobal = Vector3.right;       // (1, 0, 0)
    private readonly Vector3 forwardGlobal = Vector3.forward;   // (0, 0, 1)

    // Persistent board angles
    private float pitchAngle = 0f;
    private float rollAngle = 0f;

    // Maximum tilt
    public float MaxTiltAngle = 30f;

    void Update()
    {
        /* UPDATING THE FPS COUNTER EVERY 0.5 SECONDS */
        accumTime += Time.deltaTime;
        framesCount++;

        if (accumTime >= updateInterval)
        {
            fps = framesCount / accumTime;
            // Debug.Log(fps);

            accumTime = 0f;     // resetting trackers
            framesCount = 0;    // resetting trackers
        }

        /* USER INPUT COLLECTION AND NOTING IN MEMORY */

        app_pitch = 0f;
        app_roll = 0f;

        // Forward / Backward Tilt (Pitch)
        if (Input.GetKey(KeyCode.I)) app_pitch += 1f;
        if (Input.GetKey(KeyCode.K)) app_pitch -= 1f;

        // Left / Right Tilt (Roll)
        if (Input.GetKey(KeyCode.J)) app_roll += 1f;
        if (Input.GetKey(KeyCode.L)) app_roll -= 1f;
    }

    void FixedUpdate()
    {
        // Update stored angles
        pitchAngle += app_pitch * AngularRate * Time.fixedDeltaTime;
        rollAngle += app_roll * AngularRate * Time.fixedDeltaTime;

        pitchAngle = Mathf.Clamp(
            pitchAngle,
            -MaxTiltAngle,
            MaxTiltAngle
        );

        rollAngle = Mathf.Clamp(
            rollAngle,
            -MaxTiltAngle,
            MaxTiltAngle
        );

        Quaternion pitchRot = Quaternion.AngleAxis(pitchAngle, rightGlobal);
        Quaternion rollRot = Quaternion.AngleAxis(rollAngle, forwardGlobal);

        transform.rotation = pitchRot * rollRot;
    }

    public void ResetBoard()
    {
        pitchAngle = 0f;
        rollAngle = 0f;
        transform.rotation = Quaternion.identity;
    }
}