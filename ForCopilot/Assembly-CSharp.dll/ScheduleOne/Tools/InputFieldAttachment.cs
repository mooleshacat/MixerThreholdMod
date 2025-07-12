using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Tools
{
	// Token: 0x0200087D RID: 2173
	public class InputFieldAttachment : MonoBehaviour
	{
		// Token: 0x06003B8B RID: 15243 RVA: 0x000FC370 File Offset: 0x000FA570
		private void Awake()
		{
			InputField inputField = base.GetComponent<InputField>();
			if (inputField != null)
			{
				EventTrigger eventTrigger = inputField.gameObject.AddComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = 9;
				entry.callback.AddListener(delegate(BaseEventData data)
				{
					this.EditStart(inputField.text);
				});
				eventTrigger.triggers.Add(entry);
				inputField.onEndEdit.AddListener(new UnityAction<string>(this.EndEdit));
			}
			TMP_InputField component = base.GetComponent<TMP_InputField>();
			if (component != null)
			{
				component.onSelect.AddListener(new UnityAction<string>(this.EditStart));
				component.onEndEdit.AddListener(new UnityAction<string>(this.EndEdit));
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x000AF09F File Offset: 0x000AD29F
		private void EditStart(string newVal)
		{
			GameInput.IsTyping = true;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x000AF0A7 File Offset: 0x000AD2A7
		private void EndEdit(string newVal)
		{
			GameInput.IsTyping = false;
		}
	}
}
