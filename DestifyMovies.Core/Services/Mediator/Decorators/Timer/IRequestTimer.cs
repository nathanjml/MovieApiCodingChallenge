namespace DestifyMovies.Core.Services.Mediator.Decorators.Timer;

public interface IRequestTimer
{
    public void StartTimer();
    public TimeSpan StopTimer();
}