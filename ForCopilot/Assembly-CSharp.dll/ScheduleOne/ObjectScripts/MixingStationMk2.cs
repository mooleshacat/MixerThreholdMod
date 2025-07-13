using System;
using ScheduleOne.Product;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C3A RID: 3130
	public class MixingStationMk2 : MixingStation
	{
		// Token: 0x06005730 RID: 22320 RVA: 0x00170BC2 File Offset: 0x0016EDC2
		protected override void MinPass()
		{
			base.MinPass();
			this.UpdateScreen();
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x00170BD0 File Offset: 0x0016EDD0
		public override void MixingStart()
		{
			base.MixingStart();
			this.Animation.Play("Mixing station start");
			this.EnableScreen();
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x00170BEF File Offset: 0x0016EDEF
		public override void MixingDone()
		{
			base.MixingDone();
			this.Animation.Play("Mixing station end");
			this.DisableScreen();
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x00170C10 File Offset: 0x0016EE10
		private void EnableScreen()
		{
			if (base.CurrentMixOperation == null)
			{
				return;
			}
			this.QuantityLabel.text = base.CurrentMixOperation.Quantity.ToString() + "x";
			ProductDefinition productDefinition;
			if (base.CurrentMixOperation.IsOutputKnown(out productDefinition))
			{
				this.OutputIcon.sprite = productDefinition.Icon;
				this.OutputIcon.color = Color.white;
				this.QuestionMark.gameObject.SetActive(false);
			}
			else
			{
				this.OutputIcon.sprite = Registry.GetItem(base.CurrentMixOperation.ProductID).Icon;
				this.OutputIcon.color = Color.black;
				this.QuestionMark.gameObject.SetActive(true);
			}
			this.UpdateScreen();
			this.ScreenCanvas.enabled = true;
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x00170CE4 File Offset: 0x0016EEE4
		private void UpdateScreen()
		{
			if (base.CurrentMixOperation == null)
			{
				return;
			}
			this.ProgressLabel.text = (base.GetMixTimeForCurrentOperation() - base.CurrentMixTime).ToString() + " mins remaining";
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x00170D24 File Offset: 0x0016EF24
		private void DisableScreen()
		{
			this.ScreenCanvas.enabled = false;
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x00170D3A File Offset: 0x0016EF3A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x00170D53 File Offset: 0x0016EF53
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.MixingStationMk2Assembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x00170D6C File Offset: 0x0016EF6C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x00170D7A File Offset: 0x0016EF7A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04004046 RID: 16454
		public Animation Animation;

		// Token: 0x04004047 RID: 16455
		[Header("Screen")]
		public Canvas ScreenCanvas;

		// Token: 0x04004048 RID: 16456
		public Image OutputIcon;

		// Token: 0x04004049 RID: 16457
		public RectTransform QuestionMark;

		// Token: 0x0400404A RID: 16458
		public TextMeshProUGUI QuantityLabel;

		// Token: 0x0400404B RID: 16459
		public TextMeshProUGUI ProgressLabel;

		// Token: 0x0400404C RID: 16460
		private bool dll_Excuted;

		// Token: 0x0400404D RID: 16461
		private bool dll_Excuted;
	}
}
