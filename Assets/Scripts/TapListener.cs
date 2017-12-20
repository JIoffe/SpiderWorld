using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.VR.WSA.Input;

namespace JI.Unity.SpiderWorld.UI
{
    public class TapListener : MonoBehaviour
    {
        public GameObject coinPrefab;
        public GameObject webshotPrefab;

        [Tooltip("The keycode to use to simulate taps within the Unity editor")]
        public KeyCode editorKeycode = KeyCode.Space;

        private GestureRecognizer gestureRecognizer;

        private int mode = 1;

        void Start()
        {
            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

            gestureRecognizer.TappedEvent += OnTappedEvent;

            gestureRecognizer.StartCapturingGestures();
        }

        void OnDestroy()
        {
            gestureRecognizer.StopCapturingGestures();
            gestureRecognizer.TappedEvent -= OnTappedEvent;
        }

        //In the Unity editor we need to have a workaround to simulate taps
#if UNITY_EDITOR
        void LateUpdate()
        {
            if (Input.GetKeyDown(editorKeycode))
            {
                OnTap();
            }
        }
#endif

        private void OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            OnTap();
        }

        private void OnTap()
        {
            var player = Camera.main.transform;

            if (mode == 1)
            {
                RaycastHit hitInfo;
                if (!Physics.Raycast(player.position, player.forward, out hitInfo, 50f, (1 << 8)))
                    return;

 
                if (coinPrefab == null)
                    return;

                Instantiate(coinPrefab, hitInfo.point, Quaternion.identity);
            }else if(mode == 2)
            {
                if (webshotPrefab == null)
                    return;

                var shot = Instantiate(webshotPrefab);
                shot.transform.position = player.position;
                shot.transform.forward = player.forward;
            }
        }

        public void PlaceCoins()
        {
            mode = 1;
        }

        public void ShootWeb()
        {
            mode = 2;
        }
    }
}