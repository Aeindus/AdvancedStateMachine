using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncStateMachine {
    public class PatternExpresion<T> {
        private readonly List<T> _pattern;
        private int _currentState = 0;

        public PatternExpresion(List<T> pattern) {
            if (pattern.Count == 0)
                throw new Exception("Pattern list cannot be empty");

            _pattern = pattern.ToList();
        }

        public bool Advance(T checkpoint) {
            if (_currentState >= _pattern.Count)
                throw new Exception("Pattern must be reset to be reused");

            if (!Equal(_pattern[_currentState], checkpoint)) {
                _currentState = 0;
                return false;
            }

            _currentState++;

            // Reached the last state
            return _currentState == _pattern.Count;
        }

        public void Reset() {
            _currentState = 0;
        }

        private bool Equal(T x, T y) {
            return EqualityComparer<T>.Default.Equals(x, y);
        }
    }
}
