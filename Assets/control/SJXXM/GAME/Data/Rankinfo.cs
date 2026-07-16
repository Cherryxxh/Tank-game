using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rankinfo
{
    
    public string name;
    public int score;
    public float time;
   
    public Rankinfo()
    {
       
    }

    public Rankinfo(string name, int score, float time)
    {
       this.name = name;
       this.score = score;
       this.time = time;
    }
}


public class RankList
{
    public List<Rankinfo> rankklist;
}