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
    float jumpTime = 4F;
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
        Transform target = PlayerIO.currentPlayerIO.transform;
        if (!isJumping)
        {
            
            if (Input.GetKey(KeyCode.J))
            {
                isJumping = true;
                //Transform target = PlayerIO.currentPlayerIO.transform;
                StartCoroutine(Jumper(navAgent, target));
            }
            if (delayStart > 5F)
            {


                //if (agent.path.status == NavMeshPathStatus.PathInvalid || agent.path.status == NavMeshPathStatus.PathPartial)
                //{
                //Debug.DrawRay(transform.position, transform.forward + new Vector3(0, -0.3F, 0), Color.red);
                if (navAgent.isOnNavMesh)
                {

                    
                    //Debug.Log(agent.pathPending);
                    if (!navAgent.pathPending)
                    {
                        Debug.Log("NOT PATHFINDINGH");
                        if (navAgent.path.status == NavMeshPathStatus.PathInvalid || navAgent.path.status == NavMeshPathStatus.PathPartial)// 
                        {
                            //Debug.Log("INVALID PATH");
                            ////navAgent.GetComponent<LocalNavMeshBuilder>().GetComponent<NavMeshPath>().
                            //// navAgent.isStopped = true;
                            //Rigidbody gameObjectsRigidBody = navAgent.gameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
                            //gameObjectsRigidBody.mass = 5; // Set the GO's mass to 5 via the Rigidbody.
                            //gameObjectsRigidBody.isKinematic = true;
                            //this.GetComponent<NavMeshAgent>().enabled = false;
                            //navAgent.transform.Translate(Vector3.up * Time.deltaTime * 100, Space.World);
                            ////gameObjectsRigidBody.AddForce(transform.forward * 100F);
                            //Destroy(gameObjectsRigidBody);
                            //this.GetComponent<NavMeshAgent>().enabled = true;
                            //navAgent.SetDestination(target.position);
                            //Debug.Log("Jump");
                            ////delayStart = 0;
                            StartCoroutine(Jumper(navAgent, target));
                        }
                        else
                        {
                            float distance = Vector3.Distance(navAgent.transform.position, target.position);
                            Debug.Log(distance);
                            if (distance <= 3F)
                            {
                                navAgent.isStopped = true;
                            }
                            else
                            {
                                navAgent.isStopped = false;
                                navAgent.SetDestination(target.position);
                            }
                            //    }
                            //    else
                            //    {
                            //        Vector3 targetDir = Quaternion.AngleAxis(Random.Range(-30.0f, 30.0f), transform.up) * transform.forward;
                            //        targetDir = transform.position + targetDir.normalized * 5;
                            //        agent.SetDestination(targetDir);
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        Debug.Log("PATHFINDING");
                        if (NoPathTime > 2F)

                        {
                            Debug.Log("Path time > 2F");
                            target.LookAt(target);
                            if (transform.position.y > target.transform.position.y)
                            {
                                alterOrDestroy = false;
                                angle = new Vector3(0, -0.3F, 0);
                            }
                            else
                            {
                                alterOrDestroy = true;
                                angle = new Vector3(0, 0.3F, 0);
                            }
                            Ray ray = new Ray(transform.position, transform.forward + angle);    // 45deg towardsfloor
                            World.currentWorld.AlterWorld(ray, alterOrDestroy, damageRadius, createDebris, debrisLifetime, maxInteractionRange);
                            NoPathTime = 0;
                        }
                        NoPathTime += Time.deltaTime;
                    }

                }
                else { Debug.Log("NOT ONMESH"); }
            }
            delayStart += Time.deltaTime;
        }
    }

    //private IEnumerator FollowTarget(float range, Transform target)
    //{
    //    Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);
    //    while (Vector3.SqrMagnitude(transform.position - target.position) > 0.1f)
    //    {
    //        // did target move more than at least a minimum amount since last destination set?
    //        if (Vector3.SqrMagnitude(previousTargetPosition - target.position) > 0.1f)
    //        {
    //            agent.SetDestination(target.position);
    //            previousTargetPosition = target.position;
    //        }
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    yield return null;
    //}

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("COLLIDE");
        if (hasLanded)
        {
            if (col.gameObject.tag == "CanDamage")
            {
                this.GetComponent<NavMeshAgent>().enabled = false;
                //Rigidbody rb = this.GetComponent<Rigidbody>();
                //rb.AddExplosionForce(power, this.gameObject.transform.position, radius, 3.0F);
                StartCoroutine(Waiter(this.gameObject));
            }
                
        }
        else
        {
            hasLanded = true;
        }
       
    }

    IEnumerator Waiter(GameObject clone)
    {
        float wait_time = Random.Range(0.5f, deathLifetime);
        yield return new WaitForSeconds(wait_time);
        clone.SetActive(false);
        World.currentWorld.NPCPool.Enqueue(clone);
    }

    IEnumerator Jumper(NavMeshAgent navAgent, Transform target)
    {
        //float wait_time = 5F; //Random.Range(0.5f, deathLifetime);
        float jumpTimeCount = 0;
        navAgent.enabled = false;
        Debug.Log("JUMPINGG");

        //navAgent.GetComponent<LocalNavMeshBuilder>().GetComponent<NavMeshPath>().
        // navAgent.isStopped = true;
        Rigidbody gameObjectsRigidBody = navAgent.gameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
        gameObjectsRigidBody.freezeRotation = true;
                                                                                        //gameObjectsRigidBody.mass = 5; // Set the GO's mass to 5 via the Rigidbody.
        gameObjectsRigidBody.isKinematic = false;
        while (jumpTimeCount < jumpTime)
        {
            //float move = 1F * Time.deltaTime;
            //transform.Translate(transform.up * 1F * Time.deltaTime, Space.World);
            //transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 20, 0), move);
            gameObjectsRigidBody.AddForce(transform.forward + new Vector3(0, 5F, 0) * 0.5F);
            jumpTimeCount += Time.deltaTime;
           // Debug.Log("XXXX");
        }
        jumpTimeCount = 0;
        rig
        //tr/ansform.
        //float move = 0.5F * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 20, 0), move);

        //navAgent.transform.Translate(Vector3.up * Time.deltaTime * 100, Space.World);
        //gameObjectsRigidBody.AddForce(transform.forward * 100F);

        //this.GetComponent<NavMeshAgent>().enabled = true;
        //navAgent.SetDestination(target.position);
        //Debug.Log("Jump");
        //delayStart = 0;
        //yield return new WaitForSeconds(wait_time);
        Debug.Log("OFF AGAIN");
        Destroy(gameObjectsRigidBody);
        navAgent.enabled = true;
        navAgent.SetDestination(target.position);
        isJumping = false;
        yield return null;

    }

    public void GetNPCPos(Vector3 chunkPos, int width, int height)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            navAgent.gameObject.transform.position =  new Vector3(chunkPos.x += (width / 2), hit.point.y, chunkPos.z += (width / 2));
            //offsetDistance = hit.distance;
            //agent.gameObject.transform.position.y = 0;
            //capPos.x += (width / 2);
            //capPos.y += (height);
            //capPos.z += (width / 2);
            //Debug.DrawLine(transform.position, hit.point, Color.cyan);
         
        }
        
    }
}
