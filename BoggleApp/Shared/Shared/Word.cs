using System;
using BoggleApp.Shared.Enums;

namespace BoggleApp.Shared.Shared
{
    public class Word
    {
        public Word(string value)
        {
            Value = value;
        }

        private string text;

        public string Value
        {
            get => text;
            private set
            {
                text = value;
                Status = WordStatus.Initial;
            }   
        }

        private WordStatus _status;

        public WordStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                if (_status == WordStatus.False)
                    Points = 0;
            }
        }
        private int _points;

        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                if (_points > 0)
                    Status = WordStatus.Correct;
            }
        }
    }
}
