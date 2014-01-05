using System;
using UnityEngine;

public class agsample : Part
{
	private Transform agCore;
	protected override void onFlightStart ()
	{
		agCore = base.transform.FindChild("model").FindChild("antigravcore").FindChild("Sphere001");
	}
	protected override void onPartUpdate()
	{
		agCore.transform.Rotate(Vector3.forward * 80f * TimeWarp.deltaTime);
	}
}