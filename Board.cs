using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace B16_Ex06_1
{
    public delegate void NotifyCellDelegate(int i_Row, int i_Col, string i_Sign, Board i_Board);

    public class Board
    {
        private const string k_ZeroChar = "0";
        private const int k_Zero = 0;
        private const int k_Start = 1;
        private const int k_Default = -1;
        private const int k_MinRowCol = 4;
        private int r_Cols;
        private int r_Rows;
        private string[,] m_Matrix;
        private int[] m_BoardStatus;

        public event NotifyCellDelegate NotifyCell;

        private List<Point> m_ListOfCells = new List<Point>();

        public void SetBoardSize(int i_Rows, int i_Cols)
        {
            r_Rows = i_Rows;
            r_Cols = i_Cols;
            m_Matrix = new string[r_Rows + k_Start, r_Cols + k_Start];
            m_BoardStatus = new int[r_Cols + k_Start];
            CleanBoard();
        }

        public string[,] Matrix
        {
            get { return m_Matrix; }
        }

        private bool isWinnerDirRightSlant(int i_Col, string i_Sign)
        {
            // direction right-down, up-left
            bool isPlayerWinner = false;
            int counter = k_Start;
            int row = r_Rows - m_BoardStatus[i_Col] + 1;
            int rowSaver, colSaver;
            rowSaver = row;
            colSaver = i_Col;
            m_ListOfCells.Add(new Point(row, i_Col));
            if ((isPointInBorders(row + 1, i_Col + 1) || isPointInBorders(row - 1, i_Col - 1)) && !isPlayerWinner)
            {
                while (isPointInBorders(row + 1, i_Col + 1) && m_Matrix[row + 1, i_Col + 1] == i_Sign)
                {
                    m_ListOfCells.Add(new Point(row + 1, i_Col + 1));
                    row += k_Start;
                    i_Col += k_Start;
                    counter++;
                }

                row = rowSaver;
                i_Col = colSaver;
                while (isPointInBorders(row - 1, i_Col - 1) && m_Matrix[row - 1, i_Col - 1] == i_Sign)
                {
                    m_ListOfCells.Add(new Point(row - 1, i_Col - 1));
                    row -= 1;
                    i_Col -= 1;
                    counter++;
                }

                if (counter >= k_MinRowCol)
                {
                    isPlayerWinner = true;
                }
                else
                {
                    m_ListOfCells.Clear();
                }

                row = rowSaver;
                i_Col = colSaver;
                counter = k_Start;
            }

            return isPlayerWinner;
        }

        public void CleanBoard()
        {
            for (int i = k_Start; i <= r_Rows; i++)
            {
                for (int j = k_Start; j <= r_Cols; j++)
                {
                    m_Matrix[i, j] = k_ZeroChar;
                }
            }

            for (int i = k_Zero; i < r_Cols + k_Start; i++)
            {
                m_BoardStatus[i] = k_Zero;
            }
        }

        private bool isWinnerDirUpDown(int i_Col, string i_Sign)
        {
            // direction up, down
            bool isPlayerWinner = false;
            int counter = k_Start;
            int row = r_Rows - m_BoardStatus[i_Col] + 1;
            int rowSaver, colSaver;
            rowSaver = row;
            colSaver = i_Col;
            m_ListOfCells.Add(new Point(row, i_Col));
            if ((isPointInBorders(row - 1, i_Col) || isPointInBorders(row + 1, i_Col)) && !isPlayerWinner)
            {
                while (isPointInBorders(row - 1, i_Col) && m_Matrix[row - 1, i_Col] == i_Sign)
                {
                    counter++;
                    m_ListOfCells.Add(new Point(row - 1, i_Col));
                    row -= 1;
                }

                row = rowSaver;
                i_Col = colSaver;
                while (isPointInBorders(row + 1, i_Col) && m_Matrix[row + 1, i_Col] == i_Sign)
                {
                    counter++;
                    m_ListOfCells.Add(new Point(row + 1, i_Col));
                    row += 1;
                }

                if (counter >= k_MinRowCol)
                {
                    isPlayerWinner = true;
                }
                else
                {
                    m_ListOfCells.Clear();
                }
            }

            return isPlayerWinner;
        }

        public int Cols
        {
            get { return r_Cols; }
        }

        public int Rows
        {
            get { return r_Rows; }
        }

        public List<Point> ListOfCells
        {
            get
            {
                return m_ListOfCells;
            }
        }

        public bool IsColAvailable(int i_UserChoise)
        {
            bool isColAvailable = true;
            if (m_BoardStatus[i_UserChoise] >= r_Rows)
            {
                isColAvailable = false;
            }

            return isColAvailable;
        }

        public void InsertCoinToBoard(string i_Sign, int i_Column)
        {
            int rowLocation = r_Rows - (m_BoardStatus[i_Column]++);
            m_Matrix[rowLocation, i_Column] = i_Sign;
            if (NotifyCell != null)
            {
                NotifyCell.Invoke(rowLocation, i_Column, i_Sign, this);
            }
        }

        public void RemoveCoinFromBoard(int i_Column)
        {
            m_BoardStatus[i_Column]--;
            int rowLocation = r_Rows - m_BoardStatus[i_Column];
            m_Matrix[rowLocation, i_Column] = k_ZeroChar;
            if (NotifyCell != null)
            {
                NotifyCell.Invoke(rowLocation, i_Column, k_ZeroChar, this);
            }
        }

        public bool IsBoardFull()
        {
            bool isFull = true;
            for (int i = k_Start; i < m_BoardStatus.Length; i++)
            {
                if (m_BoardStatus[i] < r_Rows)
                {
                    isFull = false;
                    break;
                }
            }

            return isFull;
        }

        private bool isWinnerDirLeftRight(int i_Col, string i_Sign)
        {
            // direction left, right
            bool isPlayerWinner = false;
            int counter = k_Start;
            int row = r_Rows - m_BoardStatus[i_Col] + k_Start;
            int rowSaver, colSaver;
            rowSaver = row;
            colSaver = i_Col;
            m_ListOfCells.Add(new Point(row, i_Col));
            if ((isPointInBorders(row, i_Col + 1) || isPointInBorders(row, i_Col - 1)) && !isPlayerWinner)
            {
                while (isPointInBorders(row, i_Col + 1) && m_Matrix[row, i_Col + 1] == i_Sign)
                {
                    counter++;
                    m_ListOfCells.Add(new Point(row, i_Col + 1));
                    i_Col += 1;
                }

                i_Col = colSaver;
                while (isPointInBorders(row, i_Col - 1) && m_Matrix[row, i_Col - 1] == i_Sign)
                {
                    counter++;
                    m_ListOfCells.Add(new Point(row, i_Col - 1));
                    i_Col -= 1;
                }

                if (counter >= k_MinRowCol)
                {
                    isPlayerWinner = true;
                }
                else
                {
                    m_ListOfCells.Clear();
                }
            }

            return isPlayerWinner;
        }

        public bool IsWinner(int i_Col, string i_Sign)
        {
            bool isPlayerWinner = false;
            isPlayerWinner = isWinnerDirUpDown(i_Col, i_Sign);
            isPlayerWinner = isPlayerWinner ? true : isWinnerDirLeftSlant(i_Col, i_Sign);
            isPlayerWinner = isPlayerWinner ? true : isWinnerDirLeftRight(i_Col, i_Sign);
            isPlayerWinner = isPlayerWinner ? true : isWinnerDirRightSlant(i_Col, i_Sign);

            return isPlayerWinner;
        }

        private bool isPointInBorders(int i_Row, int i_Col)
        {
            bool isPlayerWinner = true;
            if (i_Row <= k_Zero || i_Row > r_Rows)
            {
                isPlayerWinner = false;
            }

            if (i_Col <= k_Zero || i_Col > r_Cols)
            {
                isPlayerWinner = false;
            }

            return isPlayerWinner;
        }

        private bool isWinnerDirLeftSlant(int i_Col, string i_Sign)
        {
            // direction up-right, left-down
            bool isPlayerWinner = false;
            int counter = k_Start;
            int row = r_Rows - m_BoardStatus[i_Col] + k_Start;
            int rowSaver, colSaver;
            rowSaver = row;
            colSaver = i_Col;
            m_ListOfCells.Add(new Point(row, i_Col));
            if ((isPointInBorders(row - 1, i_Col + 1) || isPointInBorders(row + 1, i_Col - 1)) && !isPlayerWinner)
            {
                while (isPointInBorders(row - 1, i_Col + 1) && m_Matrix[row - 1, i_Col + 1] == i_Sign)
                {
                    counter++;
                    m_ListOfCells.Add(new Point(row - 1, i_Col + 1));
                    row -= k_Start;
                    i_Col += k_Start;
                }

                row = rowSaver;
                i_Col = colSaver;
                while (isPointInBorders(row + 1, i_Col - 1) && m_Matrix[row + 1, i_Col - 1] == i_Sign)
                {
                    counter++;
                    m_ListOfCells.Add(new Point(row + 1, i_Col - 1));
                    row += 1;
                    i_Col -= 1;
                }

                if (counter >= k_MinRowCol)
                {
                    isPlayerWinner = true;
                }
                else
                {
                    m_ListOfCells.Clear();
                }

                row = rowSaver;              
            }

            return isPlayerWinner;
        }
    }
}
