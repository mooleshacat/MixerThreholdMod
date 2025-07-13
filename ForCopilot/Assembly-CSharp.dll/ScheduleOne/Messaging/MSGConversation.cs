using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.UI;
using ScheduleOne.UI.Phone;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Messaging
{
	// Token: 0x02000583 RID: 1411
	[Serializable]
	public class MSGConversation : ISaveable
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x0008CE6C File Offset: 0x0008B06C
		// (set) Token: 0x06002224 RID: 8740 RVA: 0x0008CE74 File Offset: 0x0008B074
		public bool IsSenderKnown { get; protected set; } = true;

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06002225 RID: 8741 RVA: 0x0008CE7D File Offset: 0x0008B07D
		// (set) Token: 0x06002226 RID: 8742 RVA: 0x0008CE85 File Offset: 0x0008B085
		public int index { get; protected set; }

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06002227 RID: 8743 RVA: 0x0008CE8E File Offset: 0x0008B08E
		// (set) Token: 0x06002228 RID: 8744 RVA: 0x0008CE96 File Offset: 0x0008B096
		public bool isOpen { get; protected set; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06002229 RID: 8745 RVA: 0x0008CE9F File Offset: 0x0008B09F
		// (set) Token: 0x0600222A RID: 8746 RVA: 0x0008CEA7 File Offset: 0x0008B0A7
		public bool rollingOut { get; protected set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x0600222B RID: 8747 RVA: 0x0008CEB0 File Offset: 0x0008B0B0
		// (set) Token: 0x0600222C RID: 8748 RVA: 0x0008CEB8 File Offset: 0x0008B0B8
		public bool EntryVisible { get; protected set; } = true;

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x0008CEC1 File Offset: 0x0008B0C1
		public bool AreResponsesActive
		{
			get
			{
				return this.currentResponses.Count > 0;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x0008CED1 File Offset: 0x0008B0D1
		public string SaveFolderName
		{
			get
			{
				return "MessageConversation";
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x0008CED1 File Offset: 0x0008B0D1
		public string SaveFileName
		{
			get
			{
				return "MessageConversation";
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x00047DCE File Offset: 0x00045FCE
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06002231 RID: 8753 RVA: 0x00014B5A File Offset: 0x00012D5A
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x0008CED8 File Offset: 0x0008B0D8
		// (set) Token: 0x06002233 RID: 8755 RVA: 0x0008CEE0 File Offset: 0x0008B0E0
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x0008CEE9 File Offset: 0x0008B0E9
		// (set) Token: 0x06002235 RID: 8757 RVA: 0x0008CEF1 File Offset: 0x0008B0F1
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x0008CEFA File Offset: 0x0008B0FA
		// (set) Token: 0x06002237 RID: 8759 RVA: 0x0008CF02 File Offset: 0x0008B102
		public bool HasChanged { get; set; }

		// Token: 0x06002238 RID: 8760 RVA: 0x0008CF0C File Offset: 0x0008B10C
		public MSGConversation(NPC _npc, string _contactName)
		{
			this.contactName = _contactName;
			this.sender = _npc;
			MessagesApp.Conversations.Insert(0, this);
			this.index = 0;
			NetworkSingleton<MessagingManager>.Instance.Register(_npc, this);
			this.InitializeSaveable();
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x0003DA73 File Offset: 0x0003BC73
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0008CFD5 File Offset: 0x0008B1D5
		public void SetCategories(List<EConversationCategory> cat)
		{
			this.Categories = cat;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x0008CFDE File Offset: 0x0008B1DE
		public void MoveToTop()
		{
			MessagesApp.ActiveConversations.Remove(this);
			MessagesApp.ActiveConversations.Insert(0, this);
			this.index = 0;
			PlayerSingleton<MessagesApp>.Instance.RepositionEntries();
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x0008D00C File Offset: 0x0008B20C
		protected void CreateUI()
		{
			if (this.uiCreated)
			{
				return;
			}
			this.uiCreated = true;
			PlayerSingleton<MessagesApp>.Instance.CreateConversationUI(this, out this.entry, out this.container);
			MessagesApp.ActiveConversations.Add(this);
			this.entryPreviewText = this.entry.Find("Preview").GetComponent<Text>();
			this.unreadDot = this.entry.Find("UnreadDot").GetComponent<RectTransform>();
			this.slider = this.entry.Find("Slider").GetComponent<Slider>();
			this.sliderFill = this.slider.fillRect.GetComponent<Image>();
			this.entry.Find("Button").GetComponent<Button>().onClick.AddListener(new UnityAction(this.EntryClicked));
			Button component = this.entry.Find("Hide").GetComponent<Button>();
			if (this.sender.ConversationCanBeHidden)
			{
				component.gameObject.SetActive(true);
				component.onClick.AddListener(delegate()
				{
					this.SetEntryVisibility(false);
				});
			}
			else
			{
				component.gameObject.SetActive(false);
			}
			this.scrollRectContainer = this.container.Find("ScrollContainer").GetComponent<RectTransform>();
			this.scrollRect = this.scrollRectContainer.Find("ScrollRect").GetComponent<ScrollRect>();
			this.bubbleContainer = this.scrollRect.transform.Find("Viewport/Content").GetComponent<RectTransform>();
			this.entryPreviewText.text = string.Empty;
			this.unreadDot.gameObject.SetActive(!this.read && this.messageHistory.Count > 0);
			this.responseContainer = this.container.Find("Responses").GetComponent<RectTransform>();
			this.senderInterface = this.container.Find("SenderInterface").GetComponent<MessageSenderInterface>();
			for (int i = 0; i < this.Sendables.Count; i++)
			{
				this.senderInterface.AddSendable(this.Sendables[i]);
			}
			this.RepositionEntry();
			this.SetResponseContainerVisible(false);
			this.SetOpen(false);
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x0008D237 File Offset: 0x0008B437
		public void EnsureUIExists()
		{
			if (!this.uiCreated)
			{
				this.CreateUI();
			}
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0008D248 File Offset: 0x0008B448
		protected void RefreshPreviewText()
		{
			if (this.bubbles.Count == 0)
			{
				this.entryPreviewText.text = string.Empty;
				return;
			}
			this.entryPreviewText.text = this.bubbles[this.bubbles.Count - 1].text;
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0008D29B File Offset: 0x0008B49B
		public void RepositionEntry()
		{
			if (this.entry == null)
			{
				return;
			}
			this.entry.SetSiblingIndex(MessagesApp.ActiveConversations.IndexOf(this));
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x0008D2C4 File Offset: 0x0008B4C4
		public void SetIsKnown(bool known)
		{
			this.IsSenderKnown = known;
			if (this.entry != null)
			{
				this.entry.Find("Name").GetComponent<Text>().text = (this.IsSenderKnown ? this.contactName : "Unknown");
				this.entry.Find("IconMask/Icon").GetComponent<Image>().sprite = (this.IsSenderKnown ? this.sender.MugshotSprite : PlayerSingleton<MessagesApp>.Instance.BlankAvatarSprite);
			}
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x0008D34E File Offset: 0x0008B54E
		public void EntryClicked()
		{
			this.SetOpen(true);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0008D358 File Offset: 0x0008B558
		public void SetOpen(bool open)
		{
			this.isOpen = open;
			PlayerSingleton<MessagesApp>.Instance.homePage.gameObject.SetActive(!open);
			PlayerSingleton<MessagesApp>.Instance.dialoguePage.gameObject.SetActive(open);
			if (open)
			{
				PlayerSingleton<MessagesApp>.Instance.SetCurrentConversation(this);
				PlayerSingleton<MessagesApp>.Instance.relationshipContainer.gameObject.SetActive(false);
				PlayerSingleton<MessagesApp>.Instance.standardsContainer.gameObject.SetActive(false);
				float y = 0f;
				if (this.sender.ShowRelationshipInfo)
				{
					y = 20f;
					PlayerSingleton<MessagesApp>.Instance.relationshipScrollbar.value = this.sender.RelationData.NormalizedRelationDelta;
					PlayerSingleton<MessagesApp>.Instance.relationshipTooltip.text = RelationshipCategory.GetCategory(this.sender.RelationData.RelationDelta).ToString();
					PlayerSingleton<MessagesApp>.Instance.relationshipContainer.gameObject.SetActive(true);
					Customer customer;
					if (this.sender.TryGetComponent<Customer>(out customer))
					{
						PlayerSingleton<MessagesApp>.Instance.standardsStar.color = ItemQuality.GetColor(customer.CustomerData.Standards.GetCorrespondingQuality());
						PlayerSingleton<MessagesApp>.Instance.standardsTooltip.text = customer.CustomerData.Standards.GetName() + " standards.";
						PlayerSingleton<MessagesApp>.Instance.standardsContainer.gameObject.SetActive(true);
					}
				}
				PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.text = (this.IsSenderKnown ? this.contactName : "Unknown");
				PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.rectTransform.anchoredPosition = new Vector2(-PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.preferredWidth / 2f + 30f, y);
				PlayerSingleton<MessagesApp>.Instance.iconContainerRect.anchoredPosition = new Vector2(-PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.preferredWidth / 2f - 30f, PlayerSingleton<MessagesApp>.Instance.iconContainerRect.anchoredPosition.y);
				PlayerSingleton<MessagesApp>.Instance.iconImage.sprite = (this.IsSenderKnown ? this.sender.MugshotSprite : PlayerSingleton<MessagesApp>.Instance.BlankAvatarSprite);
				this.SetRead(true);
				this.CheckSendLoop();
				for (int i = 0; i < this.responseRects.Count; i++)
				{
					this.responseRects[i].gameObject.GetComponent<MessageBubble>().RefreshDisplayedText();
				}
				for (int j = 0; j < this.bubbles.Count; j++)
				{
					this.bubbles[j].autosetPosition = false;
					this.bubbles[j].RefreshDisplayedText();
				}
			}
			else
			{
				PlayerSingleton<MessagesApp>.Instance.SetCurrentConversation(null);
			}
			this.container.gameObject.SetActive(open);
			this.SetResponseContainerVisible(this.AreResponsesActive);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x0008D63C File Offset: 0x0008B83C
		protected virtual void RenderMessage(Message m)
		{
			MessageBubble component = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<MessagesApp>.Instance.messageBubblePrefab, this.bubbleContainer).GetComponent<MessageBubble>();
			component.SetupBubble(m.text, (m.sender == Message.ESenderType.Other) ? MessageBubble.Alignment.Left : MessageBubble.Alignment.Right, false);
			float num = 0f;
			for (int i = 0; i < this.bubbles.Count; i++)
			{
				num += this.bubbles[i].height;
				num += this.bubbles[i].spacingAbove;
			}
			bool flag = false;
			if (this.messageHistory.IndexOf(m) > 0 && this.messageHistory[this.messageHistory.IndexOf(m) - 1].sender == m.sender)
			{
				flag = true;
			}
			float num2 = MessageBubble.baseBubbleSpacing;
			if (!flag)
			{
				num2 *= 10f;
			}
			if (flag && this.messageHistory[this.messageHistory.IndexOf(m) - 1].endOfGroup)
			{
				num2 *= 20f;
			}
			component.container.anchoredPosition = new Vector2(component.container.anchoredPosition.x, -num - num2 - component.height / 2f);
			component.spacingAbove = num2;
			component.showTriangle = true;
			if (flag && !this.messageHistory[this.messageHistory.IndexOf(m) - 1].endOfGroup)
			{
				this.bubbles[this.bubbles.Count - 1].showTriangle = false;
			}
			this.bubbleContainer.sizeDelta = new Vector2(this.bubbleContainer.sizeDelta.x, num + component.height + num2 + MessageBubble.baseBubbleSpacing * 10f);
			this.scrollRect.verticalNormalizedPosition = 0f;
			this.bubbles.Add(component);
			if (m.sender == Message.ESenderType.Player && PlayerSingleton<MessagesApp>.Instance.isOpen && PlayerSingleton<Phone>.Instance.IsOpen)
			{
				PlayerSingleton<MessagesApp>.Instance.MessageSentSound.Play();
			}
			else if (PlayerSingleton<Phone>.Instance.IsOpen && PlayerSingleton<MessagesApp>.Instance.isOpen && (this.isOpen || PlayerSingleton<MessagesApp>.Instance.currentConversation == null))
			{
				PlayerSingleton<MessagesApp>.Instance.MessageReceivedSound.Play();
			}
			if (this.onMessageRendered != null)
			{
				this.onMessageRendered();
			}
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x0008D88E File Offset: 0x0008BA8E
		public void SetEntryVisibility(bool v)
		{
			if (!v && !this.sender.ConversationCanBeHidden)
			{
				return;
			}
			this.EntryVisible = v;
			this.entry.gameObject.SetActive(v);
			if (!v)
			{
				this.SetRead(true);
			}
			this.HasChanged = true;
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x0008D8CC File Offset: 0x0008BACC
		public void SetRead(bool r)
		{
			this.read = r;
			if (this.read)
			{
				if (PlayerSingleton<MessagesApp>.Instance.unreadConversations.Contains(this))
				{
					PlayerSingleton<MessagesApp>.Instance.unreadConversations.Remove(this);
					PlayerSingleton<MessagesApp>.Instance.RefreshNotifications();
				}
			}
			else if (!PlayerSingleton<MessagesApp>.Instance.unreadConversations.Contains(this))
			{
				PlayerSingleton<MessagesApp>.Instance.unreadConversations.Add(this);
				PlayerSingleton<MessagesApp>.Instance.RefreshNotifications();
			}
			if (this.unreadDot != null)
			{
				this.unreadDot.gameObject.SetActive(!this.read);
			}
			this.HasChanged = true;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x0008D974 File Offset: 0x0008BB74
		public void SendMessage(Message message, bool notify = true, bool network = true)
		{
			this.EnsureUIExists();
			if (message.messageId == -1)
			{
				message.messageId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
			if (this.messageHistory.Find((Message x) => x.messageId == message.messageId) != null)
			{
				return;
			}
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendMessage(message, notify, this.sender.ID);
				return;
			}
			this.messageHistory.Add(message);
			if (this.messageHistory.Count > 10)
			{
				this.messageHistory.RemoveAt(0);
			}
			if (message.sender == Message.ESenderType.Other && notify)
			{
				this.SetEntryVisibility(true);
				if (!this.isOpen)
				{
					this.SetRead(false);
				}
				if (!this.isOpen || !PlayerSingleton<MessagesApp>.Instance.isOpen || !PlayerSingleton<Phone>.Instance.IsOpen)
				{
					Singleton<NotificationsManager>.Instance.SendNotification(this.IsSenderKnown ? this.contactName : "Unknown", message.text, PlayerSingleton<MessagesApp>.Instance.AppIcon, 5f, true);
				}
			}
			this.RenderMessage(message);
			this.RefreshPreviewText();
			this.MoveToTop();
			this.HasChanged = true;
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x0008DAC4 File Offset: 0x0008BCC4
		public void SendMessageChain(MessageChain messages, float initialDelay = 0f, bool notify = true, bool network = true)
		{
			MSGConversation.<>c__DisplayClass83_0 CS$<>8__locals1 = new MSGConversation.<>c__DisplayClass83_0();
			CS$<>8__locals1.messages = messages;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.notify = notify;
			this.EnsureUIExists();
			if (CS$<>8__locals1.messages.id == -1)
			{
				CS$<>8__locals1.messages.id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
			if (this.messageChainHistory.Find((MessageChain x) => x.id == CS$<>8__locals1.messages.id) != null)
			{
				return;
			}
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendMessageChain(CS$<>8__locals1.messages, this.sender.ID, initialDelay, CS$<>8__locals1.notify);
				return;
			}
			this.messageChainHistory.Add(CS$<>8__locals1.messages);
			this.HasChanged = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendMessageChain>g__Routine|1(CS$<>8__locals1.messages, initialDelay));
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x0008DB8C File Offset: 0x0008BD8C
		public MSGConversationData GetSaveData()
		{
			List<TextMessageData> list = new List<TextMessageData>();
			for (int i = 0; i < this.messageHistory.Count; i++)
			{
				list.Add(this.messageHistory[i].GetSaveData());
			}
			List<TextResponseData> list2 = new List<TextResponseData>();
			for (int j = 0; j < this.currentResponses.Count; j++)
			{
				list2.Add(new TextResponseData(this.currentResponses[j].text, this.currentResponses[j].label));
			}
			return new MSGConversationData(MessagesApp.ActiveConversations.IndexOf(this), this.read, list.ToArray(), list2.ToArray(), !this.EntryVisible);
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x0008DC40 File Offset: 0x0008BE40
		public virtual string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x0008DC50 File Offset: 0x0008BE50
		public virtual void Load(MSGConversationData data)
		{
			this.EnsureUIExists();
			this.index = data.ConversationIndex;
			this.SetRead(data.Read);
			if (data.MessageHistory != null)
			{
				for (int i = 0; i < data.MessageHistory.Length; i++)
				{
					Message message = new Message(data.MessageHistory[i]);
					this.messageHistory.Add(message);
					if (this.messageHistory.Count > 10)
					{
						this.messageHistory.RemoveAt(0);
					}
					this.RenderMessage(message);
				}
			}
			else
			{
				Console.LogWarning("Message history null!", null);
			}
			if (data.ActiveResponses != null)
			{
				List<Response> list = new List<Response>();
				for (int j = 0; j < data.ActiveResponses.Length; j++)
				{
					list.Add(new Response(data.ActiveResponses[j].Text, data.ActiveResponses[j].Label, null, false));
				}
				if (list.Count > 0)
				{
					this.ShowResponses(list, 0f, true);
				}
			}
			else
			{
				Console.LogWarning("Message reponses null!", null);
			}
			this.RefreshPreviewText();
			this.HasChanged = false;
			bool isHidden = data.IsHidden;
			if (data.IsHidden)
			{
				this.SetEntryVisibility(false);
			}
			if (this.onLoaded != null)
			{
				this.onLoaded();
			}
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x0008DD80 File Offset: 0x0008BF80
		public void SetSliderValue(float value, Color color)
		{
			if (this.slider == null)
			{
				return;
			}
			this.slider.value = value;
			this.sliderFill.color = color;
			this.slider.gameObject.SetActive(value > 0f);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x0008DDCC File Offset: 0x0008BFCC
		public Response GetResponse(string label)
		{
			return this.currentResponses.Find((Response x) => x.label == label);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x0008DE00 File Offset: 0x0008C000
		public void ShowResponses(List<Response> _responses, float showResponseDelay = 0f, bool network = true)
		{
			MSGConversation.<>c__DisplayClass89_0 CS$<>8__locals1 = new MSGConversation.<>c__DisplayClass89_0();
			CS$<>8__locals1.showResponseDelay = showResponseDelay;
			CS$<>8__locals1.<>4__this = this;
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.ShowResponses(this.sender.ID, _responses, CS$<>8__locals1.showResponseDelay);
				return;
			}
			this.EnsureUIExists();
			this.currentResponses = _responses;
			this.ClearResponseUI();
			for (int i = 0; i < _responses.Count; i++)
			{
				this.CreateResponseUI(_responses[i]);
			}
			if (CS$<>8__locals1.showResponseDelay == 0f)
			{
				this.SetResponseContainerVisible(true);
			}
			else
			{
				Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<ShowResponses>g__Routine|0());
			}
			this.HasChanged = true;
			if (this.onResponsesShown != null)
			{
				this.onResponsesShown();
			}
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x0008DEB4 File Offset: 0x0008C0B4
		protected void CreateResponseUI(Response r)
		{
			this.EnsureUIExists();
			MessageBubble component = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<MessagesApp>.Instance.messageBubblePrefab, this.responseContainer).GetComponent<MessageBubble>();
			float num = 5f;
			float num2 = 25f;
			component.bubble_MinWidth = this.responseContainer.rect.width - num2 * 2f;
			component.bubble_MaxWidth = this.responseContainer.rect.width - num2 * 2f;
			component.autosetPosition = false;
			component.SetupBubble(r.text, MessageBubble.Alignment.Center, true);
			float num3 = num2;
			for (int i = 0; i < this.responseRects.Count; i++)
			{
				num3 += this.responseRects[i].gameObject.GetComponent<MessageBubble>().height;
				num3 += num;
			}
			component.container.anchoredPosition = new Vector2(0f, -num3 - 35f);
			this.responseRects.Add(component.container);
			component.button.interactable = true;
			bool network = !r.disableDefaultResponseBehaviour;
			component.button.onClick.AddListener(delegate()
			{
				this.ResponseChosen(r, network);
			});
			this.responseContainer.sizeDelta = new Vector2(this.responseContainer.sizeDelta.x, num3 + component.height + num2);
			this.responseContainer.anchoredPosition = new Vector2(0f, this.responseContainer.sizeDelta.y / 2f);
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0008E068 File Offset: 0x0008C268
		private void RefreshResponseContainer()
		{
			for (int i = 0; i < this.responseRects.Count; i++)
			{
				this.responseRects[i].gameObject.GetComponent<MessageBubble>().RefreshDisplayedText();
			}
			float num = 5f;
			float num2 = 25f;
			float num3 = num2;
			for (int j = 0; j < this.responseRects.Count; j++)
			{
				num3 += this.responseRects[j].gameObject.GetComponent<MessageBubble>().height;
				num3 += num;
			}
			this.responseContainer.sizeDelta = new Vector2(this.responseContainer.sizeDelta.x, num3 + num2);
			this.responseContainer.anchoredPosition = new Vector2(0f, this.responseContainer.sizeDelta.y / 2f);
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x0008E140 File Offset: 0x0008C340
		protected void ClearResponseUI()
		{
			for (int i = 0; i < this.responseRects.Count; i++)
			{
				UnityEngine.Object.Destroy(this.responseRects[i].gameObject);
			}
			this.responseRects.Clear();
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x0008E184 File Offset: 0x0008C384
		public void SetResponseContainerVisible(bool v)
		{
			if (v)
			{
				this.scrollRectContainer.offsetMin = new Vector2(0f, this.responseContainer.sizeDelta.y);
			}
			else
			{
				this.scrollRectContainer.offsetMin = new Vector2(0f, 0f);
			}
			this.responseContainer.gameObject.SetActive(v);
			this.bubbleContainer.anchoredPosition = new Vector2(this.bubbleContainer.anchoredPosition.x, Mathf.Clamp(this.bubbleContainer.anchoredPosition.y, 1100f, float.MaxValue));
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x0008E228 File Offset: 0x0008C428
		public void ResponseChosen(Response r, bool network)
		{
			if (!this.AreResponsesActive)
			{
				return;
			}
			if (r.disableDefaultResponseBehaviour)
			{
				if (r.callback != null)
				{
					r.callback();
				}
				return;
			}
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendResponse(this.currentResponses.IndexOf(r), this.sender.ID);
				return;
			}
			this.ClearResponses(false);
			this.RenderMessage(new Message(r.text, Message.ESenderType.Player, true, -1));
			this.HasChanged = true;
			this.MoveToTop();
			if (r.callback != null)
			{
				r.callback();
			}
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x0008E2BA File Offset: 0x0008C4BA
		public void ClearResponses(bool network = false)
		{
			this.ClearResponseUI();
			this.SetResponseContainerVisible(false);
			this.currentResponses.Clear();
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.ClearResponses(this.sender.ID);
			}
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x0008E2EC File Offset: 0x0008C4EC
		public SendableMessage CreateSendableMessage(string text)
		{
			SendableMessage sendableMessage = new SendableMessage(text, this);
			this.Sendables.Add(sendableMessage);
			if (this.uiCreated)
			{
				this.senderInterface.AddSendable(sendableMessage);
			}
			return sendableMessage;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x0008E322 File Offset: 0x0008C522
		public void SendPlayerMessage(int sendableIndex, int sentIndex, bool network)
		{
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendPlayerMessage(sendableIndex, sentIndex, this.sender.ID);
				return;
			}
			this.Sendables[sendableIndex].Send(false, sentIndex);
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x0008E354 File Offset: 0x0008C554
		public void RenderPlayerMessage(SendableMessage sendable)
		{
			Message m = new Message(sendable.Text, Message.ESenderType.Player, true, -1);
			this.RenderMessage(m);
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x0008E377 File Offset: 0x0008C577
		private void CheckSendLoop()
		{
			this.CanSendNewMessage();
			PlayerSingleton<MessagesApp>.Instance.StartCoroutine(this.<CheckSendLoop>g__Loop|99_0());
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x0008E394 File Offset: 0x0008C594
		private bool CanSendNewMessage()
		{
			if (this.rollingOut)
			{
				return false;
			}
			if (this.AreResponsesActive)
			{
				return false;
			}
			return this.Sendables.FirstOrDefault((SendableMessage x) => x.ShouldShow()) != null;
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x0008E3ED File Offset: 0x0008C5ED
		[CompilerGenerated]
		private IEnumerator <CheckSendLoop>g__Loop|99_0()
		{
			while (this.isOpen)
			{
				if (this.CanSendNewMessage())
				{
					if (this.senderInterface.Visibility == MessageSenderInterface.EVisibility.Hidden)
					{
						this.senderInterface.SetVisibility(MessageSenderInterface.EVisibility.Docked);
					}
				}
				else if (this.senderInterface.Visibility != MessageSenderInterface.EVisibility.Hidden)
				{
					this.senderInterface.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
				}
				this.scrollRect.GetComponent<RectTransform>().offsetMin = new Vector2(0f, (this.senderInterface.Visibility == MessageSenderInterface.EVisibility.Docked) ? 200f : 0f);
				yield return new WaitForEndOfFrame();
			}
			this.senderInterface.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
			this.scrollRect.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
			yield break;
		}

		// Token: 0x040019F0 RID: 6640
		public const int MAX_MESSAGE_HISTORY = 10;

		// Token: 0x040019F1 RID: 6641
		public string contactName = string.Empty;

		// Token: 0x040019F2 RID: 6642
		public NPC sender;

		// Token: 0x040019F4 RID: 6644
		public List<Message> messageHistory = new List<Message>();

		// Token: 0x040019F5 RID: 6645
		public List<MessageChain> messageChainHistory = new List<MessageChain>();

		// Token: 0x040019F6 RID: 6646
		public List<MessageBubble> bubbles = new List<MessageBubble>();

		// Token: 0x040019F7 RID: 6647
		public List<SendableMessage> Sendables = new List<SendableMessage>();

		// Token: 0x040019F8 RID: 6648
		public bool read = true;

		// Token: 0x040019FD RID: 6653
		public List<EConversationCategory> Categories = new List<EConversationCategory>();

		// Token: 0x040019FE RID: 6654
		public RectTransform entry;

		// Token: 0x040019FF RID: 6655
		protected RectTransform container;

		// Token: 0x04001A00 RID: 6656
		protected RectTransform bubbleContainer;

		// Token: 0x04001A01 RID: 6657
		protected RectTransform scrollRectContainer;

		// Token: 0x04001A02 RID: 6658
		protected ScrollRect scrollRect;

		// Token: 0x04001A03 RID: 6659
		protected Text entryPreviewText;

		// Token: 0x04001A04 RID: 6660
		protected RectTransform unreadDot;

		// Token: 0x04001A05 RID: 6661
		protected Slider slider;

		// Token: 0x04001A06 RID: 6662
		protected Image sliderFill;

		// Token: 0x04001A07 RID: 6663
		protected RectTransform responseContainer;

		// Token: 0x04001A08 RID: 6664
		protected MessageSenderInterface senderInterface;

		// Token: 0x04001A09 RID: 6665
		private bool uiCreated;

		// Token: 0x04001A0A RID: 6666
		public Action onMessageRendered;

		// Token: 0x04001A0B RID: 6667
		public Action onLoaded;

		// Token: 0x04001A0C RID: 6668
		public Action onResponsesShown;

		// Token: 0x04001A0D RID: 6669
		public List<Response> currentResponses = new List<Response>();

		// Token: 0x04001A0E RID: 6670
		private List<RectTransform> responseRects = new List<RectTransform>();
	}
}
