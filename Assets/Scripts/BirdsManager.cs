using System;
using System.Collections.Generic;
using UnityEngine;

public class BirdsManager : MonoBehaviour
{

    public static BirdsManager instance;


    [SerializeField]
    public BaseBird currentBird;

    [SerializeField]
    private List<BaseBird> birdsList = new List<BaseBird>();

    [SerializeField]
    private Transform birdSpawnPoint;


    public event Action OnBirdLaunch;


    private Vector3 launchDirection;

    bool canUseAbility = false;
    public bool isBirdLaunched = false;
    int currentBirdIndex = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    Vector2 pos;

    private void Start()
    {
        if (currentBird == null && birdsList.Count > 0)
        {
            currentBird = birdsList[0];
            currentBirdIndex = 0;
        }

        if (currentBird != null)
        {
            MoveBirdToSpawnPoint(currentBird);
            EventManager.RaiseEvent(new OnNextBirdReady(currentBird.transform));
        }

        InputHandler.instance.OnDragDirection += Instance_OnDragDirection;
        InputHandler.instance.OnDragRelease += Instance_OnDragRelease;

        EventManager.AddListner<OnBirdLaunchFinished>(OnBirdLaunchFinishedListner);
    }


    void OnBirdLaunchFinishedListner(OnBirdLaunchFinished e)
    {
        isBirdLaunched = false;
        BirdLaunchCompleted();
    }

    private void Instance_OnDragRelease()
    {
        if (currentBird == null)
            return;

        if (!isBirdLaunched)
        {

            EventManager.RaiseEvent(new OnBirdLaunched(currentBird.transform));

            OnBirdLaunch?.Invoke();
            canUseAbility = true;
            isBirdLaunched = true;
            currentBird.LaunchBird(launchDirection);

            TrajectoryPrediction.instance.Save(
           currentBird.transform.position,
           launchDirection,
           currentBird.launchForce);

        }
        else
        {
            if (canUseAbility)
                currentBird.AbilityUse();
        }
    }

    private void Instance_OnDragDirection(Vector2 obj)
    {
        if (isBirdLaunched || currentBird == null)
            return;

        launchDirection = obj;

        // Calculate only X and Y based on the drag and Manager position
        float newX = pos.x + obj.x;
        float newY = pos.y + obj.y;

        // Hard-code the Z axis to exactly what you set in the inspector
        Vector3 birdPos = new Vector3(newX, newY, 0);

        currentBird.OnBirdAim(birdPos);

        TrajectoryPrediction.instance.Show(
           currentBird.transform.position,
           obj,
           currentBird.launchForce);


    }

    public void BirdLaunchCompleted()
    {
        canUseAbility = false;
        currentBirdIndex++;

        if (currentBirdIndex >= birdsList.Count)
        {
            currentBird = null;
            return;
        }

        currentBird = birdsList[currentBirdIndex];
        currentBird.gameObject.SetActive(true);
        MoveBirdToSpawnPoint(currentBird);
        EventManager.RaiseEvent(new OnNextBirdReady(currentBird.transform));
    }

    private void MoveBirdToSpawnPoint(BaseBird bird)
    {
        if (birdSpawnPoint != null)
        {
            bird.transform.position = birdSpawnPoint.position;
        }

        bird.ResetBird();
        pos = bird.transform.position;
    }


    private void OnDisable()
    {
        if (InputHandler.instance != null)
        {
            InputHandler.instance.OnDragDirection -= Instance_OnDragDirection;
            InputHandler.instance.OnDragRelease -= Instance_OnDragRelease;
        }

        EventManager.RemoveListner<OnBirdLaunchFinished>(OnBirdLaunchFinishedListner);
    }
}
