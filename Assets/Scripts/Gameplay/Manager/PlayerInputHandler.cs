using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public bool SmoothInput = true;
    public float InputSmoothTime = 0.2f;
    public float LookSensitivity = 1;
    public float WebglLookSensitivityMultiplier = 0.25f;
    public bool InvertX;
    public bool InvertY;

    GameControls controls;
    Vector2 smoothInput;
    Vector2 inputVelocity;

    public Vector2 move { get; set; }
    public Vector2 look { get; set; }
    public bool jump { get; set; }
    public bool jumpHeld { get; set; }
    public bool sprint { get; set; }
    public bool crouch { get; set; }
    public bool shootTap { get; set; }
    public bool shootHold { get; set; }
    public bool aim { get; set; }
    public bool reload { get; set; }
    public bool inspect { get; set; }
    public float weaponSwitch { get; set; }

    public bool CanUseInput => !GameManager.Instance.GameIsTransitioning;

    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void Update()
    {
        Look();
        Input();

        jump = CanUseInput && controls.Gameplay.Jump.WasPerformedThisFrame();
        jumpHeld = CanUseInput && controls.Gameplay.Jump.IsPressed();
        sprint = CanUseInput && controls.Gameplay.Sprint.IsPressed();
        crouch = CanUseInput && controls.Gameplay.Crouch.WasPerformedThisFrame();
        shootTap = CanUseInput && controls.Gameplay.ShootTap.WasPerformedThisFrame();
        shootHold = CanUseInput && controls.Gameplay.ShootHold.IsPressed();
        aim = CanUseInput && controls.Gameplay.Aim.IsPressed();
        reload = CanUseInput && controls.Gameplay.Reload.WasPerformedThisFrame();
        inspect = CanUseInput && controls.Gameplay.Inspect.WasPerformedThisFrame();
        weaponSwitch = CanUseInput ? -controls.Gameplay.WeaponSwitch.ReadValue<float>() : 0;
    }

    void Look() {
        if (CanUseInput) {
            look = controls.Gameplay.Look.ReadValue<Vector2>();

            Vector2 l = look;

            if (InvertX) l.x *= -1;
            if (InvertY) l.y *= -1;

            look = l;

            look *= LookSensitivity;

            var gamepad = Gamepad.current;

            if (gamepad != null) {
                look *= Time.deltaTime;
            } else {
                look *= 0.01f;

#if UNITY_WEBGL
                look *= WebglLookSensitivityMultiplier;
#endif
            }
        } else {
            look = Vector2.zero;
        }
    }

    void Input() {
        if (CanUseInput) {
            // Value is already normalized so no need to use ClampMagnitude()
            move = controls.Gameplay.Move.ReadValue<Vector2>();

            if (SmoothInput) {
                smoothInput = Vector2.SmoothDamp(smoothInput, move, ref inputVelocity, InputSmoothTime);
                move = smoothInput;
            }
        }
    }

    public void ResetMove() {
        move = Vector2.zero;
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
