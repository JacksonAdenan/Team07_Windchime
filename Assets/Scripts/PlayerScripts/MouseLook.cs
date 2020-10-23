﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum CameraMode
{ 
    lookMode,
    handMode,
    NEWMODE,
    pauseMode
}

public enum PlayerState
{ 
    LOOKING_AT_NOTHING,
    HOLDING_ITEM,
    LOOKING_AT_ITEM,
    LOOKING_AT_APPLIANCE,
    LOOKING_AT_SWITCH

}

public enum SwitchType
{ 
    BLENDER_BUTTON_1,
    BLENDER_BUTTON_2,
    WATER_TAP,

    ERROR
}
public class MouseLook : MonoBehaviour
{
    // Adjustable Values //
    // ------------------------------------------ //
    public float mouseSensitivity = 100f;
    public float rotationSensitivity = 100f;
    public float handControlSensitivity = 100f;
    public float handZDistance = 0.7f;
    public float handYCeilingLimit = 0.0f;
    public float handYFloorLimit = 0.0f;
    public float handXLeftLimit = 0.0f;
    public float handXRightLimit = 0.0f;
    public float PickUpUIYPos = 0.0f;
    public float ApplianceUIZPos = 3.0f;
    public float ApplianceUIYPos = 5.0f;
    public float handCollisionRadius = 0;
    public float heldItemPosX = 0;
    public float heldItemPosY = 0;
    public float heldItemPosZ = 0;

    public float tempThrowForce = 0;
    public float roationLerpSpeed;
    public float cameraCenteringDeadZone;

    // ------------------------------------------ //

    // Inspector Variables //
    // ------------------------------------------ //
    public Transform playerBody;
    public Material itemSelectedMat;
    public Material switchSelectedMat;

    public Transform hand;
    public Transform collisionSphere;
    public Transform realHandCentre;

    public Canvas PickUpUI;
    public Canvas ApplianceUI;

    public Canvas crosshairCanvas;
    public Image crosshairImage;

    public float xDeadZone;
    public float yDeadZone;

    // ------------------------------------------ //


    bool isHoldingItem = false;
    Vector3 handPos;
    public CameraMode currentCameraMode = CameraMode.lookMode;
    public static Transform selectedItem;
    public static Transform selectedAppliance = null;
    public static Transform heldItem = null;
    public Transform selectedSwitch = null;
    private Material defaultMat;
    private Material switchDefaultMat;
    float xRotation = 0.0f;

    Transform insertText = null;
    Transform notHoldingText = null;

    // Different raycasts //
    RaycastHit raycastFromHand;
    RaycastHit raycastFromScreen;
    Collider[] collisions;

    PlayerState currentPlayerState = PlayerState.LOOKING_AT_NOTHING;

    

    public float posX;
    public float posY;

    Vector3 heldItemOriginalPos;
    Vector3 previousHandMovementDir;
    Vector3 previousHandPos;
    bool isCentered = true;

    Vector3 handMovement;
    RaycastHit target;

    SwitchType selectedSwitchType;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


        
    }

    // Update is called once per frame
    void Update()
    {
        // Doing raycast from hand //
        Physics.Raycast(realHandCentre.position, realHandCentre.transform.forward * 100, out target, 100, ~(1 << 2));
        Debug.DrawRay(realHandCentre.transform.position, realHandCentre.transform.forward * 100, Color.blue);

        // Doing raycast from screen //
        Physics.Raycast(gameObject.transform.position, gameObject.transform.forward * 5, out raycastFromScreen, 5);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 5, Color.white);

        // Doing sphere check //
        collisions = Physics.OverlapSphere(collisionSphere.position, handCollisionRadius);

        CameraState(); // This is the old camera state swapping thing.


        //SelectObj();
        
        DisplayPickupUI();
        DisplayApplianceIU();

        NewSelectObj();

        UpdatePlayerState();
        InputState();
        //if (Input.GetKeyDown(KeyCode.E) && !isHoldingItem && selectedItem)
        //{
        //    PickUpItem(selectedItem);
        //}
        //else if (Input.GetKeyDown(KeyCode.C) && !isHoldingItem && selectedItem)
        //{
        //    Debug.Log("Cutting ingredient.");
        //}
        //else if (Input.GetKeyDown(KeyCode.E) && isHoldingItem)
        //{
        //    DropItem();
        //}
        //else if (Input.GetKeyDown(KeyCode.F) && isHoldingItem)
        //{
        //    ThrowItem();
        //}
        Debug.Log(currentPlayerState.ToString());

        if (!isCentered)
        {
            CentreCamera(heldItemOriginalPos);
        }
 
       
    }

    void InputState()
    {
        switch (currentPlayerState)
        {
            case PlayerState.LOOKING_AT_ITEM:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(selectedItem);
                    heldItemOriginalPos = heldItem.position;
                    previousHandPos = hand.position;
                    previousHandMovementDir = handMovement;
                    Vector3 test = heldItemOriginalPos - gameObject.transform.position;
                    if (Vector3.Dot(test.normalized, gameObject.transform.forward) <= cameraCenteringDeadZone)
                    { 
                        isCentered = false;
                    }
                    Debug.Log("INTIAL DOT = " + Vector3.Dot(-test, gameObject.transform.forward));
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    Debug.Log("Cutting ingredient.");
                }
                break;

            case PlayerState.HOLDING_ITEM:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    DropItem();
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    ThrowItem();
                }
                break;

            case PlayerState.LOOKING_AT_SWITCH:
                // Getting the type of switch the players is looking at. //
                selectedSwitchType = selectedSwitch.GetComponent<SwitchData>().type;
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log(selectedSwitchType + " pressed");
                }
                break;
            case PlayerState.LOOKING_AT_NOTHING:
                break;
            case PlayerState.LOOKING_AT_APPLIANCE:
                break;

        }
    }

    void UpdatePlayerState()
    {
        if (!isHoldingItem && selectedItem)
        {
            currentPlayerState = PlayerState.LOOKING_AT_ITEM;
        }
        else if (isHoldingItem)
        {
            currentPlayerState = PlayerState.HOLDING_ITEM;
        }
        else if (selectedSwitch)
        {
            currentPlayerState = PlayerState.LOOKING_AT_SWITCH;
        }
        else if (!isHoldingItem && !selectedItem)
        {
            currentPlayerState = PlayerState.LOOKING_AT_NOTHING;
        }
    }

    void CameraState()
    {
        switch (currentCameraMode)
        {
            case CameraMode.lookMode:
                CameraLook();
                if (Input.GetMouseButton(1))
                {
                    currentCameraMode = CameraMode.handMode;
                }
                break;
            case CameraMode.handMode:

                if (Input.GetMouseButtonUp(1))
                {
                    currentCameraMode = CameraMode.lookMode;
                }
                break;
            case CameraMode.pauseMode:
                CameraPause();
                Cursor.lockState = CursorLockMode.None;
                
                break;
        }
    }

    
    void CentreCamera(Vector3 targetPos)
    {
        isCentered = false;

        // PlayerBody rotation //
        Quaternion playerXRotationDirection = Quaternion.LookRotation(new Vector3((targetPos.x - gameObject.transform.position.x), 0, (targetPos.z - gameObject.transform.position.z)));
        playerBody.rotation = Quaternion.Lerp(playerBody.rotation, playerXRotationDirection, Time.deltaTime * roationLerpSpeed);

        // Camera up and down rotation //
        Vector3 direction = gameObject.transform.position - targetPos;
        Vector2 direction2D = new Vector2(Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z), direction.y);
        float verticalRotation = Mathf.Atan2(direction2D.y, direction2D.x);
        

        

        Quaternion rotationThing = Quaternion.Euler(new Vector3(verticalRotation * (180/3.14159f), 0, 0));
        gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, rotationThing, Time.deltaTime * roationLerpSpeed);



        // Keeping hand in place //
        hand.position = previousHandPos;

        // Breaking out of centering mechanic. //

        float dot = Vector3.Dot(-direction.normalized, gameObject.transform.forward);
        if (dot < 1 && dot > 0.99)
        {
            isCentered = true;
            Debug.Log("Centre finished");
        }

        Debug.Log(dot);
    }
    void CameraLook()
    {
        // Checking if the player is trying to move the mouse. If they are, cancel any camera centering going on. //
        if (previousHandMovementDir != handMovement)
        {
            isCentered = true;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
        //xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //posX += mouseX;
        //posY += mouseY;

        //crosshairImage.transform.localPosition = Mathf.Clamp(posX, -xDeadZone, xDeadZone);
        //crosshairImage.transform.localPosition = Mathf.Clamp(posY, -yDeadZone, yDeadZone);
        //
        //crosshairImage.transform.localPosition = new Vector3(posX, posY, 0);
        //Vector3 crossHairMovement = transform.right * mouseX + transform.up * mouseY;
        crosshairImage.transform.position = gameObject.GetComponent<Camera>().WorldToScreenPoint(target.point);

        handMovement = transform.right * mouseX + transform.up * mouseY;
        hand.transform.position += handMovement * Time.deltaTime;
        handPos = hand.transform.localPosition;

        handPos.z = Mathf.Clamp(handPos.z, handZDistance, handZDistance);
        handPos.y = Mathf.Clamp(handPos.y, handYFloorLimit, handYCeilingLimit);
        handPos.x = Mathf.Clamp(handPos.x, handXLeftLimit, handXRightLimit);


        hand.transform.localPosition = handPos;

        if (handPos.x == handXLeftLimit || handPos.x == handXRightLimit || handPos.y == handYCeilingLimit || handPos.y == handYFloorLimit)
        {
            Debug.Log("Hit X Limit");
        
            float rotMouseX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
            float rotMouseY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;
        
            xRotation -= rotMouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            gameObject.transform.Rotate(-Vector3.right * rotMouseY);
            playerBody.Rotate(Vector3.up * rotMouseX);
        }      
    }

    void CameraHandControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * handControlSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * handControlSensitivity * Time.deltaTime;

        Vector3 handMovement = transform.right * mouseX + transform.up * mouseY;


        hand.transform.position += handMovement * Time.deltaTime;

        handPos = hand.transform.localPosition;
        
        handPos.z = Mathf.Clamp(handPos.z, handZDistance, handZDistance);
        handPos.y = Mathf.Clamp(handPos.y, handYFloorLimit, handYCeilingLimit);
        handPos.x = Mathf.Clamp(handPos.x, handXLeftLimit, handXRightLimit);


        hand.transform.localPosition = handPos;

      
    }

    void CameraPause()
    { 
        
    }

    void SetSelectedItem(Transform itemToSelect)
    {
        selectedItem = itemToSelect;
        defaultMat = selectedItem.GetComponent<Renderer>().material;
        selectedItem.GetComponent<Renderer>().material = itemSelectedMat;
    }

    void RemoveSelectedItem()
    {
        selectedItem.GetComponent<Renderer>().material = defaultMat;
        selectedItem = null;
    }

    void SelectObj()
    {
        
        // Item selection stuff //
        // -------------------------------------------------------- //   
        if (IsLookingAtItem())
        {
            selectedItem = raycastFromHand.transform;

            defaultMat = selectedItem.GetComponent<Renderer>().material;
            selectedItem.GetComponent<Renderer>().material = itemSelectedMat;
           
        }

        // If the player looks away from the item //

        else if (raycastFromHand.transform != selectedItem || raycastFromHand.transform == heldItem)
        {
            if (selectedItem != null)
            {
                selectedItem.GetComponent<Renderer>().material = defaultMat;
            }
            selectedItem = null;
        }
        // -------------------------------------------------------- //  

        // Appliance selection stuff //
        // -------------------------------------------------------- // 
        if (IsLookingAtAppliance())
        {
            selectedAppliance = raycastFromScreen.transform;
        }

        else if (raycastFromScreen.transform != selectedAppliance)
        {
            selectedAppliance = null;
        }


    }

    void NewSelectObj()
    {

        if (NewIsLookingAtItem() && !isHoldingItem)
        {
            selectedItem = NewIsLookingAtItem();

            // This if statement is a cheap fix for a badly written overall system... FIX IT ONE DAY //
            if (!defaultMat)
            {
                defaultMat = selectedItem.GetComponent<Renderer>().material;
            }
            selectedItem.GetComponent<Renderer>().material = itemSelectedMat;
        }

        else if (IsLookingAtSwitch())
        {
            selectedSwitch = IsLookingAtSwitch();
        
            if (!switchDefaultMat)
            {
                switchDefaultMat = selectedSwitch.GetComponent<Renderer>().material;
            }
            selectedSwitch.GetComponent<Renderer>().material = switchSelectedMat;
        }

        else
        {
            if (selectedItem != null)
            {
                selectedItem.GetComponent<Renderer>().material = defaultMat;
            }
            selectedItem = null;

            if (selectedSwitch != null)
            {
                selectedSwitch.GetComponent<Renderer>().material = switchDefaultMat;
            }
            selectedSwitch = null;
        }

    }

    Transform NewIsLookingAtItem()
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].gameObject.tag == "Item" || collisions[i].gameObject.tag == "Ingredient" && collisions[i].gameObject)
            {
                return collisions[i].transform;

            }
        }

        return null;
    }

   
    Transform IsLookingAtSwitch()
    {
        if (target.transform.tag == "Switch")
        {
            return target.transform;
        }

        return null;
    }

    bool IsLookingAtItem()
    {
        if (raycastFromHand.transform != null && raycastFromHand.transform.CompareTag("Item") || raycastFromHand.transform.CompareTag("Ingredient") && selectedItem != raycastFromHand.transform && !isHoldingItem)
        {
            return true;
        }
        return false;
    }

    void PickUpItem(Transform itemToPickUp)
    {
        isHoldingItem = true;
        heldItem = itemToPickUp;
        itemToPickUp.SetParent(hand);
        itemToPickUp.localPosition = new Vector3(heldItemPosX, heldItemPosY, heldItemPosZ);

        itemToPickUp.GetComponent<Rigidbody>().useGravity = false;
        itemToPickUp.GetComponent<Rigidbody>().isKinematic = true;

      
    }
    void DropItem()
    {
        isHoldingItem = false;

        heldItem.GetComponent<Rigidbody>().useGravity = true;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;

        heldItem.parent = null;
        heldItem = null;
    }

    void ThrowItem()
    {
        
        Vector3 throwDirection;
        
        if (Physics.Raycast(gameObject.transform.position, gameObject.GetComponent<Camera>().transform.forward * 100, 100, ~(1 << 2)))
        { 
            throwDirection = (target.point - heldItem.position).normalized;
            heldItem.GetComponent<Rigidbody>().useGravity = false;
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.GetComponent<Rigidbody>().AddForce(throwDirection * tempThrowForce, ForceMode.Impulse);



            isHoldingItem = false;
            heldItem.parent = null;
            heldItem = null;

            Debug.Log("throw activated");
        }    
    }

    
    void DisplayPickupUI()
    {
        if (selectedItem)
        {
            if (!isHoldingItem)
            {
                PickUpUI.gameObject.SetActive(true);
            }
            else
            {
                PickUpUI.gameObject.SetActive(false);
            }

            Vector3 UIPos = selectedItem.position;
            UIPos.y += PickUpUIYPos;
            PickUpUI.transform.position = UIPos;
            PickUpUI.transform.LookAt(gameObject.transform);

        }
        else
        {
            PickUpUI.gameObject.SetActive(false);
        }
    }

    bool IsLookingAtAppliance()
    {
        if (raycastFromScreen.transform != null && raycastFromScreen.transform.CompareTag("Appliance"))
        {
            return true;
        }
        return false;
    }

    void DisplayApplianceIU()
    {
        Vector3 applianceUIPos;
        if (insertText == null && notHoldingText == null)
        {
            notHoldingText = ApplianceUI.transform.Find("notHoldingText");
            insertText = ApplianceUI.transform.Find("insertText");
        }
        if (selectedAppliance)
        {
            
            if (selectedAppliance.parent != null)
            {
                applianceUIPos = selectedAppliance.parent.position;
            }
            else
            {
                applianceUIPos = selectedAppliance.position;
            }

            // Adjusting the position based on the inspector values //
            applianceUIPos.y += ApplianceUIYPos;
            applianceUIPos.z += ApplianceUIZPos;


            // Applying new position and constantly making the UI face the player. //
            ApplianceUI.transform.position = applianceUIPos;
            ApplianceUI.transform.LookAt(gameObject.transform);



            if (IsLookingAtAppliance() && !isHoldingItem)
            {
                notHoldingText.gameObject.SetActive(true);
                insertText.gameObject.SetActive(false);
            }
            else if (IsLookingAtAppliance() && isHoldingItem)
            {
                insertText.gameObject.SetActive(true);
                notHoldingText.gameObject.SetActive(false);
                insertText.GetComponent<TextMeshProUGUI>().text = "INSERT " + heldItem.name + " [E]";
            }
            else
            {
                notHoldingText.gameObject.SetActive(false);
                insertText.gameObject.SetActive(false);
            }
        }
        else 
        {
            notHoldingText.gameObject.SetActive(false);
            insertText.gameObject.SetActive(false);
        }
    }

    // Appliance interactions //

     
    SwitchType GetInteractableSwitch(RaycastHit target)
    {
        if (target.transform.tag == "BLENDER_BUTTON_1")
        {
            return SwitchType.BLENDER_BUTTON_1;
        }
        else if (target.transform.tag == "BLENDER_BUTTON_2")
        {
            return SwitchType.BLENDER_BUTTON_2;
        }

        return SwitchType.ERROR;
    }

    void CutterSwitch1()
    {
        Debug.Log("Cutter switch 1 activated.");
    }
    void CutterSwitch2()
    {
        Debug.Log("Cutter switch 2 activated.");
    }
    void WaterTapSwitch()
    {
        Debug.Log("Water tap switch activated.");
    }

    void InsertItem()
    { }
    void RemoveItem()
    { }

    void ActivateAppliance()
    { }

}
