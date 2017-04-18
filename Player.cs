using System;
using System.Collections.Generic;
using System.Text;

namespace B16_Ex06_1
{
    public class Player
    {
        private const int k_LowestVictory = 0;
        private readonly string r_Sign;
        private string m_Name;
        private int m_NumOfVictories;
        private bool m_IsWinner;

        public Player(string i_Name, string i_Sign, bool i_ComputerPlaying)
        {
            m_Name = i_Name;
            r_Sign = i_Sign;
            m_NumOfVictories = k_LowestVictory;
        }

        public bool IsWinner
        {
            get { return m_IsWinner; }
            set { m_IsWinner = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Sign
        {
            get { return r_Sign; }
        }

        public int Victories
        {
            get { return m_NumOfVictories; }
            set { m_NumOfVictories = value; }
        }
    }
}
