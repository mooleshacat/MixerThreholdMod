using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using ScheduleOne.UI.Relations;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.ContactsApp
{
	// Token: 0x02000B12 RID: 2834
	public class ContactsApp : App<ContactsApp>
	{
		// Token: 0x06004BEC RID: 19436 RVA: 0x0013EE6C File Offset: 0x0013D06C
		protected override void Start()
		{
			base.Start();
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.CirclesContainer.gameObject.SetActive(false);
				this.DemoCirclesContainer.gameObject.SetActive(false);
				this.TutorialCirclesContainer.gameObject.SetActive(true);
				this.CirclesContainer = this.TutorialCirclesContainer;
				this.RegionSelectionContainer.gameObject.SetActive(false);
			}
			else
			{
				this.DemoCirclesContainer.gameObject.SetActive(false);
				this.TutorialCirclesContainer.gameObject.SetActive(false);
				this.CirclesContainer.gameObject.SetActive(true);
				this.RegionSelectionContainer.gameObject.SetActive(true);
				ContactsApp.RegionUI[] regionUIs = this.RegionUIs;
				for (int i = 0; i < regionUIs.Length; i++)
				{
					ContactsApp.RegionUI regionUI = regionUIs[i];
					ContactsApp.RegionUI cacheReg = regionUI;
					regionUI.Button.onClick.AddListener(delegate()
					{
						this.SetSelectedRegion(cacheReg.Region);
					});
					this.RegionDict.Add(regionUI.Region, regionUI);
				}
				this.SetSelectedRegion(this.SelectedRegion);
			}
			this.RelationCircles = this.CirclesContainer.GetComponentsInChildren<RelationCircle>(true).ToList<RelationCircle>();
			using (List<RelationCircle>.Enumerator enumerator = this.RelationCircles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ContactsApp.<>c__DisplayClass20_1 CS$<>8__locals2 = new ContactsApp.<>c__DisplayClass20_1();
					CS$<>8__locals2.<>4__this = this;
					CS$<>8__locals2.rel = enumerator.Current;
					CS$<>8__locals2.rel.LoadNPCData();
					if (CS$<>8__locals2.rel.AssignedNPC == null)
					{
						Console.LogWarning("Failed to find NPC for relation circle with ID '" + CS$<>8__locals2.rel.AssignedNPC_ID + "'", null);
					}
					else
					{
						this.RegionUIs.First((ContactsApp.RegionUI x) => x.Region == CS$<>8__locals2.rel.AssignedNPC.Region).npcs.Add(CS$<>8__locals2.rel.AssignedNPC);
						using (List<NPC>.Enumerator enumerator2 = CS$<>8__locals2.rel.AssignedNPC.RelationData.Connections.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ContactsApp.<>c__DisplayClass20_2 CS$<>8__locals3 = new ContactsApp.<>c__DisplayClass20_2();
								CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals2;
								CS$<>8__locals3.other = enumerator2.Current;
								if (!(CS$<>8__locals3.other == null))
								{
									if (CS$<>8__locals3.other.Region != CS$<>8__locals3.CS$<>8__locals1.rel.AssignedNPC.Region)
									{
										Console.LogWarning(string.Concat(new string[]
										{
											"Connection between ",
											CS$<>8__locals3.CS$<>8__locals1.rel.AssignedNPC.fullName,
											" and ",
											CS$<>8__locals3.other.fullName,
											" is invalid because they are in different regions"
										}), null);
									}
									else if (!this.connections.Exists((Tuple<NPC, NPC> x) => x.Item1 == CS$<>8__locals3.CS$<>8__locals1.rel.AssignedNPC && x.Item2 == CS$<>8__locals3.other) && !this.connections.Exists((Tuple<NPC, NPC> x) => x.Item1 == CS$<>8__locals3.other && x.Item2 == CS$<>8__locals3.CS$<>8__locals1.rel.AssignedNPC))
									{
										this.connections.Add(new Tuple<NPC, NPC>(CS$<>8__locals3.CS$<>8__locals1.rel.AssignedNPC, CS$<>8__locals3.other));
										RelationCircle otherCirc = this.GetRelationCircle(CS$<>8__locals3.other.ID);
										if (otherCirc == null)
										{
											Console.LogWarning("Failed to find relation circle for NPC with ID '" + CS$<>8__locals3.other.ID + "'", null);
										}
										else
										{
											RectTransform connectionsContainer = this.ConnectionsContainer;
											if (!GameManager.IS_TUTORIAL)
											{
												connectionsContainer = this.RegionDict[CS$<>8__locals3.CS$<>8__locals1.rel.AssignedNPC.Region].ConnectionsContainer;
											}
											RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ConnectionPrefab, connectionsContainer).GetComponent<RectTransform>();
											component.anchoredPosition = (otherCirc.Rect.anchoredPosition + CS$<>8__locals3.CS$<>8__locals1.rel.Rect.anchoredPosition) / 2f;
											Vector3 vector = otherCirc.Rect.anchoredPosition - CS$<>8__locals3.CS$<>8__locals1.rel.Rect.anchoredPosition;
											float z = -Mathf.Atan2(vector.x, vector.y) * 57.29578f;
											component.localRotation = Quaternion.Euler(0f, 0f, z);
											component.sizeDelta = new Vector2(component.sizeDelta.x, Vector3.Distance(otherCirc.Rect.anchoredPosition, CS$<>8__locals3.CS$<>8__locals1.rel.Rect.anchoredPosition));
											RelationCircle cacheRel = CS$<>8__locals3.CS$<>8__locals1.rel;
											component.name = CS$<>8__locals3.CS$<>8__locals1.rel.AssignedNPC_ID + " -> " + CS$<>8__locals3.other.ID;
											component.Find("StartButton").GetComponent<Button>().onClick.AddListener(delegate()
											{
												CS$<>8__locals3.CS$<>8__locals1.<>4__this.ZoomToRect(otherCirc.Rect);
											});
											component.Find("EndButton").GetComponent<Button>().onClick.AddListener(delegate()
											{
												CS$<>8__locals3.CS$<>8__locals1.<>4__this.ZoomToRect(cacheRel.Rect);
											});
										}
									}
								}
							}
						}
					}
				}
			}
			foreach (RelationCircle relationCircle in this.RelationCircles)
			{
				RelationCircle circ = relationCircle;
				RelationCircle relationCircle2 = relationCircle;
				relationCircle2.onClicked = (Action)Delegate.Combine(relationCircle2.onClicked, new Action(delegate()
				{
					this.CircleClicked(circ);
				}));
			}
			if (this.RelationCircles.Count > 0)
			{
				this.Select(this.RelationCircles[0]);
			}
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x0013F500 File Offset: 0x0013D700
		protected override void Update()
		{
			base.Update();
			if (base.isOpen)
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.contentMoveRoutine != null)
				{
					this.StopContentMove();
				}
				if (!GameManager.IS_TUTORIAL && this.RegionSelectionIndicator.gameObject.activeSelf)
				{
					float x = this.RegionDict[this.SelectedRegion].Button.GetComponent<RectTransform>().anchoredPosition.x;
					float x2 = this.RegionSelectionIndicator.anchoredPosition.x;
					this.RegionSelectionIndicator.anchoredPosition = new Vector2(Mathf.MoveTowards(x2, x, 1500f * Time.deltaTime), this.RegionSelectionIndicator.anchoredPosition.y);
				}
			}
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x0013F5B8 File Offset: 0x0013D7B8
		private RelationCircle GetRelationCircle(string npcID)
		{
			return this.RelationCircles.Find((RelationCircle x) => x.AssignedNPC_ID.ToLower() == npcID.ToLower());
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x0013F5E9 File Offset: 0x0013D7E9
		private void CircleClicked(RelationCircle circ)
		{
			this.Select(circ);
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x0013F5F2 File Offset: 0x0013D7F2
		private void Select(RelationCircle circ)
		{
			this.DetailPanel.Open(circ.AssignedNPC);
			this.ZoomToRect(circ.Rect);
			this.SelectionIndicator.position = circ.Rect.position;
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x0013F628 File Offset: 0x0013D828
		public void SetSelectedRegion(EMapRegion region)
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				return;
			}
			this.SelectedRegion = region;
			foreach (ContactsApp.RegionUI regionUI in this.RegionUIs)
			{
				regionUI.Container.gameObject.SetActive(regionUI.Region == region);
				regionUI.ConnectionsContainer.gameObject.SetActive(regionUI.Region == region);
				regionUI.Button.interactable = (regionUI.Region != region);
			}
			MapRegionData regionData = Singleton<Map>.Instance.GetRegionData(region);
			if (regionData.IsUnlocked)
			{
				this.LockedRegionContainer.gameObject.SetActive(false);
			}
			else
			{
				this.LockedRegionContainer.gameObject.SetActive(true);
				this.RegionRankRequirementLabel.text = regionData.RankRequirement.ToString();
			}
			this.SelectedRegionIcon.sprite = regionData.RegionSprite;
			if (regionData.StartingNPCs.Length != 0)
			{
				RelationCircle relationCircle = this.GetRelationCircle(regionData.StartingNPCs[0].ID);
				if (relationCircle != null)
				{
					this.Select(relationCircle);
				}
			}
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x0013F740 File Offset: 0x0013D940
		private void ZoomToRect(RectTransform rect)
		{
			ContactsApp.<>c__DisplayClass26_0 CS$<>8__locals1 = new ContactsApp.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			this.ContentRect.pivot = new Vector2(0f, 1f);
			CS$<>8__locals1.startScale = this.ContentRect.localScale.x;
			CS$<>8__locals1.endScale = 1f;
			CS$<>8__locals1.endPos = new Vector2(-this.ContentRect.sizeDelta.x / 2f, this.ContentRect.sizeDelta.y / 2f);
			ContactsApp.<>c__DisplayClass26_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.endPos.x = CS$<>8__locals2.endPos.x - rect.anchoredPosition.x;
			ContactsApp.<>c__DisplayClass26_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.endPos.y = CS$<>8__locals3.endPos.y - rect.anchoredPosition.y;
			this.StopContentMove();
			this.ContentRect.localScale = new Vector3(CS$<>8__locals1.endScale, CS$<>8__locals1.endScale, CS$<>8__locals1.endScale);
			this.ContentRect.anchoredPosition = CS$<>8__locals1.endPos;
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x0013F83A File Offset: 0x0013DA3A
		private void StopContentMove()
		{
			if (this.contentMoveRoutine != null)
			{
				base.StopCoroutine(this.contentMoveRoutine);
			}
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x0013F850 File Offset: 0x0013DA50
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Contacts_App_Open", open.ToString(), false);
			}
			if (open)
			{
				this.DetailPanel.Open(this.DetailPanel.SelectedNPC);
			}
		}

		// Token: 0x0400386B RID: 14443
		public EMapRegion SelectedRegion;

		// Token: 0x0400386C RID: 14444
		private Dictionary<EMapRegion, ContactsApp.RegionUI> RegionDict = new Dictionary<EMapRegion, ContactsApp.RegionUI>();

		// Token: 0x0400386D RID: 14445
		[Header("References")]
		public RectTransform CirclesContainer;

		// Token: 0x0400386E RID: 14446
		public RectTransform DemoCirclesContainer;

		// Token: 0x0400386F RID: 14447
		public RectTransform TutorialCirclesContainer;

		// Token: 0x04003870 RID: 14448
		public RectTransform ConnectionsContainer;

		// Token: 0x04003871 RID: 14449
		public RectTransform ContentRect;

		// Token: 0x04003872 RID: 14450
		public RectTransform SelectionIndicator;

		// Token: 0x04003873 RID: 14451
		public ContactsDetailPanel DetailPanel;

		// Token: 0x04003874 RID: 14452
		public ContactsApp.RegionUI[] RegionUIs;

		// Token: 0x04003875 RID: 14453
		public RectTransform RegionSelectionContainer;

		// Token: 0x04003876 RID: 14454
		public RectTransform RegionSelectionIndicator;

		// Token: 0x04003877 RID: 14455
		public RectTransform LockedRegionContainer;

		// Token: 0x04003878 RID: 14456
		public Text RegionRankRequirementLabel;

		// Token: 0x04003879 RID: 14457
		public Image SelectedRegionIcon;

		// Token: 0x0400387A RID: 14458
		[Header("Prefabs")]
		public GameObject ConnectionPrefab;

		// Token: 0x0400387B RID: 14459
		private List<RelationCircle> RelationCircles = new List<RelationCircle>();

		// Token: 0x0400387C RID: 14460
		private Coroutine contentMoveRoutine;

		// Token: 0x0400387D RID: 14461
		private List<Tuple<NPC, NPC>> connections = new List<Tuple<NPC, NPC>>();

		// Token: 0x02000B13 RID: 2835
		[Serializable]
		public class RegionUI
		{
			// Token: 0x17000A82 RID: 2690
			// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x0013F8B9 File Offset: 0x0013DAB9
			// (set) Token: 0x06004BF7 RID: 19447 RVA: 0x0013F8C1 File Offset: 0x0013DAC1
			public List<NPC> npcs { get; set; } = new List<NPC>();

			// Token: 0x0400387E RID: 14462
			public EMapRegion Region;

			// Token: 0x0400387F RID: 14463
			public Button Button;

			// Token: 0x04003880 RID: 14464
			public RectTransform Container;

			// Token: 0x04003881 RID: 14465
			public RectTransform ConnectionsContainer;
		}
	}
}
