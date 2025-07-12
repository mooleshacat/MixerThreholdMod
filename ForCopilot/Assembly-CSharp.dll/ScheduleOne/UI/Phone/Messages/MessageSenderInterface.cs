using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000B0C RID: 2828
	public class MessageSenderInterface : MonoBehaviour
	{
		// Token: 0x06004BD1 RID: 19409 RVA: 0x0013E8DC File Offset: 0x0013CADC
		public void Awake()
		{
			this.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
			this.ComposeButton.onClick.AddListener(delegate()
			{
				this.SetVisibility(MessageSenderInterface.EVisibility.Expanded);
			});
			Button[] cancelButtons = this.CancelButtons;
			for (int i = 0; i < cancelButtons.Length; i++)
			{
				cancelButtons[i].onClick.AddListener(delegate()
				{
					this.SetVisibility(MessageSenderInterface.EVisibility.Docked);
				});
			}
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x0013E93A File Offset: 0x0013CB3A
		public void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 15);
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x0013E94F File Offset: 0x0013CB4F
		private void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (this.Visibility == MessageSenderInterface.EVisibility.Expanded)
			{
				this.SetVisibility(MessageSenderInterface.EVisibility.Docked);
				exit.Used = true;
			}
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x0013E974 File Offset: 0x0013CB74
		public void SetVisibility(MessageSenderInterface.EVisibility visibility)
		{
			this.Visibility = visibility;
			RectTransform[] array = this.DockedUIElements;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(visibility == MessageSenderInterface.EVisibility.Docked);
			}
			array = this.ExpandedUIElements;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(visibility == MessageSenderInterface.EVisibility.Expanded);
			}
			if (visibility == MessageSenderInterface.EVisibility.Expanded)
			{
				this.UpdateSendables();
			}
			this.SendablesContainer.gameObject.SetActive(visibility == MessageSenderInterface.EVisibility.Expanded);
			this.Menu.anchoredPosition = new Vector2(0f, (this.Visibility == MessageSenderInterface.EVisibility.Expanded) ? this.ExpandedMenuYPos : this.DockedMenuYPos);
			base.gameObject.SetActive(visibility > MessageSenderInterface.EVisibility.Hidden);
			for (int j = 0; j < this.sendableBubbles.Count; j++)
			{
				this.sendableBubbles[j].RefreshDisplayedText();
			}
		}

		// Token: 0x06004BD5 RID: 19413 RVA: 0x0013EA54 File Offset: 0x0013CC54
		public void UpdateSendables()
		{
			for (int i = 0; i < this.sendableBubbles.Count; i++)
			{
				SendableMessage sendableMessage = this.sendableMap[this.sendableBubbles[i]];
				string text;
				if (!sendableMessage.ShouldShow())
				{
					this.sendableBubbles[i].gameObject.SetActive(false);
				}
				else if (sendableMessage.IsValid(out text))
				{
					this.sendableBubbles[i].button.interactable = true;
					this.sendableBubbles[i].gameObject.SetActive(true);
				}
				else
				{
					this.sendableBubbles[i].button.interactable = false;
					this.sendableBubbles[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06004BD6 RID: 19414 RVA: 0x0013EB20 File Offset: 0x0013CD20
		public void AddSendable(SendableMessage sendable)
		{
			MessageBubble component = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<MessagesApp>.Instance.messageBubblePrefab, this.SendablesContainer).GetComponent<MessageBubble>();
			component.SetupBubble(sendable.Text, MessageBubble.Alignment.Center, true);
			component.button.onClick.AddListener(delegate()
			{
				this.SendableSelected(sendable);
			});
			this.sendableBubbles.Add(component);
			this.sendableMap.Add(component, sendable);
			this.UpdateSendables();
		}

		// Token: 0x06004BD7 RID: 19415 RVA: 0x0013EBAF File Offset: 0x0013CDAF
		protected virtual void SendableSelected(SendableMessage sendable)
		{
			sendable.Send(true, -1);
			this.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
		}

		// Token: 0x0400384A RID: 14410
		public MessageSenderInterface.EVisibility Visibility;

		// Token: 0x0400384B RID: 14411
		[Header("Settings")]
		public float DockedMenuYPos;

		// Token: 0x0400384C RID: 14412
		public float ExpandedMenuYPos;

		// Token: 0x0400384D RID: 14413
		[Header("References")]
		public RectTransform Menu;

		// Token: 0x0400384E RID: 14414
		public RectTransform SendablesContainer;

		// Token: 0x0400384F RID: 14415
		public RectTransform[] DockedUIElements;

		// Token: 0x04003850 RID: 14416
		public RectTransform[] ExpandedUIElements;

		// Token: 0x04003851 RID: 14417
		public Button ComposeButton;

		// Token: 0x04003852 RID: 14418
		public Button[] CancelButtons;

		// Token: 0x04003853 RID: 14419
		private List<MessageBubble> sendableBubbles = new List<MessageBubble>();

		// Token: 0x04003854 RID: 14420
		private Dictionary<MessageBubble, SendableMessage> sendableMap = new Dictionary<MessageBubble, SendableMessage>();

		// Token: 0x02000B0D RID: 2829
		public enum EVisibility
		{
			// Token: 0x04003856 RID: 14422
			Hidden,
			// Token: 0x04003857 RID: 14423
			Docked,
			// Token: 0x04003858 RID: 14424
			Expanded
		}
	}
}
