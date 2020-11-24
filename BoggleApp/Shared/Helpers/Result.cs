using System;
namespace BoggleApp.Shared.Helpers
{
    public class Result<T>
    {
        public T Value { get; set; }

        public bool IsValid { get; set; }
    }
}
