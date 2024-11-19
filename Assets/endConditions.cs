using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endConditions : MonoBehaviour
{
        public static void NotifyObjectDestroyed(GameObject obj)
        {
            if (obj.CompareTag("Player"))
            {
                GameLose();
            }
            else if (obj.CompareTag("mainObjective"))
            {
                GameWin();
            }
            else
            {
                Debug.Log($"Object Destroyed: {obj.name}");
            }
        }

        private static void GameLose()
        {
            Debug.Log("Game Over: The Player has been destroyed.");
            //(restart level)
        }

        private static void GameWin()
        {
            Debug.Log("Victory: The main objective has been destroyed.");
            //load next level, end screen)
        }

}
