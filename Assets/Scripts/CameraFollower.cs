using UnityEngine;

/// <summary>
/// object smooth following target with distance
/// </summary>
public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    /// <summary>
    /// settings
    /// </summary>
    [SerializeField]
    [Range(5, 10)]
    float distanceY;
    [SerializeField]
    [Range(-5, -10)]
    float distanceZ;
    [SerializeField]
    [Range(5, 10)]
    float smooth;

    void FixedUpdate()
    {
        transform.position += (new Vector3(target.position.x, target.position.y + distanceY, target.position.z + distanceZ) - transform.position) * Time.deltaTime * smooth; /// smooth follow
    }
}
