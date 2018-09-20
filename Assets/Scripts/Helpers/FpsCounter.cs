/* **************************************************************************
 * FPS COUNTER
 * **************************************************************************
 * Written by: Annop "Nargus" Prapasapong
 * Created: 7 June 2012
 * *************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
  //---------------------------------------------------------------------
  // Editor
  //---------------------------------------------------------------------

  [SerializeField] private Text _text;
  
  //---------------------------------------------------------------------
  // Public
  //---------------------------------------------------------------------

  public float frequency = 0.5f;
  public int FramesPerSec { get; protected set; }

  //---------------------------------------------------------------------
  // Messages
  //---------------------------------------------------------------------

  private void Start()
  {
    StartCoroutine(FPS());
  }

  private IEnumerator FPS()
  {
    for (;;)
    {
      // Capture frame-per-second
      int lastFrameCount = Time.frameCount;
      float lastTime = Time.realtimeSinceStartup;
      yield return new WaitForSeconds(frequency);
      float timeSpan = Time.realtimeSinceStartup - lastTime;
      int frameCount = Time.frameCount - lastFrameCount;

      // Display it
      FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
      _text.text = FramesPerSec + " fps";
    }
  }
}