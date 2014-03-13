using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetOfStacks
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 3;
            SetOfStacks sos = new SetOfStacks(size);

            sos.Push(1);
            sos.Push(2);
            sos.Push(3);
            sos.Push(4);
            sos.Push(5);
            sos.Push(6);

            // Test cases for Pop and Push
            //Console.WriteLine(sos.Pop());
            //Console.WriteLine(sos.Pop());
            //Console.WriteLine(sos.Pop());
            //Console.WriteLine(sos.Pop());
            //Console.WriteLine(sos.Pop());

            //if (sos.IsEmpty() == false)
            //{
            //    throw new Exception("Invalid state");
            //}

            // Test cases for PopAt
            Console.WriteLine("Pop from stack {0} {1}", 0, sos.PopAt(0));
            Console.WriteLine("Pop from stack {0} {1}", 0, sos.PopAt(0));
        }
    }

    class SetOfStacks
    {
        private int size;

        private List<Stack<int>> stackList;

        private int top;

        public SetOfStacks(int capacity)
        {
            size = capacity;
            top = -1;
            stackList = new List<Stack<int>>();
        }

        public void Push(int plateId)
        {
            top++;

            if ((stackList.Count == 0) || (top % size == 0))
            {
                Stack<int> newStack = new Stack<int>();
                stackList.Add(newStack);
            }

            Stack<int> topStack = GetTopStack();

            topStack.Push(plateId);
        }

        public int Pop()
        {
            if (top == -1)
            {
                throw new Exception("No plates in stack");
            }

            Stack<int> topStack = GetTopStack();

            int result = topStack.Pop();

            if (topStack.Count == 0)
            {
                stackList.Remove(topStack);
            }

            top--;

            return result;
        }

        // Pops value from a specific sub stack
        public int PopAt(int index)
        {
            Stack<int> subStack = GetStack(index);

            if (subStack == null || subStack.Count == 0)
            {
                throw new Exception("Error: Invalid state");
            }

            int result = subStack.Pop();

            if (subStack.Count == 0)
            {
                stackList.Remove(subStack);
            }
            else
            {
                // need to rollover plates from higher
                // stacks into this one
                RollOver(index);
            }

            top--;

            return result;
        }

        // Rolls overs value, if any, from stack on top
        // of the stack with given index. Repeats for
        // every stack on top of it
        private void RollOver(int index)
        {
            Stack<int> subStack = GetStack(index);

            while ((index + 1) < stackList.Count)
            {
                int valueToRollOver = GetBottomValueFromStack(index + 1);

                subStack.Push(valueToRollOver);

                index++;  // move on to the next stack

                subStack = GetStack(index);
            }
        }

        // Gets the bottom value from given stack
        private int GetBottomValueFromStack(int stackIndex)
        {
            Stack<int> subStack = GetStack(stackIndex);

            Stack<int> tempStack = new Stack<int>();
            while (subStack.Count > 1)
            {
                tempStack.Push(subStack.Pop());
            }

            int result = subStack.Pop();

            // Now add back values
            while (tempStack.Count > 0)
            {
                subStack.Push(tempStack.Pop());
            }

            return result;
        }

        private Stack<int> GetStack(int index)
        {
            if (top == -1)
            {
                throw new Exception("Error: No stacks present");
            }

            return stackList[index];
        }

        private Stack<int> GetTopStack()
        {
            int index = top / size;

            return stackList[index];
        }

        public bool IsEmpty()
        {
            return top == -1;
        }
    }
}
