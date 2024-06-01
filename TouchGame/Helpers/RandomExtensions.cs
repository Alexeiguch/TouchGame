namespace TouchGame
{
	public class RandomExtensions
	{
		public static T Random<T>(IEnumerable<T> list)
		{
			var random = new Random();
			var index = random.Next(list.Count());
			return list.ElementAt(index);
		}

		public static IEnumerable<T> Random<T>(IEnumerable<T> list, int count = 1)
		{
            var random = new Random();
			return list.OrderBy(x => random.Next()).Take(count);
        }
    }
}

