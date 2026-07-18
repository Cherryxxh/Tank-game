using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsterobj : Tankbase
{
    private Transform targetpos;

    public Transform[] Targetpos;

    public Transform playerpos;

    public GameObject headtank;

    public float firedis = 5f;

    public float firetime = 1f;

    public Weaponobj weapon;
    private float nowtime;

    public Transform[] weaponpos;

    public GameObject bullet;

    public Texture maxhpbk;

    public Texture hpbk;

    private Rect hprect;

    private Rect maxhprect;

    private float showtime ;

    public override void Attack()
    {
        for(int i = 0; i < weaponpos.Length; i++)
        {
            GameObject obj = Instantiate(bullet,
                weaponpos[i].position, weaponpos[i].rotation);
            bulletobj bullett = obj.GetComponent<bulletobj>();
            bullett.Setfather(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetTargetpos();
    }

    // Update is called once per frame
    void Update()
    {
        
        this.transform.LookAt(targetpos);
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        float dist = Vector3.Distance(this.transform.position, targetpos.position);
        if (dist <= 0.1f)
        {
            SetTargetpos();
        }
        if(playerpos != null)
        {
            tankhead.LookAt(playerpos);
            if(Vector3.Distance(this.transform.position, playerpos.position) <= firedis)
            {
                nowtime += Time.deltaTime;
                if(nowtime >= firetime)
                {
                    Attack();
                    nowtime = 0;
                }
            }
        }
    }

    private void SetTargetpos()
    {
        if (Targetpos.Length == 0)
        {
            return;
        }
        targetpos = Targetpos[Random.Range(0, Targetpos.Length)];
    }

    
    override public void Die()
    {
        base.Die();
        Gamepanel.Instance.addscore(10);
    }

    private void OnGUI()
    {
        if(showtime > 0)
        {
            showtime -= Time.deltaTime;
            Vector3 screenpos = Camera.main.WorldToScreenPoint(this.transform.position);

            screenpos.y = Screen.height - screenpos.y;
            maxhprect.x = screenpos.x - 50f;
            maxhprect.y = screenpos.y - 50f;
            maxhprect.width = 100;
            maxhprect.height = 15;
            GUI.DrawTexture(maxhprect, maxhpbk);


        
            hprect.x = screenpos.x - 50f;
            hprect.y = screenpos.y - 50f;
            hprect.width = 100f * (float)hp / maxhp;
            hprect.height = 15;
            GUI.DrawTexture(hprect, hpbk);
        }
        
    }


    public override void Wound(Tankbase other)
    {
        base.Wound(other);
        showtime = 3f;
    }
    

}
