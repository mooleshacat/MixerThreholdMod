using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Levelling;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A82 RID: 2690
	public class RankUpCanvas : MonoBehaviour, IPostSleepEvent
	{
		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x0600485D RID: 18525 RVA: 0x0012FA04 File Offset: 0x0012DC04
		// (set) Token: 0x0600485E RID: 18526 RVA: 0x0012FA0C File Offset: 0x0012DC0C
		public bool IsRunning { get; private set; }

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x0600485F RID: 18527 RVA: 0x0012FA15 File Offset: 0x0012DC15
		// (set) Token: 0x06004860 RID: 18528 RVA: 0x0012FA1D File Offset: 0x0012DC1D
		public int Order { get; private set; }

		// Token: 0x06004861 RID: 18529 RVA: 0x0012FA28 File Offset: 0x0012DC28
		public void Start()
		{
			this.Canvas.enabled = false;
			LevelManager instance = NetworkSingleton<LevelManager>.Instance;
			instance.onRankUp = (Action<FullRank, FullRank>)Delegate.Combine(instance.onRankUp, new Action<FullRank, FullRank>(this.RankUp));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.QueuePostSleepEvent));
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0012FA82 File Offset: 0x0012DC82
		private void QueuePostSleepEvent()
		{
			if (!GameManager.IS_TUTORIAL)
			{
				Singleton<SleepCanvas>.Instance.AddPostSleepEvent(this);
			}
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0012FA98 File Offset: 0x0012DC98
		public void StartEvent()
		{
			RankUpCanvas.<>c__DisplayClass25_0 CS$<>8__locals1 = new RankUpCanvas.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			this.IsRunning = true;
			this.OpenCloseAnim.Play("Rank up open");
			int xpGained = NetworkSingleton<DailySummary>.Instance.xpGained;
			int num = NetworkSingleton<LevelManager>.Instance.TotalXP - xpGained;
			FullRank fullRank = NetworkSingleton<LevelManager>.Instance.GetFullRank(num);
			int num2 = num - NetworkSingleton<LevelManager>.Instance.GetTotalXPForRank(fullRank);
			int i = xpGained;
			CS$<>8__locals1.progressDisplays = new List<Tuple<FullRank, int, int>>();
			FullRank fullRank2 = fullRank;
			while (i > 0)
			{
				int num3 = Mathf.Min(i, NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank2.Rank));
				if (fullRank2 == fullRank)
				{
					num3 = Mathf.Min(num3, NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank2.Rank) - num2);
					CS$<>8__locals1.progressDisplays.Add(new Tuple<FullRank, int, int>(fullRank2, num2, num3 + num2));
				}
				else
				{
					CS$<>8__locals1.progressDisplays.Add(new Tuple<FullRank, int, int>(fullRank2, 0, num3));
				}
				i -= num3;
				fullRank2 = fullRank2.NextRank();
			}
			this.ProgressSlider.value = (float)num2 / (float)NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank.Rank);
			this.ProgressLabel.text = num2.ToString() + " / " + NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank.Rank).ToString() + " XP";
			this.OldRankLabel.text = FullRank.GetString(fullRank);
			this.coroutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<StartEvent>g__Routine|0());
			this.queuedRankUps.Clear();
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x0012FC28 File Offset: 0x0012DE28
		public void EndEvent()
		{
			if (!this.IsRunning)
			{
				return;
			}
			this.IsRunning = false;
			if (this.coroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.coroutine);
				this.coroutine = null;
			}
			this.OpenCloseAnim.Play();
			this.OpenCloseAnim.Play("Rank up close");
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x0012FC81 File Offset: 0x0012DE81
		public void RankUp(FullRank oldRank, FullRank newRank)
		{
			this.queuedRankUps.Add(new Tuple<FullRank, FullRank>(oldRank, newRank));
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0012FC98 File Offset: 0x0012DE98
		private void PlayRankupAnimation(FullRank oldRank, FullRank newRank, bool playSound)
		{
			this.Canvas.enabled = true;
			this.OldRankLabel.text = FullRank.GetString(oldRank);
			this.NewRankLabel.text = FullRank.GetString(newRank);
			List<Unlockable> list = new List<Unlockable>();
			if (NetworkSingleton<LevelManager>.Instance.Unlockables.ContainsKey(newRank))
			{
				list = NetworkSingleton<LevelManager>.Instance.Unlockables[newRank];
			}
			this.UnlockedItemsContainer.gameObject.SetActive(list.Count > 0);
			for (int i = 0; i < this.UnlockedItems.Length; i++)
			{
				if (i < list.Count)
				{
					this.UnlockedItems[i].Find("Icon").GetComponent<Image>().sprite = list[i].Icon;
					this.UnlockedItems[i].GetComponentInChildren<TextMeshProUGUI>().text = list[i].Title;
					this.UnlockedItems[i].gameObject.SetActive(true);
				}
				else
				{
					this.UnlockedItems[i].gameObject.SetActive(false);
				}
			}
			this.ExtraUnlocksLabel.text = ((list.Count > this.UnlockedItems.Length) ? ("+" + (list.Count - this.UnlockedItems.Length).ToString() + " more") : "");
			this.RankUpAnim.Play();
			if (playSound)
			{
				this.SoundEffect.Play();
			}
		}

		// Token: 0x040034FD RID: 13565
		public Animation OpenCloseAnim;

		// Token: 0x040034FE RID: 13566
		public Animation RankUpAnim;

		// Token: 0x040034FF RID: 13567
		public TextMeshProUGUI OldRankLabel;

		// Token: 0x04003500 RID: 13568
		public TextMeshProUGUI NewRankLabel;

		// Token: 0x04003501 RID: 13569
		public Canvas Canvas;

		// Token: 0x04003502 RID: 13570
		public GameObject UnlockedItemsContainer;

		// Token: 0x04003503 RID: 13571
		public RectTransform[] UnlockedItems;

		// Token: 0x04003504 RID: 13572
		public TextMeshProUGUI ExtraUnlocksLabel;

		// Token: 0x04003505 RID: 13573
		public AudioSourceController SoundEffect;

		// Token: 0x04003506 RID: 13574
		public Slider ProgressSlider;

		// Token: 0x04003507 RID: 13575
		public TextMeshProUGUI ProgressLabel;

		// Token: 0x04003508 RID: 13576
		public AudioSourceController BlipSound;

		// Token: 0x04003509 RID: 13577
		public AudioSourceController ClickSound;

		// Token: 0x0400350A RID: 13578
		private Coroutine coroutine;

		// Token: 0x0400350B RID: 13579
		private List<Tuple<FullRank, FullRank>> queuedRankUps = new List<Tuple<FullRank, FullRank>>();
	}
}
