using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A7B RID: 2683
	public class QuestEntryHUDUI : MonoBehaviour
	{
		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06004822 RID: 18466 RVA: 0x0012F206 File Offset: 0x0012D406
		// (set) Token: 0x06004823 RID: 18467 RVA: 0x0012F20E File Offset: 0x0012D40E
		public QuestEntry QuestEntry { get; private set; }

		// Token: 0x06004824 RID: 18468 RVA: 0x0012F218 File Offset: 0x0012D418
		public void Initialize(QuestEntry entry)
		{
			this.QuestEntry = entry;
			this.MainLabel.text = entry.Title;
			QuestHUDUI hudUI = this.QuestEntry.ParentQuest.hudUI;
			hudUI.onUpdateUI = (Action)Delegate.Combine(hudUI.onUpdateUI, new Action(this.UpdateUI));
			if (this.QuestEntry.State == EQuestState.Active)
			{
				this.FadeIn();
			}
			else
			{
				this.QuestEntry.onStart.AddListener(new UnityAction(this.FadeIn));
			}
			this.QuestEntry.onEnd.AddListener(new UnityAction(this.EntryEnded));
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0012F2C0 File Offset: 0x0012D4C0
		public void Destroy()
		{
			QuestHUDUI hudUI = this.QuestEntry.ParentQuest.hudUI;
			hudUI.onUpdateUI = (Action)Delegate.Remove(hudUI.onUpdateUI, new Action(this.UpdateUI));
			this.QuestEntry.onStart.RemoveListener(new UnityAction(this.FadeIn));
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x0012F328 File Offset: 0x0012D528
		public virtual void UpdateUI()
		{
			if (this.QuestEntry.State != EQuestState.Active)
			{
				if (!this.Animation.isPlaying)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
			if (this.QuestEntry.ParentQuest.ActiveEntryCount > 1)
			{
				this.MainLabel.text = "• " + this.QuestEntry.Title;
			}
			else
			{
				this.MainLabel.text = this.QuestEntry.Title;
			}
			base.gameObject.SetActive(true);
			this.MainLabel.ForceMeshUpdate(false, false);
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x0012F3C1 File Offset: 0x0012D5C1
		private void FadeIn()
		{
			this.QuestEntry.UpdateEntryUI();
			base.transform.SetAsLastSibling();
			this.Animation.Play("Quest entry enter");
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x0012F3EA File Offset: 0x0012D5EA
		private void EntryEnded()
		{
			if (this.QuestEntry.State == EQuestState.Completed)
			{
				this.Complete();
				return;
			}
			this.FadeOut();
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0012F407 File Offset: 0x0012D607
		private void FadeOut()
		{
			this.Animation.Play("Quest entry exit");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<FadeOut>g__Routine|11_0());
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0012F42B File Offset: 0x0012D62B
		private void Complete()
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.Animation.Play("Quest entry complete");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Complete>g__Routine|12_0());
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0012F469 File Offset: 0x0012D669
		[CompilerGenerated]
		private IEnumerator <FadeOut>g__Routine|11_0()
		{
			yield return new WaitForSeconds(this.Animation.GetClip("Quest entry exit").length);
			base.gameObject.SetActive(false);
			this.QuestEntry.UpdateEntryUI();
			yield break;
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0012F478 File Offset: 0x0012D678
		[CompilerGenerated]
		private IEnumerator <Complete>g__Routine|12_0()
		{
			yield return new WaitForSeconds(3f);
			this.FadeOut();
			yield break;
		}

		// Token: 0x040034E2 RID: 13538
		[Header("References")]
		public TextMeshProUGUI MainLabel;

		// Token: 0x040034E3 RID: 13539
		public Animation Animation;
	}
}
