using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace B16_Ex06_1
{
    public class GameManager
    {
        public const int k_RealPlayer = 1;
        public const int k_Quit = -1;
        private const string k_PlayerOneSign = "O";
        private const string k_PlayerTwoSign = "X";
        private const int k_FirstPlayer = 0;
        private const int k_SecondPlayer = 1;
        private static bool s_IsGameFinish = false;
        private int m_LastPlayerMove = 0;
        private int m_PlayerTurn = 0;

        private List<Player> m_Players;
        private Board m_Board;

        public static bool IsGameFinish
        {
            get { return s_IsGameFinish; }
            set { s_IsGameFinish = value; }
        }

        public Board Board
        {
            get { return m_Board; }
        }

        public Player GetPlayer(int i)
        {
            return m_Players[i];
        }

        public int GetPlayerVictories(int i)
        {
            return m_Players[i].Victories;
        }

        public GameManager()
        {
            m_Players = new List<Player>();
            Player firstPlayer = new Player("Player 1", k_PlayerOneSign, false);
            m_Players.Add(firstPlayer);
            Player secondPlayer = new Player("Player 2", k_PlayerTwoSign, false);
            m_Players.Add(secondPlayer);
            m_Board = new Board();
        }

        public void DoIteration(int i_Col)
        {
            m_LastPlayerMove = i_Col;
            playerMove(i_Col);
        }

        private void playerMove(int i_Col)
        {
            m_Board.InsertCoinToBoard(m_Players[m_PlayerTurn].Sign, i_Col);
            if (m_Board.IsWinner(m_LastPlayerMove, m_Players[m_PlayerTurn].Sign))
            {
                s_IsGameFinish = true;
                m_Players[m_PlayerTurn].IsWinner = true;
                m_Players[m_PlayerTurn].Victories++;
            }

            if (s_IsGameFinish || m_Board.IsBoardFull())
            {
                s_IsGameFinish = true;
            }

            nextPlayer();
        }

        private void nextPlayer()
        {
            m_PlayerTurn++;
            if (m_PlayerTurn == m_Players.Count)
            {
                m_PlayerTurn = k_FirstPlayer;
            }  
        }

        public void resetVictories()
        {
            m_PlayerTurn = k_FirstPlayer;
            foreach (Player player in m_Players)
            {
                player.Victories = 0;
                player.IsWinner = false;
            }
        }

        public void SetGame(string i_PlayerOneName, string i_PlayerTwoName, int i_rows, int i_cols)
        {
            this.m_Players[k_FirstPlayer].Name = i_PlayerOneName;
            this.m_Players[k_SecondPlayer].Name = i_PlayerTwoName;
            this.m_Board.SetBoardSize(i_rows, i_cols);
            this.m_Board.CleanBoard();
            resetVictories();
        }

        public eFinishGameOptions WhyGameFinished()
        {
            eFinishGameOptions finishReason;
            if (m_Players[k_FirstPlayer].IsWinner)
            {
                finishReason = eFinishGameOptions.PlayerOneWin;
            }
            else if(m_Players[k_SecondPlayer].IsWinner)
            {
                finishReason = eFinishGameOptions.PlayerTwoWin;
            }
            else
            {
                finishReason = eFinishGameOptions.Tie;
            }

            return finishReason;
        }

        internal string FinishGameResults(eFinishGameOptions i_FinishOption)
        {
            string winnerName = string.Empty;
            switch (i_FinishOption)
            {
                case eFinishGameOptions.PlayerOneWin:
                    winnerName = m_Players[k_FirstPlayer].Name;
                    break;
                case eFinishGameOptions.PlayerTwoWin:
                    winnerName = m_Players[1].Name;
                    break;
            }

            return winnerName;
        }

        public void Reset()
        {
            foreach (Player player in m_Players)
            {
                player.IsWinner = false;
            }

            s_IsGameFinish = false;
            m_PlayerTurn = k_FirstPlayer;
        }
    }
}
