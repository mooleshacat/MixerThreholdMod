using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Tiles;
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BF5 RID: 3061
	[RequireComponent(typeof(TrashContainer))]
	public class TrashContainerItem : GridItem, ITransitEntity
	{
		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x0015A883 File Offset: 0x00158A83
		public string Name
		{
			get
			{
				return base.ItemInstance.Name;
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x060051FE RID: 20990 RVA: 0x0015A890 File Offset: 0x00158A90
		// (set) Token: 0x060051FF RID: 20991 RVA: 0x0015A898 File Offset: 0x00158A98
		public List<ItemSlot> InputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06005200 RID: 20992 RVA: 0x0015A8A1 File Offset: 0x00158AA1
		// (set) Token: 0x06005201 RID: 20993 RVA: 0x0015A8A9 File Offset: 0x00158AA9
		public List<ItemSlot> OutputSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06005202 RID: 20994 RVA: 0x000B3D7F File Offset: 0x000B1F7F
		public Transform LinkOrigin
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06005203 RID: 20995 RVA: 0x0015A8B2 File Offset: 0x00158AB2
		public Transform[] AccessPoints
		{
			get
			{
				return this.accessPoints;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06005204 RID: 20996 RVA: 0x0015A8BA File Offset: 0x00158ABA
		public bool Selectable { get; }

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06005205 RID: 20997 RVA: 0x0015A8C2 File Offset: 0x00158AC2
		// (set) Token: 0x06005206 RID: 20998 RVA: 0x0015A8CA File Offset: 0x00158ACA
		public bool IsAcceptingItems { get; set; }

		// Token: 0x06005207 RID: 20999 RVA: 0x0015A8D4 File Offset: 0x00158AD4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.ObjectScripts.TrashContainerItem_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005208 RID: 21000 RVA: 0x0015A8F3 File Offset: 0x00158AF3
		protected override void Start()
		{
			base.Start();
			this.Container.onTrashLevelChanged.AddListener(new UnityAction(this.TrashLevelChanged));
			this.Container.onTrashAdded.AddListener(new UnityAction<string>(this.TrashAdded));
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x0015A933 File Offset: 0x00158B33
		public override void InitializeGridItem(ItemInstance instance, Grid grid, Vector2 originCoordinate, int rotation, string GUID)
		{
			base.InitializeGridItem(instance, grid, originCoordinate, rotation, GUID);
			if (!this.isGhost)
			{
				base.InvokeRepeating("CheckTrashItems", UnityEngine.Random.Range(0f, 1f), 1f);
			}
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x0015A96C File Offset: 0x00158B6C
		private void TrashLevelChanged()
		{
			base.HasChanged = true;
			if (this.Container.NormalizedTrashLevel > 0.75f)
			{
				if (!this.Flies.isPlaying)
				{
					this.Flies.Play();
					return;
				}
			}
			else if (this.Flies.isPlaying)
			{
				this.Flies.Stop();
			}
		}

		// Token: 0x0600520B RID: 21003 RVA: 0x0015A9C3 File Offset: 0x00158BC3
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.Container.TrashLevel > 0)
			{
				reason = "Contains trash";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x0015A9E3 File Offset: 0x00158BE3
		public override BuildableItemData GetBaseData()
		{
			return new TrashContainerData(base.GUID, base.ItemInstance, 0, base.OwnerGrid, this.OriginCoordinate, this.Rotation, this.Container.Content.GetData());
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x0015AA1C File Offset: 0x00158C1C
		private void TrashAdded(string trashID)
		{
			if (this.TrashAddedSound == null)
			{
				return;
			}
			float volumeMultiplier = Mathf.Clamp01((float)NetworkSingleton<TrashManager>.Instance.GetTrashPrefab(trashID).Size / 4f);
			this.TrashAddedSound.VolumeMultiplier = volumeMultiplier;
			this.TrashAddedSound.Play();
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x0015AA6C File Offset: 0x00158C6C
		public override void ShowOutline(Color color)
		{
			base.ShowOutline(color);
			this.PickupAreaProjector.enabled = true;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x0015AA81 File Offset: 0x00158C81
		public override void HideOutline()
		{
			base.HideOutline();
			this.PickupAreaProjector.enabled = false;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x0015AA98 File Offset: 0x00158C98
		private void CheckTrashItems()
		{
			for (int i = 0; i < this.TrashItemsInRadius.Count; i++)
			{
				if (!this.IsTrashValid(this.TrashItemsInRadius[i]))
				{
					this.RemoveTrashItemFromRadius(this.TrashItemsInRadius[i]);
					i--;
				}
			}
			Collider[] array = Physics.OverlapSphere(base.transform.position, this.calculatedPickupRadius, LayerMask.GetMask(new string[]
			{
				"Trash"
			}), 1);
			for (int j = 0; j < array.Length; j++)
			{
				if (this.IsPointInPickupZone(array[j].transform.position))
				{
					TrashItem componentInParent = array[j].GetComponentInParent<TrashItem>();
					if (componentInParent != null && this.IsTrashValid(componentInParent))
					{
						this.AddTrashToRadius(componentInParent);
					}
				}
			}
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x0015AB58 File Offset: 0x00158D58
		private void AddTrashToRadius(TrashItem trashItem)
		{
			if (trashItem is TrashBag)
			{
				this.AddTrashBagToRadius(trashItem as TrashBag);
				return;
			}
			if (!this.TrashItemsInRadius.Contains(trashItem))
			{
				this.TrashItemsInRadius.Add(trashItem);
				trashItem.onDestroyed = (Action<TrashItem>)Delegate.Combine(trashItem.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x0015ABB6 File Offset: 0x00158DB6
		private void AddTrashBagToRadius(TrashBag trashBag)
		{
			if (!this.TrashBagsInRadius.Contains(trashBag))
			{
				this.TrashBagsInRadius.Add(trashBag);
				trashBag.onDestroyed = (Action<TrashItem>)Delegate.Combine(trashBag.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x0015ABF4 File Offset: 0x00158DF4
		private void RemoveTrashItemFromRadius(TrashItem trashItem)
		{
			if (trashItem is TrashBag)
			{
				this.RemoveTrashBagFromRadius(trashItem as TrashBag);
				return;
			}
			if (this.TrashItemsInRadius.Contains(trashItem))
			{
				this.TrashItemsInRadius.Remove(trashItem);
				trashItem.onDestroyed = (Action<TrashItem>)Delegate.Remove(trashItem.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x0015AC53 File Offset: 0x00158E53
		private void RemoveTrashBagFromRadius(TrashBag trashBag)
		{
			if (this.TrashBagsInRadius.Contains(trashBag))
			{
				this.TrashBagsInRadius.Remove(trashBag);
				trashBag.onDestroyed = (Action<TrashItem>)Delegate.Remove(trashBag.onDestroyed, new Action<TrashItem>(this.RemoveTrashItemFromRadius));
			}
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x0015AC94 File Offset: 0x00158E94
		private bool IsTrashValid(TrashItem trashItem)
		{
			return !(trashItem == null) && this.IsPointInPickupZone(trashItem.transform.position) && !trashItem.Draggable.IsBeingDragged && base.ParentProperty.DoBoundsContainPoint(trashItem.transform.position);
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x0015ACEC File Offset: 0x00158EEC
		public bool IsPointInPickupZone(Vector3 point)
		{
			float num = Mathf.Abs(point.x - base.transform.position.x);
			float num2 = Mathf.Abs(point.z - base.transform.position.z);
			return num <= this.PickupSquareWidth && num2 <= this.PickupSquareWidth && Mathf.Abs(point.y - base.transform.position.y) <= 2f;
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x0015ADBD File Offset: 0x00158FBD
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x0015ADD6 File Offset: 0x00158FD6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.TrashContainerItemAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x0015ADEF File Offset: 0x00158FEF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x0015AE00 File Offset: 0x00159000
		protected override void dll()
		{
			base.Awake();
			this.PickupAreaProjector.size = new Vector3(this.PickupSquareWidth * 2f, this.PickupSquareWidth * 2f, 0.2f);
			this.PickupAreaProjector.enabled = false;
			this.calculatedPickupRadius = this.PickupSquareWidth * Mathf.Sqrt(2f);
		}

		// Token: 0x04003D78 RID: 15736
		public const float MAX_VERTICAL_OFFSET = 2f;

		// Token: 0x04003D79 RID: 15737
		public TrashContainer Container;

		// Token: 0x04003D7A RID: 15738
		public ParticleSystem Flies;

		// Token: 0x04003D7B RID: 15739
		public AudioSourceController TrashAddedSound;

		// Token: 0x04003D7C RID: 15740
		public DecalProjector PickupAreaProjector;

		// Token: 0x04003D7D RID: 15741
		public Transform[] accessPoints;

		// Token: 0x04003D7E RID: 15742
		[Header("Pickup settings")]
		public bool UsableByCleaners = true;

		// Token: 0x04003D7F RID: 15743
		public float PickupSquareWidth = 3.5f;

		// Token: 0x04003D84 RID: 15748
		public List<TrashItem> TrashItemsInRadius = new List<TrashItem>();

		// Token: 0x04003D85 RID: 15749
		public List<TrashBag> TrashBagsInRadius = new List<TrashBag>();

		// Token: 0x04003D86 RID: 15750
		private float calculatedPickupRadius;

		// Token: 0x04003D87 RID: 15751
		private bool dll_Excuted;

		// Token: 0x04003D88 RID: 15752
		private bool dll_Excuted;
	}
}
