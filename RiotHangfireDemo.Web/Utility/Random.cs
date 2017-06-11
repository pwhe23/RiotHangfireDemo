namespace RiotHangfireDemo
{
    public interface IRandom
    {
        int Next(int min, int max);
    };

    public class Random : IRandom
    {
        private static readonly System.Random _random = new System.Random();

        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }
    };
}