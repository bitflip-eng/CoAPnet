﻿namespace CoAPnet.Protocol.Options
{
    public sealed class CoapMessageOptionUintValue : CoapMessageOptionValue
    {
        public CoapMessageOptionUintValue(uint value)
        {
            Value = value;
        }

        public uint Value
        {
            get;
        }

        public override bool Equals(object obj)
        {
            if (obj is CoapMessageOptionUintValue other)
            {
                return Value.Equals(other.Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}