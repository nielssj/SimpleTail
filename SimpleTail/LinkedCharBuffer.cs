using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTail
{
    public class LinkedCharBuffer : IEnumerable
    {
        private LinkedList<char[]> buffers;

        public LinkedCharBuffer()
        {
            buffers = new LinkedList<char[]>();
        }

        public void addBuffer(char[] buffer)
        {
            this.buffers.AddFirst(buffer);
        }

        public IEnumerator GetEnumerator()
        {
            return new LinkedCharBufferEnumerator(buffers);
        }
    }

    public class LinkedCharBufferEnumerator : IEnumerator
    {
        private int i;
        private LinkedList<char[]> buffers;
        private LinkedList<char[]> visitedBuffers;
        private char[] currentBuffer;

        public LinkedCharBufferEnumerator(LinkedList<char[]> buffers)
        {
            i = 0;
            this.buffers = buffers;
            visitedBuffers = new LinkedList<char[]>();
            currentBuffer = buffers.First.Value;
            buffers.RemoveFirst();
            visitedBuffers.AddLast(currentBuffer);
        }

        public object Current
        {
            get
            {
                return currentBuffer[i];
            }
        }

        public bool MoveNext()
        {
            i++;

            if (i < currentBuffer.Length)
            {
                return true;
            }
            else if (buffers.Count > 0)
            {
                currentBuffer = buffers.First.Value;
                buffers.RemoveFirst();
                visitedBuffers.AddLast(currentBuffer);
                i = 0;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            visitedBuffers.Concat(buffers);
            buffers = visitedBuffers;
            visitedBuffers = new LinkedList<char[]>();
        }
    }
}
