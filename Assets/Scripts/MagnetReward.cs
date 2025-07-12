using UnityEngine;

public class MagnetReward : BaseReward
{
    public override void Collect() {
        FindFirstObjectByType<BoostsManager>().AddMagnet(1);
    }
}
