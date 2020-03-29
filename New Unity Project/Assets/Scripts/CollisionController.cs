using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/************************************************
 *  CollisionController handles the collisions of the bullets
 *  with the players and the planets
 ***********************************************/
public class CollisionController : MonoBehaviour
{
    //! Delegate used in classes that bullet can collide with
    /*!  used in other classes so the behaviour of getting hit can be changed
         Class by Class */
    public delegate void OnHit(); 
    public static event OnHit onHit;

    //! Enum used to track where the bullet came from
    public enum player { PLAYER1, PLAYER2};
    public player Player;
    float timer = 0f;


    //!On CollisionEnter function - UNITY function
    /*! takes a Collision as a parameter and returns void
     * is used to do several things when the bullet collides 
     * with various objects*/
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Affector"))
        {
            onHit();
            MonoBehaviour[] comps = gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in comps)
            {
                c.enabled = false;
            }
            gameObject.GetComponent <MeshRenderer>().enabled = false;
            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
        else if ((collision.gameObject.CompareTag("Player1") && Player == player.PLAYER2) || 
                (collision.gameObject.CompareTag("Player2") && Player == player.PLAYER1))

        {
            MonoBehaviour[] comps = gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in comps)
            {
                c.enabled = false;
            }
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(collision.gameObject);
            GameController.instance.Winner();
        }
    }

    //! Normal Unity update function
    /*! Called every frame */
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= GameController.instance.MissleLife)
        {
            onHit();
            MonoBehaviour[] comps = gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour c in comps)
            {
                c.enabled = false;
            }
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }
}
