using System;

namespace Cqrsnes.Infrastructure
{
    public class PossibleConcurrencyProblemException : Exception
    {
        public PossibleConcurrencyProblemException() :
            base("It is possible that application has concurrency problem.")
        {
        }
    }
}
