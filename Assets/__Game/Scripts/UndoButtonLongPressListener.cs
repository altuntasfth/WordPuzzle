using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace __Game.Scripts
{
    public class UndoButtonLongPressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        [Tooltip("Hold duration in seconds")]
        [Range(0.3f, 5f)] public float holdDuration = 0.5f;
        public event Action<bool> onLongPress;

        private bool isPointerDown = false;
        private bool isLongPressed = false;
        private DateTime pressTime;

        private Button button;

        private WaitForSeconds delay;


        private void Awake() {
            button = GetComponent<Button>();
            delay = new WaitForSeconds(0.1f);
        }

        public void OnPointerDown(PointerEventData eventData) {
            isPointerDown = true;
            pressTime = DateTime.Now;
            StartCoroutine(Timer());
        }


        public void OnPointerUp(PointerEventData eventData) {
            isPointerDown = false;
            isLongPressed = false;
        }

        private IEnumerator Timer() {
            while (isPointerDown && !isLongPressed) {
                double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

                if (elapsedSeconds >= holdDuration) {
                    isLongPressed = true;
                    if (button.interactable)
                        onLongPress?.Invoke(true);

                    yield break;
                }

                yield return delay;
            }
        }
    }

}