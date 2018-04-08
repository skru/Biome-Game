using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {

    public NavMeshAgent navAgent;
    bool hasLanded = false;
    public float radius = 5.0F;
    public float power = 10.0F;
    public float deathLifetime = 10F;
    public float followRadius = 200F;
    public float NoPathTime = 0.0f;

    // AlterOrDestroy
    public float maxInteractionRange = 100;
    public float damageRadius = 0.5F;
    public bool createDebris = false;
    public float debrisLifetime;
    bool alterOrDestroy = true;
    bool deltaRadius = true;
    float delayStart = 0;
    Vector3 angle;
    bool isJumping = false;
    float jumpTime = 3F;
    float offMeshCount = 10F;
    float inactiveCount = 0;
    //Transform target;

    // Use this for initialization
    public void Start()
    {
        navAgent.baseOffset = 1;
        //Transform target = PlayerIO.currentPlayerIO.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (inactiveCount < offMeshCount)
        {
            Transform target = PlayerIO.currentPlayerIO.transform;
            if (!isJumping)
            {
                //if (Input.GetKey(KeyCode.J))
                //{
                //    isJumping = true;
                //    StartCoroutine(Jumper(navAgent, target));
                //}
                if (delayStart > 5F)
                {
                    //Debug.DrawRay(transform.position, transform.forward + new Vector3(0, -0.3F, 0), Color.red);
                    if (navAgent.isOnNavMesh)
                    {
                        inactiveCount = 0;
                        if (!navAgent.pathPending)
                        {
                            if (navAgent.path.status == NavMeshPathStatus.PathInvalid || navAgent.path.status == NavMeshPathStatus.PathPartial)// 
                            {
                                //isJumping = true;
                                if (NoPathTime > 0.5F)
                                {
                                    target.LookAt(target);
                                    if (transform.position.y > target.transform.position.y)
                                    {
                                        alterOrDestroy = false;
                                        angle = new Vector3(0, -0.5F, 0);
                                    }
                                    else
                                    {
                                        alterOrDestroy = true;
                                        angle = new Vector3(0, -0.5F, 0);
                                    }
                                    //Ray ray = new Ray(transform.position, transform.forward + angle);   
                                    //World.currentWorld.AlterWorld(ray, alterOrDestroy, damageRadius, createDebris, debrisLifetime, maxInteractionRange);
                                    NoPathTime = 0;
                                    navAgent.SetDestination(target.position);
                                }
                                NoPathTime += Time.deltaTime;
                                //StartCoroutine(Jumper(navAgent, target));
                            }
                            else
                            {
                                float distance = Vector3.Distance(navAgent.transform.position, target.position);
                                //Debug.Log(distance);
                                if (distance <= 3F)
                                {
                                    navAgent.isStopped = true;
                                }
                                else
                                {
                                    navAgent.isStopped = false;
                                    navAgent.SetDestination(target.position);
                                }
                            }
                        }
                        else
                        {
                            if (NoPathTime > 0.5F)
                            {
                                target.LookAt(target);
                                if (transform.position.y > target.transform.position.y)
                                {
                                    alterOrDestroy = false;
                                    angle = new Vector3(0, -0.3F, 0);
                                }
                                else
                                {
                                    alterOrDestroy = true;
                                    angle = new Vector3(0, -0.3F, 0);
                                }
                                //Ray ray = new Ray(transform.position, transform.forward + angle);    // 45deg towardsfloor
                                //World.currentWorld.AlterWorld(ray, alterOrDestroy, damageRadius, createDebris, debrisLifetime, maxInteractionRange);
                                NoPathTime = 0;
                            }
                            NoPathTime += Time.deltaTime;
                        }

                    }
                    else
                    {
                        inactiveCount += Time.deltaTime;
                    }
                }
                delayStart += Time.deltaTime;
            }
        }
        else
        {
            Debug.Log("Destroyed npc");
            this.gameObject.SetActive(false);
            World.currentWorld.NPCPool.Enqueue(this.gameObject);
        }
    }


    //void OnCollisionEnter(Collision col)
    //{
    //    //Debug.Log("COLLIDE");
    //    if (hasLanded)
    //    {
    //        if (col.gameObject.tag == "CanDamage")
    //        {
    //            this.GetComponent<NavMeshAgent>().enabled = false;
    //            StartCoroutine(Waiter(this.gameObject));
    //        }
                
    //    }
    //    else
    //    {
    //        hasLanded = true;
    //    }
       
    //}

    IEnumerator Waiter(GameObject clone)
    {
        float wait_time = Random.Range(0.5f, deathLifetime);
        yield return new WaitForSeconds(wait_time);
        clone.SetActive(false);
        World.currentWorld.NPCPool.Enqueue(clone);
    }

    IEnumerator Jumper(NavMeshAgent navAgent, Transform target)
    {
        float jumpTimeCount = 0;
        navAgent.enabled = false;
        
        Rigidbody gameObjectsRigidBody = navAgent.gameObject.AddComponent<Rigidbody>(); 
        gameObjectsRigidBody.freezeRotation = true;
        gameObjectsRigidBody.isKinematic = false;

        navAgent.gameObject.transform.LookAt(new Vector3(target.transform.position.x, navAgent.transform.position.y, target.transform.position.z));

        while (jumpTimeCount < jumpTime)
        {
            gameObjectsRigidBody.AddForce(transform.forward + new Vector3(0, 5F, 0) * 0.5F);
            jumpTimeCount += Time.deltaTime;
        }
        jumpTimeCount = 0;

        yield return new WaitForSeconds(jumpTime);

        Destroy(gameObjectsRigidBody);
        navAgent.enabled = true;
        if (navAgent.isOnNavMesh) navAgent.SetDestination(target.position);
        isJumping = false;

    }

    public void GetNPCPos(Vector3 chunkPos, int width, int height)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            navAgent.gameObject.transform.position =  new Vector3(chunkPos.x += (width / 2), hit.point.y, chunkPos.z += (width / 2)); 
        }
    }
}
