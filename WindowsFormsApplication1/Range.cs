using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    abstract class Range
    {
        public abstract Range Add(Range other);
        public abstract Range Sub(Range other);
        public abstract Range Mul(Range other);
        public abstract Range Div(Range other);
        public abstract Range Retu();
    }

    class IntegerRange : Range
    {
        readonly int from;
        readonly int to;

        public IntegerRange(int from, int to)
        {
            this.from = from;
            this.to = to;
        }

        public override string ToString()
        {
            return from + ":" + to;
        }

        public override Range Retu()
        {
            return new IntegerRange(from, to);
        }

        public override Range Add(Range other)
        {
            var r = other as IntegerRange;
            return new IntegerRange(from + r.from, to + r.to);
        }

        public override Range Sub(Range other)
        {
            var r = other as IntegerRange;
            return new IntegerRange(from - r.to, to - r.from);
        }

        public override Range Mul(Range other)
        {
            var r = other as IntegerRange;
            int min = Math.Min(Math.Min(from * r.to, from * r.from), Math.Min(to * r.to, to * r.from));
            int max = Math.Max(Math.Max(from * r.to, from * r.from), Math.Max(to * r.to, to * r.from));
            return new IntegerRange(min, max);
        }

        public override Range Div(Range other)
        {
            var r = other as IntegerRange;
            int min = Math.Min(Math.Min(from / r.to, from / r.from), Math.Min(to / r.to, to / r.from));
            int max = Math.Max(Math.Max(from / r.to, from / r.from), Math.Max(to / r.to, to / r.from));
            return new IntegerRange(min, max);
        }
    }

    class RealRange : Range
    {
        readonly double from;
        readonly double to;

        public RealRange(double from, double to)
        {
            this.from = from;
            this.to = to;
        }

        public override string ToString()
        {
            return from + ":" + to;
        }

        public override Range Retu()
        {
            return new RealRange(from, to);
        }

        public override Range Add(Range other)
        {
            var r = other as RealRange;
            return new RealRange(from + r.from, to + r.to);
        }

        public override Range Sub(Range other)
        {
            var r = other as RealRange;
            return new RealRange(from - r.to, to - r.from);
        }

        public override Range Mul(Range other)
        {
            var r = other as RealRange;
            double min = Math.Min(Math.Min(from * r.to, from * r.from), Math.Min(to * r.to, to * r.from));
            double max = Math.Max(Math.Max(from * r.to, from * r.from), Math.Max(to * r.to, to * r.from));
            return new RealRange(min, max);
        }

        public override Range Div(Range other)
        {
            var r = other as RealRange;
            double min = Math.Min(Math.Min(from / r.to, from / r.from), Math.Min(to / r.to, to / r.from));
            double max = Math.Max(Math.Max(from / r.to, from / r.from), Math.Max(to / r.to, to / r.from));
            return new RealRange(min, max);
        }
    }
}