using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vision
{
	// Token: 0x0200028A RID: 650
	[Serializable]
	public class VisibilityAttribute
	{
		// Token: 0x06000D92 RID: 3474 RVA: 0x0003BDB8 File Offset: 0x00039FB8
		public VisibilityAttribute(string _name, float _pointsChange, float _multiplier = 1f, int attributeIndex = -1)
		{
			this.name = _name;
			this.pointsChange = _pointsChange;
			this.multiplier = _multiplier;
			if (attributeIndex == -1)
			{
				Player.Local.Visibility.activeAttributes.Add(this);
				return;
			}
			Player.Local.Visibility.activeAttributes.Insert(attributeIndex, this);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0003BE28 File Offset: 0x0003A028
		public void Delete()
		{
			Player.Local.Visibility.activeAttributes.Remove(this);
		}

		// Token: 0x04000DFA RID: 3578
		public string name = "Attribute Name";

		// Token: 0x04000DFB RID: 3579
		public float pointsChange;

		// Token: 0x04000DFC RID: 3580
		[Range(0f, 5f)]
		public float multiplier = 1f;
	}
}
