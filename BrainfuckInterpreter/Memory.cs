using System;

namespace BrainfuckInterpreter
{
    public class Memory
    {
        private readonly int m_size;
        private int m_pointer;
        private byte[] m_memory;

        public Memory(int size)
        {
            m_size = size <= 0 ? 30000 : size;

            m_memory = new byte[m_size];
            Array.Fill<byte>(m_memory, 0);

            m_pointer = 0;
        }

        public byte GetValue()
        {
            return m_memory[m_pointer];
        }

        public void Increment()
        {
            m_memory[m_pointer]++;
        }

        public void Decrement()
        {
            m_memory[m_pointer]--;
        }

        public void IncrementPointer()
        {
            if (m_pointer == m_size - 1)
            {
                m_pointer = 0;
            }
            else
            {
                m_pointer++;
            }
        }

        public void DecrementPointer()
        {
            if (m_pointer == 0)
            {
                m_pointer = m_size - 1;
            }
            else
            {
                m_pointer--;
            }
        }

        public void SetValue(byte value)
        {
            m_memory[m_pointer] = value;
        }
    }
}
