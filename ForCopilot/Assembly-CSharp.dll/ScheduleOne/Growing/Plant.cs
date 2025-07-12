using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Growing
{
	// Token: 0x020008BE RID: 2238
	public class Plant : MonoBehaviour
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003C6C RID: 15468 RVA: 0x000FE986 File Offset: 0x000FCB86
		// (set) Token: 0x06003C6D RID: 15469 RVA: 0x000FE98E File Offset: 0x000FCB8E
		public Pot Pot { get; protected set; }

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003C6E RID: 15470 RVA: 0x000FE997 File Offset: 0x000FCB97
		// (set) Token: 0x06003C6F RID: 15471 RVA: 0x000FE99F File Offset: 0x000FCB9F
		public float NormalizedGrowthProgress { get; protected set; }

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06003C70 RID: 15472 RVA: 0x000FE9A8 File Offset: 0x000FCBA8
		public bool IsFullyGrown
		{
			get
			{
				return this.NormalizedGrowthProgress >= 1f;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06003C71 RID: 15473 RVA: 0x000FE9BA File Offset: 0x000FCBBA
		public PlantGrowthStage FinalGrowthStage
		{
			get
			{
				return this.GrowthStages[this.GrowthStages.Length - 1];
			}
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x000FE9D0 File Offset: 0x000FCBD0
		public virtual void Initialize(NetworkObject pot, float growthProgress = 0f, float yieldLevel = 0f, float qualityLevel = 0f)
		{
			this.Pot = pot.GetComponent<Pot>();
			if (this.Pot == null)
			{
				Console.LogWarning("Plant.Initialize: pot is null", null);
				return;
			}
			if (yieldLevel > 0f)
			{
				this.YieldLevel = yieldLevel;
			}
			else
			{
				this.YieldLevel = this.BaseYieldLevel;
			}
			if (qualityLevel > 0f)
			{
				this.QualityLevel = qualityLevel;
			}
			else
			{
				this.QualityLevel = this.BaseQualityLevel;
			}
			for (int i = 0; i < this.FinalGrowthStage.GrowthSites.Length; i++)
			{
				this.SetHarvestableActive(i, false);
			}
			this.SetNormalizedGrowthProgress(growthProgress);
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x000FEA68 File Offset: 0x000FCC68
		public virtual void Destroy(bool dropScraps = false)
		{
			this.DestroySound.transform.SetParent(NetworkSingleton<GameManager>.Instance.Temp.transform);
			this.DestroySound.PlayOneShot(false);
			UnityEngine.Object.Destroy(this.DestroySound, 1f);
			if (dropScraps && this.PlantScrapPrefab != null)
			{
				int num = UnityEngine.Random.Range(1, 2);
				for (int i = 0; i < num; i++)
				{
					Vector3 a = this.Pot.LeafDropPoint.forward;
					a += new Vector3(0f, UnityEngine.Random.Range(-0.2f, 0.2f), 0f);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.PlantScrapPrefab.ID, this.Pot.LeafDropPoint.position + a * 0.2f, UnityEngine.Random.rotation, a * 0.5f, "", false);
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x000FEB70 File Offset: 0x000FCD70
		public virtual void MinPass()
		{
			if (this.NormalizedGrowthProgress >= 1f)
			{
				return;
			}
			if (NetworkSingleton<TimeManager>.Instance.IsEndOfDay)
			{
				return;
			}
			float num = 1f / ((float)this.GrowthTime * 60f);
			num *= this.Pot.GetAdditiveGrowthMultiplier();
			float num2;
			num *= this.Pot.GetAverageLightExposure(out num2);
			num *= this.Pot.GrowSpeedMultiplier;
			num *= num2;
			if (GameManager.IS_TUTORIAL)
			{
				num *= 0.3f;
			}
			if (this.Pot.NormalizedWaterLevel <= 0f || this.Pot.NormalizedWaterLevel > 1f)
			{
				num *= 0f;
			}
			this.SetNormalizedGrowthProgress(this.NormalizedGrowthProgress + num);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x000FEC28 File Offset: 0x000FCE28
		public virtual void SetNormalizedGrowthProgress(float progress)
		{
			progress = Mathf.Clamp(progress, 0f, 1f);
			float normalizedGrowthProgress = this.NormalizedGrowthProgress;
			this.NormalizedGrowthProgress = progress;
			this.UpdateVisuals();
			if (this.NormalizedGrowthProgress >= 1f && normalizedGrowthProgress < 1f)
			{
				this.GrowthDone();
			}
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x000FEC78 File Offset: 0x000FCE78
		protected virtual void UpdateVisuals()
		{
			int num = Mathf.FloorToInt(this.NormalizedGrowthProgress * (float)this.GrowthStages.Length);
			for (int i = 0; i < this.GrowthStages.Length; i++)
			{
				this.GrowthStages[i].gameObject.SetActive(i + 1 == num);
			}
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x000FECC6 File Offset: 0x000FCEC6
		public virtual void SetHarvestableActive(int index, bool active)
		{
			this.FinalGrowthStage.GrowthSites[index].gameObject.SetActive(active);
			this.ActiveHarvestables.Remove(index);
			if (active)
			{
				this.ActiveHarvestables.Add(index);
			}
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x000FECFC File Offset: 0x000FCEFC
		public bool IsHarvestableActive(int index)
		{
			return this.ActiveHarvestables.Contains(index);
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x000FED0C File Offset: 0x000FCF0C
		private void GrowthDone()
		{
			if (InstanceFinder.IsServer)
			{
				if (!this.Pot.IsSpawned)
				{
					Console.LogError("Pot not spawned!", null);
					return;
				}
				int num = Mathf.RoundToInt((float)this.FinalGrowthStage.GrowthSites.Length * this.YieldLevel * this.Pot.YieldMultiplier);
				num = Mathf.Clamp(num, 1, this.FinalGrowthStage.GrowthSites.Length);
				foreach (int harvestableIndex in this.GenerateUniqueIntegers(0, this.FinalGrowthStage.GrowthSites.Length - 1, num))
				{
					this.Pot.SendHarvestableActive(harvestableIndex, true);
				}
			}
			if (this.FullyGrownParticles != null)
			{
				this.FullyGrownParticles.Play();
			}
			if (this.onGrowthDone != null)
			{
				this.onGrowthDone.Invoke();
			}
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x000FEE04 File Offset: 0x000FD004
		private List<int> GenerateUniqueIntegers(int min, int max, int count)
		{
			List<int> list = new List<int>();
			if (max - min + 1 < count)
			{
				Debug.LogWarning("Range is too small to generate the requested number of unique integers.");
				return null;
			}
			List<int> list2 = new List<int>();
			for (int i = min; i <= max; i++)
			{
				list2.Add(i);
			}
			for (int j = 0; j < count; j++)
			{
				int index = UnityEngine.Random.Range(0, list2.Count);
				list.Add(list2[index]);
				list2.RemoveAt(index);
			}
			return list;
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x000FEE75 File Offset: 0x000FD075
		public void SetVisible(bool vis)
		{
			this.VisualsContainer.gameObject.SetActive(vis);
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x000FEE88 File Offset: 0x000FD088
		public virtual ItemInstance GetHarvestedProduct(int quantity = 1)
		{
			Console.LogError("Plant.GetHarvestedProduct: This method should be overridden by a subclass.", null);
			return null;
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x000FEE96 File Offset: 0x000FD096
		public PlantData GetPlantData()
		{
			return new PlantData(this.SeedDefinition.ID, this.NormalizedGrowthProgress, this.YieldLevel, this.QualityLevel, this.ActiveHarvestables.ToArray());
		}

		// Token: 0x04002B3F RID: 11071
		[Header("References")]
		public Transform VisualsContainer;

		// Token: 0x04002B40 RID: 11072
		public PlantGrowthStage[] GrowthStages;

		// Token: 0x04002B41 RID: 11073
		public Collider Collider;

		// Token: 0x04002B42 RID: 11074
		public AudioSourceController SnipSound;

		// Token: 0x04002B43 RID: 11075
		public AudioSourceController DestroySound;

		// Token: 0x04002B44 RID: 11076
		public ParticleSystem FullyGrownParticles;

		// Token: 0x04002B45 RID: 11077
		[Header("Settings")]
		public SeedDefinition SeedDefinition;

		// Token: 0x04002B46 RID: 11078
		public int GrowthTime = 48;

		// Token: 0x04002B47 RID: 11079
		public float BaseYieldLevel = 0.6f;

		// Token: 0x04002B48 RID: 11080
		public float BaseQualityLevel = 0.4f;

		// Token: 0x04002B49 RID: 11081
		public string HarvestTarget = "buds";

		// Token: 0x04002B4A RID: 11082
		[Header("Trash")]
		public TrashItem PlantScrapPrefab;

		// Token: 0x04002B4B RID: 11083
		public UnityEvent onGrowthDone;

		// Token: 0x04002B4C RID: 11084
		[Header("Plant data")]
		public float YieldLevel;

		// Token: 0x04002B4D RID: 11085
		public float QualityLevel;

		// Token: 0x04002B4E RID: 11086
		[HideInInspector]
		public List<int> ActiveHarvestables = new List<int>();
	}
}
