using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnInvisible : MonoBehaviour
{
    //kill the object soon as it dissapears
        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
}
