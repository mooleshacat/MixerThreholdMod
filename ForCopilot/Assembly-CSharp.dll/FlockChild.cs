using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class FlockChild : MonoBehaviour
{
	// Token: 0x06000195 RID: 405 RVA: 0x00008AFC File Offset: 0x00006CFC
	public void Start()
	{
		this.FindRequiredComponents();
		this.Wander(0f);
		this.SetRandomScale();
		this._thisT.position = this.findWaypoint();
		this.RandomizeStartAnimationFrame();
		this.InitAvoidanceValues();
		this._speed = this._spawner._minSpeed;
		this._spawner._activeChildren += 1f;
		this._instantiated = true;
		if (this._spawner._updateDivisor > 1)
		{
			int num = this._spawner._updateDivisor - 1;
			FlockChild._updateNextSeed++;
			this._updateSeed = FlockChild._updateNextSeed;
			FlockChild._updateNextSeed %= num;
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00008BAB File Offset: 0x00006DAB
	public void Update()
	{
		if (this._spawner._updateDivisor <= 1 || this._spawner._updateCounter == this._updateSeed)
		{
			this.SoarTimeLimit();
			this.CheckForDistanceToWaypoint();
			this.RotationBasedOnWaypointOrAvoidance();
			this.LimitRotationOfModel();
		}
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00008BE6 File Offset: 0x00006DE6
	public void OnDisable()
	{
		base.CancelInvoke();
		this._spawner._activeChildren -= 1f;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00008C08 File Offset: 0x00006E08
	public void OnEnable()
	{
		if (this._instantiated)
		{
			this._spawner._activeChildren += 1f;
			if (this._landing)
			{
				this._model.GetComponent<Animation>().Play(this._spawner._idleAnimation);
				return;
			}
			this._model.GetComponent<Animation>().Play(this._spawner._flapAnimation);
		}
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00008C78 File Offset: 0x00006E78
	public void FindRequiredComponents()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._model == null)
		{
			this._model = this._thisT.Find("Model").gameObject;
		}
		if (this._modelT == null)
		{
			this._modelT = this._model.transform;
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00008CE8 File Offset: 0x00006EE8
	public void RandomizeStartAnimationFrame()
	{
		foreach (object obj in this._model.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			animationState.time = UnityEngine.Random.value * animationState.length;
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00008D54 File Offset: 0x00006F54
	public void InitAvoidanceValues()
	{
		this._avoidValue = UnityEngine.Random.Range(0.3f, 0.1f);
		if (this._spawner._birdAvoidDistanceMax != this._spawner._birdAvoidDistanceMin)
		{
			this._avoidDistance = UnityEngine.Random.Range(this._spawner._birdAvoidDistanceMax, this._spawner._birdAvoidDistanceMin);
			return;
		}
		this._avoidDistance = this._spawner._birdAvoidDistanceMin;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00008DC4 File Offset: 0x00006FC4
	public void SetRandomScale()
	{
		float num = UnityEngine.Random.Range(this._spawner._minScale, this._spawner._maxScale);
		this._thisT.localScale = new Vector3(num, num, num);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00008E00 File Offset: 0x00007000
	public void SoarTimeLimit()
	{
		if (this._soar && this._spawner._soarMaxTime > 0f)
		{
			if (this._soarTimer > this._spawner._soarMaxTime)
			{
				this.Flap();
				this._soarTimer = 0f;
				return;
			}
			this._soarTimer += this._spawner._newDelta;
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00008E64 File Offset: 0x00007064
	public void CheckForDistanceToWaypoint()
	{
		if (!this._landing && (this._thisT.position - this._wayPoint).magnitude < this._spawner._waypointDistance + this._stuckCounter)
		{
			this.Wander(0f);
			this._stuckCounter = 0f;
			return;
		}
		if (!this._landing)
		{
			this._stuckCounter += this._spawner._newDelta;
			return;
		}
		this._stuckCounter = 0f;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00008EF0 File Offset: 0x000070F0
	public void RotationBasedOnWaypointOrAvoidance()
	{
		Vector3 vector = this._wayPoint - this._thisT.position;
		if (this._targetSpeed > -1f && vector != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(vector);
			this._thisT.rotation = Quaternion.Slerp(this._thisT.rotation, b, this._spawner._newDelta * this._damping);
		}
		if (this._spawner._childTriggerPos && (this._thisT.position - this._spawner._posBuffer).magnitude < 1f)
		{
			this._spawner.SetFlockRandomPosition();
		}
		this._speed = Mathf.Lerp(this._speed, this._targetSpeed, this._spawner._newDelta * 2.5f);
		if (this._move)
		{
			this._thisT.position += this._thisT.forward * this._speed * this._spawner._newDelta;
			if (this._avoid && this._spawner._birdAvoid)
			{
				this.Avoidance();
			}
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00009030 File Offset: 0x00007230
	public bool Avoidance()
	{
		RaycastHit raycastHit = default(RaycastHit);
		Vector3 forward = this._modelT.forward;
		bool result = false;
		Quaternion rotation = Quaternion.identity;
		Vector3 eulerAngles = Vector3.zero;
		Vector3 position = Vector3.zero;
		position = this._thisT.position;
		rotation = this._thisT.rotation;
		eulerAngles = this._thisT.rotation.eulerAngles;
		if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * this._avoidValue, ref raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.y -= (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			result = true;
		}
		else if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * -this._avoidValue, ref raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.y += (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			result = true;
		}
		if (this._spawner._birdAvoidDown && !this._landing && Physics.Raycast(this._thisT.position, -Vector3.up, ref raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.x -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			position.y += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = position;
			result = true;
		}
		else if (this._spawner._birdAvoidUp && !this._landing && Physics.Raycast(this._thisT.position, Vector3.up, ref raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.x += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			position.y -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = position;
			result = true;
		}
		return result;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x00009338 File Offset: 0x00007538
	public void LimitRotationOfModel()
	{
		Quaternion localRotation = Quaternion.identity;
		Vector3 eulerAngles = Vector3.zero;
		localRotation = this._modelT.localRotation;
		eulerAngles = localRotation.eulerAngles;
		if ((((this._soar && this._spawner._flatSoar) || (this._spawner._flatFly && !this._soar)) && this._wayPoint.y > this._thisT.position.y) || this._landing)
		{
			eulerAngles.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, -this._thisT.localEulerAngles.x, this._spawner._newDelta * 1.75f);
			localRotation.eulerAngles = eulerAngles;
			this._modelT.localRotation = localRotation;
			return;
		}
		eulerAngles.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, 0f, this._spawner._newDelta * 1.75f);
		localRotation.eulerAngles = eulerAngles;
		this._modelT.localRotation = localRotation;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00009450 File Offset: 0x00007650
	public void Wander(float delay)
	{
		if (!this._landing)
		{
			this._damping = UnityEngine.Random.Range(this._spawner._minDamping, this._spawner._maxDamping);
			this._targetSpeed = UnityEngine.Random.Range(this._spawner._minSpeed, this._spawner._maxSpeed);
			base.Invoke("SetRandomMode", delay);
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x000094B4 File Offset: 0x000076B4
	public void SetRandomMode()
	{
		base.CancelInvoke("SetRandomMode");
		if (!this._dived && UnityEngine.Random.value < this._spawner._soarFrequency)
		{
			this.Soar();
			return;
		}
		if (!this._dived && UnityEngine.Random.value < this._spawner._diveFrequency)
		{
			this.Dive();
			return;
		}
		this.Flap();
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00009514 File Offset: 0x00007714
	public void Flap()
	{
		if (this._move)
		{
			if (this._model != null)
			{
				this._model.GetComponent<Animation>().CrossFade(this._spawner._flapAnimation, 0.5f);
			}
			this._soar = false;
			this.animationSpeed();
			this._wayPoint = this.findWaypoint();
			this._dived = false;
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00009578 File Offset: 0x00007778
	public Vector3 findWaypoint()
	{
		Vector3 zero = Vector3.zero;
		zero.x = UnityEngine.Random.Range(-this._spawner._spawnSphere, this._spawner._spawnSphere) + this._spawner._posBuffer.x;
		zero.z = UnityEngine.Random.Range(-this._spawner._spawnSphereDepth, this._spawner._spawnSphereDepth) + this._spawner._posBuffer.z;
		zero.y = UnityEngine.Random.Range(-this._spawner._spawnSphereHeight, this._spawner._spawnSphereHeight) + this._spawner._posBuffer.y;
		return zero;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00009628 File Offset: 0x00007828
	public void Soar()
	{
		if (this._move)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
			this._wayPoint = this.findWaypoint();
			this._soar = true;
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00009668 File Offset: 0x00007868
	public void Dive()
	{
		if (this._spawner._soarAnimation != null)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
		}
		else
		{
			foreach (object obj in this._model.GetComponent<Animation>())
			{
				AnimationState animationState = (AnimationState)obj;
				if (this._thisT.position.y < this._wayPoint.y + 25f)
				{
					animationState.speed = 0.1f;
				}
			}
		}
		this._wayPoint = this.findWaypoint();
		this._wayPoint.y = this._wayPoint.y - this._spawner._diveValue;
		this._dived = true;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000974C File Offset: 0x0000794C
	public void animationSpeed()
	{
		foreach (object obj in this._model.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			if (!this._dived && !this._landing)
			{
				animationState.speed = UnityEngine.Random.Range(this._spawner._minAnimationSpeed, this._spawner._maxAnimationSpeed);
			}
			else
			{
				animationState.speed = this._spawner._maxAnimationSpeed;
			}
		}
	}

	// Token: 0x0400016F RID: 367
	[HideInInspector]
	public FlockController _spawner;

	// Token: 0x04000170 RID: 368
	[HideInInspector]
	public Vector3 _wayPoint;

	// Token: 0x04000171 RID: 369
	public float _speed;

	// Token: 0x04000172 RID: 370
	[HideInInspector]
	public bool _dived = true;

	// Token: 0x04000173 RID: 371
	[HideInInspector]
	public float _stuckCounter;

	// Token: 0x04000174 RID: 372
	[HideInInspector]
	public float _damping;

	// Token: 0x04000175 RID: 373
	[HideInInspector]
	public bool _soar = true;

	// Token: 0x04000176 RID: 374
	[HideInInspector]
	public bool _landing;

	// Token: 0x04000177 RID: 375
	[HideInInspector]
	public float _targetSpeed;

	// Token: 0x04000178 RID: 376
	[HideInInspector]
	public bool _move = true;

	// Token: 0x04000179 RID: 377
	public GameObject _model;

	// Token: 0x0400017A RID: 378
	public Transform _modelT;

	// Token: 0x0400017B RID: 379
	[HideInInspector]
	public float _avoidValue;

	// Token: 0x0400017C RID: 380
	[HideInInspector]
	public float _avoidDistance;

	// Token: 0x0400017D RID: 381
	private float _soarTimer;

	// Token: 0x0400017E RID: 382
	private bool _instantiated;

	// Token: 0x0400017F RID: 383
	private static int _updateNextSeed;

	// Token: 0x04000180 RID: 384
	private int _updateSeed = -1;

	// Token: 0x04000181 RID: 385
	[HideInInspector]
	public bool _avoid = true;

	// Token: 0x04000182 RID: 386
	public Transform _thisT;

	// Token: 0x04000183 RID: 387
	public Vector3 _landingPosOffset;
}
