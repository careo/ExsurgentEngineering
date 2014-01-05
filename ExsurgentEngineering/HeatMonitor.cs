
namespace ExsurgentEngineering
{
	public class HeatMonitor : PartModule
	{
		[KSPField (guiActive = true, guiName = "temperature", isPersistant = false)]
		public float temperature;

		public override void OnFixedUpdate ()
		{
			temperature = part.temperature;
		}
	}
}

