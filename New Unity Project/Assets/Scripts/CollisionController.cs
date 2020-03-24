using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public delegate void OnHit();
    public static event OnHit onHit;

    public enum player { PLAYER1, PLAYER2};
    public player Player;
    float timer = 0f;

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
