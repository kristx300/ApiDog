using PropertyChanged;
using System.ComponentModel;

namespace APIDog.Core
{
    /// <summary>
    /// Support Class with open Setter
    /// </summary>
    /// <typeparam name="T1">First Type</typeparam>
    /// <typeparam name="T2">Second Type</typeparam>
    public class AnalogTuple<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
    }

    /// <summary>
    /// Support Class with open Setter
    /// </summary>
    /// <typeparam name="T">Main Type</typeparam>
    public class AnalogTuple<T> : INotifyPropertyChanged
    {
        public T First { get; set; }
        public T Second { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}