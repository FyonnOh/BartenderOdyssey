using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.BartenderOdyssey {
    public class Scene2_Customer2 : MonoBehaviour
    {
        public Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        [YarnCommand("doAngryGesture")]
        public void doAngryGesture() {
            Debug.Log("do da ANGERYYYYY");
            anim.SetTrigger("Angry");
        }

        [YarnCommand("Idle")]
        public void doIdle() {
            anim.SetTrigger("Idle");
        }

    }
}
