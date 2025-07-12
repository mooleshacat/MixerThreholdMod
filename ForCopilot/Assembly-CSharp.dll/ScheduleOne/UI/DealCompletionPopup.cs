using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Quests;
using ScheduleOne.UI.Relations;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A12 RID: 2578
	public class DealCompletionPopup : Singleton<DealCompletionPopup>
	{
		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x0600456A RID: 17770 RVA: 0x00123427 File Offset: 0x00121627
		// (set) Token: 0x0600456B RID: 17771 RVA: 0x0012342F File Offset: 0x0012162F
		public bool IsPlaying { get; protected set; }

		// Token: 0x0600456C RID: 17772 RVA: 0x00123438 File Offset: 0x00121638
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00123460 File Offset: 0x00121660
		public void PlayPopup(Customer customer, float satisfaction, float originalRelationshipDelta, float basePayment, List<Contract.BonusPayment> bonuses)
		{
			DealCompletionPopup.<>c__DisplayClass18_0 CS$<>8__locals1 = new DealCompletionPopup.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.customer = customer;
			CS$<>8__locals1.bonuses = bonuses;
			CS$<>8__locals1.originalRelationshipDelta = originalRelationshipDelta;
			CS$<>8__locals1.basePayment = basePayment;
			CS$<>8__locals1.satisfaction = satisfaction;
			this.IsPlaying = true;
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
			}
			this.routine = base.StartCoroutine(CS$<>8__locals1.<PlayPopup>g__Routine|0());
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x001234CC File Offset: 0x001216CC
		private void SetRelationshipLabel(float delta)
		{
			ERelationshipCategory category = RelationshipCategory.GetCategory(delta);
			this.RelationshipLabel.text = category.ToString();
			this.RelationshipLabel.color = RelationshipCategory.GetColor(category);
		}

		// Token: 0x0400321F RID: 12831
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003220 RID: 12832
		public RectTransform Container;

		// Token: 0x04003221 RID: 12833
		public CanvasGroup Group;

		// Token: 0x04003222 RID: 12834
		public Animation Anim;

		// Token: 0x04003223 RID: 12835
		public TextMeshProUGUI Title;

		// Token: 0x04003224 RID: 12836
		public TextMeshProUGUI PaymentLabel;

		// Token: 0x04003225 RID: 12837
		public TextMeshProUGUI SatisfactionValueLabel;

		// Token: 0x04003226 RID: 12838
		public RelationCircle RelationCircle;

		// Token: 0x04003227 RID: 12839
		public TextMeshProUGUI RelationshipLabel;

		// Token: 0x04003228 RID: 12840
		public Gradient SatisfactionGradient;

		// Token: 0x04003229 RID: 12841
		public AudioSourceController SoundEffect;

		// Token: 0x0400322A RID: 12842
		public TextMeshProUGUI[] BonusLabels;

		// Token: 0x0400322B RID: 12843
		private Coroutine routine;
	}
}
