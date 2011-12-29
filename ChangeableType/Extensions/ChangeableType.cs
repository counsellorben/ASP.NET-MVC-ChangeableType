using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChangeableType.Extensions
{
    // our base classes for our Changeable<T> class
    public interface IChangeable
    {
        bool Changed { get; set; }
    }

    public class Changeable<T> : IChangeable
    {
        public bool Changed { get; set; }
        public T Value { get; set; }
    }
}