using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {

    public NavMeshAgent agent;
    bool hasLanded = false;
    public float radius = 5.0F;
    public float power = 10.0F;
    public float deathLifetime = 10F;
    public float followRadius = 200F;
    public float NoPathTime = 0.0f;
    // Use this for initialization
    public void Start()
    {
        agent.baseOffset = 1;
        
    }

    // Update is called once per frame
    void Update () {
        //.Log();
        if (agent.isOnNavMesh)
        {
            Transform target = PlayerIO.currentPlayerIO.transform;
            //Debug.Log(agent.pathPending);
            if (!agent.pathPending)
            {

                
                float distance = Vector3.Distance(agent.transform.position, target.position);

                if (distance <= 5F)
                {
                    //this.GetComponent<NavMeshAgent>().enabled = false;
                    //StartCoroutine(waiter(this.gameObject));
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                    agent.SetDestination(target.position);
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
            else
            {
                if (NoPathTime > 8F)
                {
                    //Debug.Log("DDDDOO");
                    //agent.isStopped = true;
                    //agent.
                    target.LookAt(target);
                    RaycastHit hit;

                    Ray ray = new Ray(transform.position, transform.forward);
                    World.currentWorld.DestroyWorld(ray, true);
                    //if (Physics.Raycast(transform.position, Vector3.forward, out hit))
                    //{
                    //    Vector3 p = hit.point;
                    //   // Debug.Log(p);
                    //    Chunk chunk = hit.transform.GetComponent<Chunk>();
                    //    if (chunk != null)
                    //    {
                            
                    //        Debug.Log(p.y);
                    //    }
                        
                    //}
                    NoPathTime = 0;
                }
                NoPathTime += Time.deltaTime;
            }
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
                StartCoroutine(waiter(this.gameObject));
            }
                
        }
        else
        {
            hasLanded = true;
        }
       
    }

    IEnumerator waiter(GameObject clone)
    {
        float wait_time = Random.Range(0.5f, deathLifetime);
        yield return new WaitForSeconds(wait_time);
        clone.SetActive(false);
        World.currentWorld.objectPool.Enqueue(clone);
    }

    public void GetNPCPos(Vector3 chunkPos, int width, int height)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            agent.gameObject.transform.position =  new Vector3(chunkPos.x += (width / 2), hit.point.y, chunkPos.z += (width / 2));
            //offsetDistance = hit.distance;
            //agent.gameObject.transform.position.y = 0;
            //capPos.x += (width / 2);
            //capPos.y += (height);
            //capPos.z += (width / 2);
            //Debug.DrawLine(transform.position, hit.point, Color.cyan);
         
        }
        
    }
}
