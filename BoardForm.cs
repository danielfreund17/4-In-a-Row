using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace B16_Ex06_1
{
    public delegate void MenuNotifyDelegate();

    public delegate void TimerDelegate();

    public class BoardForm : Form
    {
        private const int k_FirstRow = 0;
        private const int k_CheckCounter = 2;
        private const int k_Start = 1;
        private const int k_CellSize = 60;
        private const string k_FirstPlayer = "O";
        private const string k_SecondPlayer = "X";

        public event MenuNotifyDelegate NewTournirEvent;

        public event MenuNotifyDelegate NewGameEvent;

        public event MenuNotifyDelegate ExitEvent;

        public event MenuNotifyDelegate HelpEvent;

        public event MenuNotifyDelegate AboutEvent;

        public event MenuNotifyDelegate PropertiesEvent;

        public event TimerDelegate TimerFinish;

        private string m_firstPlayerName;
        private string m_secondPlayerName;
        private int m_TimerSteper = 0;
        private System.Windows.Forms.Timer insertCoin;
        private System.ComponentModel.IContainer components;
        private PictureBox[,] pictureBox;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem gameToolStripMenuItem;
        private ToolStripMenuItem startANewGameToolStripMenuItem;
        private ToolStripMenuItem startANewTournirToolStripMenuItem;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem howToPlayToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private int m_CurrentCol = 0, m_CurrentRow = 0, m_BoardRow = 4, m_BoardCol = 4;
        private string m_Sign = "O";
        private PictureBox CellPointer;
        private System.Windows.Forms.Timer tm_Winner;
        private List<Point> m_ListOfCells;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel currentName;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel firstPlayerLabel;
        private ToolStripStatusLabel firstPlayerVictory;
        private ToolStripStatusLabel secondPlayerLabel;
        private ToolStripStatusLabel secondPlayerVictory;
        private GraphicsPath newPicture;
        private Bitmap newRegion;
        private bool m_isBoardSet = false;

        public BoardForm()
        {
            InitializeComponent();
            this.setSize(6, 7);
        }

        public BoardForm(GameManager io_GameManager, EventHandler io_Func)
        {
            newPicture = new GraphicsPath();
            newRegion = new Bitmap(Properties.Resources.newEmptyCell);
            newPicture = QuickCalculateGraphicsPath(newRegion, 12, 12);
            m_BoardCol = io_GameManager.Board.Cols;
            m_BoardRow = io_GameManager.Board.Rows;
            pictureBox = new PictureBox[10, 11];
            setSize(m_BoardRow, m_BoardCol);
            initControls(m_BoardRow, m_BoardCol);
            initColsSelectionButtons(m_BoardCol, io_Func);
            InitializeComponent();
            setLabels(io_GameManager);
            m_isBoardSet = true;
            createCellPointer();
        }

        private static GraphicsPath QuickCalculateGraphicsPath(Bitmap i_Bitmap, int i_X, int i_Y)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            Color transparentColor = i_Bitmap.GetPixel(i_X, i_Y);
            int startRegionArea = -1;
            Color pixelColor = Color.Empty;
            BitmapData bitmapData = i_Bitmap.LockBits(new Rectangle(0, 0, i_Bitmap.Width, i_Bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            IntPtr scanL = bitmapData.Scan0;
            int yOffset = bitmapData.Stride - (i_Bitmap.Width * 3);

            unsafe
            {
                int bitmapHeight = i_Bitmap.Height;
                int bitmapWidth = i_Bitmap.Width;

                byte* p = (byte*)(void*)scanL;
                for (int y = 0; y <= bitmapHeight - 1; y++)
                {
                    for (int x = 0; x <= bitmapWidth - 1; x++)
                    {
                        int B = (int)p[0];
                        int G = (int)p[1];
                        int R = (int)p[2];
                        pixelColor = Color.FromArgb(R, G, B);
                        if (pixelColor == transparentColor && startRegionArea != -1)
                        {
                            graphicsPath.AddRectangle(new Rectangle(startRegionArea, y, (x - 1) - startRegionArea, 1));
                            startRegionArea = -1;
                        }

                        if (pixelColor != transparentColor && startRegionArea == -1)
                        {
                            startRegionArea = x;
                        }

                        p += 3;
                    }

                    if (startRegionArea != -1)
                    {
                        graphicsPath.AddRectangle(new Rectangle(startRegionArea, y, i_Bitmap.Width - startRegionArea, 1));
                        startRegionArea = -1;
                    }

                    p += yOffset;
                }
            }

            i_Bitmap.UnlockBits(bitmapData);
            return graphicsPath;
        }

        private void showLabels()
        {
            this.currentName.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.firstPlayerVictory.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.firstPlayerLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.secondPlayerLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.secondPlayerVictory.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.DisplayStyle = ToolStripItemDisplayStyle.Text;
        }

        public void ResizeBoard(GameManager i_GameManager, EventHandler io_Func)
        {
            int row = i_GameManager.Board.Rows;
            int col = i_GameManager.Board.Cols;
            cleanBoard();
            initControls(row, col);
            initColsSelectionButtons(col, io_Func);
            setSize(row, col);
            setLabels(i_GameManager);
            this.CellPointer.Image = global::B16_Ex06_1.Properties.Resources.CoinYellow;
            m_isBoardSet = true;
        }

        private void setLabels(GameManager io_GameManager)
        {
            const int firstPlayer = 0;
            const int secondPlayer = 1;

            int firstPlayerVictories = io_GameManager.GetPlayerVictories(firstPlayer);
            m_firstPlayerName = io_GameManager.GetPlayer(firstPlayer).Name + ":";
            this.currentName.Text = m_firstPlayerName;
            int secondPlayerVictories;

            secondPlayerVictories = io_GameManager.GetPlayerVictories(1);
            m_secondPlayerName = io_GameManager.GetPlayer(secondPlayer).Name + ":";

            this.firstPlayerLabel.Text = m_firstPlayerName;
            this.firstPlayerVictory.Text = firstPlayerVictory.ToString();
            this.secondPlayerLabel.Text = m_secondPlayerName;
            this.secondPlayerVictory.Text = secondPlayerVictory.ToString();
            showLabels();
        }

        private void initColsSelectionButtons(int i_Cols, EventHandler io_Func)
        {
            for (int j = k_FirstRow; j < i_Cols; j++)
            {
                (pictureBox[j, k_FirstRow] as Control).Text = (j + 1).ToString();
                this.pictureBox[j, k_FirstRow].Enabled = true;
                this.pictureBox[j, k_FirstRow].Image = null;
                this.pictureBox[j, k_FirstRow].Click += io_Func;
                this.pictureBox[j, k_FirstRow].MouseEnter += controls_MouseEnter;
            }
        }

        private void controls_MouseEnter(object sender, EventArgs e)
        {
            if (!insertCoin.Enabled)
            {
                CellPointer.Location = (sender as PictureBox).Location;
                CellPointer.BringToFront();
            }
        }

        private void initControls(int i_Rows, int i_Cols)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardForm));
            for (int i = k_FirstRow; i < i_Cols; i++)
            {
                for (int j = k_FirstRow; j <= i_Rows; j++)
                {
                    this.pictureBox[i, j] = new PictureBox();
                    this.pictureBox[i, j].Image = B16_Ex06_1.Properties.Resources.EmptyCell;
                    this.pictureBox[i, j].Location = new System.Drawing.Point((i * k_CellSize) + 20, (j * k_CellSize) + 40);
                    this.pictureBox[i, j].Enabled = false;
                    this.pictureBox[i, j].SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    this.pictureBox[i, j].Size = new System.Drawing.Size(k_CellSize, k_CellSize);
                    this.pictureBox[i, j].TabIndex = 0;
                    this.pictureBox[i, j].Show();
                    this.pictureBox[i, j].TabStop = false;
                    this.pictureBox[i, j].BringToFront();
                    this.pictureBox[i, j].BackColor = Color.CornflowerBlue;
                    this.Controls.Add(pictureBox[i, j]);
                    if (j != k_FirstRow)
                    {
                        this.pictureBox[i, j].Region = new Region(newPicture);
                    }
                }
            }
        }

        private void cleanBoard()
        {
            for (int i = k_FirstRow; i < 10; i++)
            {
                for (int j = k_FirstRow; j <= 10; j++)
                {
                    if (this.pictureBox[i, j] != null)
                    {
                        this.pictureBox[i, j].Hide();
                    }
                }
            }
        }

        private void setSize(int i_Rows, int i_Cols)
        {
            this.Text = "4 In A Row - By Daniel & Idan";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(
                (i_Cols * k_CellSize) + k_CellSize,
                (i_Rows + 3) * k_CellSize);
        }

        private void createCellPointer()
        {
            this.CellPointer.Show();
            this.CellPointer.Image = global::B16_Ex06_1.Properties.Resources.CoinYellow;

            GraphicsPath gp2 = new GraphicsPath();
            gp2.AddEllipse(new Rectangle(0, 0, CellPointer.Width - 1, CellPointer.Height - 1));
            CellPointer.Region = new Region(gp2);
            CellPointer.BackColor = Color.Black;
        }

        public void UpdateBoardForm(int i_Row, int i_Col, string i_Sign, Board i_Board)
        {
            m_CurrentCol = i_Col;
            m_CurrentRow = i_Row;
            m_Sign = i_Sign;
            CellPointer.SendToBack();
            insertCoin.Enabled = true;
            CellPointer.Location = pictureBox[m_CurrentCol - 1, 0].Location;
            this.MouseMove -= BoardForm_MouseMove;
            checkColomn(i_Board, i_Col);
        }

        private void checkColomn(Board i_Board, int i_Col)
        {
            if (!i_Board.IsColAvailable(i_Col))
            {
                pictureBox[i_Col - 1, k_FirstRow].Enabled = false;
            }
            else
            {
                pictureBox[i_Col - 1, k_FirstRow].Enabled = true;
            }
        }

        private void insertCoin_Tick(object sender, EventArgs e)
        {
            if (CellPointer.Location == this.pictureBox[m_CurrentCol - 1, m_CurrentRow].Location)
            {
                doWhenReachedBottom();
            }
            else
            {
                CellPointer.Top += 10;
            }
        }

        public void doWhenReachedBottom()
        {
            if (m_Sign == k_SecondPlayer)
            {
                pictureBox[m_CurrentCol - 1, m_CurrentRow].Image = B16_Ex06_1.Properties.Resources.FullCellRed;
                pictureBox[m_CurrentCol - 1, m_CurrentRow].Region = new Region();
            }
            else
            {
                pictureBox[m_CurrentCol - 1, m_CurrentRow].Image = B16_Ex06_1.Properties.Resources.FullCellYellow;
                pictureBox[m_CurrentCol - 1, m_CurrentRow].Region = new Region();
            }

            CellPointer.BringToFront();
            CellPointer.Location = pictureBox[m_CurrentCol - 1, 0].Location;
            this.MouseMove += BoardForm_MouseMove;
            insertCoin.Enabled = false;
            if (TimerFinish != null)
            {
                TimerFinish.Invoke();
            }
        }

        public void SetNextPlayer()
        {
            if (m_Sign == k_SecondPlayer)
            {
                CellPointer.Image = B16_Ex06_1.Properties.Resources.CoinYellow;
                currentName.Text = m_firstPlayerName;
            }
            else
            {
                CellPointer.Image = B16_Ex06_1.Properties.Resources.CoinRed;
                currentName.Text = m_secondPlayerName;
            }
        }

        public void ResetGame(GameManager io_GameManager)
        {
            const int k_FirstPlayer = 0, k_SecondPlayer = 1;
            this.firstPlayerVictory.Text = io_GameManager.GetPlayer(k_FirstPlayer).Victories.ToString();
            this.secondPlayerVictory.Text = io_GameManager.GetPlayer(k_SecondPlayer).Victories.ToString();
            m_BoardRow = io_GameManager.Board.Rows;
            m_BoardCol = io_GameManager.Board.Cols;
            resetAllCells();
            resetColsButtons();
            setLabels(io_GameManager);
            tm_Winner.Enabled = false;
            CellPointer.Image = B16_Ex06_1.Properties.Resources.CoinYellow;
        }

        private void resetAllCells()
        {
            for (int i = k_FirstRow; i < m_BoardCol; i++)
            {
                for (int j = 1; j <= m_BoardRow; j++)
                {
                    this.pictureBox[i, j].Region = new Region(newPicture);
                    pictureBox[i, j].Image = B16_Ex06_1.Properties.Resources.EmptyCell;
                }
            }
        }

        private void resetColsButtons()
        {
            for (int j = k_FirstRow; j < m_BoardCol; j++)
            {
                pictureBox[j, k_FirstRow].Enabled = true;
                pictureBox[j, k_FirstRow].Image = null;
            }
        }

        private void resetPlayers()
        {
            currentName.Text = m_firstPlayerName;
            CellPointer.Image = B16_Ex06_1.Properties.Resources.CoinYellow;
            this.currentName.Text = m_firstPlayerName;
            this.firstPlayerLabel.Text = m_firstPlayerName;
            this.firstPlayerVictory.Text = k_FirstRow.ToString();
            this.secondPlayerLabel.Text = m_secondPlayerName;
            this.secondPlayerVictory.Text = k_FirstRow.ToString();
        }

        public bool IsTimerIsRunning
        {
            get { return insertCoin.Enabled; }
        }

        public bool IsBoardSet
        {
            get
            {
                return m_isBoardSet;
            }

            set
            {
                m_isBoardSet = value;
            }
        }

        private void startANewTournirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewTournirEvent != null)
            {
                NewTournirEvent.Invoke();
            }

            resetAllCells();
            resetColsButtons();
            resetPlayers();
        }

        private void startANewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewGameEvent != null)
            {
                CellPointer.Image = B16_Ex06_1.Properties.Resources.CoinYellow;
                NewGameEvent.Invoke();
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PropertiesEvent != null)
            {
                PropertiesEvent.Invoke();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExitEvent != null)
            {
                ExitEvent.Invoke();
            }
        }

        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (HelpEvent != null)
            {
                HelpEvent.Invoke();
            }
        }

        private void tm_Winner_Tick(object sender, EventArgs e)
        {
            if (m_TimerSteper % k_CheckCounter == k_FirstRow)
            {
                PaintCellsPink(m_ListOfCells, m_Sign);
            }
            else
            {
                PaintCellsDefault(m_ListOfCells, m_Sign);
            }

            m_TimerSteper++;
        }

        public void PaintCells(List<Point> i_CellsToPaint, string i_Player)
        {
            m_ListOfCells = i_CellsToPaint;
            m_Sign = i_Player;
            CellPointer.Hide();
            tm_Winner.Enabled = true;
        }

        private void PaintCellsPink(List<Point> i_CellsToPaint, string i_Player)
        {
            foreach (Point point in i_CellsToPaint)
            {
                if (i_Player == k_FirstPlayer)
                {
                    this.pictureBox[point.Y - 1, point.X].Image = B16_Ex06_1.Properties.Resources.WinnerYellow;
                }
                else
                {
                    this.pictureBox[point.Y - 1, point.X].Image = B16_Ex06_1.Properties.Resources.WinnerRed;
                }
            }
        }

        private void PaintCellsDefault(List<Point> i_CellsToPaint, string i_Player)
        {
            foreach (Point point in i_CellsToPaint)
            {
                if (i_Player == k_FirstPlayer)
                {
                    this.pictureBox[point.Y - 1, point.X].Image = B16_Ex06_1.Properties.Resources.FullCellYellow;
                }
                else
                {
                    this.pictureBox[point.Y - 1, point.X].Image = B16_Ex06_1.Properties.Resources.FullCellRed;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AboutEvent != null)
            {
                AboutEvent.Invoke();
            }
        }

        private void BoardForm_MouseMove(object sender, MouseEventArgs e)
        {
            CellPointer.Show();
            CellPointer.Location = e.Location;
            CellPointer.BringToFront();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BoardForm));
            this.insertCoin = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startANewGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startANewTournirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToPlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CellPointer = new System.Windows.Forms.PictureBox();
            this.tm_Winner = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.firstPlayerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.firstPlayerVictory = new System.Windows.Forms.ToolStripStatusLabel();
            this.secondPlayerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.secondPlayerVictory = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.CellPointer).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();

            //// insertCoin

            this.insertCoin.Interval = 1;
            this.insertCoin.Tick += new System.EventHandler(this.insertCoin_Tick);

            //// menuStrip1

            this.menuStrip1.BackColor = System.Drawing.Color.SteelBlue;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
            this.gameToolStripMenuItem,
            this.helpToolStripMenuItem
            });
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.Stretch = false;
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";

            //// gameToolStripMenuItem

            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
            this.startANewGameToolStripMenuItem,
            this.startANewTournirToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem
            });
            this.gameToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.gameToolStripMenuItem.Text = "Game";

            //// startANewGameToolStripMenuItem

            this.startANewGameToolStripMenuItem.Name = "startANewGameToolStripMenuItem";
            this.startANewGameToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.startANewGameToolStripMenuItem.Text = "Start a New Game";
            this.startANewGameToolStripMenuItem.Click += new System.EventHandler(this.startANewGameToolStripMenuItem_Click);

            //// startANewTournirToolStripMenuItem

            this.startANewTournirToolStripMenuItem.Name = "startANewTournirToolStripMenuItem";
            this.startANewTournirToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.startANewTournirToolStripMenuItem.Text = "Start a New Tournir";
            this.startANewTournirToolStripMenuItem.Click += new System.EventHandler(this.startANewTournirToolStripMenuItem_Click);

            //// propertiesToolStripMenuItem

            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.propertiesToolStripMenuItem.Text = "Properties...";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);

            //// toolStripSeparator2

            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(204, 6);

            //// exitToolStripMenuItem

            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);

            //// helpToolStripMenuItem

            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
            this.howToPlayToolStripMenuItem,
            this.toolStripSeparator3,
            this.aboutToolStripMenuItem
            });
            this.helpToolStripMenuItem.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.helpToolStripMenuItem.Text = "Help";

            //// howToPlayToolStripMenuItem

            this.howToPlayToolStripMenuItem.Name = "howToPlayToolStripMenuItem";
            this.howToPlayToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.howToPlayToolStripMenuItem.Text = "How to play?";
            this.howToPlayToolStripMenuItem.Click += new System.EventHandler(this.howToPlayToolStripMenuItem_Click);

            //// toolStripSeparator3

            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(155, 6);

            //// aboutToolStripMenuItem

            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);

            //// CellPointer

            this.CellPointer.Enabled = false;
            this.CellPointer.Location = new System.Drawing.Point(-1, 36);
            this.CellPointer.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.CellPointer.Name = "CellPointer";
            this.CellPointer.Size = new System.Drawing.Size(60, 60);
            this.CellPointer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.CellPointer.TabIndex = 0;
            this.CellPointer.TabStop = false;

            //// tm_Winner

            this.tm_Winner.Interval = 200;
            this.tm_Winner.Tick += new System.EventHandler(this.tm_Winner_Tick);

            //// statusStrip1

            this.statusStrip1.BackColor = System.Drawing.Color.PapayaWhip;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.currentName,
            this.toolStripStatusLabel3,
            this.firstPlayerLabel,
            this.firstPlayerVictory,
            this.secondPlayerLabel,
            this.secondPlayerVictory
            });
            this.statusStrip1.Location = new System.Drawing.Point(0, 239);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";

            //// toolStripStatusLabel1

            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel1.Text = "Current Player:";

            //// toolStripStatusLabel2

            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);

            //// currentName

            this.currentName.Name = "currentName";
            this.currentName.Size = new System.Drawing.Size(0, 17);

            //// toolStripStatusLabel3

            this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel3.Text = "current";

            //// firstPlayerLabel

            this.firstPlayerLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.firstPlayerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.firstPlayerLabel.Name = "firstPlayerLabel";
            this.firstPlayerLabel.Size = new System.Drawing.Size(0, 17);
            this.firstPlayerLabel.Text = "first";

            //// firstPlayerVictory

            this.firstPlayerVictory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.firstPlayerVictory.Name = "firstPlayerVictory";
            this.firstPlayerVictory.Size = new System.Drawing.Size(0, 17);
            this.firstPlayerVictory.Text = "0";

            //// secondPlayerLabel

            this.secondPlayerLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.secondPlayerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.secondPlayerLabel.Name = "secondPlayerLabel";
            this.secondPlayerLabel.Size = new System.Drawing.Size(0, 17);
            this.secondPlayerLabel.Text = "second";

            //// secondPlayerVictory

            this.secondPlayerVictory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.secondPlayerVictory.Name = "secondPlayerVictory";
            this.secondPlayerVictory.Size = new System.Drawing.Size(0, 17);
            this.secondPlayerVictory.Text = "0";

            //// BoardForm

            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CellPointer);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "BoardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BoardForm_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.CellPointer).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
