using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generating and controlling Planes, Coins, Obstacles
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    Transform pool;
    /// <summary>
    ///settings 
    /// </summary>
    [SerializeField]
    int startPlanesCount = 15;
    [SerializeField]
    float distanceBetweenPlanes = 9;
    [SerializeField]
    int distanceBetweenObjects = 3;
    [SerializeField]
    float deathPostitonZForPlane = -25f;
    /// <summary>
    /// Chance for empty space on plane 2 = 0% 3 = 33% 4 = 50%
    /// </summary>
    [SerializeField]
    [Range(2,4)]
    int chanceForEmptySpaceOnPlane = 3;
    /// <summary>
    ///Reference for last plane in activePlanePool
    /// </summary>
    Transform lastPlane;
    /// <summary>
    ///Prefabs 
    /// </summary>
    [SerializeField]
    GameObject coinPrefab;
    [SerializeField]
    GameObject obstaclePrefab;
    [SerializeField]
    Transform planePrefab;
    /// <summary>
    ///Pools for objects 
    /// </summary>
    Queue<Transform> activePlanePool;
    Stack<Transform> disactivePlanePool;
    Queue<Transform> activeCoinPool;
    Stack<Transform> disactiveCoinPool;
    Queue<Transform> activeObstaclePool;
    Stack<Transform> disactiveObstaclePool;
    private void Awake()
    {
        activePlanePool = new Queue<Transform>();
        disactivePlanePool = new Stack<Transform>();
        activeCoinPool = new Queue<Transform>();
        disactiveCoinPool = new Stack<Transform>();
        activeObstaclePool = new Queue<Transform>();
        disactiveObstaclePool = new Stack<Transform>();
        ///<summary>
        ///Instantiate first element and set object on refernce lastPlane
        ///</summary>
        lastPlane = Instantiate(planePrefab);
        activePlanePool.Enqueue(lastPlane);
        /// <summary>
        ///Adding first planes
        /// </summary>    
        for (int i = 0; i < startPlanesCount; ++i)
        {
            AddPlane();
        }
    }
    /// <summary>
    /// Generating object on plane, using distance betweenobject for setting positions on X coordinate, can only be used for a row of 3 objects,
    /// loop have 3 iteration for each position on Plane, in loop for first getting random value to define type of object, 0 - Coin, 1 - Obstacle, else empty,
    /// then checking disactive pool count.
    /// if disactive pool not is empty using AddObjectActivePoolFromDisactivePool() function wich we transfer current position on X coordinate, 
    /// first element from disactive pool, and activepool, function getting object from pool.
    /// if disactive pool is empty using CreateObjectOnTrace() function wich we transfer current position on X coordinate, and object prefab, function instanciate new object.
    /// </summary>
    void GenerateObjectsOnPlane()
    {
        for (int i = -distanceBetweenObjects; i < distanceBetweenObjects + 1; i+= distanceBetweenObjects)
        {
            int randomValue = Random.Range(0, chanceForEmptySpaceOnPlane);
            if (randomValue == 0)
            {
                if (disactiveCoinPool.Count != 0)
                {
                    AddObjectToActivePoolFromDisactivePool(i, disactiveCoinPool.Pop(), activeCoinPool);
                }
                else
                {
                    activeCoinPool.Enqueue(CreateObjectOnTrace(i, coinPrefab));
                }
            }
            if (randomValue == 1)
            {
                if (disactiveObstaclePool.Count != 0)
                {
                    AddObjectToActivePoolFromDisactivePool(i, disactiveObstaclePool.Pop(), activeObstaclePool);
                }
                else
                {
                    activeObstaclePool.Enqueue(CreateObjectOnTrace(i, obstaclePrefab));
                }
            }
        }
    }
    /// <summary>
    /// Setting object as child of last plane in active planes queue.
    /// Set local position, X get from params, Y does not change, Z 0f for center position on this coordinate.
    /// Add object to active objects pool.
    /// </summary>
    /// <param name="positionX"></param>
    /// <param name="disactiveObjectFromPool"></param>
    /// <param name="activeObjectsPool"></param>
    void AddObjectToActivePoolFromDisactivePool(int positionX, Transform disactiveObjectFromPool, Queue<Transform> activeObjectsPool)
    {
        disactiveObjectFromPool.SetParent(lastPlane);
        disactiveObjectFromPool.localPosition = new Vector3(positionX, disactiveObjectFromPool.position.y, 0f);
        activeObjectsPool.Enqueue(disactiveObjectFromPool);
    }
    /// <summary>
    /// Instantiate prefab.
    /// Setting object as child of last plane in active planes queue.
    /// Set local position, X get from params, Y does not change, Z 0f for center position on this coordinate
    /// </summary>
    /// <param name="positionX"></param>
    /// <param name="prefab"></param>
    /// <returns></returns>
    Transform CreateObjectOnTrace(int positionX, GameObject prefab)
    {
        Transform obj = Instantiate(prefab.transform);
        obj.transform.SetParent(lastPlane);
        obj.transform.localPosition = new Vector3(positionX, obj.transform.position.y, 0f);

        return obj;
    }
    /// <summary>
    /// Recording the Z position of the last plane.
    /// Check disactive pool count.
    /// If the disactive pool is not empty, set the first item of the disactive pool to
    /// the refernce(lastPlane) of the last active plate and delete from disactive pool.
    /// If the disactive pool is empty, instantiate new plane and set to
    /// the refernce(lastPlane) of the last active plate.
    /// Add object to active plane pool.
    /// Set position   (direction * (recorded Z position of the penultimate Plane + distance between planes))
    /// </summary>
    void AddPlane()
    {
        float lastPlanePositionZ = lastPlane.position.z;
        if (disactivePlanePool.Count != 0)
        {
            lastPlane = disactivePlanePool.Pop();
        }
        else
        {
            lastPlane = Instantiate(planePrefab).transform;
        }
        activePlanePool.Enqueue(lastPlane);
        lastPlane.position = Vector3.forward * (lastPlanePositionZ + distanceBetweenPlanes);
    }
    /// <summary>
    ///Using in LateUpdate()
    ///Check first Plane Z position in active queue
    ///Adding plane
    ///Generating Object on this Plane
    ///Pushing first Plane in active queue to disactive Pool
    /// </summary>
    void FirstPlaneRester()
    {
        if (activePlanePool.Peek().position.z < deathPostitonZForPlane)
        {
            AddPlane();
            GenerateObjectsOnPlane();
            disactivePlanePool.Push(activePlanePool.Dequeue());
        }
    }
    /// <summary>
    ///Using in LateUpdate()
    ///Check active coin pool is not empty and first Plane Z position in active queue
    ///Set active state if player picked this coin and disactivate it before
    ///Change parent when waiting for call to active pool
    ///Pushing first Coin in active queue to disactive Pool 
    /// </summary>
    void FirstCoinReseter()
    {
        if (activeCoinPool.Count != 0 && activeCoinPool.Peek().parent.position.z < deathPostitonZForPlane + 15f)
        {
            activeCoinPool.Peek().gameObject.SetActive(true);
            activeCoinPool.Peek().SetParent(pool);
            disactiveCoinPool.Push(activeCoinPool.Dequeue());
        }
    }
    /// <summary>
    ///Using in LateUpdate()
    ///Check active obstacle pool is not empty and first Plane Z position in active queue
    ///Change parent when waiting for call to active pool
    ///Pushing first obstacle in active queue to disactive Pool 
    /// </summary>
    void FirstObstacleReseter()
    {
        if (activeObstaclePool.Count != 0 && activeObstaclePool.Peek().parent.position.z < deathPostitonZForPlane + 15f)
        {
            activeObstaclePool.Peek().SetParent(pool);
            disactiveObstaclePool.Push(activeObstaclePool.Dequeue());
        }
    }

    private void LateUpdate()
    {
        FirstPlaneRester();
        FirstCoinReseter();
        FirstObstacleReseter();
    }
}