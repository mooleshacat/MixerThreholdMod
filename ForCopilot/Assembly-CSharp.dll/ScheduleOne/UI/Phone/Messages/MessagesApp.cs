using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Messaging;
using ScheduleOne.Persistence;
using ScheduleOne.UI.Tooltips;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000B08 RID: 2824
	public class MessagesApp : App<MessagesApp>
	{
		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06004BBA RID: 19386 RVA: 0x0013E3D6 File Offset: 0x0013C5D6
		// (set) Token: 0x06004BBB RID: 19387 RVA: 0x0013E3DE File Offset: 0x0013C5DE
		public MSGConversation currentConversation { get; private set; }

		// Token: 0x06004BBC RID: 19388 RVA: 0x0013E3E8 File Offset: 0x0013C5E8
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
			Singleton<LoadManager>.Instance.onPreSceneChange.RemoveListener(new UnityAction(this.Clean));
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
			this.dialoguePage.gameObject.SetActive(false);
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x0013E478 File Offset: 0x0013C678
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x0013E480 File Offset: 0x0013C680
		private void Loaded()
		{
			MessagesApp.ActiveConversations = (from x in MessagesApp.ActiveConversations
			orderby x.index
			select x).ToList<MSGConversation>();
			this.RepositionEntries();
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x0013E4BB File Offset: 0x0013C6BB
		private void Clean()
		{
			MessagesApp.Conversations.Clear();
			MessagesApp.ActiveConversations.Clear();
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x0013E4D4 File Offset: 0x0013C6D4
		public void CreateConversationUI(MSGConversation c, out RectTransform entry, out RectTransform container)
		{
			entry = UnityEngine.Object.Instantiate<GameObject>(this.conversationEntryPrefab, this.conversationEntryContainer).GetComponent<RectTransform>();
			entry.Find("Name").GetComponent<Text>().text = (c.IsSenderKnown ? c.contactName : "Unknown");
			entry.Find("IconMask/Icon").GetComponent<Image>().sprite = (c.IsSenderKnown ? c.sender.MugshotSprite : this.BlankAvatarSprite);
			entry.SetAsLastSibling();
			if (c.Categories != null && c.Categories.Count > 0)
			{
				MessagesApp.CategoryInfo categoryInfo = this.GetCategoryInfo(c.Categories[0]);
				RectTransform component = entry.Find("Category").GetComponent<RectTransform>();
				Text component2 = component.Find("Label").GetComponent<Text>();
				component2.text = categoryInfo.Name[0].ToString();
				LayoutRebuilder.ForceRebuildLayoutImmediate(component2.rectTransform);
				component.GetComponent<Image>().color = categoryInfo.Color;
				component.anchoredPosition = new Vector2(225f + entry.Find("Name").GetComponent<Text>().preferredWidth, component.anchoredPosition.y);
				component.gameObject.SetActive(true);
			}
			else
			{
				entry.Find("Category").gameObject.SetActive(false);
			}
			container = UnityEngine.Object.Instantiate<GameObject>(this.conversationContainerPrefab, this.conversationContainer).GetComponent<RectTransform>();
			this.RepositionEntries();
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x0013E658 File Offset: 0x0013C858
		public void RepositionEntries()
		{
			for (int i = 0; i < MessagesApp.ActiveConversations.Count; i++)
			{
				MessagesApp.ActiveConversations[i].RepositionEntry();
			}
			for (int j = 0; j < MessagesApp.ActiveConversations.Count; j++)
			{
				MessagesApp.ActiveConversations[j].RepositionEntry();
			}
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x0013E6AF File Offset: 0x0013C8AF
		public void ReturnButtonClicked()
		{
			if (this.currentConversation != null)
			{
				this.currentConversation.SetOpen(false);
			}
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x0013E6C5 File Offset: 0x0013C8C5
		public void RefreshNotifications()
		{
			base.SetNotificationCount(this.unreadConversations.Count);
			Singleton<HUD>.Instance.UnreadMessagesPrompt.gameObject.SetActive(this.unreadConversations.Count > 0);
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x0013E6FA File Offset: 0x0013C8FA
		public override void Exit(ExitAction exit)
		{
			if (!base.isOpen || exit.Used)
			{
				base.Exit(exit);
				return;
			}
			if (this.currentConversation != null)
			{
				this.currentConversation.SetOpen(false);
				exit.Used = true;
			}
			base.Exit(exit);
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x0013E738 File Offset: 0x0013C938
		public void SetCurrentConversation(MSGConversation conversation)
		{
			if (conversation == this.currentConversation)
			{
				return;
			}
			MSGConversation currentConversation = this.currentConversation;
			this.currentConversation = conversation;
			if (currentConversation != null)
			{
				currentConversation.SetOpen(false);
			}
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x0013E768 File Offset: 0x0013C968
		public MessagesApp.CategoryInfo GetCategoryInfo(EConversationCategory category)
		{
			return this.categoryInfos.Find((MessagesApp.CategoryInfo x) => x.Category == category);
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x0013E79C File Offset: 0x0013C99C
		public void FilterByCategory(int category)
		{
			for (int i = 0; i < this.CategoryButtons.Length; i++)
			{
				this.CategoryButtons[i].interactable = true;
			}
			for (int j = 0; j < MessagesApp.ActiveConversations.Count; j++)
			{
				MessagesApp.ActiveConversations[j].entry.gameObject.SetActive(MessagesApp.ActiveConversations[j].Categories.Contains((EConversationCategory)category));
			}
			this.ClearFilterButton.gameObject.SetActive(true);
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x0013E820 File Offset: 0x0013CA20
		public void ClearFilter()
		{
			for (int i = 0; i < MessagesApp.ActiveConversations.Count; i++)
			{
				MessagesApp.ActiveConversations[i].entry.gameObject.SetActive(true);
			}
			for (int j = 0; j < this.CategoryButtons.Length; j++)
			{
				this.CategoryButtons[j].interactable = true;
			}
			this.ClearFilterButton.gameObject.SetActive(false);
		}

		// Token: 0x04003827 RID: 14375
		public static List<MSGConversation> Conversations = new List<MSGConversation>();

		// Token: 0x04003828 RID: 14376
		public static List<MSGConversation> ActiveConversations = new List<MSGConversation>();

		// Token: 0x04003829 RID: 14377
		public List<MessagesApp.CategoryInfo> categoryInfos;

		// Token: 0x0400382A RID: 14378
		[Header("References")]
		[SerializeField]
		protected RectTransform conversationEntryContainer;

		// Token: 0x0400382B RID: 14379
		[SerializeField]
		protected RectTransform conversationContainer;

		// Token: 0x0400382C RID: 14380
		public GameObject homePage;

		// Token: 0x0400382D RID: 14381
		public GameObject dialoguePage;

		// Token: 0x0400382E RID: 14382
		public Text dialoguePageNameText;

		// Token: 0x0400382F RID: 14383
		public RectTransform relationshipContainer;

		// Token: 0x04003830 RID: 14384
		public Scrollbar relationshipScrollbar;

		// Token: 0x04003831 RID: 14385
		public Tooltip relationshipTooltip;

		// Token: 0x04003832 RID: 14386
		public RectTransform standardsContainer;

		// Token: 0x04003833 RID: 14387
		public Image standardsStar;

		// Token: 0x04003834 RID: 14388
		public Tooltip standardsTooltip;

		// Token: 0x04003835 RID: 14389
		public RectTransform iconContainerRect;

		// Token: 0x04003836 RID: 14390
		public Image iconImage;

		// Token: 0x04003837 RID: 14391
		public Sprite BlankAvatarSprite;

		// Token: 0x04003838 RID: 14392
		public DealWindowSelector DealWindowSelector;

		// Token: 0x04003839 RID: 14393
		public PhoneShopInterface PhoneShopInterface;

		// Token: 0x0400383A RID: 14394
		public CounterofferInterface CounterofferInterface;

		// Token: 0x0400383B RID: 14395
		public RectTransform ClearFilterButton;

		// Token: 0x0400383C RID: 14396
		public Button[] CategoryButtons;

		// Token: 0x0400383D RID: 14397
		public AudioSourceController MessageReceivedSound;

		// Token: 0x0400383E RID: 14398
		public AudioSourceController MessageSentSound;

		// Token: 0x0400383F RID: 14399
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject conversationEntryPrefab;

		// Token: 0x04003840 RID: 14400
		[SerializeField]
		protected GameObject conversationContainerPrefab;

		// Token: 0x04003841 RID: 14401
		public GameObject messageBubblePrefab;

		// Token: 0x04003842 RID: 14402
		public List<MSGConversation> unreadConversations = new List<MSGConversation>();

		// Token: 0x02000B09 RID: 2825
		[Serializable]
		public class CategoryInfo
		{
			// Token: 0x04003844 RID: 14404
			public EConversationCategory Category;

			// Token: 0x04003845 RID: 14405
			public string Name;

			// Token: 0x04003846 RID: 14406
			public Color Color;
		}
	}
}
