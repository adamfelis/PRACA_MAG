using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.UI
{
    public class GUIUpdater :MonoBehaviour
    {
        private GameObject aileronLabel, elevatorLabel, rudderLabel;
        private GameObject rollLabel, pitchLabel, yawLabel;
        private GameObject forwardVelocityLabel, rightVelocityLabel, downVelocityLabel;
        private GameObject positionXLabel, positionYLabel, positionZLabel;

        public IAircraft Aircraft;
        private void Start()
        {
            rollLabel = GameObject.FindGameObjectWithTag(Tags.Roll);
            pitchLabel = GameObject.FindGameObjectWithTag(Tags.Pitch);
            yawLabel = GameObject.FindGameObjectWithTag(Tags.Yaw);

            aileronLabel = GameObject.FindGameObjectWithTag(Tags.Aileron);
            elevatorLabel = GameObject.FindGameObjectWithTag(Tags.Elevator);
            rudderLabel = GameObject.FindGameObjectWithTag(Tags.Rudder);

            forwardVelocityLabel = GameObject.FindGameObjectWithTag(Tags.VelocityForwardU);
            rightVelocityLabel = GameObject.FindGameObjectWithTag(Tags.VelociyRightV);
            downVelocityLabel = GameObject.FindGameObjectWithTag(Tags.VelocityDownW);

            positionXLabel = GameObject.FindGameObjectWithTag(Tags.PositionX);
            positionYLabel = GameObject.FindGameObjectWithTag(Tags.PositionY);
            positionZLabel = GameObject.FindGameObjectWithTag(Tags.PositionZ);
        }

        private void Update()
        {
            if (Aircraft == null)
                return;
            updateSteers();
            updateRotations();
            updateVelocities();
            updatePositions();
        }
        private void updateSteers()
        {
            string format = "n2";
            aileronLabel.GetComponent<Text>().text = (Aircraft.Xi * Mathf.Rad2Deg).ToString(format);
            elevatorLabel.GetComponent<Text>().text = (Aircraft.Eta * Mathf.Rad2Deg).ToString(format);
            rudderLabel.GetComponent<Text>().text = (Aircraft.Zeta * Mathf.Rad2Deg).ToString(format);
        }

        private void updateRotations()
        {
            string format = "n2";
            rollLabel.GetComponent<Text>().text = (Aircraft.Phi * Mathf.Rad2Deg).ToString(format);
            pitchLabel.GetComponent<Text>().text = (Aircraft.Theta * Mathf.Rad2Deg).ToString(format);
            yawLabel.GetComponent<Text>().text = (Aircraft.Psi * Mathf.Rad2Deg).ToString(format);
        }

        private void updateVelocities()
        {
            string format = "n0";
            string appendix = " m/s";
            forwardVelocityLabel.GetComponent<Text>().text = (Aircraft.Velocity.x).ToString(format) + appendix;
            downVelocityLabel.GetComponent<Text>().text = (Aircraft.Velocity.y).ToString(format) + appendix;
            rightVelocityLabel.GetComponent<Text>().text = (Aircraft.Velocity.z).ToString(format) + appendix;
        }

        private void updatePositions()
        {
            string format = "n0";
            string appendix = " m";
            positionXLabel.GetComponent<Text>().text = (Aircraft.Position.x).ToString(format) + appendix;
            positionYLabel.GetComponent<Text>().text = (Aircraft.Position.y).ToString(format) + appendix;
            positionZLabel.GetComponent<Text>().text = (Aircraft.Position.z).ToString(format) + appendix;
        }
    }


}
