using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BrainfuckInterpreter
{
    public class Interpreter
    {
        private FileStream m_file;
        private Memory m_memory;

        private const int LOOP_START = 0x005B;
        private const int LOOP_END = 0x005D;
        private const int DECREMENT = 0x002D;
        private const int INCREMENT = 0x002B;
        private const int MOVE_RIGHT = 0x003E;
        private const int MOVE_LEFT = 0x003C;
        private const int PRINT = 0x002E;
        private const int INPUT = 0x002C;

        private Dictionary<long, long> m_jumps;

        public Interpreter(string filename, int size)
        {
            m_memory = new Memory(size);
            m_file = File.OpenRead(filename);
            m_jumps = new Dictionary<long, long>();

            PreCalculateJumps();
        }

        public void Start()
        {
            while (m_file.Length > m_file.Position)
            {
                var curr = m_file.ReadByte();

                switch (curr)
                {
                    case LOOP_START:
                        HandleLoopStart();
                        break;
                    case LOOP_END:
                        HandleLoopEnd();
                        break;
                    case INCREMENT:
                        m_memory.Increment();
                        break;
                    case DECREMENT:
                        m_memory.Decrement();
                        break;
                    case MOVE_RIGHT:
                        m_memory.IncrementPointer();
                        break;
                    case MOVE_LEFT:
                        m_memory.DecrementPointer();
                        break;
                    case PRINT:
                        HandlePrint();
                        break;
                    case INPUT:
                        HandleInput();
                        break;
                    default:
                        break;
                }
            }
        }

        private void PreCalculateJumps()
        {
            var i = 1L;
            var tempList = new List<long>();

            int current;

            while ((current = m_file.ReadByte()) != -1)
            {
                if (current == LOOP_START)
                {
                    tempList.Add(i);
                }
                else if (current == LOOP_END && tempList.Count > 0)
                {
                    var target = tempList[^1];
                    tempList.RemoveAt(tempList.Count - 1);

                    m_jumps[target] = i;
                    m_jumps[i] = target;
                }

                i++;
            }

            m_file.Seek(0, SeekOrigin.Begin);
        }

        private void HandleLoopEnd()
        {
            if (m_memory.GetValue() != 0)
            {
                Jump();
            }
        }

        private void HandleLoopStart()
        {
            if (m_memory.GetValue() == 0)
            {
                Jump();
            }
        }

        private void HandlePrint()
        {
            var value = m_memory.GetValue();

            var str = Encoding.ASCII.GetString(new byte[] { value });

            Console.Write(str);
        }

        private void HandleInput()
        {
            var rawInput = Console.ReadLine();

            if (byte.TryParse(rawInput, out var byteInput))
            {
                m_memory.SetValue(byteInput);
            }
            else
            {
                var inputBytes = Encoding.ASCII.GetBytes(rawInput);

                if (inputBytes.Length > 0)
                {
                    m_memory.SetValue(inputBytes[0]);
                }
            }
        }

        private void Jump()
        {
            var target = m_jumps[m_file.Position];

            m_file.Seek(target, SeekOrigin.Begin);
        }
    }
}
