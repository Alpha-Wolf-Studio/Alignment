using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UiMainMenuButtons : MonoBehaviour
{
    public List<UiButtonEffect> buttonsMenu = new List<UiButtonEffect>();
    public Transform planet;
    public Rigidbody rbPlanet;
    private Vector3 lastVelocity;
    private readonly Quaternion[] rotations = new Quaternion[]
    {
        new Quaternion(0.5333418f, -0.2974675f, -0.3062021f, 0.7302739f),  // Play
        new Quaternion(0.4840734f, 0.007219282f, -0.4119702f, 0.7719465f), // Options
        new Quaternion(-0.734323f, -0.4655148f, -0.1191639f, 0.4794432f),  // Credits
        new Quaternion(-0.6754118f, 0.1885183f, 0.4723541f, 0.5340052f)    // Exit
    };

    private float onTime;
    private float maxTime = 0.5f;
    private Quaternion currentRotation;
    private Quaternion lastRotation;
    private void Awake()
    {
        rbPlanet = planet.GetComponent<Rigidbody>();
    }
    void Start()
    {
        buttonsMenu[0].onButtonEnter += SetPlanetPositionOnPlay;
        buttonsMenu[1].onButtonEnter += SetPlanetPositionOnOptions;
        buttonsMenu[2].onButtonEnter += SetPlanetPositionOnCredits;
        buttonsMenu[3].onButtonEnter += SetPlanetPositionOnExit;

        buttonsMenu[0].onButtonExit += PlayPlanet;
        buttonsMenu[1].onButtonExit += PlayPlanet;
        buttonsMenu[2].onButtonExit += PlayPlanet;
        buttonsMenu[3].onButtonExit += PlayPlanet;
    }
    public void SetPlanetPositionOnPlay()
    {
        StopPlanet();
        currentRotation = rotations[0];
    }
    public void SetPlanetPositionOnOptions()
    {
        StopPlanet();
        currentRotation = rotations[1];
    }
    public void SetPlanetPositionOnCredits()
    {
        StopPlanet();
        currentRotation = rotations[2];
    }
    public void SetPlanetPositionOnExit()
    {
        StopPlanet();
        currentRotation = rotations[3];
    }
    void StopPlanet()
    {
        lastVelocity = rbPlanet.angularVelocity;
        rbPlanet.constraints = RigidbodyConstraints.FreezeAll;
        onTime = 0;
        StartCoroutine(SetRotation());
    }
    void PlayPlanet()
    {
        StopAllCoroutines();
        rbPlanet.constraints = RigidbodyConstraints.FreezePosition;
        rbPlanet.angularVelocity = lastVelocity;
        onTime = 0;
    }
    IEnumerator SetRotation()
    {
        lastRotation = planet.rotation;
        while (onTime < maxTime)
        {
            onTime += Time.deltaTime;
            float lerp = onTime / maxTime;
            planet.rotation = Quaternion.Lerp(lastRotation, currentRotation, lerp);
            yield return null;
        }
    }
}