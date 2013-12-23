//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;
//
//namespace ExsurgentEngineering
//{
//	[KSPAddon (KSPAddon.Startup.Flight, false)]
//	public class CerberusEngineController : MonoBehaviour
//	{
//		Vessel currentVessel;
//
//		public List<ModuleEngines> engines;
//
//		public float thrustAvailable;
//
//		public void Awake ()
//		{
//			Debug.Log ("CerberusEngineController.Awake");
//		}
//
//		public void Start ()
//		{
//			Debug.Log ("CerberusEngineController.Start");
//			GameEvents.onVesselWasModified.Add (OnVesselWasModified);
//			GameEvents.onVesselChange.Add (OnVesselChange);
//			
//
//			currentVessel = FlightGlobals.ActiveVessel;
//			currentVessel.OnFlyByWire += OnFlyByWire;
//
//			UpdateEngines ();
//		}
//
//		public void Update ()
//		{
//			//Debug.Log ("CerberusEngineController.Update");
////			Debug.Log ("CerberusEngineController.ThrustAvailable: " + thrustAvailable);
//		}
//		//
//		//		public void LateUpdate ()
//		//		{
//		//			Debug.Log ("CerberusEngineController.LateUpdate");
//		//		}
//		//
//		public void FixedUpdate ()
//		{
////					Debug.Log ("CerberusEngineController.FixedUpdate");
//			UpdateEngines ();
//		}
//
//		public void OnVesselWasModified (Vessel data)
//		{
//			Debug.Log ("CerberusEngineController.OnVesselWasModified: " + data);
//			UpdateEngines ();
//		}
//
//		public void UpdateEngines ()
//		{
////			engines = (from part in currentVessel.Parts 
////			           from module in part.Modules.OfType<ModuleEngines> ()
////			           select module).ToList ();
//
//
//			Debug.Log ("UpdateEngines");
//
//			engines = (from engine in currentVessel.FindPartModulesImplementing<ModuleEngines> ()
//			           where engine.isOperational
//			          select engine).OrderByDescending (engine => engine.realIsp).ToList ();
//
//			Debug.Log ("engines: " + engines);
//
//			thrustAvailable = engines.Sum (engine => engine.maxThrust);
//			Debug.Log ("thrustAvailable: " + thrustAvailable);
//
////			foreach (var engine in engines) {
////				var msg = string.Format ("engine: {0}. isOperational: {1}", engine.name, engine.isOperational);
////				Debug.Log (msg);
////			}
//		}
//
//		public void OnVesselChange (Vessel data)
//		{
//			Debug.Log ("CerberusEngineController.OnVesselChange: " + data);
//			UpdateEngines ();
//		}
//
//		public void OnFlyByWire (FlightCtrlState ctrlState)
//		{
//			UpdateThrust (ctrlState);
//
//			//Debug.Log ("CerberusEngineController.OnFlyByWire: " + ctrlState);
//		}
//
//		public void UpdateThrust (FlightCtrlState ctrlState)
//		{
//			var throttle = ctrlState.mainThrottle;
//			var thrustRequested = throttle * thrustAvailable;
//
//			Debug.Log ("thrustRequested: " + thrustRequested);
//
//			var thrustRemaining = thrustRequested;
//
//			var enginesByIsp = engines.GroupBy (engine => engine.realIsp).ToDictionary (e => e.Key, v => v.ToList ());
//		
//			foreach (var kvp in enginesByIsp) {
//				
//				var engineGroup = kvp.Value;
//				var maxGroupThrust = engineGroup.Sum (engine => engine.maxThrust);
//
//
//
//				var limiter = 0.0f;
//				if (thrustRemaining >= 0) {
////					var wanted = maxGroupThrust / thrustRemaining;
//					var capacity = maxGroupThrust / thrustRemaining;
//					limiter = capacity * throttle;
//				}
//				
////				limiter = Mathf.Clamp (limiter, 0f, 1 / (throttle));
//				foreach (var engine in engineGroup) {
//					Debug.Log (" -- engine.realIsp: " + engine.realIsp);
//					Debug.Log (" -- engine.maxThrust: " + engine.maxThrust);
//
//					engine.thrustlimiter = limiter;
//					thrustRemaining -= engine.maxThrust * throttle * engine.thrustlimiter;
//				}
//
//			}
//			
//
//
//
//
//			
//
//			
//			
////			Debug.Log("UpdateThrust()")
//		}
//	}
//}
///*
// * 
// * with two engines of thrust 100 with Isp 50 and 100
//* if we have a throttle of 0.50
//* then we want a thrust of 100
//* so groupFraction for the first engine is 0.50
//* maxgroupthust is 100
//* throttle * thrustlimit * maxThrust < maxThrust
//* (throttle * thrustlimit) / maxThrust = 1
//* 
//* 
//* 
//* maxGroupThrust 100
//* thrustRemaining 100
//* 
//* maxGroupThrust 100
//* thrustRemaining 50
//* 
//* maxGroupThrust / thrustRemaining = 2
//* 
//* throttle = 0.5
//* 
//
//* 
//*/