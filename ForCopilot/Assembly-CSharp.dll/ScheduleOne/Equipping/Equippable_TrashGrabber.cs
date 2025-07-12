using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Equipping
{
	// Token: 0x02000956 RID: 2390
	public class Equippable_TrashGrabber : Equippable_Viewmodel
	{
		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x0011087B File Offset: 0x0010EA7B
		// (set) Token: 0x06004075 RID: 16501 RVA: 0x00110882 File Offset: 0x0010EA82
		public static Equippable_TrashGrabber Instance { get; private set; }

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06004076 RID: 16502 RVA: 0x0011088A File Offset: 0x0010EA8A
		public static bool IsEquipped
		{
			get
			{
				return Equippable_TrashGrabber.Instance != null;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x00110897 File Offset: 0x0010EA97
		// (set) Token: 0x06004078 RID: 16504 RVA: 0x0011089F File Offset: 0x0010EA9F
		private float currentDropTime { get; set; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06004079 RID: 16505 RVA: 0x001108A8 File Offset: 0x0010EAA8
		// (set) Token: 0x0600407A RID: 16506 RVA: 0x001108B0 File Offset: 0x0010EAB0
		private float timeSinceLastDrop { get; set; } = 100f;

		// Token: 0x0600407B RID: 16507 RVA: 0x001108BC File Offset: 0x0010EABC
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			this.trashGrabberInstance = (item as TrashGrabberInstance);
			TrashGrabberInstance trashGrabberInstance = this.trashGrabberInstance;
			trashGrabberInstance.onDataChanged = (Action)Delegate.Combine(trashGrabberInstance.onDataChanged, new Action(this.RefreshVisuals));
			this.defaultBinPosition = new Pose(this.Bin.localPosition, this.Bin.localRotation);
			this.defaultBinScale = this.Bin.localScale;
			Equippable_TrashGrabber.Instance = this;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("trashgrabber");
			this.RefreshVisuals();
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x00110950 File Offset: 0x0010EB50
		public override void Unequip()
		{
			base.Unequip();
			TrashGrabberInstance trashGrabberInstance = this.trashGrabberInstance;
			trashGrabberInstance.onDataChanged = (Action)Delegate.Remove(trashGrabberInstance.onDataChanged, new Action(this.RefreshVisuals));
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			Equippable_TrashGrabber.Instance = null;
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x00110990 File Offset: 0x0010EB90
		protected override void Update()
		{
			base.Update();
			this.timeSinceLastDrop += Time.deltaTime;
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.currentDropTime = Mathf.Clamp(this.currentDropTime + Time.deltaTime, 0f, this.DropTime);
				if (this.trashGrabberInstance.GetTotalSize() > 0)
				{
					if (!this.TrashDropSound.isPlaying)
					{
						this.TrashDropSound.Play();
					}
					this.TrashDropSound.VolumeMultiplier = Mathf.Lerp(this.TrashDropSound.VolumeMultiplier, 1f, Time.deltaTime * 4f);
					if (this.currentDropTime >= this.DropTime - 0.05f && this.timeSinceLastDrop >= 0.15f)
					{
						this.timeSinceLastDrop = 0f;
						this.EjectTrash();
					}
				}
				else
				{
					this.TrashDropSound.VolumeMultiplier = Mathf.Lerp(this.TrashDropSound.VolumeMultiplier, 0f, Time.deltaTime * 4f);
				}
			}
			else
			{
				this.currentDropTime = Mathf.Clamp(this.currentDropTime - Time.deltaTime, 0f, this.DropTime);
				this.TrashDropSound.VolumeMultiplier = Mathf.Lerp(this.TrashDropSound.VolumeMultiplier, 0f, Time.deltaTime * 4f);
			}
			float t = Mathf.SmoothStep(0f, 1f, this.currentDropTime / this.DropTime);
			this.Bin.localPosition = Vector3.Lerp(this.defaultBinPosition.position, this.BinRaisedPosition.localPosition, t);
			this.Bin.localRotation = Quaternion.Lerp(this.defaultBinPosition.rotation, this.BinRaisedPosition.localRotation, t);
			this.Bin.localScale = Vector3.Lerp(this.defaultBinScale, this.BinRaisedPosition.localScale, t);
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x00110B74 File Offset: 0x0010ED74
		private void EjectTrash()
		{
			if (this.trashGrabberInstance.GetTotalSize() <= 0)
			{
				return;
			}
			List<string> trashIDs = this.trashGrabberInstance.GetTrashIDs();
			string id = trashIDs[trashIDs.Count - 1];
			this.trashGrabberInstance.RemoveTrash(id, 1);
			NetworkSingleton<TrashManager>.Instance.CreateTrashItem(id, PlayerSingleton<PlayerCamera>.Instance.transform.TransformPoint(this.TrashDropOffset), UnityEngine.Random.rotation, PlayerSingleton<PlayerMovement>.Instance.Controller.velocity + PlayerSingleton<PlayerCamera>.Instance.transform.forward * this.DropForce, "", false);
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x00110C10 File Offset: 0x0010EE10
		private void OnDestroy()
		{
			if (Equippable_TrashGrabber.Instance == this)
			{
				Equippable_TrashGrabber.Instance = null;
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x00110C28 File Offset: 0x0010EE28
		public void PickupTrash(TrashItem item)
		{
			this.GrabAnim.Stop();
			this.GrabAnim.Play();
			this.trashGrabberInstance.AddTrash(item.ID, 1);
			item.DestroyTrash();
			if (this.onPickup != null)
			{
				this.onPickup.Invoke();
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x00110C77 File Offset: 0x0010EE77
		public int GetCapacity()
		{
			return 20 - this.trashGrabberInstance.GetTotalSize();
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x00110C88 File Offset: 0x0010EE88
		private void RefreshVisuals()
		{
			float num = Mathf.Clamp01((float)this.trashGrabberInstance.GetTotalSize() / 20f);
			this.TrashContent.localPosition = Vector3.Lerp(this.TrashContent_Min.localPosition, this.TrashContent_Max.localPosition, num);
			this.TrashContent.localScale = Vector3.Lerp(this.TrashContent_Min.localScale, this.TrashContent_Max.localScale, num);
			this.TrashContent.gameObject.SetActive(num > 0f);
		}

		// Token: 0x04002DE4 RID: 11748
		public const float TrashDropSpacing = 0.15f;

		// Token: 0x04002DE5 RID: 11749
		[Header("References")]
		public Transform TrashContent;

		// Token: 0x04002DE6 RID: 11750
		public Transform TrashContent_Min;

		// Token: 0x04002DE7 RID: 11751
		public Transform TrashContent_Max;

		// Token: 0x04002DE8 RID: 11752
		public Animation GrabAnim;

		// Token: 0x04002DE9 RID: 11753
		public Transform Bin;

		// Token: 0x04002DEA RID: 11754
		public Transform BinRaisedPosition;

		// Token: 0x04002DEB RID: 11755
		public AudioSourceController TrashDropSound;

		// Token: 0x04002DEC RID: 11756
		[Header("Settings")]
		public float DropTime = 0.4f;

		// Token: 0x04002DED RID: 11757
		public float DropForce = 1f;

		// Token: 0x04002DEE RID: 11758
		public Vector3 TrashDropOffset;

		// Token: 0x04002DEF RID: 11759
		public UnityEvent onPickup;

		// Token: 0x04002DF2 RID: 11762
		private TrashGrabberInstance trashGrabberInstance;

		// Token: 0x04002DF3 RID: 11763
		private Pose defaultBinPosition;

		// Token: 0x04002DF4 RID: 11764
		private Vector3 defaultBinScale;
	}
}
