using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A85 RID: 2693
	public class RegionUnlockedCanvas : Singleton<RegionUnlockedCanvas>, IPostSleepEvent
	{
		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06004871 RID: 18545 RVA: 0x00130272 File Offset: 0x0012E472
		// (set) Token: 0x06004872 RID: 18546 RVA: 0x0013027A File Offset: 0x0012E47A
		public bool IsRunning { get; private set; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06004873 RID: 18547 RVA: 0x00130283 File Offset: 0x0012E483
		// (set) Token: 0x06004874 RID: 18548 RVA: 0x0013028B File Offset: 0x0012E48B
		public int Order { get; private set; } = 5;

		// Token: 0x06004875 RID: 18549 RVA: 0x00130294 File Offset: 0x0012E494
		public void QueueUnlocked(EMapRegion _region)
		{
			this.region = _region;
			Singleton<SleepCanvas>.Instance.AddPostSleepEvent(this);
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x001302A8 File Offset: 0x0012E4A8
		public void StartEvent()
		{
			this.IsRunning = true;
			MapRegionData regionData = Singleton<Map>.Instance.GetRegionData(this.region);
			this.RegionLabel.text = regionData.Name;
			this.RegionImage.sprite = regionData.RegionSprite;
			List<NPC> npcsInRegion = NPCManager.GetNPCsInRegion(this.region);
			int num = npcsInRegion.Count((NPC x) => x.GetComponent<Customer>() != null);
			int num2 = npcsInRegion.Count((NPC x) => x is Dealer);
			int num3 = npcsInRegion.Count((NPC x) => x is Supplier);
			this.RegionDescription.text = string.Empty;
			if (num > 0)
			{
				TextMeshProUGUI regionDescription = this.RegionDescription;
				regionDescription.text = regionDescription.text + num.ToString() + " potential customer" + ((num > 1) ? "s" : "");
			}
			if (num2 > 0)
			{
				if (this.RegionDescription.text.Length > 0)
				{
					TextMeshProUGUI regionDescription2 = this.RegionDescription;
					regionDescription2.text += "\n";
				}
				TextMeshProUGUI regionDescription3 = this.RegionDescription;
				regionDescription3.text = regionDescription3.text + num2.ToString() + " dealer" + ((num2 > 1) ? "s" : "");
			}
			if (num3 > 0)
			{
				if (this.RegionDescription.text.Length > 0)
				{
					TextMeshProUGUI regionDescription4 = this.RegionDescription;
					regionDescription4.text += "\n";
				}
				TextMeshProUGUI regionDescription5 = this.RegionDescription;
				regionDescription5.text = regionDescription5.text + num3.ToString() + " supplier" + ((num3 > 1) ? "s" : "");
			}
			this.OpenCloseAnim.Play("Rank up open");
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x0013048A File Offset: 0x0012E68A
		public void EndEvent()
		{
			if (!this.IsRunning)
			{
				return;
			}
			this.OpenCloseAnim.Play("Rank up close");
			this.IsRunning = false;
		}

		// Token: 0x0400351F RID: 13599
		public Animation OpenCloseAnim;

		// Token: 0x04003520 RID: 13600
		public TextMeshProUGUI RegionLabel;

		// Token: 0x04003521 RID: 13601
		public TextMeshProUGUI RegionDescription;

		// Token: 0x04003522 RID: 13602
		public Image RegionImage;

		// Token: 0x04003523 RID: 13603
		private EMapRegion region;
	}
}
