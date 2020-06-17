using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace XD
{
    public static class PlayerInputExt
    {
        public static float GetMoveX(this Gamepad pad)
        {
            return pad.leftStick.ReadValue().x;
        }

        public static float GetMoveY(this Gamepad pad)
        {
            return pad.leftStick.ReadValue().y;
        }

        public static Vector2 GetMoveVec(this Gamepad pad)
        {
            Vector2 leftStickValue = pad.leftStick.ReadValue();
            if (leftStickValue.magnitude <= 0.1f) leftStickValue = Vector3.zero;
            return leftStickValue;
        }

        public static Vector2 GetMoveVec(this Keyboard keyboard, int index, Vector2 lastMoveVec)
        {
            Vector2 ret = Vector2.zero;
            switch (index)
            {
                case 0:
                    ret.x = GetAxisKeyboard(keyboard.aKey.isPressed, keyboard.dKey.isPressed, keyboard.aKey.wasPressedThisFrame, keyboard.dKey.wasPressedThisFrame, lastMoveVec.x);
                    ret.y = GetAxisKeyboard(keyboard.sKey.isPressed, keyboard.wKey.isPressed, keyboard.sKey.wasPressedThisFrame, keyboard.wKey.wasPressedThisFrame, lastMoveVec.y);
                    break;
                case 1:
                    ret.x = GetAxisKeyboard(keyboard.hKey.isPressed, keyboard.kKey.isPressed, keyboard.hKey.wasPressedThisFrame, keyboard.kKey.wasPressedThisFrame, lastMoveVec.x);
                    ret.y = GetAxisKeyboard(keyboard.jKey.isPressed, keyboard.uKey.isPressed, keyboard.jKey.wasPressedThisFrame, keyboard.uKey.wasPressedThisFrame, lastMoveVec.y);
                    break;
                case 2:
                    ret.x = GetAxisKeyboard(keyboard.leftArrowKey.isPressed, keyboard.rightArrowKey.isPressed, keyboard.leftArrowKey.wasPressedThisFrame, keyboard.rightArrowKey.wasPressedThisFrame, lastMoveVec.x);
                    ret.y = GetAxisKeyboard(keyboard.downArrowKey.isPressed, keyboard.upArrowKey.isPressed, keyboard.downArrowKey.wasPressedThisFrame, keyboard.upArrowKey.wasPressedThisFrame, lastMoveVec.y);
                    break;
                case 3:
                    ret.x = GetAxisKeyboard(keyboard.numpad4Key.isPressed, keyboard.numpad6Key.isPressed, keyboard.numpad4Key.wasPressedThisFrame, keyboard.numpad6Key.wasPressedThisFrame, lastMoveVec.x);
                    ret.y = GetAxisKeyboard(keyboard.numpad5Key.isPressed, keyboard.numpad8Key.isPressed, keyboard.numpad5Key.wasPressedThisFrame, keyboard.numpad8Key.wasPressedThisFrame, lastMoveVec.y);
                    break;
            }
            return ret;
        }

        public static void UpdateInteract(this Keyboard keyboard, int index, ref bool interactPressedThisFrame, ref bool interactReleasedThisFrame)
        {
            switch (index)
            {
                case 0:
                    interactPressedThisFrame = keyboard.fKey.wasPressedThisFrame;
                    interactReleasedThisFrame = keyboard.fKey.wasReleasedThisFrame;
                    break;
                case 1:
                    interactPressedThisFrame = keyboard.lKey.wasPressedThisFrame;
                    interactReleasedThisFrame = keyboard.lKey.wasReleasedThisFrame;
                    break;
                case 2:
                    interactPressedThisFrame = keyboard.numpad0Key.wasPressedThisFrame;
                    interactReleasedThisFrame = keyboard.numpad0Key.wasReleasedThisFrame;
                    break;
                case 3:
                    interactPressedThisFrame = keyboard.numpadPlusKey.wasPressedThisFrame;
                    interactReleasedThisFrame = keyboard.numpadPlusKey.wasReleasedThisFrame;
                    break;
            }
        }

        public static float GetAxisKeyboard(bool negativePressed, bool positivePressed, bool negativeThisFrame, bool positiveThisFrame, float lastAxisValue)
        {
            Vector2 ret = Vector2.zero;
            if (negativeThisFrame) return -1.0f;
            else if (positiveThisFrame) return 1.0f;
            else
            {
                if (negativePressed && positivePressed && lastAxisValue != 0.0f) return lastAxisValue;
                else if (negativePressed) return  -1.0f;
                else if (positivePressed) return 1.0f;
                else return 0.0f;
            }
        }
    }   
}
