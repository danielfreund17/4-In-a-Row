using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace B16_Ex06_1
{
    public enum eWhoPlays
    {
        FirstPlayer = 0,
        SecondPlayer = 1
    }

    public enum eFinishGameOptions
    {
        PlayerOneWin = 0,
        PlayerTwoWin = 1,
        Tie
    }

    public class UserInterface
    {
        private const int k_MaxBoardSize = 10;
        private const int k_MinBoardSize = 4;
        private BoardForm m_BoardForm;
        private GameSettingsForm m_GameSettingsForm;
        private GameManager m_GameManager;
        private Welcome m_Welcome;
        private HelpForm helpFormat;
        private AboutForm aboutForm;
        private DialogResult result;
        private bool m_ResetNextGame;
        private bool m_ResetNow;

        public UserInterface()
        {
            m_Welcome = new Welcome();
            m_GameManager = new GameManager();
            m_ResetNextGame = false;
            m_ResetNow = false;
        }

        public void RunUserInterface()
        {
            helpFormat = new HelpForm();
            aboutForm = new AboutForm();
            m_Welcome.ShowDialog();
            setUpGameSettings();
            exitApplication();
        }

        private void setUpGameSettings()
        {
            m_GameSettingsForm = new GameSettingsForm();
            m_GameSettingsForm.SetStartButtonMethod(new EventHandler(this.buttonStart_Clicked));
            m_BoardForm = new BoardForm();
            m_BoardForm.Show();
            m_GameSettingsForm.ShowDialog();
        }

        private void buttonStart_Clicked(object sender, EventArgs e)
        {
            if (m_GameSettingsForm.IsNamesEntered())
            {
                setGame();
            }
            else
            {
                MessageBox.Show("You must enter a name for each player!");
            }
        }

        private void setGame()
        {
            if (m_BoardForm.IsBoardSet)
            {
                m_GameSettingsForm.Close();
                m_GameSettingsForm.Hide();
                m_BoardForm.Activate();
                string str = "Start a new game?";
                result = MessageBox.Show(str, "4 In A Row", MessageBoxButtons.YesNo);
                checkDialogAnswer();
            }
            else
            {
                m_BoardForm.Close();
                setGameHelper();
            }
        }

        private void checkDialogAnswer()
        {
            if (result == DialogResult.Yes)
            {
                m_ResetNow = true;
                setGameHelper();
            }
            else
            {
                m_ResetNextGame = true;
                MessageBox.Show("New board size will take effect" + Environment.NewLine + "on the next game.");
            }
        }

        private void setGameHelper()
        {
            int rows = m_GameSettingsForm.GetRows;
            int cols = m_GameSettingsForm.GetCols;
            string firstPlayerName = m_GameSettingsForm.PlayerOneName;
            string secondPlayerName = m_GameSettingsForm.PlayerTwoName;
            playGame(firstPlayerName, secondPlayerName, rows, cols);
        }

        private void playGame(string i_PlayerOneName, string i_PlayerTwoName, int i_Rows, int i_Cols)
        {
            m_GameSettingsForm.Hide();
            m_GameSettingsForm.Close();
            m_GameManager.SetGame(i_PlayerOneName, i_PlayerTwoName, i_Rows, i_Cols);
            setBoardToPlay();
        }

        private void setBoardToPlay()
        {
            if (!m_BoardForm.IsBoardSet)
            {
                m_BoardForm = new BoardForm(m_GameManager, new EventHandler(buttonCol_Clicked));
                setBoardNotifiers();
                m_BoardForm.ShowDialog();
            }

            if(m_ResetNextGame || m_ResetNow)
            {
                m_GameManager.Reset();
                m_BoardForm.ResizeBoard(m_GameManager, new EventHandler(buttonCol_Clicked));
                m_ResetNow = false;
                m_ResetNextGame = false;
            }

            m_BoardForm.ResetGame(m_GameManager);
        }

        private void setBoardNotifiers()
        {
            m_BoardForm.NewTournirEvent += this.newTournir;
            m_BoardForm.PropertiesEvent += this.setProperties;
            m_BoardForm.HelpEvent += this.showHelpForm;
            m_BoardForm.AboutEvent += this.showAbout;
            m_GameManager.Board.NotifyCell += m_BoardForm.UpdateBoardForm;
            m_BoardForm.NewGameEvent += this.anotherRound;
            m_BoardForm.ExitEvent += this.exitApplication;
            m_BoardForm.TimerFinish += this.checkIfGameFinish;
        }

        private void buttonCol_Clicked(object sender, EventArgs e)
        {
            if (!m_BoardForm.IsTimerIsRunning)
            {
                doIteration((sender as Control).Text);
            }
        }

        private void doIteration(string i_Col)
        {
            int userChoice = int.Parse(i_Col);
            m_GameManager.DoIteration(userChoice);          
        }

        private void checkIfGameFinish()
        {
            if (GameManager.IsGameFinish)
            {
                if(m_ResetNextGame)
                {
                    GameManager.IsGameFinish = false;
                    setGameHelper();
                }
                else
                {
                    afterMatch();
                }
            }
            else
            {
                m_BoardForm.SetNextPlayer();
            }
        }

        private void afterMatch()
        {
            eFinishGameOptions finishReason = m_GameManager.WhyGameFinished();
            DialogResult result;

            switch (finishReason)
            {
                case eFinishGameOptions.Tie:
                    result = gameEndedWithTie();
                    break;
                default:
                    result = gameEndedWithWin(finishReason);
                    break;
            }

            switch (result)
            {
                case DialogResult.Yes:
                    anotherRound();
                    break;
                default:
                    exitApplication();
                    break;
            }
        }

        private DialogResult gameEndedWithTie()
        {
            DialogResult result = MessageBox.Show(
                "Tie!! " + Environment.NewLine + "Another Round?",
                "A Tie",
                MessageBoxButtons.YesNo);

            return result;
        }

        private DialogResult gameEndedWithWin(eFinishGameOptions i_FinishOption)
        {
            string winnerName;
            winnerName = m_GameManager.FinishGameResults(i_FinishOption);
            string str = string.Format("{0} Won!!" + Environment.NewLine + "Another Round?", winnerName);
            m_BoardForm.PaintCells(m_GameManager.Board.ListOfCells, m_GameManager.GetPlayer((int)i_FinishOption).Sign);
            result = MessageBox.Show(str, "A Win", MessageBoxButtons.YesNo);
            return result;
        }

        private void newTournir()
        {
            m_GameManager.resetVictories();
            anotherRound();
        }

        private void setProperties()
        {
            m_GameSettingsForm = new GameSettingsForm();
            m_GameSettingsForm.SetStartButtonMethod(new EventHandler(this.buttonStart_Clicked));
            m_GameSettingsForm.ShowDialog();
        }

        private void showHelpForm()
        {
            helpFormat.ShowDialog();
        }

        private void showAbout()
        {
            aboutForm.ShowDialog();
        }

        private void anotherRound()
        {
            if(m_ResetNextGame)
            {
                setGameHelper();
            }
            else
            {
                m_GameManager.Board.CleanBoard();
                m_BoardForm.ResetGame(m_GameManager);
                m_GameManager.Reset();
            }
        }

        private void exitApplication()
        {
            MessageBox.Show("Thanks for using our cool App!!!", "Bye Bye", MessageBoxButtons.OK);
            Application.Exit();
        }
    }
}
