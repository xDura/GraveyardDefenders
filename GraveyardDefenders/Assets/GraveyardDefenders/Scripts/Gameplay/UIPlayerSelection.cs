using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using System;

namespace XD
{
    public class UIPlayerSelection : MonoBehaviour
    {
        public Sprite playerSprite;
        public Sprite noneSprite;
        public Sprite xSprite;
        public Sprite squareSprite;
        public Sprite keyabord0Sprite;
        public Sprite keyabord1Sprite;
        public Sprite keyabord2Sprite;
        public Sprite keyabord3Sprite;

        public List<Image> selectedPlayerImages = new List<Image>(Constants.maxPlayers);
        public List<Image> inputImages = new List<Image>(Constants.maxPlayers);

        public void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 0);
            }
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 1);
            }
            if(Keyboard.current.numpad0Key.wasReleasedThisFrame)
            {
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 2);
            }
            if(Keyboard.current.numpadPlusKey.wasReleasedThisFrame)
            {
                PlayerInput.Instance.AddPlayerAttempt(Keyboard.current, 3);
            }
            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                Gamepad currentPad = Gamepad.all[i];
                if(currentPad.xButton.wasReleasedThisFrame) PlayerInput.Instance.AddPlayerAttempt(currentPad, 0);
            }

            UpdateUI();
        }

        public void UpdateUI()
        {
            int gamepadCount = Gamepad.all.Count;
            int keyboardCount = Constants.maxPlayers - gamepadCount;

            for (int i = 0; i < gamepadCount; i++)
            {
                Gamepad current = Gamepad.all[i];
                if (PlayerInput.Instance.devices.Contains(Gamepad.all[i]))
                {
                    selectedPlayerImages[i].sprite = playerSprite;
                    inputImages[i].sprite = null;
                }
                else
                {
                    selectedPlayerImages[i].sprite = noneSprite;
                    inputImages[i].sprite = GetActionSprite(current, 0);
                }
            }

            for (int i = 0; i < keyboardCount; i++)
            {
                int realindex = gamepadCount + i;
                int keyboardIndex = i;

                bool hasPlayer = HasKeyboardPlayer(keyboardIndex);
                if (hasPlayer)
                {
                    selectedPlayerImages[realindex].sprite = playerSprite;
                    inputImages[realindex].sprite = null;
                }
                else
                {
                    selectedPlayerImages[realindex].sprite = noneSprite;
                    inputImages[realindex].sprite = GetActionSprite(Keyboard.current, keyboardIndex);
                    //Debug.Log("Draw as has to be added");
                }
            }
        }

        public bool HasKeyboardPlayer(int index)
        {
            for (int i = 0; i < PlayerInput.Instance.devices.Count; i++)
            {
                if (PlayerInput.Instance.devices[i] is Keyboard && PlayerInput.Instance.deviceIndices[i] == index) return true;
            }

            return false;
        }

        public Sprite GetActionSprite(InputDevice device, int index)
        {
            if(device is DualShockGamepad)
                return squareSprite;
            else if(device is XInputController)
                return xSprite;
            else if(device is Keyboard)
            {
                if (index == 0) return keyabord0Sprite;
                if (index == 1) return keyabord1Sprite;
                if (index == 2) return keyabord2Sprite;
                if (index == 3) return keyabord3Sprite;
            }

            return null;
        }
    }   
}
