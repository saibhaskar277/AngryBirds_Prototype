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
        pos = currentBird.transform.position;
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
        if (isBirdLaunched)
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
        currentBird = birdsList[currentBirdIndex];
        currentBird.gameObject.SetActive(true);
    }


    private void OnDisable()
    {
        InputHandler.instance.OnDragDirection -= Instance_OnDragDirection;
        InputHandler.instance.OnDragRelease -= Instance_OnDragRelease;
    }
}
