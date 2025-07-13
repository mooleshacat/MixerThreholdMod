using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.Recording
{
	// Token: 0x0200081D RID: 2077
	[CreateAssetMenu(fileName = "DrivePath", menuName = "ScriptableObjects/DrivePath", order = 1)]
	[Serializable]
	public class DrivePath : ScriptableObject
	{
		// Token: 0x04002869 RID: 10345
		public int fps = 24;

		// Token: 0x0400286A RID: 10346
		public List<VehicleKeyFrame> keyframes = new List<VehicleKeyFrame>();
	}
}
