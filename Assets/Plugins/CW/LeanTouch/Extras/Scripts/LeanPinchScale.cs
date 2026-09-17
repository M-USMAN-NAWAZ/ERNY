using UnityEngine;
using CW.Common;
using System;
using System.Collections;
namespace Lean.Touch
{
	/// <summary>This component allows you to scale the current GameObject relative to the specified camera using the pinch gesture.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanPinchScale")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Pinch Scale")]
	public class LeanPinchScale : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The camera that will be used to calculate the zoom.
		/// None/null = MainCamera.</summary>
		public Camera Camera { set { _camera = value; } get { return _camera; } } [SerializeField] private Camera _camera;
		
		/// <summary>Should the scaling be performed relative to the finger center?</summary>
		public bool Relative { set { relative = value; } get { return relative; } } [SerializeField] private bool relative;

		/// <summary>Keep the bottom-center of the renderers fixed while pinching, so the object only grows upward/outward.</summary>
		public bool ScaleFromBottomCenter { set { scaleFromBottomCenter = value; } get { return scaleFromBottomCenter; } } [SerializeField] private bool scaleFromBottomCenter;

		/// <summary>Limit how small or large this object can become while pinching.</summary>
		public bool ClampScale { set { clampScale = value; } get { return clampScale; } } [SerializeField] private bool clampScale;

		public float MinScale { set { minScale = value; } get { return minScale; } } [SerializeField] private float minScale = 0.5f;

		public float MaxScale { set { maxScale = value; } get { return maxScale; } } [SerializeField] private float maxScale = 2.0f;
		
		/// <summary>The sensitivity of the scaling.
		/// 1 = Default.
		/// 2 = Double.</summary>
		public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } } [SerializeField] private float sensitivity = 1.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		public float Damping { set { damping = value; } get { return damping; } } [SerializeField] private float damping = -1.0f;

		[SerializeField]
		private Vector3 remainingScale;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		protected virtual void Update()
		{
			/*
			// Scaling disabled for AR objects. Uncomment this block if pinch scaling is needed again.

			// Store
			var oldScale = transform.localPosition;

			// Get the fingers we want to use
			var fingers = Use.UpdateAndGetFingers();

			// Calculate pinch scale, and make sure it's valid
			var pinchScale = LeanGesture.GetPinchScale(fingers);

			if (pinchScale != 1.0f)
			{
				pinchScale = Mathf.Pow(pinchScale, sensitivity);
				var bottomCenter = scaleFromBottomCenter == true ? GetBottomCenter() : transform.position;

				// Perform the translation if this is a relative scale
				if (relative == true)
				{
					var pinchScreenCenter = LeanGesture.GetScreenCenter(fingers);

					if (transform is RectTransform)
					{
						TranslateUI(pinchScale, pinchScreenCenter);
					}
					else
					{
						Translate(pinchScale, pinchScreenCenter);
					}
				}

				transform.localScale *= pinchScale;
				ClampScaleIfNeeded();
				if (scaleFromBottomCenter == true)
				{
					transform.position += bottomCenter - GetBottomCenter();
				}

				remainingScale += transform.localPosition - oldScale;
			}

			// Get t value
			var factor = CwHelper.DampenFactor(damping, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingScale = Vector3.Lerp(remainingScale, Vector3.zero, factor);

			// Shift this transform by the change in delta
			transform.localPosition = oldScale + remainingScale - newRemainingScale;

			// Update remainingDelta with the dampened value
			remainingScale = newRemainingScale;
			*/
		}

		private void ClampScaleIfNeeded()
		{
			if (clampScale == false)
			{
				return;
			}

			var safeMinScale = Mathf.Max(0.001f, Mathf.Min(minScale, maxScale));
			var safeMaxScale = Mathf.Max(safeMinScale, Mathf.Max(minScale, maxScale));
			var clampedScale = Mathf.Clamp(transform.localScale.x, safeMinScale, safeMaxScale);

			transform.localScale = Vector3.one * clampedScale;
		}

		protected virtual void TranslateUI(float pinchScale, Vector2 pinchScreenCenter)
		{
			var camera = _camera;
			
			if (camera == null)
			{
				var canvas = transform.GetComponentInParent<Canvas>();

				if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				{
					//camera = canvas.worldCamera;
				}
			}

			// Screen position of the transform
			var screenPoint = RectTransformUtility.WorldToScreenPoint(camera, transform.position);

			// Push the screen position away from the reference point based on the scale
			screenPoint.x = pinchScreenCenter.x + (screenPoint.x - pinchScreenCenter.x) * pinchScale;
			screenPoint.y = pinchScreenCenter.y + (screenPoint.y - pinchScreenCenter.y) * pinchScale;

			// Convert back to world space
			var worldPoint = default(Vector3);

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, screenPoint, camera, out worldPoint) == true)
			{
				transform.position = worldPoint;
			}
		}

		protected virtual void Translate(float pinchScale, Vector2 screenCenter)
		{
			// Make sure the camera exists
			var camera = CwHelper.GetCamera(_camera, gameObject);

			if (camera != null)
			{
				// Screen position of the transform
				var screenPosition = camera.WorldToScreenPoint(transform.position);

				// Push the screen position away from the reference point based on the scale
				screenPosition.x = screenCenter.x + (screenPosition.x - screenCenter.x) * pinchScale;
				screenPosition.y = screenCenter.y + (screenPosition.y - screenCenter.y) * pinchScale;

				// Convert back to world space
				transform.position = camera.ScreenToWorldPoint(screenPosition);
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}

		private Vector3 GetBottomCenter()
		{
			var renderers = GetComponentsInChildren<Renderer>(true);
			var bounds = default(Bounds);
			var foundBounds = false;

			for (var i = 0; i < renderers.Length; i++)
			{
				if (renderers[i].enabled == false)
				{
					continue;
				}

				if (foundBounds == false)
				{
					bounds = renderers[i].bounds;
					foundBounds = true;
				}
				else
				{
					bounds.Encapsulate(renderers[i].bounds);
				}
			}

			if (foundBounds == false)
			{
				return transform.position;
			}

			return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
	using UnityEditor;
	using TARGET = LeanPinchScale;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET), true)]
	public class LeanPinchScale_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("Use");
			Draw("_camera", "The camera that will be used to calculate the zoom.\n\nNone/null = MainCamera.");
			Draw("relative", "Should the scaling be performed relative to the finger center?");
			Draw("scaleFromBottomCenter", "Keep the bottom-center of the renderers fixed while pinching.");
			Draw("clampScale", "Limit how small or large this object can become while pinching.");
			Draw("minScale", "The smallest local scale allowed while pinching.");
			Draw("maxScale", "The largest local scale allowed while pinching.");
			Draw("sensitivity", "The sensitivity of the scaling.\n\n1 = Default.\n\n2 = Double.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
		}
	}
}
#endif
