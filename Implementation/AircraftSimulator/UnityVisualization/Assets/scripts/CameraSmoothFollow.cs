﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public enum CameraView
    {
        BackView,
        FrontView,
        LeftView,
        RightView
    }

    public enum InterpolationType
    {
        Normal,
        EaseOut,
        EaseIn,
        Exponential,
        Smooth
    }
    public class CameraSmoothFollow : MonoBehaviour
    {
        private Transform backCamera, frontCamera, leftCamera, rightCamera, mainCamera;
        private bool animationPending = false;
        private bool animationOnInterrupted = false;
        private InterpolationType type = InterpolationType.Normal;
        public float T(float t)
        {
            switch (type)
            {
                case InterpolationType.Normal:
                    return t;
                case InterpolationType.EaseIn:
                    return 1f - Mathf.Cos(t*Mathf.PI*0.5f);
                case InterpolationType.EaseOut:
                    return Mathf.Sin(t*Mathf.PI*0.5f);
                case InterpolationType.Exponential:
                    return t*t;
                case InterpolationType.Smooth:
                    return t*t*t*(t*(6f*t - 15f) + 10f);

            }
            return t;
        }
        void Start()
        {
            backCamera = transform.FindChild("BackCamera");
            frontCamera = transform.FindChild("FrontCamera");
            leftCamera = transform.FindChild("LeftCamera");
            rightCamera = transform.FindChild("RightCamera");
            mainCamera = transform.FindChild("MainCamera");
        }

        public void SmoothTransist(CameraView view)
        {
            StopAllCoroutines();
            if (view == CameraView.LeftView)
                StartCoroutine(interpolate(leftCamera.transform));
            else if (view == CameraView.RightView)
                StartCoroutine(interpolate(rightCamera.transform));
            else if (view == CameraView.FrontView)
                StartCoroutine(interpolate(frontCamera.transform));
        }

        float calculateAnimationTime(Transform target, Transform current)
        {
            float patternAnimationTime = 3.0f;
            float patternDist = Vector3.Distance(backCamera.position, rightCamera.position);
            var dist = Vector3.Distance(target.position, current.position);

            return dist/patternDist * patternAnimationTime;
        }

        public void Interrupt()
        {
            StopAllCoroutines();
            StartCoroutine(interpolate(backCamera));
        }

        IEnumerator interpolate(Transform target)
        {
            Transform captured = mainCamera;
            float targetAnimationTime = calculateAnimationTime(target, captured);
            float animationTime = 0f;
            while(animationTime < 1.0f)
            {                
                float t = animationTime/targetAnimationTime;
                t = T(t);
                mainCamera.rotation = Quaternion.Lerp(captured.rotation, target.rotation, t);
                mainCamera.position = Vector3.Lerp(captured.position, target.position, t);

                animationTime += Time.deltaTime;
                if (animationTime > 1.0f)
                    animationTime = 1.0f;
                yield return new WaitForEndOfFrame();
            }
            mainCamera.rotation = target.rotation;
            mainCamera.position = target.position;
        }

        private float minZoomRange = -5.0f;
        private float maxZoomRange = 20.0f;
        private float currentZoom = 0.0f;
        public void ZoomCamera(float delta)
        {
            var newZoom = Mathf.Clamp(currentZoom + delta, minZoomRange, maxZoomRange);
            delta = newZoom - currentZoom;
            currentZoom = newZoom;

            backCamera.position -= backCamera.forward*delta;
            mainCamera.position -= mainCamera.forward*delta;
            rightCamera.position -= rightCamera.forward * delta;
            leftCamera.position -= leftCamera.forward * delta;
            frontCamera.position -= frontCamera.forward * delta;
        }
    }
}
