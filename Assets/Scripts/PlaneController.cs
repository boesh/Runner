using UnityEngine;

/// <summary>
/// plane controller using for move plane with attached childs(coins, obstacles)
/// </summary>
public class PlaneController : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position += -Vector3.forward * Time.fixedDeltaTime * GameManager.planeSpeed; /// Move plane using static var from game manager for same values in each planes
    }
}
