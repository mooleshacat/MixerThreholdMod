using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Map.Infrastructure
{
	// Token: 0x02000C90 RID: 3216
	public class Intersection : MonoBehaviour
	{
		// Token: 0x06005A34 RID: 23092 RVA: 0x0017C3F2 File Offset: 0x0017A5F2
		protected virtual void Start()
		{
			Singleton<CoroutineService>.Instance.StartCoroutine(this.Run());
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x0017C405 File Offset: 0x0017A605
		protected IEnumerator Run()
		{
			for (;;)
			{
				this.SetPath1Lights(TrafficLight.State.Green);
				this.SetPath2Lights(TrafficLight.State.Red);
				if (this.timeOffset != 0f)
				{
					yield return new WaitForSecondsRealtime(Mathf.Abs(this.timeOffset));
					this.timeOffset = 0f;
				}
				yield return new WaitForSecondsRealtime(this.path1Time);
				this.SetPath1Lights(TrafficLight.State.Orange);
				yield return new WaitForSecondsRealtime(TrafficLight.amberTime);
				this.SetPath1Lights(TrafficLight.State.Red);
				yield return new WaitForSecondsRealtime(1f);
				this.SetPath2Lights(TrafficLight.State.Green);
				yield return new WaitForSecondsRealtime(this.path2Time);
				this.SetPath2Lights(TrafficLight.State.Orange);
				yield return new WaitForSecondsRealtime(TrafficLight.amberTime);
				this.SetPath2Lights(TrafficLight.State.Red);
				yield return new WaitForSecondsRealtime(1f);
			}
			yield break;
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x0017C414 File Offset: 0x0017A614
		protected void SetPath1Lights(TrafficLight.State state)
		{
			foreach (TrafficLight trafficLight in this.path1Lights)
			{
				trafficLight.state = state;
			}
			if (state == TrafficLight.State.Green)
			{
				using (List<GameObject>.Enumerator enumerator2 = this.path1Obstacles.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameObject gameObject = enumerator2.Current;
						gameObject.gameObject.SetActive(false);
					}
					return;
				}
			}
			foreach (GameObject gameObject2 in this.path1Obstacles)
			{
				gameObject2.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x0017C4F4 File Offset: 0x0017A6F4
		protected void SetPath2Lights(TrafficLight.State state)
		{
			foreach (TrafficLight trafficLight in this.path2Lights)
			{
				trafficLight.state = state;
			}
			if (state == TrafficLight.State.Green)
			{
				using (List<GameObject>.Enumerator enumerator2 = this.path2Obstacles.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameObject gameObject = enumerator2.Current;
						gameObject.gameObject.SetActive(false);
					}
					return;
				}
			}
			foreach (GameObject gameObject2 in this.path2Obstacles)
			{
				gameObject2.gameObject.SetActive(true);
			}
		}

		// Token: 0x04004240 RID: 16960
		[Header("References")]
		[SerializeField]
		protected List<TrafficLight> path1Lights = new List<TrafficLight>();

		// Token: 0x04004241 RID: 16961
		[SerializeField]
		protected List<TrafficLight> path2Lights = new List<TrafficLight>();

		// Token: 0x04004242 RID: 16962
		[SerializeField]
		protected List<GameObject> path1Obstacles = new List<GameObject>();

		// Token: 0x04004243 RID: 16963
		[SerializeField]
		protected List<GameObject> path2Obstacles = new List<GameObject>();

		// Token: 0x04004244 RID: 16964
		[Header("Settings")]
		[SerializeField]
		protected float path1Time = 10f;

		// Token: 0x04004245 RID: 16965
		[SerializeField]
		protected float path2Time = 10f;

		// Token: 0x04004246 RID: 16966
		[SerializeField]
		protected float timeOffset;
	}
}
