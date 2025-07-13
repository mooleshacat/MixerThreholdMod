using System;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Emotions
{
	// Token: 0x020009CE RID: 2510
	[Serializable]
	public class AvatarEmotionPreset
	{
		// Token: 0x060043D6 RID: 17366 RVA: 0x0011C9C0 File Offset: 0x0011ABC0
		public static AvatarEmotionPreset Lerp(AvatarEmotionPreset start, AvatarEmotionPreset end, AvatarEmotionPreset neutralPreset, float lerp)
		{
			AvatarEmotionPreset avatarEmotionPreset = new AvatarEmotionPreset();
			avatarEmotionPreset.PresetName = "Lerp";
			avatarEmotionPreset.FaceTexture = ((lerp > 0f) ? end.FaceTexture : start.FaceTexture);
			avatarEmotionPreset.LeftEyeRestingState = Eye.EyeLidConfiguration.Lerp(start.LeftEyeRestingState, end.LeftEyeRestingState, lerp);
			avatarEmotionPreset.RightEyeRestingState = Eye.EyeLidConfiguration.Lerp(start.RightEyeRestingState, end.RightEyeRestingState, lerp);
			float browAngleChange_L = start.BrowAngleChange_L;
			float browAngleChange_R = start.BrowAngleChange_R;
			float browHeightChange_L = start.BrowHeightChange_L;
			float browHeightChange_R = start.BrowHeightChange_R;
			float num = end.BrowAngleChange_L;
			float num2 = end.BrowAngleChange_R;
			float num3 = end.BrowHeightChange_L;
			float num4 = end.BrowHeightChange_R;
			if (end.PresetName != "Neutral")
			{
				num += neutralPreset.BrowAngleChange_L;
				num2 += neutralPreset.BrowAngleChange_R;
				num3 += neutralPreset.BrowHeightChange_L;
				num4 += neutralPreset.BrowHeightChange_R;
			}
			avatarEmotionPreset.BrowAngleChange_L = Mathf.Lerp(browAngleChange_L, num, lerp);
			avatarEmotionPreset.BrowAngleChange_R = Mathf.Lerp(browAngleChange_R, num2, lerp);
			avatarEmotionPreset.BrowHeightChange_L = Mathf.Lerp(browHeightChange_L, num3, lerp);
			avatarEmotionPreset.BrowHeightChange_R = Mathf.Lerp(browHeightChange_R, num4, lerp);
			return avatarEmotionPreset;
		}

		// Token: 0x040030A6 RID: 12454
		public string PresetName = "Preset Name";

		// Token: 0x040030A7 RID: 12455
		public Texture2D FaceTexture;

		// Token: 0x040030A8 RID: 12456
		public Eye.EyeLidConfiguration LeftEyeRestingState;

		// Token: 0x040030A9 RID: 12457
		public Eye.EyeLidConfiguration RightEyeRestingState;

		// Token: 0x040030AA RID: 12458
		[Range(-30f, 30f)]
		public float BrowAngleChange_L;

		// Token: 0x040030AB RID: 12459
		[Range(-30f, 30f)]
		public float BrowAngleChange_R;

		// Token: 0x040030AC RID: 12460
		[Range(-1f, 1f)]
		public float BrowHeightChange_L;

		// Token: 0x040030AD RID: 12461
		[Range(-1f, 1f)]
		public float BrowHeightChange_R;
	}
}
