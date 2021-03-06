﻿using System;

namespace Enable.QueryComposition
{
    public class TakeQueryOption
    {
        private readonly int _value;
        private readonly bool _hasValue;

        public TakeQueryOption()
        {
        }

        public TakeQueryOption(int value)
        {
            _value = value;
            _hasValue = true;
        }

        public bool HasValue
        {
            get
            {
                return _hasValue;
            }
        }

        public int Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new InvalidOperationException("Nullable object does not have a value.");
                }

                return _value;
            }
        }

        public int GetValueOrDefault()
        {
            return _value;
        }

        public int GetValueOrDefault(int defaultValue)
        {
            return _hasValue ? _value : defaultValue;
        }
    }
}
