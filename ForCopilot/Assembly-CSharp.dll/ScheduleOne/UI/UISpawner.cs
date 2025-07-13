using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A93 RID: 2707
	public class UISpawner : MonoBehaviour
	{
		// Token: 0x060048B8 RID: 18616 RVA: 0x001310E4 File Offset: 0x0012F2E4
		private void Start()
		{
			this.nextSpawnTime = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval);
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x00131104 File Offset: 0x0012F304
		private void Update()
		{
			if (this.SpawnRateMultiplier == 0f)
			{
				return;
			}
			if (Time.time > this.nextSpawnTime)
			{
				this.nextSpawnTime = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval) / this.SpawnRateMultiplier;
				if (this.Prefabs.Length != 0)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Prefabs[UnityEngine.Random.Range(0, this.Prefabs.Length)], base.transform);
					if (this.UniformScale)
					{
						float num = UnityEngine.Random.Range(this.MinScale.x, this.MaxScale.x);
						gameObject.transform.localScale = new Vector3(num, num, 1f);
					}
					else
					{
						gameObject.transform.localScale = new Vector3(UnityEngine.Random.Range(this.MinScale.x, this.MaxScale.x), UnityEngine.Random.Range(this.MinScale.y, this.MaxScale.y), 1f);
					}
					gameObject.transform.localPosition = new Vector3(UnityEngine.Random.Range(-this.SpawnArea.rect.width / 2f, this.SpawnArea.rect.width / 2f), UnityEngine.Random.Range(-this.SpawnArea.rect.height / 2f, this.SpawnArea.rect.height / 2f), 0f);
					if (this.OnSpawn != null)
					{
						this.OnSpawn.Invoke(gameObject);
					}
				}
			}
		}

		// Token: 0x0400355B RID: 13659
		public RectTransform SpawnArea;

		// Token: 0x0400355C RID: 13660
		public GameObject[] Prefabs;

		// Token: 0x0400355D RID: 13661
		public float MinInterval = 1f;

		// Token: 0x0400355E RID: 13662
		public float MaxInterval = 5f;

		// Token: 0x0400355F RID: 13663
		public float SpawnRateMultiplier = 1f;

		// Token: 0x04003560 RID: 13664
		public Vector2 MinScale = Vector2.one;

		// Token: 0x04003561 RID: 13665
		public Vector2 MaxScale = Vector2.one;

		// Token: 0x04003562 RID: 13666
		public bool UniformScale = true;

		// Token: 0x04003563 RID: 13667
		private float nextSpawnTime;

		// Token: 0x04003564 RID: 13668
		public UnityEvent<GameObject> OnSpawn;
	}
}
