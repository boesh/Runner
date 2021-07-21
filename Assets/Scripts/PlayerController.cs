using UnityEngine;

/// <summary>
/// Control player position, check player collision with coins and obstacles
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Settings
    /// </summary>
    [SerializeField]
    float jumpDistance = 5f;
    [SerializeField]
    int bordersLimit = 3;
    [SerializeField]
    float moveSpeed = 10f;
    /// <summary>
    /// target postions
    /// </summary>
    float targetPositionX = 0f;
    float targetPositionY;
    float startPositionY;

    [SerializeField]
    PauseMenuController pm;

    private void Awake()
    {
        startPositionY = transform.position.y;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            pm.Death();
        }
        if (other.tag == "Coin")
        {
            other.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Get inputs for set target player position, if target position in borders change player position
    /// </summary>
    void PlayerInputs()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (targetPositionX > -bordersLimit)
            {
                targetPositionX -= bordersLimit;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (targetPositionX < bordersLimit)
            {
                targetPositionX += bordersLimit;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (targetPositionY == 1)
            {
                targetPositionY = jumpDistance;
            }
        }
    }
    void Update()
    {
        PlayerInputs();
    }
    private void FixedUpdate()
    {
        transform.position += (new Vector3(targetPositionX, targetPositionY) - transform.position) * Time.deltaTime * moveSpeed; /// smooth player moving

        if (targetPositionY > startPositionY) /// check current Y position with startY position 
        {
            targetPositionY -= Time.deltaTime * moveSpeed * 2f; ///smooth player fall by reducing target Y postion
        }
        else
        {
            targetPositionY = startPositionY; /// set start Y position
        }
    }
}
