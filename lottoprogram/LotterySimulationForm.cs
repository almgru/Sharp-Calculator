using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace LotterySimulation
{
    // Used to disable and enable controls depending on the state of the simulation
    enum SimulationState
    {
        Running,
        Completed,
        Canceled
    }

    /*
     * Main form for the lottery simulator program.
     * 
     * The program lets the user select a lottery row, consisting of 7 unique numbers
     * between 1 and 35, and the number of draws that should be simulated. When the user 
     * presses the simulate button, a background task is dispatched that simulates the desired 
     * number of draws by randomly selecting a lottery row and checking how many of the
     * randomly generated numbers match the lottery row selected by the user. When the 
     * simulation is complete, the number of occurrences of 5, 6 and 7 matching numbers
     * is displayed.
     * 
     * When the simulation is running, the user can cancel the simulation by pressing the
     * cancel button.
     */
    public partial class LotterySimulationForm : Form
    {
        private const int NR_OF_LOTTERY_NUMBERS = 7;
        private const int LOTTERY_MIN_NR = 1;
        private const int LOTTERY_MAX_NR = 35;
        private const int PROGRESS_BAR_RESOLUTION = 1;
        private const int MIN_MATCHES = 5;

        // Used to keep track of how long the simulation takes
        private Stopwatch stopwatch;

        public LotterySimulationForm()
        {
            InitializeComponent();
        }

        #region Event handlers
        private void ButtonBeginSimulation_Clicked(object sender, EventArgs e)
        {
            List<int> numbers;

            try
            {
                numbers = GetLotteryRow();
            }
            catch (ArgumentException ex)
            {
                ShowErrorDialog(ex.Message);
                return;
            }

            // No need to round or do input validation, control is restricted to integers
            int nrOfDraws = (int)InputNrOfDraws.Value;

            SetUIState(SimulationState.Running);
            stopwatch = Stopwatch.StartNew();

            // Run the simulation as a background task in order to not block the UI.
            SimulationWorker.RunWorkerAsync(
                new Tuple<ICollection<int>, int>(numbers, nrOfDraws)
            );
        }

        private void ButtonCancelSimulation_Clicked(object sender, EventArgs e)
        {
            SimulationWorker.CancelAsync();
            stopwatch.Stop();
            SetUIState(SimulationState.Canceled);
        }

        /*
         * Called when the background simulation worker is dispatched.
         */
        private void SimulationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // PerformSimulation throws OperationCanceledException when canceled, so this needs
            // to be handled.
            try
            {
                e.Result = PerformSimulation(worker, e);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true; // Mark task as cancelled
            }
        }

        /*
         * Called when the background simulation worker has completed the simulation, or the 
         * simulation has been cancelled.
         */
        private void SimulationWorker_RunWorkerCompleted(object sender,
                                                         RunWorkerCompletedEventArgs e)
        {
            stopwatch.Stop();

            if (e.Error != null) // Check if the task threw any unhandled exceptions
            {
                ShowErrorDialog(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                SetUIState(SimulationState.Canceled);
            }
            else
            {
                Dictionary<int, int> result = e.Result as Dictionary<int, int>;
                SetOutputFields(result[5], result[6], result[7]);
                SetUIState(SimulationState.Completed);
            }
        }

        /*
         * Called when a worker reports progress. Updates progress bar.
         */
        private void SimulationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
        }
        #endregion

        #region UI methods
        /*
         * Enables or disables UI controls depending the state of the simulation.
         */
        private void SetUIState(SimulationState state)
        {
            switch (state)
            {
                case SimulationState.Running:
                    SetInputFieldsEnabled(false);
                    ClearOutputFields();
                    ProgressBar.Value = ProgressBar.Minimum;
                    ButtonBeginSimulation.Enabled = false;
                    ButtonCancelSimulation.Enabled = true;
                    LabelProgress.Text = "Simulerar dragningar. Vänligen vänta...";
                    break;

                case SimulationState.Completed:
                    SetInputFieldsEnabled(true);
                    ButtonBeginSimulation.Enabled = true;
                    ButtonCancelSimulation.Enabled = false;
                    ProgressBar.Value = ProgressBar.Maximum;
                    LabelProgress.Text = 
                        $"Simulering färdig efter {stopwatch.ElapsedMilliseconds / 1000.0} sek.";
                    break;

                case SimulationState.Canceled:
                    SetInputFieldsEnabled(true);
                    ButtonBeginSimulation.Enabled = true;
                    ButtonCancelSimulation.Enabled = false;
                    ProgressBar.Value = ProgressBar.Maximum;
                    LabelProgress.Text = "Simulering avbruten.";
                    break;

                default:
                    throw new ArgumentException("Unknown simulation state.");
            }
        }

        private void ClearOutputFields()
        {
            OutputNrOf5.Clear();
            OutputNrOf6.Clear();
            OutputNrOf7.Clear();
        }

        private void SetOutputFields(int fives, int sixes, int sevens)
        {
            OutputNrOf5.Text = fives.ToString("N0");
            OutputNrOf6.Text = sixes.ToString("N0");
            OutputNrOf7.Text = sevens.ToString("N0");
        }

        private void SetInputFieldsEnabled(bool enabled)
        {
            InputLotteryNr1.Enabled = enabled;
            InputLotteryNr2.Enabled = enabled;
            InputLotteryNr3.Enabled = enabled;
            InpuLotterytNr4.Enabled = enabled;
            InputLotteryNr5.Enabled = enabled;
            InputLotteryNr6.Enabled = enabled;
            InputLotteryNr7.Enabled = enabled;
            InputNrOfDraws.Enabled = enabled;
        }

        private void ShowErrorDialog(string message)
        {
            MessageBox.Show(
                this,
                message,
                "Felmeddelande",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private NumericUpDown GetLotteryNumberInputField(int forNumber)
        {
            switch (forNumber)
            {
                case 1:
                    return InputLotteryNr1;

                case 2:
                    return InputLotteryNr2;

                case 3:
                    return InputLotteryNr3;

                case 4:
                    return InpuLotterytNr4;

                case 5:
                    return InputLotteryNr5;

                case 6:
                    return InputLotteryNr6;

                case 7:
                    return InputLotteryNr7;

                default:
                    throw new ArgumentException($"No input for lottery number {forNumber}.");
            }
        }

        #endregion

        /*
         *  Collects the lotto numbers from the input controls and validates them.
         *  
         *  Since the controls themselves handle most of the validation (only integer, 
         *  min/max value, etc.), the only validation that is required here is checking for 
         *  duplicated numbers.
         *  
         *  Throws ArgumentException if validation fails.
         */
        private List<int> GetLotteryRow()
        {
            List<int> numbers = new List<int>();

            for (int i = 1; i <= NR_OF_LOTTERY_NUMBERS; i++)
            {
                // Get control for lottery number i.
                NumericUpDown input = GetLotteryNumberInputField(i);

                // No need to round or do input validation, control is restricted to integers
                int value = (int)input.Value;

                // Validate that each number is unique
                if (numbers.Contains(value))
                {
                    throw new ArgumentException("Varje nummer i lottoraden måste vara unikt.");
                }

                numbers.Add(value);
            }

            return numbers;
        }

        /*
         * Given a collection of 7 unique numbers, simulates a lottery draw and returns the 
         * number of matching numbers. Only returns a meaningful result if at least 'atLeast'
         * numbers match.
         */
        private int PerformDraw(Random random, ICollection<int> numbers, int atLeast)
        {
            int matches = 0;
            int numbersCount = numbers.Count;
            List<int> drawnNumbers = new List<int>();

            for (int i = 0; i < NR_OF_LOTTERY_NUMBERS; i++)
            {
                int drawn;

                do // Make sure there are no duplicates in the simulated lottery row
                {
                    drawn = random.Next(LOTTERY_MIN_NR, LOTTERY_MAX_NR + 1);
                } while (drawnNumbers.Contains(drawn));

                drawnNumbers.Add(drawn);

                if (numbers.Contains(drawn))
                {
                    matches++;
                }

                // Optimization: Do not continue if it's not possible to get at least 'atLeast' 
                // matching numbers.
                if (numbersCount - i - 1 < atLeast - matches)
                {
                    break;
                }
            }

            return matches;
        }

        /*
         * Starts the simulation and perform n draws, where n is the user specified nr of draws.
         * 
         * Returns a dictionary where the keys are numbers between 5 and 7 and the values are
         * the number of draws that had that number of matching numbers. For example key 5 in
         * the dictionary stores the number of simulated draws that had that had 5 matching 
         * numbers.
         * 
         * Throws OperationCanceledException if the user cancels the simulation.
         */
        private Dictionary<int, int> PerformSimulation(BackgroundWorker worker, DoWorkEventArgs e)
        {
            // Extract arguments from event
            Tuple<ICollection<int>, int> args = e.Argument as Tuple<ICollection<int>, int>;
            ICollection<int> lotteryNumbers = args.Item1;
            int nrOfDraws = args.Item2;

            // Dictionary for storing result as occurrences of matching numbers between 5 and 7. 
            Dictionary<int, int> result = new Dictionary<int, int>
            {
                [5] = 0,
                [6] = 0,
                [7] = 0
            };
            Random random = new Random();

            // Used to keep track of how much progress has been made since the progress bar was
            // last updated.
            int previousPercentage = 0;

            for (int i = 0; i < nrOfDraws; i++)
            {
                // Cancel the task if the user has pressed the cancel button.
                if (worker.CancellationPending)
                {
                    // Abort the task by raising an exception. We could have just breaked out of
                    // the loop and it would have the same effect. That would have returned a
                    // result though, which I feel is the wrong approach. Returning null would
                    // also work, but I think it's more explicit to raise an exception.
                    throw new OperationCanceledException();
                } else // Otherwise, simulate a lottery draw and report progress
                {
                    int numberOfMatches = PerformDraw(random, lotteryNumbers, MIN_MATCHES);

                    if (numberOfMatches >= MIN_MATCHES)
                    {
                        result[numberOfMatches]++;
                        int percentage = (int)(Math.Round((i / (double)nrOfDraws) * 100));

                        // Only report progress if it has changed meaningfully
                        if (percentage >= previousPercentage + PROGRESS_BAR_RESOLUTION)
                        {
                            previousPercentage = percentage;
                            worker.ReportProgress(percentage);
                        }
                    }
                }
            }

            return result;
        }
    }
}
