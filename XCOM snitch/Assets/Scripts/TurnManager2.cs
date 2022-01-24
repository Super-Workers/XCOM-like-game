using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager2 : MonoBehaviour 
{
    static Dictionary<string, List<Tactics2Move>> units = new Dictionary<string, List<Tactics2Move>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<Tactics2Move> turnTeam = new Queue<Tactics2Move>();

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
	}

    static void InitTeamTurnQueue()
    {
        List<Tactics2Move> teamList = units[turnKey.Peek()];

        foreach (Tactics2Move unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    public static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        Tactics2Move unit = turnTeam.Dequeue();
        unit.EndTurn();

        if (turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(Tactics2Move unit)
    {
        List<Tactics2Move> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<Tactics2Move>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }
}
