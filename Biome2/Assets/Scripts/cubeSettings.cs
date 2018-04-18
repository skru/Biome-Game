//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class cubeSettings : MonoBehaviour {

//    public float lifetime;

//    public static Queue<GameObject> objectPool
//    {
//        get { return PlayerIO.currentPlayerIO.objectPool; }
//    }

//    // Use this for initialization
//    void Start () {
//        //Destroy(gameObject, Random.Range(0.5f, lifetime));
//        gameObject.SetActive(false);
//        objectPool.Enqueue(gameObject);
//        //StartCoroutine(waiter());       
//        De
//    }

//    IEnumerator waiter()
//    {
//        int wait_time = Random.Range(0, 5);
//        gameObject.SetActive(false);
//        objectPool.Enqueue(gameObject);
//        yield return new WaitForSeconds(wait_time);
        
//    }
//}
