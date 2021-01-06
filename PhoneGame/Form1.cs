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
        //Section of variables needed
        Label label;
        Random random = new Random();
        int count = 1;
        List<Label> sequenceList = new List<Label>();
        List<Label> userInputList = new List<Label>();
        bool play = false; // using this to ignore mouse clicks until it is time
        Label click;
        // Workingwith sound player
        SoundPlayer phoneRing = new SoundPlayer(Properties.Resources.phone_ring2);
        SoundPlayer selectionSound = new SoundPlayer(Properties.Resources.click_x);
        SoundPlayer buzzer = new SoundPlayer(Properties.Resources.buzzer_x);
        SoundPlayer tried = new SoundPlayer(Properties.Resources.tried);
        SoundPlayer whoops = new SoundPlayer(Properties.Resources.whoops);

        public Form1() {
            InitializeComponent();
            MessageBox.Show("How to play:\n\n" +
                "Closely follow the sequence, then carefully\n" +
                " click the boxes in the same order they were displayed.  \n" +
                "If you correctly choose the boxes in the right order, \n" +
                "the next sequence will increase by one.  Please wait until the \n" +
                "program finishes revealing the boxes for that round before \n" +
                "clicking.  Good luck!");
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
            play = true; //turns on mouse clicks 
            count++; //just to reset count to start at 1 again 
            UserPlayTimer.Start(); //30 sec timer for max time user is allowed to make picks
        }

        private void Form1_Click(object sender, EventArgs e) {
            if (!play) {
                whoops.Play();
                return; //if user clicks it will essentially be ignored
            }
            selectionSound.Play();
            click = sender as Label;
            userInputList.Add(click);
            count++;
            if (sequenceList.Count == userInputList.Count) {
                UserPlayTimer.Stop();
                play = false; //ignore mouse clicks again until ready
                CheckForWin();
            }
        }

        private void UserPlayTimer_Tick(object sender, EventArgs e) {
            UserPlayTimer.Stop();
            buzzer.Play();
            MessageBox.Show("Sorry! Time ran out.  You lose!\n" +
                $"  Your score: {sequenceList.Count - 1} ");
            Close();
        }

        private void CheckForWin() {
            UserPlayTimer.Stop();
            for (int i = 0; i < (count - 1); i++) { //count is minus 1 because it was intentionally set to be one higher each round in another method but here it is still checking the prev round
                if (sequenceList.ElementAt(i).Name != userInputList.ElementAt(i).Name) {//compare  each index item of both lists and game over if they are not equal
                    tried.Play();
                    MessageBox.Show("Sorry!  You lose!\n" +
                          $"  Your score: { sequenceList.Count - 1}");
                    Close();
                }
            }
            sequenceList.Clear();
            userInputList.Clear();
            nextRound.Start(); //start a 1 sec timer for an appropriate delay before next round
        }

        private void nextRound_Tick(object sender, EventArgs e) {
            nextRound.Stop();
            SetUpGame();

        }
    }
}
