namespace SongPlayer.Dal.Contract
{
    public interface ITimeViewModel
    {
        void OnSetTimer(TimeSpan time);
        void OnResetCountdown();
        void UpdateCountdown();
    }
}
