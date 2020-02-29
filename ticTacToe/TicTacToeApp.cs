using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ticTacToe
{
    public partial class main_form : Form
    {
        private bool turn = true; // true -> player ; false -> computer 


        public main_form()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Text = "X";
            b.Enabled = false;

            string winner = isThereAWinner();
            if(winner != null)
            {
                MessageBox.Show(winner + " is the winner!");
                startNewGame();
                return;
            }

            if (isGameOver() == true)
            {
                MessageBox.Show("DRAW! Game over!");
                startNewGame();
            }
            else
            {
                computersTurn((Button)sender);
                winner = isThereAWinner();
                if (winner != null)
                {
                    MessageBox.Show(winner + " is the winner!");
                    startNewGame();
                    return;
                }
            }
        }


        private void computersTurn(Button playerMove)
        {
            string playerMoveColumn = playerMove.Name.Substring(0, 1);
            int playerMoveRow = Convert.ToInt32(playerMove.Name.Substring(1, 1));

            isPlayerAboutToWin(playerMoveColumn, playerMoveRow);
           
        }

        private void randomMove()
        {
            Console.WriteLine("Random move");
            int emptyCells = 0;
            Button[] buttons = new Button[Controls.Count];
            foreach (Control control in Controls)
            {
                if (control is Button && control.Enabled == true)
                {
                    buttons[emptyCells] = (Button)control;
                    emptyCells++;
                }
            }

            Random rnd = new Random();
            int nextTurnIndex = rnd.Next(0, emptyCells);

            buttons[nextTurnIndex].Text = "O";
            buttons[nextTurnIndex].Enabled = false;
        }

        private void isPlayerAboutToWin(string column,int row)
        {
            bool moveDone = false;
            string previousColumn;
            string nextColumn;
            int previousRow;
            int nextRow;

            string diagonalNextColumn;
            string diagonalPreviousColumn;
            int diagonalNextRow;
            int diagonalPreviousRow;

            switch (column)
            {
                case "A":
                    previousColumn = null;
                    nextColumn = "B";
                    break;
                case "B":
                    previousColumn = "A";
                    nextColumn = "C";
                    break;
                case "C":
                    previousColumn = "B";
                    nextColumn = null;
                    break;
                default:
                    previousColumn = null;
                    nextColumn = null;
                    break;
            }
            switch (row)
            {
                case 1:
                    previousRow = 999;
                    nextRow = 2;
                    break;
                case 2:
                    previousRow = 1;
                    nextRow = 3;
                    break;
                case 3:
                    previousRow = 2;
                    nextRow = 999;
                    break;
                default:
                    previousRow = 999;
                    nextRow = 999;
                    break;
            }

            // Case player is about to win : has two colums or two rows next to each other filled with X
            List<string> dangerousButtons = new List<string>();
            string currentMove = column + row.ToString();

            // Horizontal 
            if(previousColumn != null) dangerousButtons.Add(previousColumn + row.ToString());
            if(nextColumn != null) dangerousButtons.Add(nextColumn + row.ToString());
            // Vertical
            if(previousRow != 999) dangerousButtons.Add(column + previousRow.ToString());
            if(nextRow != 999) dangerousButtons.Add(column + nextRow.ToString());
            // Diagonal
            if (previousColumn != null && nextColumn != null && previousRow != 999 && nextRow != 999)
            {
                dangerousButtons.Add(previousColumn + (row - 1).ToString());
                dangerousButtons.Add(previousColumn + (row + 1).ToString());
                dangerousButtons.Add(nextColumn + (row - 1).ToString());
                dangerousButtons.Add(nextColumn + (row + 1).ToString());
            }
           

            foreach (string button in dangerousButtons)
            {
                if (Controls[button].Text == "X")
                {
                    //  The player wins with the next move - > defense needed
                    // Check if a move is needed vertically or horizontally
                    if(column == button.Substring(0, 1))
                    {
                        Console.WriteLine("SAME COLUMN!");
                        // Same column - > move vertially
                        if(Convert.ToInt32(button.Substring(1,1)) == 1)
                        {
                            if(Controls[column + "3"].Enabled == true)
                            {
                                Controls[column + "3"].Text = "O";
                                Controls[column + "3"].Enabled = false;
                                moveDone = true;
                                break;
                            }
                        }
                        else
                        {
                            if(Controls[column + "1"].Enabled == true)
                            {
                                Controls[column + "1"].Text = "O";
                                Controls[column + "1"].Enabled = false;
                                moveDone = true;
                                break;
                            }
                        }
                    }
                    else if(row == Convert.ToInt32(button.Substring(1, 1)))
                    {
                        Console.WriteLine("SAME ROW!");
                        if (button.Substring(0, 1) == "A")
                        {
                            if(Controls["C" + row].Enabled == true)
                            {
                                Controls["C" + row].Text = "O";
                                Controls["C" + row].Enabled = false;
                                moveDone = true;
                                break;
                            }
                        }
                        else
                        {
                            if(Controls["A" + row].Enabled == true)
                            {
                                Controls["A" + row].Text = "O";
                                Controls["A" + row].Enabled = false;
                                moveDone = true;
                                break;
                            }
                        }
                    }

                }
            }

            if (!moveDone)
            {
                randomMove();
            }

        }
       

        private string isThereAWinner()
        {
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))
                return A1.Text;
            if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled))
                return B1.Text;
            if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))
                return C1.Text;
            if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!A1.Enabled))
                return A1.Text;
            if ((C1.Text == B2.Text) && (B2.Text == A3.Text) && (!C1.Enabled))
                return C1.Text;

            if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))
                return A1.Text;
            if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled))
                return A2.Text;
            if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))
                return A3.Text;


            return null;
        }


        private void startNewGame()
        {
            turn = true;
            foreach (Control control in Controls)
            {
                if (control is Button)
                {
                    control.Text = "";
                    control.Enabled = true;
                }
            }
        }

        private bool isGameOver()
        {
            foreach(Control control in Controls)
            {
                if(control is Button)
                {
                    if (control.Text == "") return false;
                }
            }
            return true;
        }
    }
}
