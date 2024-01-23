// Author: Nick Hwang
// For: Beat Em Up Style Tutorials
// youtube.com/c/nickhwang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTester : MonoBehaviour
{
    [SerializeField] private bool canAttack = true;
    
    [SerializeField] private Collider2D inLineCollider;
    
    [SerializeField] private LayerMask enemyLayer;
    
    PlayerInput input;
    Controls controls = new Controls();
    private ContactFilter2D contactFilter2D;
    public List<Collider2D> cols = new List<Collider2D>();
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        contactFilter2D.SetLayerMask(enemyLayer);
    }

    // Update is called once per frame
    void Update()
    {
        controls = input.GetInput();
        //if (controls.AttackState)
       // {
            inLineCollider.OverlapCollider(contactFilter2D, cols);
            if (cols.Count > 0)
            {
                foreach (var col in cols)
                {
                    print(col.transform.name);
                    if (col.TryGetComponent(out SpriteRenderer sr))
                    {
                        sr.color = Color.red;
                     StartCoroutine(Countdown(.5f, sr));
                    }
                }
            }
        //}
    }

    IEnumerator Countdown(float seconds, SpriteRenderer sr)
    {
        float counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(seconds);
            counter--;
        }
        //DoStuff();
        sr.color = Color.white;
        
    }

}
