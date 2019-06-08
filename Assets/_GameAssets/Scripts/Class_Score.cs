public class Class_Score
{
    public enum ScoreID
    {
        kill,
        headshot,
        chest_shot,
        limb_shot,
        bullet_hit,
        unlock,
        buy
    }
    public ScoreID id;
    public int scoreValue;
    public string scoreDesc;
}
