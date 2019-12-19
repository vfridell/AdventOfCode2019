using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Numerics;

namespace AdventOfCode2019
{
    public class IntcodeComputer : IObservable<BigInteger>, IObserver<BigInteger>
    {
        public Dictionary<BigInteger, Func<BigInteger, List<ParamMode>, Dictionary<BigInteger, BigInteger>, BigInteger>> opCodes;
        public BigInteger Output { get; set; }
        public List<BigInteger> Input { get; set; }
        private int _inputIndex = 0;
        public enum ParamMode { Position = 0, Immediate = 1, Relative = 2 };
        public bool Debug { get; set; } = false;
        private AutoResetEvent _inputAvailableEvent = new AutoResetEvent(false);
        private BigInteger _relativeBase = 0;
        private BigInteger _instructionPointer = 0;

        private Dictionary<BigInteger, BigInteger> _runningProgram;

        private object _termLock = new object();
        private object _forkLock = new object();

        private bool _forceTerminate = false;
        public void ForceTerminate()
        {
            lock(_termLock)
            {
                _forceTerminate = true;
                _inputAvailableEvent.Set();
            }
        }

        public void SendOutput(BigInteger o)
        {
            if(Debug)Console.WriteLine($"Output: {o}");
            Output = o;
            foreach(var observer in observers) observer.OnNext(Output);
        }

        public IntcodeComputer()
        {
            Input = new List<BigInteger>();
            opCodes = new Dictionary<BigInteger, Func<BigInteger, List<ParamMode>, Dictionary<BigInteger, BigInteger>, BigInteger>>()
            {
                { 1, (p, m, l) => //Add
                    {
                        if(Debug) Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"Add { GetParamValue(p+1, l, m[0]) } + {GetParamValue(p+2, l, m[1])} => index {l[p+3]} ");
                        SetParamValue(p+3, l, m[2], GetParamValue(p+1, l, m[0]) + GetParamValue(p+2, l, m[1]));
                        return p+4;
                    }
                },
                { 2, (p, m, l) => //Mult
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"Mult { GetParamValue(p+1, l, m[0])} * {GetParamValue(p+2, l, m[1])} => index {l[p+3]} ");
                        SetParamValue(p+3, l, m[2], GetParamValue(p+1, l, m[0]) * GetParamValue(p+2, l, m[1]));
                        return p+4;
                    }
                },
                { 3, (p, m, l) => //Input
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]}");
                        while(_inputIndex >= Input.Count)
                        {
                            if(Debug) Console.WriteLine($"Waiting for Input...");
                            _inputAvailableEvent.WaitOne();
                            lock(_termLock)
                            {
                                if(_forceTerminate) return p;
                            }
                        }
                        if(Debug)Console.WriteLine($"Input {Input[_inputIndex]} => index {l[p+1]}");
                        SetParamValue(p+1, l, m[0], Input[_inputIndex++]);
                        return p+2;
                    }
                },
                { 4, (p, m, l) => //Output
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]}");
                        SendOutput(GetParamValue(p+1, l, m[0])) ;
                        return p+2;
                    }
                },
                { 5, (p, m, l) => //Jump if True
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]}");
                        if(Debug)Console.WriteLine($"JIFT {GetParamValue(p+1, l, m[0]) } is non-zero? jump to index {GetParamValue(p+2, l, m[1]) }  ");
                        return GetParamValue(p+1, l, m[0]) != 0 ? GetParamValue(p+2, l, m[1])  : p+3;
                    }
                },
                { 6, (p, m, l) => // Jump if False
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]}");
                        if(Debug)Console.WriteLine($"JIFF {GetParamValue(p+1, l, m[0]) } is zero? jump to index {GetParamValue(p+2, l, m[1])}  ");
                        return GetParamValue(p+1, l, m[0]) == 0 ? GetParamValue(p+2, l, m[1])  : p+3;
                    }
                },
                { 7, (p, m, l) => // Less Than
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"{GetParamValue(p+1, l, m[0]) } < {GetParamValue(p+2, l, m[1])} => index {l[p+3]} = 1 else index {l[p+3]} = 0");
                        if(GetParamValue(p+1, l, m[0]) < GetParamValue(p+2, l, m[1])) 
                            SetParamValue(p+3, l, m[2], 1);
                        else 
                            SetParamValue(p+3, l, m[2], 0);
                        return p+4;
                    }
                },
                { 8, (p, m, l) => // Equal
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]},{l[p+2]},{l[p+3]}");
                        if(Debug)Console.WriteLine($"{GetParamValue(p+1, l, m[0]) } == {GetParamValue(p+2, l, m[1])} => index {l[p+3]} = 1 else index {l[p+3]} = 0");
                        if(GetParamValue(p+1, l, m[0]) == GetParamValue(p+2, l, m[1]))
                            SetParamValue(p+3, l, m[2], 1);
                        else
                            SetParamValue(p+3, l, m[2], 0);
                        return p+4;
                    }
                },
                { 9, (p, m, l) => //Adjust relative base
                    {
                        if(Debug)Console.WriteLine($"{p} => {l[p]},{l[p+1]}");
                        if(Debug)Console.WriteLine($"Adjust Relative Base. Arg: {l[p]} Current Relative base: {_relativeBase} result: {GetParamValue(p+1, l, m[0]) + _relativeBase}");
                        _relativeBase = GetParamValue(p+1, l, m[0]) + _relativeBase;
                        return p+2;
                    }
                },
            };
        }

        public void SetParamValue(BigInteger p, Dictionary<BigInteger, BigInteger> l, ParamMode m, BigInteger value)
        {
            if (m == ParamMode.Position)
            {
                if (Debug) Console.WriteLine($"Write {value} to {l[p]}");
                if (!l.ContainsKey(l[p]))
                {
                    l.Add(l[p], value);
                    return;
                }
                else
                    l[l[p]] = value;
            }
            else if (m == ParamMode.Immediate)
            {
                throw new Exception("Cannot set in Immediate mode");
            }
            else
            {
                if (Debug) Console.WriteLine($"Write {value} to {l[p] + _relativeBase}");
                if (!l.ContainsKey(l[p] + _relativeBase))
                {
                    l.Add(l[p] + _relativeBase, value);
                    return;
                }
                else
                    l[l[p] + _relativeBase] = value;
            }
        }

        public BigInteger GetParamValue(BigInteger p, Dictionary<BigInteger, BigInteger> l, ParamMode m)
        {
            if (m == ParamMode.Position)
            {
                if (!l.ContainsKey(l[p]))
                {
                    l.Add(l[p], 0);
                    return 0;
                }
                else
                    return l[l[p]];
            }
            else if (m == ParamMode.Immediate)
            {
                return l[p];
            }
            else
            {
                if (!l.ContainsKey(l[p] + _relativeBase))
                {
                    l.Add(l[p] + _relativeBase, 0);
                    return 0;
                }
                else
                    return l[l[p] + _relativeBase];
            }
        }

        public List<ParamMode> GetModes(BigInteger opCode)
        {
            if (opCode < 100) return new List<ParamMode>() { 0, 0, 0, 0, 0, 0, 0, 0, };
            var s = opCode.ToString().PadLeft(10, '0');
            var s1 = s.Substring(0, 8);
            return s.Substring(0, 8).Select(c => c == '0' ? ParamMode.Position : (c == '1' ? ParamMode.Immediate : ParamMode.Relative)).Reverse().ToList();
        }

        public void RunProgram(List<int> programCode) => RunProgram(programCode.Cast<BigInteger>().ToList());
        
        public void RunProgram(List<BigInteger> programCode)
        {
            Output = 0;
            _inputIndex = 0;
            _relativeBase = 0;
            _runningProgram = new Dictionary<BigInteger, BigInteger>();
            for (int i = 0; i < programCode.Count; i++) _runningProgram.Add(i, programCode[i]);

            _instructionPointer = 0;
            MainProgramLoop();
        }

        public void Continue()
        {
            MainProgramLoop();
        }

        private void MainProgramLoop()
        {
            BigInteger opCode = (int)_runningProgram[_instructionPointer];
            List<ParamMode> m = GetModes(opCode);
            opCode = int.Parse(opCode.ToString().PadLeft(10, '0').Substring(8, 2));
            while (opCode != 99)
            {
                lock (_termLock) { if (_forceTerminate) return; }
                lock (_forkLock)
                {
                    BigInteger newP = opCodes[opCode](_instructionPointer, m, _runningProgram);
                    _instructionPointer = newP;
                }
                opCode = _runningProgram[_instructionPointer];
                m = GetModes(opCode);
                opCode = int.Parse(opCode.ToString().PadLeft(10, '0').Substring(8, 2));
            }
            foreach (var observer in observers) observer.OnCompleted();
            return;
        }

        List<IObserver<BigInteger>> observers = new List<IObserver<BigInteger>>();
        public IDisposable Subscribe(IObserver<BigInteger> observer)
        {
            // Check whether observer is already registered. If not, add it
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
            return new Unsubscriber<BigInteger>(observers, observer);

        }

        public void OnNext(BigInteger value)
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

        public IntcodeComputer Clone()
        {
            IntcodeComputer clone = new IntcodeComputer();
            lock (_forkLock)
            {
                clone._forceTerminate = false;
                clone._runningProgram = new Dictionary<BigInteger, BigInteger>(_runningProgram);
                clone._instructionPointer = _instructionPointer;
                clone.Input = new List<BigInteger>(Input);
                clone.Output = Output;
                clone._inputIndex = _inputIndex;
                clone.Debug = Debug;
                clone._relativeBase = _relativeBase;
            }
            return clone;
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
