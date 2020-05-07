namespace Models
{
	public class FuzzyTopsis
	{
		public string method { get; set; }
		public int numCriteria { get; set; }
		public int numberOfAlternative { get; set; }
		public string[][] dgData { get; set; }
		public string[][] dgWeight { get; set; }
	}
}