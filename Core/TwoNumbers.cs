namespace Core
{
    public class TwoNumbers
    {
        public decimal FirstNumber { get; set; }
        public decimal SecondNumber { get; set; }

        public TwoNumbers GenerateTwoRandomNumbers()
        {
            Random random = new Random();
            return new TwoNumbers
            {
                FirstNumber = (decimal)(random.NextDouble() * 100), // Generates a random decimal number between 0 and 100
                SecondNumber = (decimal)(random.NextDouble() * 100) // Generates a random decimal number between 0 and 100
            };
        }
    }
}
