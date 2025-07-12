using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework
{
	// Token: 0x020009AB RID: 2475
	public class EyebrowController : MonoBehaviour
	{
		// Token: 0x0600431E RID: 17182 RVA: 0x0011A7AC File Offset: 0x001189AC
		public void ApplySettings(AvatarSettings settings)
		{
			this.SetLeftBrowRestingHeight(settings.EyebrowRestingHeight);
			this.SetRightBrowRestingHeight(settings.EyebrowRestingHeight);
			this.leftBrow.SetScale(settings.EyebrowScale);
			this.rightBrow.SetScale(settings.EyebrowScale);
			this.leftBrow.SetThickness(settings.EyebrowThickness);
			this.rightBrow.SetThickness(settings.EyebrowThickness);
			this.leftBrow.SetRestingAngle(settings.EyebrowRestingAngle);
			this.rightBrow.SetRestingAngle(settings.EyebrowRestingAngle);
			this.leftBrow.SetColor(settings.HairColor);
			this.rightBrow.SetColor(settings.HairColor);
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x0011A859 File Offset: 0x00118A59
		public void SetLeftBrowRestingHeight(float normalizedHeight)
		{
			this.leftBrow.SetRestingHeight(normalizedHeight);
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x0011A867 File Offset: 0x00118A67
		public void SetRightBrowRestingHeight(float normalizedHeight)
		{
			this.rightBrow.SetRestingHeight(normalizedHeight);
		}

		// Token: 0x04002FE6 RID: 12262
		[Header("References")]
		public Eyebrow leftBrow;

		// Token: 0x04002FE7 RID: 12263
		public Eyebrow rightBrow;
	}
}
