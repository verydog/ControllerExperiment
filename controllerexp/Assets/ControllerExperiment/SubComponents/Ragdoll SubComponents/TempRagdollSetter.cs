﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerExperiment
{
    public class TempRagdollSetter : MonoBehaviour
    {
        public bool KinematicMovement;
        
        public GameObject mirrorJoint;
        public float PositionSyncSpeed;
        public float RotationSyncSpeed;

        Rigidbody myRigidBody;
        ConfigurableJoint myJoint;

        public Vector3 MirrorAnchorPosition;
        public Vector3 MirrorTargetPosition;

        private void Start()
        {
            myRigidBody = this.gameObject.GetComponent<Rigidbody>();
            myJoint = this.gameObject.GetComponent<ConfigurableJoint>();

            MirrorAnchorPosition = mirrorJoint.transform.position;
        }

        private void FixedUpdate()
        {
            myRigidBody.isKinematic = KinematicMovement;

            if (KinematicMovement)
            {
                myRigidBody.useGravity = false;

                Vector3 targetPosition = Vector3.Lerp(myJoint.transform.position, GetMyAnchorPosition(), Time.deltaTime * PositionSyncSpeed);
                myRigidBody.MovePosition(targetPosition);

                Quaternion targetRotation = Quaternion.Lerp(myJoint.transform.rotation, mirrorJoint.transform.rotation, Time.deltaTime * RotationSyncSpeed);
                myRigidBody.MoveRotation(targetRotation);

                Debug.DrawLine(Vector3.zero, GetMyAnchorPosition(), Color.yellow, 0.5f);
            }
            else
            {
                MirrorTargetPosition = mirrorJoint.transform.position - MirrorAnchorPosition;
                //Debug.DrawLine(Vector3.zero, GetMyWorldTargetPosition(), Color.red);

                Debug.DrawLine(Vector3.zero, GetMyWorldTargetPosition(), Color.yellow);
                myJoint.targetPosition = MirrorTargetPosition;
            }
        }

        Vector3 GetMyAnchorPosition()
        {
            if (myJoint.connectedBody == null)
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 myAnchor = myJoint.connectedBody.position + myJoint.connectedAnchor;

                return myAnchor;
            }
        }

        Vector3 GetMyWorldTargetPosition()
        {
            if (myJoint.connectedBody == null)
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 myTargetPosition = myRigidBody.position + myJoint.targetPosition;

                return myTargetPosition;
            }
        }
    }
}