using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShifting : MonoBehaviour
{
    [SerializeField] float durationToShiftCamera = 0.8f;
    [SerializeField] float shiftingSpace = 0.5f;
    [SerializeField] float lerpTime = 0.5f;

    CinemachineFramingTransposer framingTransposer;
    PlayerInputHandler playerInput;
    float shiftingTimer;
    float targetYPositionShift;
    bool hasShifted;

    // Start is called before the first frame update
    void Start()
    {
        framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        playerInput = GameManager.Instance.GetPlayer().GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.move.y > 0.1f) {

            if (shiftingTimer == 0) {
                shiftingTimer = Time.time;
            }

            float holdTime = Time.time - shiftingTimer;

            if (holdTime >= durationToShiftCamera && !hasShifted) {
                targetYPositionShift = framingTransposer.m_ScreenY + shiftingSpace;
                hasShifted = true;
            }

        } else if (playerInput.move.y < -0.1f) {

            if (shiftingTimer == 0)
                shiftingTimer = Time.time;

            float holdTime = Time.time - shiftingTimer;

            if (holdTime >= durationToShiftCamera && !hasShifted) {
                targetYPositionShift = framingTransposer.m_ScreenY - shiftingSpace;
                hasShifted = true;
            }

        } else {
            shiftingTimer = 0;
            targetYPositionShift = 0.5f;
            hasShifted = false;
        }

        framingTransposer.m_ScreenY = Mathf.Lerp(framingTransposer.m_ScreenY, targetYPositionShift, Time.deltaTime * lerpTime);
    }
}
