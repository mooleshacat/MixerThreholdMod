using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A7E RID: 2686
	public class QuestHUDUI : MonoBehaviour
	{
		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x0600483A RID: 18490 RVA: 0x0012F57F File Offset: 0x0012D77F
		// (set) Token: 0x0600483B RID: 18491 RVA: 0x0012F587 File Offset: 0x0012D787
		public Quest Quest { get; private set; }

		// Token: 0x0600483C RID: 18492 RVA: 0x0012F590 File Offset: 0x0012D790
		public void Initialize(Quest quest)
		{
			this.Quest = quest;
			Quest quest2 = this.Quest;
			quest2.onSubtitleChanged = (Action)Delegate.Combine(quest2.onSubtitleChanged, new Action(this.UpdateMainLabel));
			UnityEngine.Object.Instantiate<RectTransform>(this.Quest.IconPrefab, base.transform.Find("Title/IconContainer")).GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 20f);
			this.UpdateUI();
			if (this.Quest.QuestState == EQuestState.Active)
			{
				this.FadeIn();
			}
			else
			{
				this.Quest.onQuestBegin.AddListener(new UnityAction(this.FadeIn));
				base.gameObject.SetActive(false);
			}
			this.Quest.onQuestEnd.AddListener(new UnityAction<EQuestState>(this.EntryEnded));
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x0012F664 File Offset: 0x0012D864
		public void Destroy()
		{
			Quest quest = this.Quest;
			quest.onSubtitleChanged = (Action)Delegate.Remove(quest.onSubtitleChanged, new Action(this.UpdateMainLabel));
			QuestEntryHUDUI[] componentsInChildren = base.GetComponentsInChildren<QuestEntryHUDUI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Destroy();
			}
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x0012F6B8 File Offset: 0x0012D8B8
		public void UpdateUI()
		{
			this.UpdateMainLabel();
			this.UpdateExpiry();
			if (this.onUpdateUI != null)
			{
				this.onUpdateUI();
			}
			this.hudUILayout.CalculateLayoutInputVertical();
			this.hudUILayout.SetLayoutVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.hudUILayout.transform);
			this.hudUILayout.enabled = false;
			this.hudUILayout.enabled = true;
			this.UpdateShade();
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<UpdateUI>g__DelayFix|13_0());
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x0012F73E File Offset: 0x0012D93E
		public void UpdateMainLabel()
		{
			this.MainLabel.text = this.Quest.GetQuestTitle() + this.Quest.Subtitle;
			this.MainLabel.ForceMeshUpdate(false, false);
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x000045B1 File Offset: 0x000027B1
		public void UpdateExpiry()
		{
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x0012F773 File Offset: 0x0012D973
		public void UpdateShade()
		{
			this.Shade.sizeDelta = new Vector2(550f, this.hudUILayout.preferredHeight + 120f);
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x0012F79B File Offset: 0x0012D99B
		public void BopIcon()
		{
			base.transform.Find("Title/IconContainer").GetComponent<Animation>().Play();
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0012F7B8 File Offset: 0x0012D9B8
		private void FadeIn()
		{
			if (this.Quest.IsTracked)
			{
				base.gameObject.SetActive(true);
			}
			this.Animation.Play("Quest enter");
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0012F7E4 File Offset: 0x0012D9E4
		private void EntryEnded(EQuestState endState)
		{
			if (endState == EQuestState.Completed)
			{
				this.Complete();
				return;
			}
			this.FadeOut();
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x0012F7F7 File Offset: 0x0012D9F7
		private void FadeOut()
		{
			this.Animation.Play("Quest exit");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<FadeOut>g__Routine|20_0());
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x0012F81B File Offset: 0x0012DA1B
		private void Complete()
		{
			this.Animation.Play("Quest complete");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Complete>g__Routine|21_0());
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x0012F852 File Offset: 0x0012DA52
		[CompilerGenerated]
		private IEnumerator <UpdateUI>g__DelayFix|13_0()
		{
			yield return new WaitForEndOfFrame();
			this.hudUILayout.CalculateLayoutInputVertical();
			this.hudUILayout.SetLayoutVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.hudUILayout.transform);
			this.hudUILayout.enabled = false;
			this.hudUILayout.enabled = true;
			this.UpdateShade();
			yield break;
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x0012F861 File Offset: 0x0012DA61
		[CompilerGenerated]
		private IEnumerator <FadeOut>g__Routine|20_0()
		{
			yield return new WaitForSeconds(0.5f);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x0012F870 File Offset: 0x0012DA70
		[CompilerGenerated]
		private IEnumerator <Complete>g__Routine|21_0()
		{
			yield return new WaitForSeconds(3f);
			this.FadeOut();
			yield break;
		}

		// Token: 0x040034EA RID: 13546
		public string CriticalTimeColor = "FF7A7A";

		// Token: 0x040034EC RID: 13548
		[Header("References")]
		public RectTransform EntryContainer;

		// Token: 0x040034ED RID: 13549
		public TextMeshProUGUI MainLabel;

		// Token: 0x040034EE RID: 13550
		public VerticalLayoutGroup hudUILayout;

		// Token: 0x040034EF RID: 13551
		public Animation Animation;

		// Token: 0x040034F0 RID: 13552
		public RectTransform Shade;

		// Token: 0x040034F1 RID: 13553
		public Action onUpdateUI;
	}
}
