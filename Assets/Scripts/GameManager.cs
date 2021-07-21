using System.Collections;
using UnityEngine;

/// <summary>
/// settings, Couroutine for Plane
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// settings
    /// </summary>
    [SerializeField]
    float startPlaneSpeed;
    [SerializeField]
    float termPlaneSpeed;
    [SerializeField]
    float termsInterval;

    public static float planeSpeed; /// static ver using in PlaneController for setting speed

    private void Awake()
    {
        planeSpeed = startPlaneSpeed;
    }
    private void Start()
    {
        StartCoroutine(TraceSpeedIncraser());
    }
    /// <summary>
    /// Coroutine for increase planeSpeed
    /// </summary>
    /// <returns></returns>
    IEnumerator TraceSpeedIncraser()
    {
        for (; ; )
        {
            planeSpeed += termPlaneSpeed;
            yield return new WaitForSeconds(termsInterval);
        }
    }
   
}
