using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI.Compass
{
	// Token: 0x02000B7B RID: 2939
	public class CompassManager : Singleton<CompassManager>
	{
		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x00046252 File Offset: 0x00044452
		private Transform cam
		{
			get
			{
				return PlayerSingleton<PlayerCamera>.Instance.transform;
			}
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x00149CA4 File Offset: 0x00147EA4
		protected override void Awake()
		{
			base.Awake();
			this.notchPositions = new List<Transform>(this.NotchPointContainer.GetComponentsInChildren<Transform>());
			this.notchPositions.Remove(this.NotchPointContainer);
			for (int i = 0; i < this.notchPositions.Count; i++)
			{
				GameObject original = this.NotchPrefab;
				int num = Mathf.RoundToInt((float)(i + 1) / (float)this.notchPositions.Count * 360f);
				if (num % 90 == 0)
				{
					original = this.DirectionIndicatorPrefab;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, this.NotchUIContainer);
				CompassManager.Notch notch = new CompassManager.Notch();
				notch.Rect = gameObject.GetComponent<RectTransform>();
				notch.Group = gameObject.GetComponent<CanvasGroup>();
				this.notches.Add(notch);
				if (num % 90 == 0)
				{
					string text = "N";
					if (num == 90)
					{
						text = "E";
					}
					else if (num == 180)
					{
						text = "S";
					}
					else if (num == 270)
					{
						text = "W";
					}
					notch.Rect.GetComponentInChildren<TextMeshProUGUI>().text = text;
				}
			}
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x00149DB5 File Offset: 0x00147FB5
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.Canvas.enabled = (Singleton<HUD>.Instance.canvas.enabled && this.CompassEnabled);
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x00149DE4 File Offset: 0x00147FE4
		private void FixedUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			if (Singleton<HUD>.Instance.canvas.enabled)
			{
				this.UpdateNotches();
				this.UpdateElements();
			}
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x00149E0B File Offset: 0x0014800B
		public void SetCompassEnabled(bool enabled)
		{
			this.CompassEnabled = enabled;
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x00149E14 File Offset: 0x00148014
		public void SetVisible(bool visible)
		{
			if (this.lerpContainerPositionCoroutine != null)
			{
				base.StopCoroutine(this.lerpContainerPositionCoroutine);
			}
			this.lerpContainerPositionCoroutine = base.StartCoroutine(this.<SetVisible>g__LerpContainerPosition|28_0(visible ? this.OpenYPos : this.ClosedYPos, visible));
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x00149E50 File Offset: 0x00148050
		private void UpdateNotches()
		{
			for (int i = 0; i < this.notchPositions.Count; i++)
			{
				float x;
				float num;
				this.GetCompassData(this.notchPositions[i].position, out x, out num);
				this.notches[i].Rect.anchoredPosition = new Vector2(x, 0f);
				this.notches[i].Group.alpha = num;
				this.notches[i].Rect.gameObject.SetActive(num > 0f);
			}
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x00149EEC File Offset: 0x001480EC
		private void UpdateElements()
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				this.UpdateElement(this.elements[i]);
			}
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x00149F24 File Offset: 0x00148124
		private void UpdateElement(CompassManager.Element element)
		{
			if (!element.Visible || element.Transform == null)
			{
				element.Group.alpha = 0f;
			}
			else
			{
				float x;
				float alpha;
				this.GetCompassData(element.Transform.position, out x, out alpha);
				element.Rect.anchoredPosition = new Vector2(x, 0f);
				element.Group.alpha = alpha;
				float num = Vector3.Distance(this.cam.position, element.Transform.position);
				if (num <= 50f)
				{
					element.DistanceLabel.text = Mathf.CeilToInt(num).ToString() + "m";
				}
				else
				{
					element.DistanceLabel.text = string.Empty;
				}
			}
			element.Rect.gameObject.SetActive(element.Group.alpha > 0f);
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x0014A010 File Offset: 0x00148210
		public void GetCompassData(Vector3 worldPosition, out float xPos, out float alpha)
		{
			Vector3 normalized = Vector3.ProjectOnPlane(this.cam.forward, Vector3.up).normalized;
			Vector3 to = worldPosition - this.cam.position;
			to.y = 0f;
			float num = Vector3.SignedAngle(normalized, to, Vector3.up);
			xPos = Mathf.Clamp(num / this.AngleDivisor, -1f, 1f) * this.CompassUIRange * 0.5f;
			alpha = 1f;
			if (Mathf.Abs(num) > this.FullAlphaRange)
			{
				alpha = 1f - (Mathf.Abs(num) - this.FullAlphaRange) / (this.AngleDivisor - this.FullAlphaRange);
			}
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x0014A0C4 File Offset: 0x001482C4
		public CompassManager.Element AddElement(Transform transform, RectTransform contentPrefab, bool visible = true)
		{
			CompassManager.Element element = new CompassManager.Element();
			element.Transform = transform;
			element.Rect = UnityEngine.Object.Instantiate<GameObject>(this.ElementPrefab, this.ElementUIContainer).GetComponent<RectTransform>();
			element.Group = element.Rect.GetComponent<CanvasGroup>();
			element.DistanceLabel = element.Rect.Find("Text").GetComponent<TextMeshProUGUI>();
			RectTransform component = UnityEngine.Object.Instantiate<RectTransform>(contentPrefab, element.Rect).GetComponent<RectTransform>();
			component.anchoredPosition = Vector2.zero;
			component.sizeDelta = this.ElementContentSize;
			element.Visible = visible;
			this.elements.Add(element);
			this.UpdateElement(element);
			return element;
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x0014A168 File Offset: 0x00148368
		public void RemoveElement(Transform transform, bool alsoDestroyRect = true)
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (this.elements[i].Transform == transform)
				{
					this.RemoveElement(this.elements[i], alsoDestroyRect);
					return;
				}
			}
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x0014A1B8 File Offset: 0x001483B8
		public void RemoveElement(CompassManager.Element el, bool alsoDestroyRect = true)
		{
			if (alsoDestroyRect)
			{
				UnityEngine.Object.Destroy(el.Rect.gameObject);
			}
			this.elements.Remove(el);
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x0014A263 File Offset: 0x00148463
		[CompilerGenerated]
		private IEnumerator <SetVisible>g__LerpContainerPosition|28_0(float yPos, bool visible)
		{
			if (visible)
			{
				this.Container.gameObject.SetActive(true);
			}
			float t = 0f;
			Vector2 startPos = this.Container.anchoredPosition;
			Vector2 endPos = new Vector2(startPos.x, yPos);
			while (t < 1f)
			{
				t += Time.deltaTime * 7f;
				this.Container.anchoredPosition = new Vector2(0f, Mathf.Lerp(startPos.y, endPos.y, t));
				yield return null;
			}
			this.Container.anchoredPosition = endPos;
			this.Container.gameObject.SetActive(visible);
			yield break;
		}

		// Token: 0x04003A4E RID: 14926
		public const float DISTANCE_LABEL_THRESHOLD = 50f;

		// Token: 0x04003A4F RID: 14927
		[Header("References")]
		public RectTransform Container;

		// Token: 0x04003A50 RID: 14928
		public Transform NotchPointContainer;

		// Token: 0x04003A51 RID: 14929
		public RectTransform NotchUIContainer;

		// Token: 0x04003A52 RID: 14930
		public RectTransform ElementUIContainer;

		// Token: 0x04003A53 RID: 14931
		public Canvas Canvas;

		// Token: 0x04003A54 RID: 14932
		[Header("Prefabs")]
		public GameObject DirectionIndicatorPrefab;

		// Token: 0x04003A55 RID: 14933
		public GameObject NotchPrefab;

		// Token: 0x04003A56 RID: 14934
		public GameObject ElementPrefab;

		// Token: 0x04003A57 RID: 14935
		[Header("Settings")]
		public bool CompassEnabled = true;

		// Token: 0x04003A58 RID: 14936
		public Vector2 ElementContentSize = new Vector2(20f, 20f);

		// Token: 0x04003A59 RID: 14937
		public float CompassUIRange = 800f;

		// Token: 0x04003A5A RID: 14938
		public float FullAlphaRange = 40f;

		// Token: 0x04003A5B RID: 14939
		public float AngleDivisor = 60f;

		// Token: 0x04003A5C RID: 14940
		public float ClosedYPos = 30f;

		// Token: 0x04003A5D RID: 14941
		public float OpenYPos = -50f;

		// Token: 0x04003A5E RID: 14942
		private List<Transform> notchPositions = new List<Transform>();

		// Token: 0x04003A5F RID: 14943
		private List<CompassManager.Notch> notches = new List<CompassManager.Notch>();

		// Token: 0x04003A60 RID: 14944
		private List<CompassManager.Element> elements = new List<CompassManager.Element>();

		// Token: 0x04003A61 RID: 14945
		private Coroutine lerpContainerPositionCoroutine;

		// Token: 0x02000B7C RID: 2940
		public class Notch
		{
			// Token: 0x04003A62 RID: 14946
			public RectTransform Rect;

			// Token: 0x04003A63 RID: 14947
			public CanvasGroup Group;
		}

		// Token: 0x02000B7D RID: 2941
		public class Element
		{
			// Token: 0x04003A64 RID: 14948
			public bool Visible;

			// Token: 0x04003A65 RID: 14949
			public RectTransform Rect;

			// Token: 0x04003A66 RID: 14950
			public CanvasGroup Group;

			// Token: 0x04003A67 RID: 14951
			public TextMeshProUGUI DistanceLabel;

			// Token: 0x04003A68 RID: 14952
			public Transform Transform;
		}
	}
}
