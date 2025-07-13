using System;
using System.Collections;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000177 RID: 375
	public class SpillController : MonoBehaviour
	{
		// Token: 0x06000731 RID: 1841 RVA: 0x00020628 File Offset: 0x0001E828
		private void Start()
		{
			this.lv = base.GetComponent<LiquidVolume>();
			this.dropTemplates = new GameObject[10];
			for (int i = 0; i < 10; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spill);
				gameObject.transform.localScale *= UnityEngine.Random.Range(0.45f, 0.65f);
				gameObject.GetComponent<Renderer>().material.color = Color.Lerp(this.lv.liquidColor1, this.lv.liquidColor2, UnityEngine.Random.value);
				gameObject.SetActive(false);
				this.dropTemplates[i] = gameObject;
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000206CC File Offset: 0x0001E8CC
		private void Update()
		{
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				base.transform.Rotate(Vector3.forward * Time.deltaTime * 10f);
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				base.transform.Rotate(-Vector3.forward * Time.deltaTime * 10f);
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00020740 File Offset: 0x0001E940
		private void FixedUpdate()
		{
			Vector3 a;
			float num;
			if (this.lv.GetSpillPoint(out a, out num, 1f, LEVEL_COMPENSATION.None))
			{
				for (int i = 0; i < 15; i++)
				{
					int num2 = UnityEngine.Random.Range(0, 10);
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dropTemplates[num2]);
					gameObject.SetActive(true);
					Rigidbody component = gameObject.GetComponent<Rigidbody>();
					component.transform.position = a + UnityEngine.Random.insideUnitSphere * 0.01f;
					component.AddForce(new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value * 0.1f - 0.2f, UnityEngine.Random.value - 0.5f));
					base.StartCoroutine(this.DestroySpill(gameObject));
				}
				this.lv.level -= num / 10f + 0.001f;
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00020820 File Offset: 0x0001EA20
		private IEnumerator DestroySpill(GameObject spill)
		{
			yield return new WaitForSeconds(1f);
			UnityEngine.Object.Destroy(spill);
			yield break;
		}

		// Token: 0x040007EA RID: 2026
		public GameObject spill;

		// Token: 0x040007EB RID: 2027
		private LiquidVolume lv;

		// Token: 0x040007EC RID: 2028
		private GameObject[] dropTemplates;

		// Token: 0x040007ED RID: 2029
		private const int DROP_TEMPLATES_COUNT = 10;
	}
}
