using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class Result
    {
        public bool Successful { get; }
        public string ErrorMessage { get; }

        private Result()
        {
            Successful = true;   
        }

        private Result(string errorMessage)
        {
            Successful = false;
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        public static Result Success() => new Result();

        public static Result Failure(string errorMessage) => new Result(errorMessage);

        public static implicit operator bool(Result r) => r.Successful;
    }
}
