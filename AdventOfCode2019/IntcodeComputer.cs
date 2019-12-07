using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode2019
{
    public class IntcodeComputer : IObservable<int>, IObserver<int>
    {
        public Dictionary<int, Func<int, List<ParamMode>, List<int>, int>> opCodes;
        public int Output { get; set; }
        public List<int> Input { get; set; }
        private int _inputIndex = 0;
        public enum ParamMode { Position = 0, Immediate = 1};
        public bool Debug { get; set; } = false;
        private AutoResetEvent _inputAvailableEvent = new AutoResetEvent(false);

        public void SendOutput(int o)
        {
            Console.WriteLine($"Output: {o}");
            Output = o;
            foreach(var observer in observers) observer.OnNext(Output);
        }

        public IntcodeComputer()
        {
            Input = new List<int>();
            opCodes = new Dictionary<int, Func<int, List<ParamMode>, List<int>, int>>()
            {
                { 1, (p, m, l) => //Add
                    {
                        if(Debug) Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"Add {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} + {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} ");
                        l[l[p+3]] = (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) + (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] );
                        return p+4;
                    }
                },
                { 2, (p, m, l) => //Mult
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"Mult {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} * {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} ");
                        l[l[p+3]] = (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) * (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] );
                        return p+4;
                    }
                },
                { 3, (p, m, l) => //Input
                    {
                        if(Debug)Console.WriteLine($"Input {Input} => index {l[p+1]}");
                        while(_inputIndex >= Input.Count)
                        {
                            _inputAvailableEvent.WaitOne();
                        }
                        l[l[p+1]] = Input[_inputIndex++];
                        return p+2;
                    }
                },
                { 4, (p, m, l) => //Output
                    {
                        SendOutput(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) ;
                        return p+2;
                    }
                },
                { 5, (p, m, l) => //Jump if True
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]}");
                        if(Debug)Console.WriteLine($"JIFT {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} is non-zero? jump to index {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])}  ");
                        return (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) != 0 ? (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] ) : p+3;
                    }
                },
                { 6, (p, m, l) => // Jump if False
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]}");
                        if(Debug)Console.WriteLine($"JIFF {(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} is zero? jump to index {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])}  ");
                        return (m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) == 0 ? (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] ) : p+3;
                    }
                },
                { 7, (p, m, l) => // Less Than
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"{(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} < {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} = 1 else index {l[p+3]} = 0");
                        if((m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) < (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] )) l[l[p+3]] = 1;
                        else l[l[p+3]] = 0;
                        return p+4;
                    }
                },
                { 8, (p, m, l) => // Greater Than
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"{(m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1])} == {(m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2])} => index {l[p+3]} = 1 else index {l[p+3]} = 0");
                        if((m[0] == ParamMode.Position ? l[l[p+1]] : l[p+1]) == (m[1] == ParamMode.Position ? l[l[p+2]] : l[p+2] )) l[l[p+3]] = 1;
                        else  l[l[p+3]] = 0;
                        return p+4;
                    }
                },
            };
        }
             
        public List<ParamMode> GetModes(int opCode)
        {
            if (opCode < 100) return new List<ParamMode>() { 0, 0, 0, 0, 0, 0, 0, 0, };
            var s = opCode.ToString().PadLeft(10, '0');
            var s1 = s.Substring(0, 8);
            return s.Substring(0, 8).Select(c => c == '0' ? ParamMode.Position : ParamMode.Immediate).Reverse().ToList();
        }

        public List<int> RunProgram(List<int> programCode)
        {
            Output = 0;
            _inputIndex = 0;
            List<int> currentInput = new List<int>(programCode);

            int p = 0;
            int opCode = currentInput[p];
            List<ParamMode> m = GetModes(opCode);
            opCode = int.Parse(opCode.ToString().PadLeft(10, '0').Substring(8, 2));
            while (opCode != 99)
            {
                int newP = opCodes[opCode](p, m, currentInput);
                p = newP;
                opCode = currentInput[p];
                m = GetModes(opCode);
                opCode = int.Parse(opCode.ToString().PadLeft(10, '0').Substring(8, 2));
            }
            foreach (var observer in observers) observer.OnCompleted();
            return currentInput;
        }

        List<IObserver<int>> observers = new List<IObserver<int>>();
        public IDisposable Subscribe(IObserver<int> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber<int>(observers, observer);

        }

        public void OnNext(int value)
        {
            Input.Add(value);
            _inputAvailableEvent.Set();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            // do nothing
        }
    }

    internal class Unsubscriber<IntcodeComputer> : IDisposable
    {
        private List<IObserver<IntcodeComputer>> _observers;
        private IObserver<IntcodeComputer> _observer;

        internal Unsubscriber(List<IObserver<IntcodeComputer>> observers, IObserver<IntcodeComputer> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
