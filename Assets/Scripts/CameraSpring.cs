using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    [Min(.01f)]
    [SerializeField] private float halfLife = .075f;
    [SerializeField] private float frequency = 18f;
    [SerializeField] private float angularDisplacement = 2f;
    [SerializeField] private float linearDisplacement = .05f;



    private Vector3 _springPosition;
    private Vector3 _springVelocity;


    public void Initialize()
    {
        _springPosition = transform.position;
        _springVelocity = Vector3.zero;
        
    }

    public void UpdateSpring(float deltaTime, Vector3 up)
    {
        transform.localPosition = Vector3.zero;
        Spring(ref _springPosition, ref _springVelocity, transform.position, halfLife, frequency, deltaTime);
        var localSpringPosition = _springPosition - transform.position;
        var springHeight = Vector3.Dot(localSpringPosition, up);

        transform.localEulerAngles = new Vector3(-springHeight * angularDisplacement, 0f, 0f);
        transform.localPosition += localSpringPosition * linearDisplacement;

    }

    private static void Spring(ref Vector3 current, ref Vector3 velocity, Vector3 target, float halfLife, float frequency, float timeStep)
    {
        var dampingRatio = -Mathf.Log(.5f) /( frequency * halfLife);
        var f = 1.0f + 2.0f * timeStep * dampingRatio * frequency;
        var oo = frequency * frequency;
        var hoo = timeStep * oo;
        var hhoo = timeStep * hoo;
        var detInv = 1.0f / (f + hhoo);
        var detX = f * current + timeStep * velocity + hhoo * target;
        var detV = velocity + hoo * (target - current);
        current = detInv * detX;
        velocity = detInv * detV;

        

    }
}
