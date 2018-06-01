using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Advanced
{
    /*@brief the trigger of Hitbox2D
   * @detail trigging collider2D events
   * @author Kay
   * @date 2018-06-01
   * @version 0.0.1
   * */
    [RequireComponent(typeof(Collider2D))]
    public class Hitbox2DTrigger : MonoBehaviour
    {
        public Hitbox2DEvent notifier;

        private Collider2D coll;

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            notifier.OnEnter(collision, coll);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            notifier.OnStay(collision, coll);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            notifier.OnExit(collision, coll);
        }
    }
}

