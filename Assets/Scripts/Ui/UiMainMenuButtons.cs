using UnityEngine;
public class UiMainMenuButtons : MonoBehaviour
{
    public Transform planet;
    private readonly Quaternion[] rotations = new Quaternion[]
    {
        new Quaternion(0.5333418f, -0.2974675f, -0.3062021f, 0.7302739f),       // Play
        new Quaternion(0.4527639f, 0.1676113f, -0.5003538f, 0.7187194f),        // Options
        new Quaternion(-0.734323f, -0.4655148f, -0.1191639f, 0.4794432f),       // Credits
        new Quaternion(-0.6754118f, 0.1885183f, 0.4723541f, 0.5340052f)         // Exit
    };
    private Quaternion currentRotation;
    
    void Start()
    {
        currentRotation = rotations[0];
        planet.rotation = currentRotation;
    }
    void Update()
    {
        
    }

    void StopMovement()
    {
        StopAllCoroutines();
    }
}