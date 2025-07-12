using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Phone.Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.Map
{
	// Token: 0x02000C86 RID: 3206
	public class POI : MonoBehaviour
	{
		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060059FA RID: 23034 RVA: 0x0017B911 File Offset: 0x00179B11
		// (set) Token: 0x060059FB RID: 23035 RVA: 0x0017B919 File Offset: 0x00179B19
		public bool UISetup { get; protected set; }

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060059FC RID: 23036 RVA: 0x0017B922 File Offset: 0x00179B22
		// (set) Token: 0x060059FD RID: 23037 RVA: 0x0017B92A File Offset: 0x00179B2A
		public string MainText { get; protected set; } = string.Empty;

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060059FE RID: 23038 RVA: 0x0017B933 File Offset: 0x00179B33
		// (set) Token: 0x060059FF RID: 23039 RVA: 0x0017B93B File Offset: 0x00179B3B
		public RectTransform UI { get; protected set; }

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x06005A00 RID: 23040 RVA: 0x0017B944 File Offset: 0x00179B44
		// (set) Token: 0x06005A01 RID: 23041 RVA: 0x0017B94C File Offset: 0x00179B4C
		public RectTransform IconContainer { get; protected set; }

		// Token: 0x06005A02 RID: 23042 RVA: 0x0017B958 File Offset: 0x00179B58
		private void OnEnable()
		{
			if (this.UI == null)
			{
				if (PlayerSingleton<MapApp>.Instance == null)
				{
					base.StartCoroutine(this.<OnEnable>g__Wait|27_0());
					return;
				}
				if (this.UI == null)
				{
					this.UI = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefab, PlayerSingleton<MapApp>.Instance.PoIContainer).GetComponent<RectTransform>();
					this.InitializeUI();
				}
			}
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x0017B9C2 File Offset: 0x00179BC2
		private void OnDisable()
		{
			if (this.UI != null)
			{
				UnityEngine.Object.Destroy(this.UI.gameObject);
				this.UI = null;
			}
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x0017B9E9 File Offset: 0x00179BE9
		private void Update()
		{
			if (this.AutoUpdatePosition && PlayerSingleton<MapApp>.InstanceExists && PlayerSingleton<MapApp>.Instance.isOpen)
			{
				this.UpdatePosition();
			}
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x0017BA0C File Offset: 0x00179C0C
		public void SetMainText(string text)
		{
			this.mainTextSet = true;
			this.MainText = text;
			if (this.mainLabel != null)
			{
				this.mainLabel.text = text;
			}
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x0017BA38 File Offset: 0x00179C38
		public virtual void UpdatePosition()
		{
			if (this.UI == null)
			{
				return;
			}
			if (!Singleton<MapPositionUtility>.InstanceExists)
			{
				return;
			}
			this.UI.anchoredPosition = Singleton<MapPositionUtility>.Instance.GetMapPosition(base.transform.position);
			if (this.Rotate)
			{
				this.IconContainer.localEulerAngles = new Vector3(0f, 0f, Vector3.SignedAngle(base.transform.forward, Vector3.forward, Vector3.up));
			}
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x0017BAB8 File Offset: 0x00179CB8
		public virtual void InitializeUI()
		{
			this.mainLabel = this.UI.Find("MainLabel").GetComponent<Text>();
			if (this.mainLabel == null)
			{
				Console.LogError("Failed to find main label", null);
			}
			if (this.MainTextVisibility == POI.TextShowMode.Off || this.MainTextVisibility == POI.TextShowMode.OnHover)
			{
				this.mainLabel.enabled = false;
			}
			else
			{
				this.mainLabel.enabled = true;
			}
			this.eventTrigger = this.UI.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = 0;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.HoverStart();
			});
			this.eventTrigger.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = 1;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.HoverEnd();
			});
			this.eventTrigger.triggers.Add(entry);
			this.button = this.UI.GetComponent<Button>();
			this.button.onClick.AddListener(delegate()
			{
				this.Clicked();
			});
			this.IconContainer = this.UI.Find("IconContainer").GetComponent<RectTransform>();
			if (this.IconContainer == null)
			{
				Console.LogError("Failed to find icon container", null);
			}
			if (!this.mainTextSet)
			{
				this.SetMainText(this.DefaultMainText);
			}
			else
			{
				this.SetMainText(this.MainText);
			}
			if (this.onUICreated != null)
			{
				this.onUICreated.Invoke();
			}
			this.UISetup = true;
			this.UpdatePosition();
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x0017BC42 File Offset: 0x00179E42
		protected virtual void HoverStart()
		{
			if (this.MainTextVisibility == POI.TextShowMode.OnHover)
			{
				this.mainLabel.enabled = true;
			}
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x0017BC59 File Offset: 0x00179E59
		protected virtual void HoverEnd()
		{
			if (this.MainTextVisibility == POI.TextShowMode.OnHover)
			{
				this.mainLabel.enabled = false;
			}
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x0017BC70 File Offset: 0x00179E70
		protected virtual void Clicked()
		{
			PlayerSingleton<MapApp>.Instance.FocusPosition(this.UI.anchoredPosition);
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x0017BCB3 File Offset: 0x00179EB3
		[CompilerGenerated]
		private IEnumerator <OnEnable>g__Wait|27_0()
		{
			yield return new WaitUntil(() => PlayerSingleton<MapApp>.Instance != null);
			if (!base.enabled)
			{
				yield break;
			}
			if (this.UI == null)
			{
				this.UI = UnityEngine.Object.Instantiate<GameObject>(this.UIPrefab, PlayerSingleton<MapApp>.Instance.PoIContainer).GetComponent<RectTransform>();
				this.InitializeUI();
			}
			yield break;
		}

		// Token: 0x04004208 RID: 16904
		public POI.TextShowMode MainTextVisibility = POI.TextShowMode.Always;

		// Token: 0x04004209 RID: 16905
		public string DefaultMainText = "PoI Main Text";

		// Token: 0x0400420A RID: 16906
		public bool AutoUpdatePosition = true;

		// Token: 0x0400420B RID: 16907
		public bool Rotate;

		// Token: 0x0400420D RID: 16909
		[SerializeField]
		protected GameObject UIPrefab;

		// Token: 0x04004210 RID: 16912
		protected Text mainLabel;

		// Token: 0x04004211 RID: 16913
		protected Button button;

		// Token: 0x04004212 RID: 16914
		protected EventTrigger eventTrigger;

		// Token: 0x04004213 RID: 16915
		private bool mainTextSet;

		// Token: 0x04004214 RID: 16916
		public UnityEvent onUICreated;

		// Token: 0x02000C87 RID: 3207
		public enum TextShowMode
		{
			// Token: 0x04004216 RID: 16918
			Off,
			// Token: 0x04004217 RID: 16919
			Always,
			// Token: 0x04004218 RID: 16920
			OnHover
		}
	}
}
