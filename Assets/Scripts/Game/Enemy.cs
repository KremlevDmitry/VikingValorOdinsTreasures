using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubesGame
{
    public class Enemy : MonoBehaviour
    {
        public Game Game = default;


        public void StartFight()
        {
            StopFight();
            StartCoroutine(_fighting = Fighting());
        }

        public void StopFight()
        {
            if (_fighting != null)
            {
                StopCoroutine(_fighting);
                _fighting = null;
            }
        }

        private IEnumerator _fighting = null;
        private IEnumerator Fighting()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                Game?.TryClick();
            }
        }
    }
}