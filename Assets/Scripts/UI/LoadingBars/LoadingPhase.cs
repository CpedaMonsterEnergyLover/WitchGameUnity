
public class LoadingPhase 
{
    public string Title { get; private set; }

    public LoadingPhase(string title)
    {
        Title = title;
    }

    public float ComputedProgress { get; private set; }

    public void ComputeProgress(int phaseIndex, int totalPhases)
    {
        ComputedProgress = (float) (phaseIndex + 1) / totalPhases;
    }
}
