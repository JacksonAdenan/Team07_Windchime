using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomAnimation : MonoBehaviour
{
    GameManager gameManager;
    MouseLook playerController;

    public List<Transform> movementNodes;
    public bool isActive = false;
    public float movementSpeed = 5;

    private bool hasReachedNextNode = false;
    private bool isAnimationFinished = false;

    private int currentNodeIndex = 0;

    private int nodeCount = 0;


    

    // Start is called before the first frame update
    void Start()
    {
        if (isActive)
        { 
            nodeCount = movementNodes.Count;
            gameManager = GameManager.GetInstance();
            playerController = gameManager.playerController;
            playerController.isCentered = false;
            Debug.Log(currentNodeIndex + "=========== CURRENT NODE INDEX ==========");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimationFinished && isActive)
        {
            Move();


            playerController.FinishCameraCenter(movementNodes[currentNodeIndex].position, playerController.transform.position);
        }

        else if (isAnimationFinished)
        {
            isActive = false;
            Debug.Log("Player animation complete.");
            playerController.currentCameraMode = CameraMode.FPS_CONTROL;
        }
    }

    private void MoveTowardNode(Transform node)
    {
        
        Vector3 directionToMove = node.position - gameObject.transform.position;
        gameObject.GetComponent<CharacterController>().Move(directionToMove.normalized * movementSpeed * Time.deltaTime);

        if (Mathf.Abs(node.position.z - gameObject.transform.position.z) < 0.25f)
        {
            playerController.isCentered = true;
            hasReachedNextNode = true;
            Debug.Log("============ REACHED NEXT NODE at index " + currentNodeIndex);
            
        }
    }

    private void Move()
    {
        Transform currentNode = movementNodes[currentNodeIndex];

        if (!hasReachedNextNode)
        {
            MoveTowardNode(currentNode);
            LookTowardsNode(currentNode);
        }

        else if (hasReachedNextNode)
        {
            Debug.Log("+++++++++++++ [BEFORE] ++++++++++ " + currentNodeIndex);
            if (currentNodeIndex + 1 < nodeCount)
            {
                currentNodeIndex += 1;
                Debug.Log("+++++++++++++ [AFTER] ++++++++++ " + currentNodeIndex);
                Debug.Log("========MOVING TO NEXT NODE ======== " + (currentNodeIndex + 1));
                hasReachedNextNode = false;
            }
            else
            {
                isAnimationFinished = true;
            }
        }

        
    }

    private void LookTowardsNode(Transform node)
    {
        if (playerController.isCentered == false)
        {
            playerController.CentreCamera(node.position);
        }
    }

    public void StartAnimation()
    {
        playerController.currentCameraMode = CameraMode.ANIMATION;
        isActive = true;
    }
}
