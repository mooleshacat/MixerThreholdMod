using System;

namespace ScheduleOne.Vision
{
	// Token: 0x02000289 RID: 649
	[Serializable]
	public class UniqueVisibilityAttribute : VisibilityAttribute
	{
		// Token: 0x06000D91 RID: 3473 RVA: 0x0003BDA0 File Offset: 0x00039FA0
		public UniqueVisibilityAttribute(string _name, float _pointsChange, string _uniquenessCode, float _multiplier = 1f, int attributeIndex = -1) : base(_name, _pointsChange, _multiplier, attributeIndex)
		{
			this.uniquenessCode = _uniquenessCode;
		}

		// Token: 0x04000DF9 RID: 3577
		public string uniquenessCode;
	}
}
