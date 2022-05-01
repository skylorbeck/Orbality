using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSimulator : MonoBehaviour
{
    [SerializeField] private Transform colliders;
    [SerializeField] private Transform ball;
    [SerializeField] private GameObject collidedBall;
    private PlayerController _ballPC;
    private PlayerPreviewer _ballPreviewer;
    private Transform _simBall;
    private Rigidbody2D _simBallRb;
    private PegManager _simPegManager;
    [SerializeField] private int predictionLength = 10;
    private Scene _simScene;
    private PhysicsScene2D _physicsScene;
    private LineRenderer _lineRenderer;
    private Vector2 _force = Vector2.zero;
    private float _torque =0f;
    private GoalScript _simGoal;
    public bool _previewing = true;
    private Transform _collidedBall;


    void Start()
    {
        _simScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        _physicsScene = _simScene.GetPhysicsScene2D();
        Physics.autoSimulation = false;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = predictionLength;
        _simBall = Instantiate(ball.gameObject, ball.position, ball.rotation).transform;
        _simBall.gameObject.tag = "Simulated";
        _ballPC = ball.GetComponent<PlayerController>();
        if (_ballPC!= null)
        {
            _previewing = false;
            _simBall.GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            _ballPreviewer = _simBall.GetComponent<PlayerPreviewer>();
            _ballPreviewer.SetSimulation();
            ball.GetComponent<PlayerPreviewer>().SetSimulation();
        }

        // _simBall.GetComponent<CircleCollider2D>().enabled = false;
        _simBallRb = _simBall.GetComponent<Rigidbody2D>();
        _simBall.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(_simBall.gameObject, _simScene);
        foreach (Transform collider in colliders)
        {
            var ghostCollider = Instantiate(collider.gameObject, collider.position, collider.rotation);
            if (ghostCollider.gameObject.tag.Equals("NoSim"))
            {
                foreach (Transform ghostChild in ghostCollider.transform)
                {
                    ghostChild.tag = "Simulated";
                }

                ghostCollider.gameObject.tag = "Simulated";
            }

            Renderer renderer = ghostCollider.GetComponent<Renderer>();
            if (renderer!)
            {
                renderer.enabled = false;
            }

            Rotator rotator = ghostCollider.GetComponent<Rotator>();
            if (rotator!)
            {
                rotator.enabled = false;
                CopyCat copyCat = ghostCollider.GetOrAddComponent<CopyCat>();
                copyCat.SetOriginal(collider.transform);
            }

            SceneManager.MoveGameObjectToScene(ghostCollider, _simScene);
        }

        if (!_previewing)
        {
            _simPegManager = FindObjectsOfType<PegManager>()[0];
            _simGoal = FindObjectsOfType<GoalScript>()[0];
        }

        _collidedBall = Instantiate(collidedBall).transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (_previewing||( !_ballPC.GetHasShot() && _ballPC.GetAwake()))
        {
            _lineRenderer.enabled = true;
            _simBall.position = ball.position;
            _simBall.rotation = ball.rotation;
            _simBallRb.velocity = Vector2.zero;
            _simBallRb.angularVelocity = 0;
            _simBallRb.AddForce(_force);
            _simBallRb.AddTorque(_torque);
            for (int i = 0; i < predictionLength; i++)
            {
                _physicsScene.Simulate(Time.fixedDeltaTime);
                _lineRenderer.SetPosition(i, _simBall.position);
                if (_previewing && _ballPreviewer.Collided())
                {
                    _collidedBall.position = _ballPreviewer.GetCollisionPoint();
                    _collidedBall.GetComponent<Renderer>().enabled = true;
                }
            }
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    public void SetForce(Vector2 force, float torque)
    {
        _force = force;
        _torque = torque;
    }

    public void Reset()
    {
        _lineRenderer.positionCount = predictionLength;
        _simPegManager.Reset(true);
        _simGoal.Reset(false);
    }
}