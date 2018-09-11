using UnityEngine;

namespace Lean.Touch
{
  public class ScaleObject : MonoBehaviour
  {
    [Tooltip("Ignore fingers with StartedOverGui?")]
    public bool IgnoreStartedOverGui = true;

    [Tooltip("Ignore fingers with IsOverGui?")]
    public bool IgnoreIsOverGui;

    [Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
    public int RequiredFingerCount;

    [Tooltip("Does scaling require an object to be selected?")]
    public LeanSelectable RequiredSelectable;

    [Tooltip("The camera that will be used to calculate the zoom (None = MainCamera)")]
    public Camera Camera;

    [Tooltip("The minimum scale value on all axes")]
    public Vector3 ScaleMin;

    [Tooltip("The maximum scale value on all axes")]
    public Vector3 ScaleMax;

    // Update is called once per frame
    void Update()
    {
      // Get the fingers we want to use
      var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount,
        RequiredSelectable);

      // Calculate pinch scale, and make sure it's valid
      var pinchScale = LeanGesture.GetPinchScale(fingers, 0);

      if (pinchScale > 0.0f)
      {
        var pinchScreenCenter = LeanGesture.GetScreenCenter(fingers);

        Translate(pinchScale, pinchScreenCenter);
      }


      // Perform the scaling
      Scale(transform.localScale * pinchScale);
    }

    protected virtual void Translate(float pinchScale, Vector2 screenCenter)
    {
      // Make sure the camera exists
      var camera = LeanTouch.GetCamera(Camera, gameObject);

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
    }

    protected virtual void Scale(Vector3 scale)
    {
      scale.x = Mathf.Clamp(scale.x, ScaleMin.x, ScaleMax.x);
      scale.y = Mathf.Clamp(scale.y, ScaleMin.y, ScaleMax.y);
      scale.z = Mathf.Clamp(scale.z, ScaleMin.z, ScaleMax.z);

      transform.localScale = scale;
    }
  }
}