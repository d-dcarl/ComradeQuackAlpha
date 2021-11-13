using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    private const int OVERHEAD_CAMERA = 0;
    private const int PLAYER_CAMERA = 1;

    public Camera playerCamera;
    public Camera overheadCamera;

    public GameObject playerCameraPosition;
    public GameObject overheadCameraPosition;

    private GameObject targetPosition;
    public float transitionTime = 0.25f;
    private Vector3 positionVelocity;
    private Quaternion rotationVelocity;

    // 0 for player, 1 for overhead
    private int currentCamera = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        currentCamera = PLAYER_CAMERA;
        overheadCamera.transform.SetPositionAndRotation(playerCameraPosition.transform.position, playerCameraPosition.transform.rotation);
        targetPosition = playerCameraPosition;
        GameManager.Instance.cameraTransitioning = false;
        GameManager.Instance.isOverheadView = false;
        playerCamera.enabled = true;
        overheadCamera.enabled = false;
        
        ShowPlayerView();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !GameManager.Instance.cameraTransitioning)
        {
            if (currentCamera == PLAYER_CAMERA)
            {
                ShowOverheadView();
            }
            else if (currentCamera == OVERHEAD_CAMERA)
            {
                ShowPlayerView();
            }
        }
        
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (GameManager.Instance.cameraTransitioning)
        {
            overheadCamera.transform.position = Vector3.SmoothDamp(overheadCamera.transform.position, targetPosition.transform.position, ref positionVelocity, transitionTime);
            overheadCamera.transform.rotation = QuaternionUtil.SmoothDamp(overheadCamera.transform.rotation, targetPosition.transform.rotation, ref rotationVelocity, transitionTime);
        
            if (Vector3.Distance(overheadCamera.transform.position, targetPosition.transform.position) < 0.05f)
            {
                GameManager.Instance.cameraTransitioning = false;
                
                if (currentCamera == PLAYER_CAMERA)
                {
                    playerCamera.enabled = true;
                    overheadCamera.enabled = false;
                    GameManager.Instance.isOverheadView = false;
                }
            }
        }
    }

    public void ShowPlayerView()
    {
        currentCamera = PLAYER_CAMERA;
        overheadCamera.transform.SetPositionAndRotation(overheadCameraPosition.transform.position, overheadCameraPosition.transform.rotation);
        targetPosition = playerCameraPosition;
        positionVelocity = Vector3.zero;
        rotationVelocity = Quaternion.identity;
        GameManager.Instance.cameraTransitioning = true;
    }
    
    public void ShowOverheadView()
    {
        currentCamera = OVERHEAD_CAMERA;
        overheadCamera.transform.SetPositionAndRotation(playerCameraPosition.transform.position, playerCameraPosition.transform.rotation);
        targetPosition = overheadCameraPosition;
        positionVelocity = Vector3.zero;
        rotationVelocity = Quaternion.identity;
        GameManager.Instance.cameraTransitioning = true;
        GameManager.Instance.isOverheadView = true;
        
        playerCamera.enabled = false;
        overheadCamera.enabled = true;
    }
}
