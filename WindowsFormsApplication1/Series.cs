using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Series
    {
        private Range[] arr = {};
        
        public void Set(Range[] arr)
        {
            this.arr = arr;
        }

        public Range[] Get()
        {
            return this.arr;
        }

        public void Add(Range el)
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = el;
        }

        public void Del(int num)
        {
            for (int i = num; i < arr.Length - 1; i++)
                arr[i] = arr[i + 1];
            Array.Resize(ref arr, arr.Length - 1);
        }
    }
}