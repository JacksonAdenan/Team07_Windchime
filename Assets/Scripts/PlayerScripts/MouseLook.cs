
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum CameraMode
{ 
    FPS_CONTROL,
    HAND_CONTROL,
    NEWMODE,
    pauseMode
}

public enum PlayerState
{ 
    LOOKING_AT_NOTHING,
    HOLDING_ITEM,
    HOLDING_WATER,
    LOOKING_AT_ITEM,
    LOOKING_AT_APPLIANCE,
    LOOKING_AT_SWITCH,
    ERROR

}

public enum SwitchType
{ 
    CUTTER_SWITCH_1,
    CUTTER_SWITCH_2,
    WATER_TAP,
    CANON_BUTTON,
    ORDER_ACCEPT,
    ORDER_REJECT,
    BLENDER_BUTTON,

    ERROR
}
public class MouseLook : MonoBehaviour
{
    // Constants //
    const float INTERACT_DISTANCE = 2;


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
    //public Material waterSelectedMat;

    public Vector3 handFPSPos;
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
    public CameraMode currentCameraMode = CameraMode.HAND_CONTROL;
    public static Transform selectedItem;
    public static Transform selectedWater;
    public static Transform selectedAppliance = null;
    public static Transform heldItem = null;
    public static Transform heldWater = null;
    public Transform selectedSwitch = null;
    public Material defaultMat;
    //private Material defaultWaterMat;
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


    // Hand swaying stuff. //
    float handVelocity = 0;
    float handAcceleration = 0;
    float accelerationTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


        
    }

    // Update is called once per frame
    void Update()
    {
        //// Doing raycast from hand //
        //Physics.Raycast(realHandCentre.position, realHandCentre.transform.forward * 100, out target, 100, ~(1 << 2));
        //Debug.DrawRay(realHandCentre.transform.position, realHandCentre.transform.forward * 100, Color.blue);
        //
        //// Doing raycast from screen //
        //Physics.Raycast(gameObject.transform.position, gameObject.transform.forward * 5, out raycastFromScreen, 5);
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 5, Color.white);

        // Raycast code is now controlled by this. //
        CalculateTarget();

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



        // Acceleration timer counting. //
        accelerationTimer += Time.deltaTime;

        

           
        
 
       
    }

    void InputState()
    {
        switch (currentPlayerState)
        {
            case PlayerState.LOOKING_AT_ITEM:
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (selectedItem.tag != "Soup")
                    {
                        // If the thing they want to pick up is a water. //
                        // Freeing up the WaterTap so the player can get more water if they want.
                        if (selectedItem.tag == "Water")
                        {
                            CookingManager.currentWaterTapState = WaterTapState.EMPTY;
                            Debug.Log("Water tap is now unoccupied.");
                        }

                        PickUpItem(selectedItem);
                    }
                    
                    else if (selectedItem.tag == "Soup")
                    {
                        PickUpSoup(selectedItem);
                        Debug.Log("PICKED UP SOUP");
                    }

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
                    ActivateSwitch(selectedSwitchType);
                }
                break;
            case PlayerState.LOOKING_AT_NOTHING:
                break;
            case PlayerState.LOOKING_AT_APPLIANCE:
                if (selectedAppliance.parent && selectedAppliance.parent.GetComponent<ApplianceData>().applianceType == ApplianceType.COOKING_ORB)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (isHoldingItem && heldItem.tag == "Water")
                        {
                            RemoveItem();
                            CookingManager.AddWater();
                        }
                        else if (CookingManager.currentCookingOrbState == CookingOrbState.INGREDIENTS_AND_WATER)
                        {
                            CookingManager.MakeSoup();
                        }
                    }
                }
                else if (selectedAppliance.parent && selectedAppliance.parent.GetComponent<ApplianceData>().applianceType == ApplianceType.CATCHER)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (isHoldingItem && heldItem.tag == "Capsule" && !CookingManager.hasCapsule)
                        {
                            
                            RemoveItem();
                            CookingManager.AttachCapsule();
                        }
                        else if (CookingManager.hasCapsule && !isHoldingItem)
                        {
                            Debug.Log("REMOVED CAPSULE FROM CATCHER");
                            if (CookingManager.currentCatcherState == CatcherState.FULL_CAPSULE)
                            {
                                Detach(CookingManager.filledAttachedCapsule);
                            }
                            else
                            {
                                Detach(CookingManager.emptyAttachedCapsule);
                            }


                            // REMEMBER TO RUN REMOVE CAPSULE FUNCTION TO CLEAR THE CATCHER. //
                            CookingManager.RemoveCapsule();
                        }
                    }
                }
                else if (selectedAppliance.parent && selectedAppliance.parent.GetComponent<ApplianceData>().applianceType == ApplianceType.BLENDER)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (isHoldingItem && heldItem.tag == "InteractableBlenderCover" && CookingManager.currentBlenderState == BlenderState.NOT_COVERED)
                        {
                            RemoveItem();
                            CookingManager.AttachBlenderCover();
                        }
                        else if (CookingManager.currentBlenderState == BlenderState.COVERED && !isHoldingItem)
                        {
                            Debug.Log("REMOVED BLENDER COVER");
                            DetachBlenderCover(CookingManager.blenderCover);
      
                            // REMEMBER TO RUN REMOVE BLENDER COVER FUNCTION TO SET THE APPROPRIATE VALUE ON THE BLENDER. //
                            CookingManager.RemoveBlenderCover();
                        }
                    }
                }
                else if (selectedAppliance.parent && selectedAppliance.parent.GetComponent<ApplianceData>().applianceType == ApplianceType.CANON)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (isHoldingItem && heldItem.tag == "Capsule" && !CookingManager.isLoaded)
                        {
                            CookingManager.LoadCanon(heldItem.GetComponent<SoupData>().theSoup);
                            RemoveItem();
                        }
                        else if (CookingManager.isLoaded)
                        {
                            Detach(CookingManager.canonCapsule);
                            CookingManager.UnloadCanon();
                            //Debug.Log("Tried to unload the canon but that feature doesn't exist yet.");
                        }
                    }
                }
                break;

        }
    }

    void UpdatePlayerState()
    {
        if (!isHoldingItem && selectedItem)
        {
            currentPlayerState = PlayerState.LOOKING_AT_ITEM;
            //hand.transform.GetChild(0).GetComponent<Animator>().
            hand.transform.GetChild(0).GetComponent<Animator>().SetBool("IsPointing", true);
        }
        else if (selectedAppliance)
        {
            currentPlayerState = PlayerState.LOOKING_AT_APPLIANCE;
        }
        else if (isHoldingItem)
        {
            currentPlayerState = PlayerState.HOLDING_ITEM;
            hand.transform.GetChild(0).GetComponent<Animator>().SetBool("IsGrabbing", true);
            hand.transform.GetChild(0).GetComponent<Animator>().SetBool("IsPointing", false);

        }
        else if (selectedSwitch)
        {
            currentPlayerState = PlayerState.LOOKING_AT_SWITCH;
            hand.transform.GetChild(0).GetComponent<Animator>().SetBool("IsPointing", true);
        }
        else if (!isHoldingItem && !selectedItem)
        {
            currentPlayerState = PlayerState.LOOKING_AT_NOTHING;
            hand.transform.GetChild(0).GetComponent<Animator>().SetBool("IsPointing", false);
            hand.transform.GetChild(0).GetComponent<Animator>().SetBool("IsGrabbing", false);
        }
        else
        {
            currentPlayerState = PlayerState.ERROR;
        }
    }

    void CameraState()
    {
        switch (currentCameraMode)
        {
            case CameraMode.HAND_CONTROL:
                CameraLook();  
                if (Input.GetMouseButtonDown(1))
                {
                    currentCameraMode = CameraMode.FPS_CONTROL;

                    // Setting the hand to the correct position. //
                    hand.localPosition = handFPSPos;
                }
                break;
            case CameraMode.FPS_CONTROL:
                CameraLookFPS();
                if (Input.GetMouseButtonDown(1))
                {
                    currentCameraMode = CameraMode.HAND_CONTROL;
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

        // Old crosshair placement code. I'm gonna try implement a new way which will be smoother and wont break if the player isn't looking at anything.
        crosshairImage.transform.position = gameObject.GetComponent<Camera>().WorldToScreenPoint(target.point);

        //Vector3 crosshairDir = realHandCentre.position + realHandCentre.forward * (5);
        //Debug.DrawLine(realHandCentre.position, crosshairDir, Color.red, 1);
        //crosshairImage.transform.position = gameObject.GetComponent<Camera>().WorldToScreenPoint(crosshairDir);

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

    void CameraLookFPS()
    {
        // Old crosshair placement code. I'm gonna try implement a new way which will be smoother and wont break if the player isn't looking at anything.
        crosshairImage.transform.position = gameObject.GetComponent<Camera>().WorldToScreenPoint(target.point);

        float mouseX = Input.GetAxis("Mouse X") * handControlSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * handControlSensitivity * Time.deltaTime;


        float rotMouseX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
        float rotMouseY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;

        

        xRotation -= rotMouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        gameObject.transform.Rotate(-Vector3.right * rotMouseY);
        playerBody.Rotate(Vector3.up * rotMouseX);


        // Hand swaying in the direction of the camera turn. //

        

        handAcceleration = mouseX;
        handAcceleration = Mathf.Clamp(handAcceleration, -0.05f, 0.05f);
        handVelocity += (handAcceleration) * Time.deltaTime;
        
        
        
   

        if (handAcceleration != 0)
        {
            float newHandX = hand.localPosition.x + handVelocity;
            newHandX = Mathf.Clamp(newHandX, handFPSPos.x - 0.3f, handFPSPos.x + 0.3f);
            hand.localPosition = new Vector3(newHandX, hand.localPosition.y, hand.localPosition.z);
        }
        else
        {
            hand.localPosition = new Vector3(Mathf.Lerp(hand.localPosition.x, handFPSPos.x, 0.05f), hand.localPosition.y, hand.localPosition.z);
            accelerationTimer = 0;
            handVelocity = 0;
   
        }

    }

    void ReduceAcceleration(ref float acceleration)
    {
        
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

    void NewSelectObj()
    {

        // Resetting ingredients/items/water //
        if (NewIsLookingAtItem() == null)
        {

            if (selectedItem != null)
            {
                selectedItem.GetComponent<Renderer>().material = defaultMat;
                defaultMat = null;
            }
            selectedItem = null;
        }

        // This is to prevent the bug where if the player is selecting something and then instantly selects something else without having a period inbetween of not selecting something, the default material doesn't 
        // update properly.
        if (selectedItem != NewIsLookingAtItem())
        {
            if (selectedItem != null)
            {
                selectedItem.GetComponent<Renderer>().material = defaultMat;
                defaultMat = null;
            }
        }

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

            // this if statement protects bug if switch doesn't have a renderer. temporary because I don't how to put renderers on UI switches. //
            if (selectedSwitch.GetComponent<Renderer>() != null)
            { 
                if (!switchDefaultMat)
                {
                    switchDefaultMat = selectedSwitch.GetComponent<Renderer>().material;
                }
                selectedSwitch.GetComponent<Renderer>().material = switchSelectedMat;
            }
        }


        // Appliance selection stuff //
        // -------------------------------------------------------- // 
        else if (IsLookingAtAppliance())
        {
            selectedAppliance = IsLookingAtAppliance();
        }

        // Resetting switches //
        if (IsLookingAtSwitch() == null)
        { 
            if (selectedSwitch != null)
            {
                // This if statement protects bug if the switch doesn't have a renderer. only temporary hopefully while we have UI switches. //
                if (selectedSwitch.GetComponent<Renderer>() != null)
                { 
                    selectedSwitch.GetComponent<Renderer>().material = switchDefaultMat;
                    switchDefaultMat = null;
                }
            }
            selectedSwitch = null;
        }

        if (IsLookingAtAppliance() == null)
        { 
            selectedAppliance = null;
        }
        

    }

    Transform NewIsLookingAtItem()
    {
        //for (int i = 0; i < collisions.Length; i++)
        //{
        //    if (collisions[i].gameObject.tag == "Item" || collisions[i].gameObject.tag == "Ingredient" || collisions[i].gameObject.tag == "Water" || collisions[i].gameObject.tag == "Soup" || collisions[i].gameObject.tag == "SoupPortion" || collisions[i].gameObject.tag == "Capsule" || collisions[i].gameObject.tag == "InteractableBlenderCover")
        //    {
        //       
        //        return collisions[i].transform;
        //
        //    }
        //}
        //
        //return null;

        if ((target.transform.tag == "Item" || target.transform.tag == "Ingredient" || target.transform.tag == "Water" || target.transform.tag == "Soup" || target.transform.tag == "SoupPortion" || target.transform.tag == "Capsule" || target.transform.tag == "InteractableBlenderCover") && (gameObject.transform.position - target.transform.position).magnitude < INTERACT_DISTANCE)
        {
            if (target.transform.childCount > 0)
            {
                return target.transform.GetChild(0);
            }
            else
            {
                return target.transform;
            }

        }

        return null;
    }

   
    Transform IsLookingAtSwitch()
    {
        if (target.transform.tag == "Switch" && (gameObject.transform.position - target.transform.position).magnitude < INTERACT_DISTANCE)
        {
            return target.transform;
        }

        return null;
    }
    Transform IsLookingAtAppliance()
    {
        if (target.transform.tag == "Appliance" && (gameObject.transform.position - target.transform.position).magnitude < INTERACT_DISTANCE)
        {
            return target.transform;
        }

        return null;
    }
    void PickUpItem(Transform itemToPickUp)
    {
        // Stopping the selection if your holding an item //
        itemToPickUp.GetComponent<Renderer>().material = defaultMat;
        selectedItem = null;
        defaultMat = null;

        isHoldingItem = true;
        if (itemToPickUp.parent != null)
        {
          
            heldItem = itemToPickUp.parent;
            heldItem.SetParent(hand);
            heldItem.localPosition = new Vector3(heldItemPosX, heldItemPosY, heldItemPosZ);


            // this is the parent it doesnt have these things !. So i have to get the children.//
            heldItem.GetComponent<Rigidbody>().useGravity = false;
            heldItem.GetComponent<Rigidbody>().isKinematic = true;          
        }
        else
        { 
            heldItem = itemToPickUp;
            itemToPickUp.SetParent(hand);
            itemToPickUp.localPosition = new Vector3(heldItemPosX, heldItemPosY, heldItemPosZ);

            itemToPickUp.GetComponent<Rigidbody>().useGravity = false;
            itemToPickUp.GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }
    void PickUpSoup(Transform itemToPickUp)
    {
        // Reducing the current portions in the soup. //
        itemToPickUp.GetComponent<SoupData>().currentPortions -= 1;

        Transform soupPortion = Instantiate(itemToPickUp, itemToPickUp.position, itemToPickUp.rotation);
        soupPortion.localScale = soupPortion.localScale / 2;

        // This is really iffy but its so that when you pick up soup the material is the default rather than the selected material.
        soupPortion.GetComponent<Renderer>().material = defaultMat;
        // Another fix to stop the pink material glitch after picking up soup.
        itemToPickUp.GetComponent<Renderer>().material = defaultMat;
        selectedItem = null;
        defaultMat = null;

        soupPortion.tag = "SoupPortion";

        isHoldingItem = true;
        heldItem = soupPortion;
        soupPortion.SetParent(hand);
        soupPortion.localPosition = new Vector3(heldItemPosX, heldItemPosY, heldItemPosZ);

        soupPortion.GetComponent<Rigidbody>().useGravity = false;
        soupPortion.GetComponent<Rigidbody>().isKinematic = true;

        // Manually setting held items soup data because instantiate isn't working i think. //
        //heldItem.GetComponent<SoupData>().theSoup = itemToPickUp.GetComponent<SoupData>().theSoup;


        if (heldItem.GetComponent<SoupData>().theSoup == null)
        {
            Debug.Log("THE OSUP WAS NULL ALL ALONG!!");
        }
        else
        {
            Debug.Log("THE SOUP DOES WORK SPICY : " + heldItem.GetComponent<SoupData>().theSoup.spicyValue);
        }
        Debug.Log("CREATED SOUP!");

    }

    void LoadCanon()
    { 
        
    }
    void Detach(Transform itemToPickUp)
    {
        Transform capsule = Instantiate(itemToPickUp, itemToPickUp.position, itemToPickUp.rotation);
        capsule.tag = "Capsule";

        // Giving the capsule appropriate soup data. //
        if (capsule.GetComponent<SoupData>() == null)
        {
            capsule.gameObject.AddComponent<SoupData>();
            SoupData soupData = capsule.GetComponent<SoupData>();

            // Just set current portions and max portions to 5. Doesn't really matter just yet. //
            soupData.currentPortions = 5;
            soupData.maxPortions = 5;


            // Can only set the type of soup if the catcher contains soup. //
            if (CookingManager.currentPortions.Count > 0)
            {
                Debug.Log("SET CAPSULES SOUP DATA");
                capsule.gameObject.GetComponent<SoupData>().theSoup = CookingManager.currentPortions[0];
            }
        }
        
        // Not only do we set the parent prefab to have a capsule tag, but also the children it has. // 
        for (int i = 0; i < capsule.childCount; i++)
        {
            capsule.GetChild(0).tag = "Capsule";
        }

        isHoldingItem = true;
        heldItem = capsule;
        capsule.SetParent(hand);
        capsule.localPosition = new Vector3(heldItemPosX, heldItemPosY, heldItemPosZ);

        capsule.GetComponent<Rigidbody>().useGravity = false;
        capsule.GetComponent<Rigidbody>().isKinematic = true;
    }

    void DetachBlenderCover(Transform itemToPickUp)
    {
        Transform blenderCover = Instantiate(itemToPickUp, itemToPickUp.position, itemToPickUp.rotation);
        blenderCover.tag = "InteractableBlenderCover";

        // Not only do we set the parent prefab to have a capsule tag, but also the children it has. // 
        for (int i = 0; i < blenderCover.childCount; i++)
        {
            blenderCover.GetChild(0).tag = "InteractableBlenderCover";
        }

        isHoldingItem = true;
        heldItem = blenderCover;
        blenderCover.SetParent(hand);
        blenderCover.localPosition = new Vector3(heldItemPosX, heldItemPosY, heldItemPosZ);

        blenderCover.GetComponent<Rigidbody>().useGravity = false;
        blenderCover.GetComponent<Rigidbody>().isKinematic = true;

        // TEMPORARY FIX ME //
        // Because the mesh on the blender isn't working properly, we can't put a mesh collider on it. This means we have to use a box collider and set it to be a trigger so that ingredients can
        // be inside of it.

        blenderCover.GetComponent<BoxCollider>().isTrigger = false;
    }
    void DropItem()
    {
        isHoldingItem = false;

        // Have to write exception code for capsules since they have children >:( //
        if (heldItem.tag == "Capsule")
        {
            //heldItem.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
            //heldItem.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;

            // ^ This is commented out for now because although the children have the mesh collider, the parent prefab still has the rigidbody. Hopefully works? //

            heldItem.GetComponent<Rigidbody>().useGravity = false;
            heldItem.GetComponent<Rigidbody>().isKinematic = false;

            heldItem.parent = null;
            heldItem = null;
        }
        else
        { 
            heldItem.GetComponent<Rigidbody>().useGravity = false;
            heldItem.GetComponent<Rigidbody>().isKinematic = false;

            heldItem.parent = null;
            heldItem = null;
        
        }
        
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
            return SwitchType.CUTTER_SWITCH_1;
        }
        else if (target.transform.tag == "BLENDER_BUTTON_2")
        {
            return SwitchType.CUTTER_SWITCH_2;
        }

        return SwitchType.ERROR;
    }

    

    void ActivateSwitch(SwitchType switchType)
    {
        switch (switchType)
        {
            case SwitchType.CUTTER_SWITCH_1:
                CookingManager.CutterSwitch1();
                break;
            case SwitchType.CUTTER_SWITCH_2:
                CookingManager.CutterSwitch2();
                break;
            case SwitchType.WATER_TAP:
                CookingManager.WaterTapSwitch();
                break;
            case SwitchType.ORDER_ACCEPT:
                OrderManager.AcceptOrder(OrderManager.requestedOrders[0]);
                break;
            case SwitchType.ORDER_REJECT:
                OrderManager.RejectOrder();
                break;
            case SwitchType.CANON_BUTTON:
                CookingManager.ShootCapsule();
                break;
            case SwitchType.BLENDER_BUTTON:
                CookingManager.ActivateBlender();
                break;
                
        }
    }

    void InsertItem()
    { }
    void RemoveItem()
    {
        isHoldingItem = false;

        heldItem.GetComponent<Rigidbody>().useGravity = false;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;

        heldItem.parent = null;
        heldItem.gameObject.SetActive(false);
        heldItem = null;
    }

    void ActivateAppliance()
    { }

    void CalculateTarget()
    {
        if (currentCameraMode == CameraMode.HAND_CONTROL)
        {
            Debug.Log("Raycast from hand");
            // Doing raycast from hand //
            Physics.Raycast(realHandCentre.position, realHandCentre.transform.forward * 100, out target, 100, ~(1 << 2));
            Debug.DrawRay(realHandCentre.transform.position, realHandCentre.transform.forward * 100, Color.blue);

            
        }
        else if (currentCameraMode == CameraMode.FPS_CONTROL)
        {
            Debug.Log("Raycast from screen.");
            // Doing raycast from screen //
            Physics.Raycast(gameObject.transform.position, gameObject.transform.forward * 100, out target, 100, ~(1 << 2));
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 100, Color.white);
        }
    }

}
