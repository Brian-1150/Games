using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace PhoneGame {
    public partial class Form1 : Form {
        Label label;
        Random random = new Random();
        int count = 1;
        int countClicks = 0;
        List<Label> sequenceList = new List<Label>();
        List<Label> userInputList = new List<Label>();
        bool play = true;
        bool correct = true;
        Label click;
        SoundPlayer phoneRing = new SoundPlayer(PhoneGame.Properties.Resources.phone_ring2);
        //SoundPlayer selectionSound = new SoundPlayer(Properties.Resources.selectionSound);

        public Form1() {
            InitializeComponent();
            MessageBox.Show("how to play?");
            SetUpGame();
           

        }
        private void SetUpGame() {
            if (count == 0)
                UserPlayGame();
            else
                NumberOfTimes();

        }
        private void NumberOfTimes() {
            int randomNumber;
            randomNumber = random.Next(0, tableLayoutPanel1.Controls.Count);
            if (tableLayoutPanel1.Controls[randomNumber] is Label)
                label = (Label)tableLayoutPanel1.Controls[randomNumber];
            else return;
            phoneRing.Play();
            label.ForeColor = Color.Black;
            ringDurations.Start();
        }

        private void ringDurations_Tick(object sender, EventArgs e) {
            ringDurations.Stop();
            label.ForeColor = Color.FromArgb(128, 255, 128);
            sequenceList.Add(label);
            count--;
            SetUpGame();

        }
        private void UserPlayGame() {
            MessageBox.Show("You have 5 seconds. Go!");
            count++;
            UserPlayTimer.Start();
        }

        private void Form1_Click(object sender, EventArgs e) {
            click = sender as Label;
            userInputList.Add(click);
            count++;
        }

        private void UserPlayTimer_Tick(object sender, EventArgs e) {
            UserPlayTimer.Stop();
            if (CheckForWin())
                SetUpGame();
            else {
                MessageBox.Show("Sorry! You lose!");
                Close();
            }
        }

        private bool CheckForWin() {
            for (int i = 0; i < (count - 1); i++) {

                
                if (sequenceList.ElementAt(i).Name == userInputList.ElementAt(i).Name) {
                    correct = true;
               
                }
                else {
                    correct = false;
                    return false;
                }
            }
            sequenceList.Clear();
            userInputList.Clear();
            return true;
        }

    }
}
