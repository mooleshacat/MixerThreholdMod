using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200019F RID: 415
	public class BlendingHelper
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x00026CD7 File Offset: 0x00024ED7
		public BlendingHelper(ProfileBlendingState state)
		{
			this.m_State = state;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00026CE6 File Offset: 0x00024EE6
		public void UpdateState(ProfileBlendingState state)
		{
			this.m_State = state;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00026CF0 File Offset: 0x00024EF0
		public Color ProfileColorForKey(SkyProfile profile, string key)
		{
			float time = (profile == this.m_State.toProfile) ? 0f : this.m_State.timeOfDay;
			return profile.GetGroup<ColorKeyframeGroup>(key).ColorForTime(time);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00026D30 File Offset: 0x00024F30
		public float ProfileNumberForKey(SkyProfile profile, string key)
		{
			float time = (profile == this.m_State.toProfile) ? 0f : this.m_State.timeOfDay;
			return profile.GetGroup<NumberKeyframeGroup>(key).NumericValueAtTime(time);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00026D70 File Offset: 0x00024F70
		public SpherePoint ProfileSpherePointForKey(SkyProfile profile, string key)
		{
			float time = (profile == this.m_State.toProfile) ? 0f : this.m_State.timeOfDay;
			return profile.GetGroup<SpherePointKeyframeGroup>(key).SpherePointForTime(time);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00026DB0 File Offset: 0x00024FB0
		public void BlendColor(string key)
		{
			this.BlendColor(key, this.ProfileColorForKey(this.m_State.fromProfile, key), this.ProfileColorForKey(this.m_State.toProfile, key), this.m_State.progress);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00026DE8 File Offset: 0x00024FE8
		public void BlendColorOut(string key)
		{
			this.BlendColor(key, this.ProfileColorForKey(this.m_State.fromProfile, key), this.ProfileColorForKey(this.m_State.fromProfile, key).Clear(), this.m_State.outProgress);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00026E25 File Offset: 0x00025025
		public void BlendColorIn(string key)
		{
			this.BlendColor(key, this.ProfileColorForKey(this.m_State.toProfile, key).Clear(), this.ProfileColorForKey(this.m_State.toProfile, key), this.m_State.inProgress);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00026E62 File Offset: 0x00025062
		public void BlendColor(string key, Color from, Color to, float progress)
		{
			this.m_State.blendedProfile.GetGroup<ColorKeyframeGroup>(key).keyframes[0].color = Color.LerpUnclamped(from, to, progress);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00026E8E File Offset: 0x0002508E
		public void BlendNumber(string key)
		{
			this.BlendNumber(key, this.ProfileNumberForKey(this.m_State.fromProfile, key), this.ProfileNumberForKey(this.m_State.toProfile, key), this.m_State.progress);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00026EC6 File Offset: 0x000250C6
		public void BlendNumberOut(string key, float toValue = 0f)
		{
			this.BlendNumber(key, this.ProfileNumberForKey(this.m_State.fromProfile, key), toValue, this.m_State.outProgress);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00026EED File Offset: 0x000250ED
		public void BlendNumberIn(string key, float fromValue = 0f)
		{
			this.BlendNumber(key, fromValue, this.ProfileNumberForKey(this.m_State.toProfile, key), this.m_State.inProgress);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00026F14 File Offset: 0x00025114
		public void BlendNumber(string key, float from, float to, float progress)
		{
			this.m_State.blendedProfile.GetGroup<NumberKeyframeGroup>(key).keyframes[0].value = Mathf.Lerp(from, to, progress);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00026F40 File Offset: 0x00025140
		public void BlendSpherePoint(string key)
		{
			this.BlendSpherePoint(key, this.ProfileSpherePointForKey(this.m_State.fromProfile, "MoonPositionKey"), this.ProfileSpherePointForKey(this.m_State.toProfile, "MoonPositionKey"), this.m_State.progress);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00026F80 File Offset: 0x00025180
		public void BlendSpherePoint(string key, SpherePoint from, SpherePoint to, float progress)
		{
			Vector3 vector = Vector3.Slerp(from.GetWorldDirection(), to.GetWorldDirection(), progress);
			this.m_State.blendedProfile.GetGroup<SpherePointKeyframeGroup>(key).keyframes[0].spherePoint = new SpherePoint(vector.normalized);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00026FD0 File Offset: 0x000251D0
		public ProfileFeatureBlendingMode GetFeatureAnimationMode(string featureKey)
		{
			bool flag = this.m_State.fromProfile.IsFeatureEnabled(featureKey, true);
			bool flag2 = this.m_State.toProfile.IsFeatureEnabled(featureKey, true);
			if (flag && flag2)
			{
				return ProfileFeatureBlendingMode.Normal;
			}
			if (flag && !flag2)
			{
				return ProfileFeatureBlendingMode.FadeFeatureOut;
			}
			if (!flag && flag2)
			{
				return ProfileFeatureBlendingMode.FadeFeatureIn;
			}
			return ProfileFeatureBlendingMode.None;
		}

		// Token: 0x04000957 RID: 2391
		private ProfileBlendingState m_State;
	}
}
