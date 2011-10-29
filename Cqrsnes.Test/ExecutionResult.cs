namespace Cqrsnes.Test
{
    public class ExecutionResult
    {
        public ExecutionResult()
        {
            IsPassed = false;
            Details = "Was not executed yet.";
        }

        public bool IsPassed { get; set; }

        public string Details { get; set; }

        public override string ToString()
        {
            return Details;
        }
    }
}