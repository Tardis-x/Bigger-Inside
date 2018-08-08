﻿using UnityEngine;

namespace ua.org.gdg.devfest
{
  public class GoToStartPositionStateBehaviour : StateMachineBehaviour
  {
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      var speakerAnimationScript = animator.gameObject.GetComponent<SpeakerAnimationScript>();
      speakerAnimationScript.DestinateToStart();
      speakerAnimationScript.GoToCurrentDestination();
    }

    private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
      animator.gameObject.GetComponent<SpeakerAnimationScript>().LookAtTheCrowd();
    }
  }
}