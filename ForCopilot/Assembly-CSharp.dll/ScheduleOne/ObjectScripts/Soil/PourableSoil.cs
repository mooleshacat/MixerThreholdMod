using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts.Soil
{
	// Token: 0x02000C56 RID: 3158
	public class PourableSoil : Pourable
	{
		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x060058FD RID: 22781 RVA: 0x0017801A File Offset: 0x0017621A
		// (set) Token: 0x060058FE RID: 22782 RVA: 0x00178022 File Offset: 0x00176222
		public int currentCut { get; protected set; }

		// Token: 0x060058FF RID: 22783 RVA: 0x0017802B File Offset: 0x0017622B
		protected override void Awake()
		{
			base.Awake();
			this.highlightScale = this.Highlights[0].transform.localScale;
			this.UpdateHighlights();
			this.ClickableEnabled = false;
		}

		// Token: 0x06005900 RID: 22784 RVA: 0x00178058 File Offset: 0x00176258
		protected override void Update()
		{
			base.Update();
			this.timeSinceStart += Time.deltaTime;
			this.UpdateHighlights();
		}

		// Token: 0x06005901 RID: 22785 RVA: 0x00178078 File Offset: 0x00176278
		private void UpdateHighlights()
		{
			if (this.Highlights[0] == null)
			{
				return;
			}
			for (int i = 0; i < this.Highlights.Length; i++)
			{
				if (this.IsOpen || i < this.currentCut)
				{
					this.Highlights[i].gameObject.SetActive(false);
				}
				else
				{
					float num = (float)i / (float)this.Highlights.Length;
					float num2 = Mathf.Sin(Mathf.Clamp(this.timeSinceStart * 5f - num, 0f, float.MaxValue)) + 1f;
					this.Highlights[i].transform.localScale = new Vector3(this.highlightScale.x * num2, this.highlightScale.y, this.highlightScale.z * num2);
				}
			}
		}

		// Token: 0x06005902 RID: 22786 RVA: 0x00178148 File Offset: 0x00176348
		protected override void PourAmount(float amount)
		{
			base.PourAmount(amount);
			this.SoilBag.localScale = new Vector3(1f, Mathf.Lerp(0.45f, 1f, this.currentQuantity / this.StartQuantity), 1f);
			if (base.IsPourPointOverPot())
			{
				if (this.TargetPot.SoilID != this.SoilDefinition.ID)
				{
					this.TargetPot.SetSoilID(this.SoilDefinition.ID);
					this.TargetPot.SetSoilUses(this.SoilDefinition.Uses);
				}
				this.TargetPot.SetSoilState(Pot.ESoilState.Flat);
				this.TargetPot.AddSoil(amount);
			}
			if (this.TargetPot.SoilLevel >= this.TargetPot.SoilCapacity)
			{
				Singleton<TaskManager>.Instance.currentTask.Success();
			}
		}

		// Token: 0x06005903 RID: 22787 RVA: 0x00178222 File Offset: 0x00176422
		protected override bool CanPour()
		{
			return this.IsOpen && base.CanPour();
		}

		// Token: 0x06005904 RID: 22788 RVA: 0x00178234 File Offset: 0x00176434
		public void Cut()
		{
			this.TopColliders[this.currentCut].enabled = false;
			this.LerpCut(this.currentCut);
			if (this.currentCut == this.Bones.Length - 1)
			{
				this.FinishCut();
			}
			this.SnipSound.AudioSource.pitch = 0.9f + (float)this.currentCut * 0.05f;
			this.SnipSound.PlayOneShot(false);
			int currentCut = this.currentCut;
			this.currentCut = currentCut + 1;
		}

		// Token: 0x06005905 RID: 22789 RVA: 0x001782BC File Offset: 0x001764BC
		private void FinishCut()
		{
			this.IsOpen = true;
			Rigidbody rigidbody = this.TopParent.gameObject.AddComponent<Rigidbody>();
			this.TopParent.transform.SetParent(null);
			rigidbody.interpolation = 1;
			rigidbody.AddRelativeForce(Vector3.forward * 1.5f, 2);
			rigidbody.AddRelativeForce(Vector3.up * 0.3f, 2);
			rigidbody.AddTorque(Vector3.up * 1.5f, 2);
			this.ClickableEnabled = true;
			if (this.onOpened != null)
			{
				this.onOpened.Invoke();
			}
			UnityEngine.Object.Destroy(this.TopParent.gameObject, 3f);
			UnityEngine.Object.Destroy(this.TopMesh.gameObject, 3f);
		}

		// Token: 0x06005906 RID: 22790 RVA: 0x00178380 File Offset: 0x00176580
		private void LerpCut(int cutIndex)
		{
			PourableSoil.<>c__DisplayClass25_0 CS$<>8__locals1 = new PourableSoil.<>c__DisplayClass25_0();
			CS$<>8__locals1.bone = this.Bones[cutIndex];
			CS$<>8__locals1.startRot = CS$<>8__locals1.bone.localRotation;
			CS$<>8__locals1.endRot = CS$<>8__locals1.bone.localRotation * Quaternion.Euler(0f, 0f, 10f);
			base.StartCoroutine(CS$<>8__locals1.<LerpCut>g__Routine|0());
		}

		// Token: 0x0400411E RID: 16670
		public const float TEAR_ANGLE = 10f;

		// Token: 0x0400411F RID: 16671
		public const float HIGHLIGHT_CYCLE_TIME = 5f;

		// Token: 0x04004120 RID: 16672
		public bool IsOpen;

		// Token: 0x04004121 RID: 16673
		public SoilDefinition SoilDefinition;

		// Token: 0x04004122 RID: 16674
		[Header("References")]
		public Transform SoilBag;

		// Token: 0x04004123 RID: 16675
		public Transform[] Bones;

		// Token: 0x04004124 RID: 16676
		public List<Collider> TopColliders;

		// Token: 0x04004125 RID: 16677
		public MeshRenderer[] Highlights;

		// Token: 0x04004126 RID: 16678
		public Transform TopParent;

		// Token: 0x04004127 RID: 16679
		public AudioSourceController SnipSound;

		// Token: 0x04004128 RID: 16680
		public SkinnedMeshRenderer TopMesh;

		// Token: 0x0400412A RID: 16682
		public UnityEvent onOpened;

		// Token: 0x0400412B RID: 16683
		private Vector3 highlightScale = Vector3.zero;

		// Token: 0x0400412C RID: 16684
		private float timeSinceStart;
	}
}
