using System;
using UnityEngine;
using System.Collections.Generic;

namespace ExsurgentEngineering
{
	public class SmarterGimbal : ModuleGimbal
	{
		[KSPField (isPersistant = false)]
		public float pitchRange = 10f;

		[KSPField (isPersistant = false)]
		public float yawRange = 10f;

		[KSPField (isPersistant = false)]
		new public String gimbalTransformName;

		[KSPField (isPersistant = false)]
		new public float gimbalResponseSpeed = 10f;

		[KSPField (isPersistant = false)]
		new public bool useGimbalResponseSpeed;

		public Dictionary<Transform,Quaternion> transformsAndRotations = new Dictionary<Transform,Quaternion> ();

		public override string GetInfo ()
		{
			return String.Format ("Smarter Thrust Vectoring Enabled\n - Pitch: {0}\n - Yaw: {1}", pitchRange, yawRange);
		}

		public override void OnLoad (ConfigNode node)
		{
			// use gimbalRange for pitch and yaw ranges if present
			if (node.HasValue ("gimbalRange")) {
				var configRange = float.Parse (node.GetValue ("gimbalRange"));
				pitchRange = configRange;
				yawRange = configRange;
			}
		}

		public override void OnStart (StartState state)
		{
			if (gimbalLock)
				LockGimbal ();
			else
				FreeGimbal ();

			foreach (var gimbalTransform in part.FindModelTransforms(gimbalTransformName)) {
				transformsAndRotations.Add (gimbalTransform, gimbalTransform.localRotation);
			}
		}

		public override void OnFixedUpdate ()
		{
			// FIXME: really, what should the guard conditions be? 
			if (HighLogic.LoadedSceneIsEditor)
				return;
			if (FlightGlobals.ActiveVessel != vessel)
				return;
			if (gimbalLock)
				return;

			var vesselCoM = vessel.findWorldCenterOfMass ();
			var partCoM = part.rigidbody.worldCenterOfMass;
			var displacement = vesselCoM - partCoM;

			// position in front of, or behind,  CoM
			var pitchYawDot = Vector3.Dot (displacement, vessel.transform.up);
			var pitchYawSign = Mathf.Sign (pitchYawDot);

			
//			var rollAngle = 0f;
//			if (part.symmetryMode > 0) {
//				var rollDot = RollAxis ();
//				var rollDotSign = Mathf.Sign (rollDot);
//

//				rollAngle = roll * rollDotSign * pitchRange * -1; // TODO: figure out why the -1 is needed. I didn't think it was
//			}
//

			var yawAngle = vessel.ctrlState.yaw * yawRange * pitchYawSign;
			var pitchAngle = vessel.ctrlState.pitch * pitchRange * pitchYawSign;
			//var rollAngle = vessel.ctrlState.roll * pitchRange * -1; // TODO: use correct mix of pitch and yaw...
		
//			Debug.Log ("yawAngle: " + yawAngle);
//			Debug.Log ("pitchAngle: " + pitchAngle);
//			Debug.Log ("rollAngle: " + rollAngle);


//			var gimbalAngle = pitchAngle + rollAngle;
//			gimbalAngle = Mathf.Clamp (gimbalAngle, -pitchRange, pitchRange);

			foreach (var pair in transformsAndRotations) {
				var gimbalTransform = pair.Key;
				var initialRotation = pair.Value;

				var yawAxis = gimbalTransform.InverseTransformDirection (vessel.ReferenceTransform.forward);
				var pitchAxis = gimbalTransform.InverseTransformDirection (vessel.ReferenceTransform.right);


//				if (Vector3.Dot(part.transform.position.normalized, vessel.ReferenceTransform.right) < 0) // below 0 means the engine is on the left side of the craft
			

//				var rollAxis = gimbalTransform.InverseTransformDirection (new Vector3 (displacement.x, displacement.y));

				
// roll axis will be some mix of pitch and yaw. 
				// if engines are positioned like jets... then it will indeed be the pitch axis....
				// and we can just add the angle...., modified by dot product sign....

				// if the engines are positioned at right angles, we add roll to yaw... ... modified by dot product sign

				// if the engines are halfway... we mix. 

				// if the engines are left and right, the roll axis is the pitch axis...
				// dot product at 90 degrees is 0.
			
				// cross pitch axis and displacement of part to CoM... 


//				var rollAxis = gimbalTransform.InverseTransformDirection (displacement);

//				Debug.Log ("yawAxis: " + yawAxis);
//				Debug.Log ("pitchAxis: " + pitchAxis);
//				Debug.Log ("rollAxis: " + rollAxis);

				var yawRotation = Quaternion.AngleAxis (yawAngle, yawAxis);
				var pitchRotation = Quaternion.AngleAxis (pitchAngle, pitchAxis);
				var rollRotation = Quaternion.AngleAxis (rollAngle, rollAxis);

//				Debug.Log ("yawRotation: " + yawRotation.eulerAngles);
//				Debug.Log ("pitchRotation: " + pitchRotation.eulerAngles);
//				Debug.Log ("rollRotation: " + rollRotation.eulerAngles);

				var targetRotation = initialRotation * pitchRotation * yawRotation * rollRotation;
//				Debug.Log ("targetRotation: " + targetRotation.eulerAngles);

				if (useGimbalResponseSpeed) {
					var angle = gimbalResponseSpeed * TimeWarp.fixedDeltaTime;
					gimbalTransform.localRotation = Quaternion.RotateTowards (gimbalTransform.localRotation, targetRotation, angle);
				} else {
					gimbalTransform.localRotation = targetRotation;
				}
			}
		}
	}
}

