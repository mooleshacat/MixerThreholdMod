using System;
using System.Collections.Generic;
using EasyButtons;
using EPOOutline;
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Map;
using ScheduleOne.Property;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000759 RID: 1881
	public class LoadingDock : MonoBehaviour, IGUIDRegisterable, ITransitEntity
	{
		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x000D2C94 File Offset: 0x000D0E94
		// (set) Token: 0x0600329A RID: 12954 RVA: 0x000D2C9C File Offset: 0x000D0E9C
		public LandVehicle DynamicOccupant { get; private set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600329B RID: 12955 RVA: 0x000D2CA5 File Offset: 0x000D0EA5
		// (set) Token: 0x0600329C RID: 12956 RVA: 0x000D2CAD File Offset: 0x000D0EAD
		public LandVehicle StaticOccupant { get; private set; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600329D RID: 12957 RVA: 0x000D2CB6 File Offset: 0x000D0EB6
		public bool IsInUse
		{
			get
			{
				return this.DynamicOccupant != null || this.StaticOccupant != null;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x000D2CD4 File Offset: 0x000D0ED4
		// (set) Token: 0x0600329F RID: 12959 RVA: 0x000D2CDC File Offset: 0x000D0EDC
		public Guid GUID { get; protected set; }

		// Token: 0x060032A0 RID: 12960 RVA: 0x000D2CE8 File Offset: 0x000D0EE8
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x000D2D10 File Offset: 0x000D0F10
		public string Name
		{
			get
			{
				return "Loading Dock " + (ArrayExt.IndexOf<LoadingDock>(this.ParentProperty.LoadingDocks, this) + 1).ToString();
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x000D2D42 File Offset: 0x000D0F42
		// (set) Token: 0x060032A3 RID: 12963 RVA: 0x000D2D4A File Offset: 0x000D0F4A
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x000D2D53 File Offset: 0x000D0F53
		// (set) Token: 0x060032A5 RID: 12965 RVA: 0x000D2D5B File Offset: 0x000D0F5B
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060032A6 RID: 12966 RVA: 0x000D2D64 File Offset: 0x000D0F64
		public Transform LinkOrigin
		{
			get
			{
				return this.uiPoint;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x000D2D6C File Offset: 0x000D0F6C
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000D2D74 File Offset: 0x000D0F74
		public bool Selectable { get; } = 1;

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000D2D7C File Offset: 0x000D0F7C
		// (set) Token: 0x060032AA RID: 12970 RVA: 0x000D2D84 File Offset: 0x000D0F84
		public bool IsAcceptingItems { get; set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000D2D8D File Offset: 0x000D0F8D
		// (set) Token: 0x060032AC RID: 12972 RVA: 0x000D2D95 File Offset: 0x000D0F95
		public bool IsDestroyed { get; set; }

		// Token: 0x060032AD RID: 12973 RVA: 0x000D2D9E File Offset: 0x000D0F9E
		private void Awake()
		{
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000D2DB7 File Offset: 0x000D0FB7
		private void Start()
		{
			base.InvokeRepeating("RefreshOccupant", UnityEngine.Random.Range(0f, 1f), 1f);
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000D2DD8 File Offset: 0x000D0FD8
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x000D2DE8 File Offset: 0x000D0FE8
		private void RefreshOccupant()
		{
			LandVehicle closestVehicle = this.VehicleDetector.closestVehicle;
			if (closestVehicle != null && closestVehicle.speed_Kmh < 2f)
			{
				this.SetOccupant(this.VehicleDetector.closestVehicle);
			}
			else
			{
				this.SetOccupant(null);
			}
			if (this.StaticOccupant != null && !this.StaticOccupant.IsVisible)
			{
				this.SetStaticOccupant(null);
			}
			if (this.DynamicOccupant != null)
			{
				Vector3 position = this.DynamicOccupant.transform.position - this.DynamicOccupant.transform.forward * (this.DynamicOccupant.boundingBoxDimensions.z / 2f + 0.6f);
				this.accessPoints[0].transform.position = position;
				this.accessPoints[0].transform.rotation = Quaternion.LookRotation(this.DynamicOccupant.transform.forward, Vector3.up);
				this.accessPoints[0].transform.localPosition = new Vector3(this.accessPoints[0].transform.localPosition.x, 0f, this.accessPoints[0].transform.localPosition.z);
			}
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x000D2F34 File Offset: 0x000D1134
		private void SetOccupant(LandVehicle occupant)
		{
			if (occupant == this.DynamicOccupant)
			{
				return;
			}
			Console.Log(string.Concat(new string[]
			{
				"Loading dock ",
				base.name,
				" is ",
				(occupant == null) ? "empty" : "occupied",
				"."
			}), null);
			this.DynamicOccupant = occupant;
			this.InputSlots.Clear();
			this.OutputSlots.Clear();
			if (this.DynamicOccupant != null)
			{
				this.OutputSlots.AddRange(this.DynamicOccupant.Storage.ItemSlots);
			}
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x000D2FE0 File Offset: 0x000D11E0
		public void SetStaticOccupant(LandVehicle vehicle)
		{
			this.StaticOccupant = vehicle;
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x000D2FEC File Offset: 0x000D11EC
		public virtual void ShowOutline(Color color)
		{
			if (this.OutlineEffect == null)
			{
				this.OutlineEffect = base.gameObject.AddComponent<Outlinable>();
				this.OutlineEffect.OutlineParameters.BlurShift = 0f;
				this.OutlineEffect.OutlineParameters.DilateShift = 0.5f;
				this.OutlineEffect.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
				foreach (GameObject gameObject in this.OutlineRenderers)
				{
					MeshRenderer[] array = new MeshRenderer[0];
					array = new MeshRenderer[]
					{
						gameObject.GetComponent<MeshRenderer>()
					};
					for (int j = 0; j < array.Length; j++)
					{
						OutlineTarget outlineTarget = new OutlineTarget(array[j], 0);
						this.OutlineEffect.TryAddTarget(outlineTarget);
					}
				}
			}
			this.OutlineEffect.OutlineParameters.Color = color;
			Color32 c = color;
			c.a = 9;
			this.OutlineEffect.OutlineParameters.FillPass.SetColor("_PublicColor", c);
			this.OutlineEffect.enabled = true;
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x000D3111 File Offset: 0x000D1311
		public virtual void HideOutline()
		{
			if (this.OutlineEffect != null)
			{
				this.OutlineEffect.enabled = false;
			}
		}

		// Token: 0x040023BE RID: 9150
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x040023C4 RID: 9156
		public Property ParentProperty;

		// Token: 0x040023C5 RID: 9157
		public VehicleDetector VehicleDetector;

		// Token: 0x040023C6 RID: 9158
		public ParkingLot Parking;

		// Token: 0x040023C7 RID: 9159
		public Transform uiPoint;

		// Token: 0x040023C8 RID: 9160
		public Transform[] accessPoints;

		// Token: 0x040023C9 RID: 9161
		public GameObject[] OutlineRenderers;

		// Token: 0x040023CA RID: 9162
		private Outlinable OutlineEffect;
	}
}
