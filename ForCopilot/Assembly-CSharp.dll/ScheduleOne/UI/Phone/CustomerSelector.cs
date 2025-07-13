using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AD1 RID: 2769
	public class CustomerSelector : MonoBehaviour
	{
		// Token: 0x06004A40 RID: 19008 RVA: 0x001380AC File Offset: 0x001362AC
		public void Awake()
		{
			for (int i = 0; i < Customer.UnlockedCustomers.Count; i++)
			{
				this.CreateEntry(Customer.UnlockedCustomers[i]);
			}
			Customer.onCustomerUnlocked = (Action<Customer>)Delegate.Combine(Customer.onCustomerUnlocked, new Action<Customer>(this.CreateEntry));
			this.Close();
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x00138105 File Offset: 0x00136305
		public void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 7);
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x00138119 File Offset: 0x00136319
		private void OnDestroy()
		{
			Customer.onCustomerUnlocked = (Action<Customer>)Delegate.Remove(Customer.onCustomerUnlocked, new Action<Customer>(this.CreateEntry));
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x0013813B File Offset: 0x0013633B
		private void Exit(ExitAction action)
		{
			if (action == null)
			{
				return;
			}
			if (action.Used)
			{
				return;
			}
			if (this != null && base.gameObject != null && base.gameObject.activeInHierarchy)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x0013817C File Offset: 0x0013637C
		public void Open()
		{
			for (int i = 0; i < this.customerEntries.Count; i++)
			{
				if (this.entryToCustomer[this.customerEntries[i]].AssignedDealer != null)
				{
					this.customerEntries[i].gameObject.SetActive(false);
				}
				else
				{
					this.customerEntries[i].gameObject.SetActive(true);
				}
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x000C6C29 File Offset: 0x000C4E29
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x00138200 File Offset: 0x00136400
		private void CreateEntry(Customer customer)
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ButtonPrefab, this.EntriesContainer).GetComponent<RectTransform>();
			component.Find("Mugshot").GetComponent<Image>().sprite = customer.NPC.MugshotSprite;
			component.Find("Name").GetComponent<Text>().text = customer.NPC.fullName;
			component.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.CustomerSelected(customer);
			});
			this.customerEntries.Add(component);
			this.entryToCustomer.Add(component, customer);
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x001382BC File Offset: 0x001364BC
		private void CustomerSelected(Customer customer)
		{
			if (customer.AssignedDealer == null && this.onCustomerSelected != null)
			{
				this.onCustomerSelected.Invoke(customer);
			}
			this.Close();
		}

		// Token: 0x04003696 RID: 13974
		public GameObject ButtonPrefab;

		// Token: 0x04003697 RID: 13975
		[Header("References")]
		public RectTransform EntriesContainer;

		// Token: 0x04003698 RID: 13976
		public UnityEvent<Customer> onCustomerSelected;

		// Token: 0x04003699 RID: 13977
		private List<RectTransform> customerEntries = new List<RectTransform>();

		// Token: 0x0400369A RID: 13978
		private Dictionary<RectTransform, Customer> entryToCustomer = new Dictionary<RectTransform, Customer>();
	}
}
