// ReSharper disable InconsistentNaming
public class Responce
{
    public AchievementResponce Achievement;
    public bool Status;
}

public class AchievementResponce
{
    public Rewards[] Rewards;
}

public class Rewards
{
    public int Amount;
    public Reward Reward;
}

public class Reward
{
    public string RewardKey;
    public string RewardIconUrl;
}
