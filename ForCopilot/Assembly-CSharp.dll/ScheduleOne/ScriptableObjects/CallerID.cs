using System;
using UnityEngine;

namespace ScheduleOne.ScriptableObjects
{
	// Token: 0x020007B8 RID: 1976
	[CreateAssetMenu(fileName = "CallerID", menuName = "ScriptableObjects/CallerID", order = 1)]
	[Serializable]
	public class CallerID : ScriptableObject
	{
		// Token: 0x0400261C RID: 9756
		public string Name;

		// Token: 0x0400261D RID: 9757
		public Sprite ProfilePicture;
	}
}
