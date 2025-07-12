using System;
using ScheduleOne.Employees;
using ScheduleOne.Management;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000B4B RID: 2891
	public class BotanistUIElement : WorldspaceUIElement
	{
		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06004D13 RID: 19731 RVA: 0x00144FB9 File Offset: 0x001431B9
		// (set) Token: 0x06004D14 RID: 19732 RVA: 0x00144FC1 File Offset: 0x001431C1
		public Botanist AssignedBotanist { get; protected set; }

		// Token: 0x06004D15 RID: 19733 RVA: 0x00144FCC File Offset: 0x001431CC
		public void Initialize(Botanist bot)
		{
			this.AssignedBotanist = bot;
			this.AssignedBotanist.Configuration.onChanged.AddListener(new UnityAction(this.RefreshUI));
			this.TitleLabel.text = bot.fullName;
			this.RefreshUI();
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x00145028 File Offset: 0x00143228
		protected virtual void RefreshUI()
		{
			BotanistConfiguration botanistConfiguration = this.AssignedBotanist.Configuration as BotanistConfiguration;
			this.NoSupply.gameObject.SetActive(botanistConfiguration.Supplies.SelectedObject == null);
			if (botanistConfiguration.Supplies.SelectedObject != null)
			{
				this.SupplyIcon.sprite = botanistConfiguration.Supplies.SelectedObject.ItemInstance.Icon;
				this.SupplyIcon.gameObject.SetActive(true);
			}
			else
			{
				this.SupplyIcon.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.PotRects.Length; i++)
			{
				if (botanistConfiguration.AssignedStations.SelectedObjects.Count > i)
				{
					this.PotRects[i].Find("Icon").GetComponent<Image>().sprite = botanistConfiguration.AssignedStations.SelectedObjects[i].ItemInstance.Icon;
					this.PotRects[i].Find("Icon").gameObject.SetActive(true);
				}
				else
				{
					this.PotRects[i].Find("Icon").gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04003981 RID: 14721
		[Header("References")]
		public Image SupplyIcon;

		// Token: 0x04003982 RID: 14722
		public GameObject NoSupply;

		// Token: 0x04003983 RID: 14723
		public TextMeshProUGUI SupplyLabel;

		// Token: 0x04003984 RID: 14724
		public RectTransform[] PotRects;
	}
}
