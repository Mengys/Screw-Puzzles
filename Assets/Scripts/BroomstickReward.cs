using UnityEngine;

public class BroomstickReward : BaseReward
{
    override public void Collect() {
        FindFirstObjectByType<BoostsManager>().AddBroomstick(1);
    }
}
