


using UnityEngine;

public class OnBirdLaunchFinished : IGameEvent
{

}



public class OnBirdLaunched : IGameEvent
{
    public Transform Bird;

    public OnBirdLaunched(Transform Bird)
    {
        this.Bird = Bird;
    }
}

