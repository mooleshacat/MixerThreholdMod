using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001B2 RID: 434
	[Serializable]
	public class KeyframeGroup<T> : IKeyframeGroup where T : IBaseKeyframe
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00027BF5 File Offset: 0x00025DF5
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x00027BFD File Offset: 0x00025DFD
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00027C06 File Offset: 0x00025E06
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x00027C0E File Offset: 0x00025E0E
		public string id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00027C18 File Offset: 0x00025E18
		public KeyframeGroup(string name)
		{
			this.name = name;
			this.id = Guid.NewGuid().ToString();
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00027C56 File Offset: 0x00025E56
		public void AddKeyFrame(T keyFrame)
		{
			this.keyframes.Add(keyFrame);
			this.SortKeyframes();
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00027C6A File Offset: 0x00025E6A
		public void RemoveKeyFrame(T keyFrame)
		{
			if (this.keyframes.Count == 1)
			{
				Debug.LogError("You must have at least 1 keyframe in every group.");
				return;
			}
			this.keyframes.Remove(keyFrame);
			this.SortKeyframes();
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00027C98 File Offset: 0x00025E98
		public void RemoveKeyFrame(IBaseKeyframe keyframe)
		{
			this.RemoveKeyFrame((T)((object)keyframe));
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00027CA6 File Offset: 0x00025EA6
		public int GetKeyFrameCount()
		{
			return this.keyframes.Count;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00027CB3 File Offset: 0x00025EB3
		public T GetKeyframe(int index)
		{
			return this.keyframes[index];
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00027CC1 File Offset: 0x00025EC1
		public void SortKeyframes()
		{
			this.keyframes.Sort();
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00027CCE File Offset: 0x00025ECE
		public float CurveAdjustedBlendingTime(InterpolationCurve curve, float t)
		{
			if (curve == InterpolationCurve.Linear)
			{
				return t;
			}
			if (curve == InterpolationCurve.EaseInEaseOut)
			{
				return Mathf.Clamp01((t < 0.5f) ? (2f * t * t) : (-1f + (4f - 2f * t) * t));
			}
			return t;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00027D08 File Offset: 0x00025F08
		public T GetPreviousKeyFrame(float time)
		{
			T result;
			T t;
			if (!this.GetSurroundingKeyFrames(time, out result, out t))
			{
				return default(T);
			}
			return result;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00027D30 File Offset: 0x00025F30
		public bool GetSurroundingKeyFrames(float time, out T beforeKeyframe, out T afterKeyframe)
		{
			beforeKeyframe = default(T);
			afterKeyframe = default(T);
			int index;
			int index2;
			if (this.GetSurroundingKeyFrames(time, out index, out index2))
			{
				beforeKeyframe = this.GetKeyframe(index);
				afterKeyframe = this.GetKeyframe(index2);
				return true;
			}
			return false;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00027D78 File Offset: 0x00025F78
		public bool GetSurroundingKeyFrames(float time, out int beforeIndex, out int afterIndex)
		{
			beforeIndex = 0;
			afterIndex = 0;
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't return nearby keyframes since it's empty.");
				return false;
			}
			if (this.keyframes.Count == 1)
			{
				return true;
			}
			T t = this.keyframes[0];
			if (time < t.time)
			{
				beforeIndex = this.keyframes.Count - 1;
				afterIndex = 0;
				return true;
			}
			int num = 0;
			for (int i = 0; i < this.keyframes.Count; i++)
			{
				t = this.keyframes[i];
				if (t.time >= time)
				{
					break;
				}
				num = i;
			}
			int num2 = (num + 1) % this.keyframes.Count;
			beforeIndex = num;
			afterIndex = num2;
			return true;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00027E33 File Offset: 0x00026033
		public static float ProgressBetweenSurroundingKeyframes(float time, BaseKeyframe beforeKey, BaseKeyframe afterKey)
		{
			return KeyframeGroup<T>.ProgressBetweenSurroundingKeyframes(time, beforeKey.time, afterKey.time);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00027E48 File Offset: 0x00026048
		public static float ProgressBetweenSurroundingKeyframes(float time, float beforeKeyTime, float afterKeyTime)
		{
			if (afterKeyTime > beforeKeyTime && time <= beforeKeyTime)
			{
				return 0f;
			}
			float num = KeyframeGroup<T>.WidthBetweenCircularValues(beforeKeyTime, afterKeyTime);
			return Mathf.Clamp01(KeyframeGroup<T>.WidthBetweenCircularValues(beforeKeyTime, time) / num);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00027E79 File Offset: 0x00026079
		public static float WidthBetweenCircularValues(float begin, float end)
		{
			if (begin <= end)
			{
				return end - begin;
			}
			return 1f - begin + end;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00027E8C File Offset: 0x0002608C
		public void TrimToSingleKeyframe()
		{
			if (this.keyframes.Count == 1)
			{
				return;
			}
			this.keyframes.RemoveRange(1, this.keyframes.Count - 1);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00027EB8 File Offset: 0x000260B8
		public InterpolationDirection GetShortestInterpolationDirection(float previousKeyValue, float nextKeyValue, float minValue, float maxValue)
		{
			float num;
			float num2;
			this.CalculateCircularDistances(previousKeyValue, nextKeyValue, minValue, maxValue, out num, out num2);
			if (num2 > num)
			{
				return InterpolationDirection.Reverse;
			}
			return InterpolationDirection.Foward;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00027EDB File Offset: 0x000260DB
		public void CalculateCircularDistances(float previousKeyValue, float nextKeyValue, float minValue, float maxValue, out float forwardDistance, out float reverseDistance)
		{
			if (nextKeyValue < previousKeyValue)
			{
				forwardDistance = maxValue - previousKeyValue + (nextKeyValue - minValue);
			}
			else
			{
				forwardDistance = nextKeyValue - previousKeyValue;
			}
			reverseDistance = minValue + maxValue - forwardDistance;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00027F00 File Offset: 0x00026100
		public float InterpolateFloat(InterpolationCurve curve, InterpolationDirection direction, float time, float beforeTime, float nextTime, float previousKeyValue, float nextKeyValue, float minValue, float maxValue)
		{
			float t = KeyframeGroup<T>.ProgressBetweenSurroundingKeyframes(time, beforeTime, nextTime);
			float num = this.CurveAdjustedBlendingTime(curve, t);
			if (direction == InterpolationDirection.Auto)
			{
				return this.AutoInterpolation(num, previousKeyValue, nextKeyValue);
			}
			InterpolationDirection interpolationDirection = direction;
			float num2;
			float num3;
			this.CalculateCircularDistances(previousKeyValue, nextKeyValue, minValue, maxValue, out num2, out num3);
			if (interpolationDirection == InterpolationDirection.ShortestPath)
			{
				if (num3 > num2)
				{
					interpolationDirection = InterpolationDirection.Foward;
				}
				else
				{
					interpolationDirection = InterpolationDirection.Reverse;
				}
			}
			if (interpolationDirection == InterpolationDirection.Foward)
			{
				return this.ForwardInterpolation(num, previousKeyValue, nextKeyValue, minValue, maxValue, num2);
			}
			if (interpolationDirection == InterpolationDirection.Reverse)
			{
				return this.ReverseInterpolation(num, previousKeyValue, nextKeyValue, minValue, maxValue, num3);
			}
			Debug.LogError("Unhandled interpolation direction: " + interpolationDirection.ToString() + ", returning min value.");
			return minValue;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00027FA1 File Offset: 0x000261A1
		public float AutoInterpolation(float curvedTime, float previousValue, float nextValue)
		{
			return Mathf.Lerp(previousValue, nextValue, curvedTime);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00027FAC File Offset: 0x000261AC
		public float ForwardInterpolation(float time, float previousKeyValue, float nextKeyValue, float minValue, float maxValue, float distance)
		{
			if (previousKeyValue <= nextKeyValue)
			{
				return Mathf.Lerp(previousKeyValue, nextKeyValue, time);
			}
			float num = time * distance;
			float num2 = maxValue - previousKeyValue;
			if (num <= num2)
			{
				return previousKeyValue + num;
			}
			return minValue + (num - num2);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00027FE0 File Offset: 0x000261E0
		public float ReverseInterpolation(float time, float previousKeyValue, float nextKeyValue, float minValue, float maxValue, float distance)
		{
			if (nextKeyValue <= previousKeyValue)
			{
				return Mathf.Lerp(previousKeyValue, nextKeyValue, time);
			}
			float num = time * distance;
			float num2 = previousKeyValue - minValue;
			if (num <= num2)
			{
				return previousKeyValue - num;
			}
			return maxValue - (num - num2);
		}

		// Token: 0x0400097A RID: 2426
		public List<T> keyframes = new List<T>();

		// Token: 0x0400097B RID: 2427
		[SerializeField]
		private string m_Name;

		// Token: 0x0400097C RID: 2428
		[SerializeField]
		private string m_Id;
	}
}
