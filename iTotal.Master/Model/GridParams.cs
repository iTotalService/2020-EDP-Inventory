using System;
using System.Collections.Generic;
using System.Text;

namespace iTotal.Master.Model
{
    public class GridParams<T>
    {
        public string key { get; set; }
        public string action { get; set; }
        public List<T> added { get; set; }
        public List<T> changed { get; set; }
        public List<T> deleted { get; set; }
        public T value { get; set; }
    }
}
