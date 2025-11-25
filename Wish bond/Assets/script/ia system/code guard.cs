using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum State
{
    idle, // etat de depart
    patrol, // etat principale de ronde
    investigate, // etat de recher en cas de bruit ou detection niveau 1 (sur 2)
    detected, // etat de poursuite du joueur 
    //research  // etat de recherche apres avoir detecter et perdu le joueur de vue (et ne l'entend pas) 
    count
    

}
public class codeguard : MonoBehaviour
{
    private patrolState _patrol_state;
    private patrolState _investigate_state;
    private patrolState _detected_state;

    private State state = State.idle;

    [SerializeField] private guardbase _gb;


    [SerializeField] private List<Vector3> _liste_position;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _timer_value;


    private int _index_pos = 0;
    private List<Vector3> _liste_chemin = new List<Vector3>();
    private Vector3 _target_pos = Vector3.zero;

    private List<Vector3> _liste_follow = new List<Vector3>();

    private float _timer = 0;

    private List<GameObject> _liste_object_see = new List<GameObject>();

    bool[,] switch_state = { 
        { false, true, true, true, false },
        { true, false, true, true, false },
        { true, true, false, false, true },
        { true, true,true, false, false },
        { true, false, false,true,false}
    };

    private List<Vector3> _liste_point_view = new List<Vector3>();

    // detection systeme

    private bool _liste_change = false;
    private GameObject _target;

    [SerializeField] private float _detect1_level = 0;
    private float _detect2_level = 0;



    [SerializeField] private Image q1;
    [SerializeField] private Image q2;

    [SerializeField] private Image d1;
    [SerializeField] private Image d2;



    void Start()
    {
        if (_gb  == null)
        {
            Debug.Log("erreur guard so null destruction");
            Destroy(this); 
            return;
        }
        if (TryGetComponent<NavMeshAgent>(out var component))
            _agent = component;
        else
        {
            Debug.Log("erreur guard nav mesh agent null destruction");
            Destroy(this);
            return;
        }

        if (_gb.point_cone < 3)_gb.point_cone = 3;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.idle:
                UpdateIdle();
                break;
            case State.patrol:
                UpdatePatrol();
                break;
            case State.investigate:
                UpdateInvestigate();
                break;
        }
    }




    private void Try_switch_state(State s)
    {
        if (switch_state[((int)state), ((int)s)])
        {
            switch (state) // sortie de state
            {
                case State.idle:
                    _timer = 0;
                    break;
                case State.patrol:

                    break;
                case State.investigate: 
                    
                    break;
            }

            state = s; 

            switch (state) // entre de state
            {
                case State.idle:
                    _timer = _timer_value;
                    break;
                case State.patrol:

                    break;
                case State.investigate:

                    break;
            }
        }
        return;
    }

    private void UpdateIdle()
    {
        if (_timer < 0)
            Try_switch_state(State.patrol);
        _timer -= Time.deltaTime;
    }

    private void UpdatePatrol()
    {
        Move();
    }

    private void UpdateInvestigate()
    {
        if (_liste_object_see.Count > 0)
        {
            if (_liste_change)
                foreach(var element in _liste_object_see)
                {
                    if (_target == null)
                        _target = element;
                    if (element.GetComponent<Suspect_interface>().GetImportance_level() > _target.GetComponent<Suspect_interface>().GetImportance_level())
                        _target = element;
                }

            //if (_target.GetComponent<Suspect_interface>().GetImportance_level() == 10 )
            //    if(_gb.detecter)
            //        Try_switch_state(State.detected)

            _detect1_level += _target.GetComponent<Suspect_interface>().GetDetectionLevel() * _gb.detection_multiplie * Time.deltaTime;
            Change();

            if (_detect1_level >= _gb.detecte_level)
            {
                _detect1_level = _gb.detecte_level;
                Try_switch_state(State.detected);
            }
        }
        else
        {
            if (_gb.timer_investigate > 0)
                _gb.timer_investigate -= Time.deltaTime;

            else if (_detect1_level > 0)
            {
                _detect1_level -= _gb.suspition_decrease * Time.deltaTime;
                Change();
            }

            else
            {

                _detect1_level = 0;
                Change();
                Try_switch_state(State.idle);
            }
        }
    }

    private void Change()
    {
        if (_detect1_level > 0)
        {
            q1.gameObject.SetActive(true);
            q2.gameObject.SetActive(true);
            q2.fillAmount = _detect1_level / _gb.detecte_level;
        }
        else
        {
            q1.gameObject.SetActive(false);
            q2.gameObject.SetActive(false);
        }
    }

    private void Move() // deplace l'entiter vers le point actif 
    {
        CheckPos();

        Vector3 dir = new Vector3((_target_pos.x - transform.position.x), 0.0f, (_target_pos.z - transform.position.z)).normalized;

        if (dir != Vector3.zero)
            transform.forward = dir;

        _agent.Move(dir * _gb.speed * Time.deltaTime);

    }

    private void CheckPos()
    {
        if (_liste_chemin.Count == 0)
        {
            NewParcour();
            if (_liste_chemin.Count == 0) return;
        }

        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_target_pos.x, _target_pos.z)) <= _gb.error_move_marge)
        {
            while (_liste_chemin.Count > 0 && Vector3.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(_liste_chemin[0].x, _liste_chemin[0].z)) <= _gb.error_move_marge)
            {
                _liste_chemin.RemoveAt(0);
            }

            if (_liste_chemin.Count == 0)
            {
                NewParcour();
                if (_liste_chemin.Count == 0) return;
            }
        }
        _target_pos = _liste_chemin[0];
    }


    private void NewParcour()
    {
        _index_pos++;
        if (_index_pos >= _liste_position.Count)
            _index_pos = 0;

        NavMeshPath path = new NavMeshPath();

        if (_agent.CalculatePath(_liste_position[_index_pos], path))
        {
            _liste_chemin.Clear();

            foreach (var point in path.corners)
            {
                _liste_chemin.Add(point);
            }
        }
        Try_switch_state(State.idle);
    }


    private void NewParcour(GameObject target) // quand le jouer est detecter ou un son 
    {
        NavMeshPath path = new NavMeshPath();

        if (_agent.CalculatePath(target.transform.position, path))
        {
            _liste_follow.Clear();

            foreach (var point in path.corners)
            {
                _liste_follow.Add(point);
            }
        }
    }

    public void AddTargetView(GameObject other) // pour detecter les element vue
    {
        _liste_object_see.Add(other);
        _liste_change = true;
        Try_switch_state(State.investigate);
    }
    public void RemoveTargetView(GameObject other)
    {
        _liste_object_see.Remove(other);
        _liste_change = true;
    }

}
