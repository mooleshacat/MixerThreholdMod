using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.Trash
{
	// Token: 0x02000865 RID: 2149
	public class TrashBag_Equippable : Equippable_Viewmodel
	{
		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06003A83 RID: 14979 RVA: 0x000F7BEC File Offset: 0x000F5DEC
		public static bool IsHoveringTrash
		{
			get
			{
				return Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.activeSelf;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06003A84 RID: 14980 RVA: 0x000F7C02 File Offset: 0x000F5E02
		// (set) Token: 0x06003A85 RID: 14981 RVA: 0x000F7C0A File Offset: 0x000F5E0A
		public bool IsBaggingTrash { get; private set; }

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06003A86 RID: 14982 RVA: 0x000F7C13 File Offset: 0x000F5E13
		// (set) Token: 0x06003A87 RID: 14983 RVA: 0x000F7C1B File Offset: 0x000F5E1B
		public bool IsPickingUpTrash { get; private set; }

		// Token: 0x06003A88 RID: 14984 RVA: 0x000F7C24 File Offset: 0x000F5E24
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(false);
			Singleton<TrashBagCanvas>.Instance.Open();
			this.PickupAreaProjector.transform.SetParent(NetworkSingleton<GameManager>.Instance.Temp);
			this.PickupAreaProjector.transform.localScale = Vector3.one;
			this.PickupAreaProjector.transform.forward = -Vector3.up;
			this.PickupAreaProjector.gameObject.SetActive(false);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x000F7CB1 File Offset: 0x000F5EB1
		public override void Unequip()
		{
			base.Unequip();
			Singleton<TrashBagCanvas>.Instance.Close();
			UnityEngine.Object.Destroy(this.PickupAreaProjector.gameObject);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x000F7CD4 File Offset: 0x000F5ED4
		protected override void Update()
		{
			base.Update();
			Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(false);
			TrashContainer hoveredTrashContainer = this.GetHoveredTrashContainer();
			this.PickupAreaProjector.gameObject.SetActive(false);
			if (this.IsBaggingTrash)
			{
				if (!GameInput.GetButton(GameInput.ButtonCode.Interact) || hoveredTrashContainer != this._baggedContainer)
				{
					this.StopBagTrash(false);
					return;
				}
				this._bagTrashTime += Time.deltaTime;
				Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
				Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
				Singleton<HUD>.Instance.ShowRadialIndicator(this._bagTrashTime / 1f);
				if (this._bagTrashTime >= 1f)
				{
					this.StopBagTrash(true);
				}
				return;
			}
			else if (this.IsPickingUpTrash)
			{
				List<TrashItem> list = new List<TrashItem>();
				RaycastHit hit;
				if (this.RaycastLook(out hit) && this.IsPickupLocationValid(hit))
				{
					list = this.GetTrashItemsAtPoint(hit.point);
				}
				if (!GameInput.GetButton(GameInput.ButtonCode.Interact) || list.Count == 0)
				{
					this.StopPickup(false);
					return;
				}
				this._pickupTrashTime += Time.deltaTime;
				Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
				Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
				Singleton<HUD>.Instance.ShowRadialIndicator(this._pickupTrashTime / 1f);
				this.PickupAreaProjector.transform.position = hit.point + Vector3.up * 0.1f;
				this.PickupAreaProjector.gameObject.SetActive(true);
				if (this._pickupTrashTime >= 1f)
				{
					this.StopPickup(true);
				}
				return;
			}
			else
			{
				if (hoveredTrashContainer != null && hoveredTrashContainer.CanBeBagged())
				{
					this._baggedContainer = hoveredTrashContainer;
					Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
					Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
					if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
					{
						this.StartBagTrash(hoveredTrashContainer);
					}
					return;
				}
				RaycastHit hit2;
				if (hoveredTrashContainer == null && this.RaycastLook(out hit2) && this.IsPickupLocationValid(hit2))
				{
					this.PickupAreaProjector.transform.position = hit2.point + Vector3.up * 0.1f;
					this.PickupAreaProjector.gameObject.SetActive(true);
					if (this.GetTrashItemsAtPoint(hit2.point).Count > 0)
					{
						this.PickupAreaProjector.fadeFactor = 0.5f;
						Singleton<TrashBagCanvas>.Instance.InputPrompt.SetLabel("Bag trash");
						Singleton<TrashBagCanvas>.Instance.InputPrompt.gameObject.SetActive(true);
						if (GameInput.GetButtonDown(GameInput.ButtonCode.Interact))
						{
							this.StartPickup();
							return;
						}
					}
					else
					{
						this.PickupAreaProjector.fadeFactor = 0.05f;
					}
				}
				return;
			}
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x000F7FB4 File Offset: 0x000F61B4
		private TrashContainer GetHoveredTrashContainer()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(2.75f, out raycastHit, Singleton<InteractionManager>.Instance.Interaction_SearchMask, true, 0f))
			{
				TrashContainer componentInParent = raycastHit.collider.GetComponentInParent<TrashContainer>();
				if (componentInParent != null)
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x000F7FFD File Offset: 0x000F61FD
		private bool RaycastLook(out RaycastHit hit)
		{
			return PlayerSingleton<PlayerCamera>.Instance.LookRaycast(3f, out hit, this.PickupLookMask, true, 0f);
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x000F801B File Offset: 0x000F621B
		private bool IsPickupLocationValid(RaycastHit hit)
		{
			return Vector3.Angle(hit.normal, Vector3.up) <= 5f;
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x000F8038 File Offset: 0x000F6238
		private List<TrashItem> GetTrashItemsAtPoint(Vector3 pos)
		{
			Collider[] array = Physics.OverlapSphere(pos, 0.45f, Singleton<InteractionManager>.Instance.Interaction_SearchMask, 2);
			List<TrashItem> list = new List<TrashItem>();
			Collider[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				TrashItem componentInParent = array2[i].GetComponentInParent<TrashItem>();
				if (componentInParent != null && componentInParent.CanGoInContainer)
				{
					list.Add(componentInParent);
				}
			}
			return list;
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x000F8097 File Offset: 0x000F6297
		private void StartBagTrash(TrashContainer container)
		{
			this.IsBaggingTrash = true;
			this._bagTrashTime = 0f;
			this._baggedContainer = container;
			this.RustleSound.Play();
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x000F80C0 File Offset: 0x000F62C0
		private void StopBagTrash(bool complete)
		{
			this.IsBaggingTrash = false;
			this._bagTrashTime = 0f;
			this.RustleSound.Stop();
			if (complete)
			{
				this._baggedContainer.BagTrash();
				this.BagSound.PlayOneShot(true);
				this.itemInstance.ChangeQuantity(-1);
			}
			this._baggedContainer = null;
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x000F8117 File Offset: 0x000F6317
		private void StartPickup()
		{
			this.IsPickingUpTrash = true;
			this._pickupTrashTime = 0f;
			this.RustleSound.Play();
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x000F8138 File Offset: 0x000F6338
		private void StopPickup(bool complete)
		{
			this.IsPickingUpTrash = false;
			this._pickupTrashTime = 0f;
			this.PickupAreaProjector.gameObject.SetActive(false);
			this.RustleSound.Stop();
			if (complete)
			{
				List<TrashItem> trashItemsAtPoint = this.GetTrashItemsAtPoint(this.PickupAreaProjector.transform.position);
				foreach (TrashItem trashItem in trashItemsAtPoint)
				{
					trashItem.DestroyTrash();
				}
				this.itemInstance.ChangeQuantity(-1);
				TrashContentData content = new TrashContentData(trashItemsAtPoint);
				NetworkSingleton<TrashManager>.Instance.CreateTrashBag(NetworkSingleton<TrashManager>.Instance.TrashBagPrefab.ID, this.PickupAreaProjector.transform.position + Vector3.up * 0.4f, Quaternion.identity, content, default(Vector3), "", false);
				this.BagSound.PlayOneShot(true);
			}
		}

		// Token: 0x04002A0A RID: 10762
		public const float TRASH_CONTAINER_INTERACT_DISTANCE = 2.75f;

		// Token: 0x04002A0B RID: 10763
		public const float BAG_TRASH_TIME = 1f;

		// Token: 0x04002A0C RID: 10764
		public const float PICKUP_RANGE = 3f;

		// Token: 0x04002A0D RID: 10765
		public const float PICKUP_AREA_RADIUS = 0.5f;

		// Token: 0x04002A10 RID: 10768
		public LayerMask PickupLookMask;

		// Token: 0x04002A11 RID: 10769
		[Header("References")]
		public DecalProjector PickupAreaProjector;

		// Token: 0x04002A12 RID: 10770
		public AudioSourceController RustleSound;

		// Token: 0x04002A13 RID: 10771
		public AudioSourceController BagSound;

		// Token: 0x04002A14 RID: 10772
		private float _bagTrashTime;

		// Token: 0x04002A15 RID: 10773
		private TrashContainer _baggedContainer;

		// Token: 0x04002A16 RID: 10774
		private float _pickupTrashTime;
	}
}
