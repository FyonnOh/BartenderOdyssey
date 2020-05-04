using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.BartenderOdyssey
{
    public class RotateCharacter : MonoBehaviour
    {
        public GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            // DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
            // dialogueRunner.AddCommandHandler("rotateTowardsPlayer", RotateTowardsPlayer);
        }

        private void RotateTowardsPlayer(string[] parameters, System.Action onComplete)
        {
            if (parameters.Length != 2)
            {
                Debug.LogErrorFormat("<<rotateTowardsPlayer>> expects 2 parameters");
                onComplete();
                return;
            }

            if (float.TryParse(parameters[1], out float duration))
            {
                StartCoroutine(DoRotateTowardsPlayer(duration, onComplete));
            }
            else
            {
                Debug.LogErrorFormat($"Invalid number parameter {parameters[1]} for <<rotateTowardsPlayer>>");
                onComplete();
            }
        }

        private IEnumerator DoRotateTowardsPlayer(float duration, System.Action onComplete)
        {
            Quaternion oldRotation = transform.rotation;
            Vector3 direction =  player.transform.position - transform.position;
            // Debug.Log($"Player position: {player.transform.position}; BarOwner position: {transform.position}");
            // Debug.Log($"Direction vector: {direction}");
            direction.x = direction.z = 0f;
            // direction.Normalize();

            Quaternion towards = Quaternion.Euler(transform.eulerAngles + (Vector3.up * 60));

            Vector3 originalForward = transform.forward;

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                transform.rotation = Quaternion.Slerp(oldRotation, towards, t / duration);
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
                yield return null;
            }

            onComplete();
        }
    }
}

