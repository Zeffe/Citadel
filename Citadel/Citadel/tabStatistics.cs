using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Citadel
{
    public partial class formMain
    {
        void updateStatistics() {
            if (studentLength != 0)
            {
                try
                {
                    // Show the bar graphs.
                    pnlgMale.Show(); pnlgFemale.Show();
                    pnlgActive.Show(); pnlgNonactive.Show();
                    pnlgFees.Show(); pnlgNoFees.Show();

                    // Initialize the data on the statistic page.                   
                    lblStudentCount.Text = "Total Students: " + studentLength.ToString();
                    lblStudentCount2.Text = studentLength.ToString();
                    totalFees = 0;

                    // Reinitialize all of the elements.
                    listYears.Items.Clear();
                    listStates.Items.Clear();
                    pnlgActive.Height = (int)statHeight;
                    pnlgNonactive.Height = (int)statHeight;
                    pnlgMale.Height = (int)statHeight;
                    pnlgFemale.Height = (int)statHeight;
                    pnlgFees.Height = (int)statHeight;
                    pnlgNoFees.Height = (int)statHeight;

                    // Temporary variables for displaying grade statstics.
                    int _fresh = 0, _soph = 0, _junio = 0, _senio = 0, _colle = 0;
                    hasFees = 0; noFees = 0; aYes = 0; aNo = 0; females = 0; males = 0;
                    List<String> tempYears = new List<String>();
                    List<String> tempState = new List<String>();

                    for (int i = 0; i < students.GetLength(0); i++)
                    {
                        double _temp = 0;
                        if (students[i, 0] == null) break;
                        Double.TryParse(students[i, 3].Trim('$'), out _temp);
                        totalFees += _temp;
                        switch (students[i, 7])
                        {
                            case "9": _fresh++; break;
                            case "10": _soph++; break;
                            case "11": _junio++; break;
                            case "12": _senio++; break;
                            case "13+": _colle++; break;
                        }
                        if (students[i, 3] != "$0.00") hasFees++;
                        else noFees++;
                        if (students[i, 5] == "1") aYes++;    // Get the amount of active and nonactive members.
                        else aNo++;
                        if (students[i, 6] == "0") females++; // Get the amount of males and females.
                        else males++;

                        // Check if the list contains the state.
                        if (!tempState.Contains(students[i, 11]))
                        {
                            tempState.Add(students[i, 11]);
                            states[students[i, 11]] = 1;
                        }
                        else
                        {
                            states[students[i, 11]]++;
                        }

                        // Check if the list contains the year.                        
                        if (!tempYears.Contains(students[i, 4]))
                        {
                            tempYears.Add(students[i, 4]);
                            years[students[i, 4]] = 1;
                        }
                        else
                        {
                            years[students[i, 4]]++;
                        }
                    }

                    // Add the states to the listbox.
                    foreach (String state in tempState)
                    {
                        listStates.Items.Add(state + " :: " + states[state]);
                        cmbReportState.Items.Add(state);
                        cmbReportState.SelectedIndex = 0;
                    }

                    // Add the years to the listbox.
                    foreach (String year in tempYears)
                    {
                        listYears.Items.Add(year + " :: " + years[year]);
                    }

                    string fees = totalFees.ToString();
                    if (fees.Contains('.'))
                    {
                        switch (fees.Split('.').Length)
                        {
                            case 0: fees += "00"; break;
                            case 1: fees += "0"; break;
                            case 2: break;
                        }
                    }
                    else
                    {
                        fees += ".00";
                    }

                    lblFeesDue.Text = "Total Fees: $" + fees;
                    lblActiveStudents.Text = "Active Students: " + aYes.ToString();

                    lblgM.Text = "M: " + males.ToString();
                    lblgF.Text = "F: " + females.ToString();

                    lblaYes.Text = "Yes: " + aYes.ToString();
                    lblaNo.Text = "No: " + aNo.ToString();

                    lblg9.Text = "9th: " + _fresh.ToString();
                    lblg10.Text = "10th: " + _soph.ToString();
                    lblg11.Text = "11th: " + _junio.ToString();
                    lblg12.Text = "12th: " + _senio.ToString();
                    lblg13.Text = "13+: " + _colle.ToString();

                    // Update the graphs on the statistic page.
                    statPercentage(males, females, pnlgMale, pnlgFemale, lblpMale, lblpFemale);
                    statPercentage(aYes, aNo, pnlgActive, pnlgNonactive, lblpActive, lblpNonactive);
                    statPercentage(hasFees, noFees, pnlgFees, pnlgNoFees, lblpFees, lblpNoFees);
                }
                catch
                {
                    msgbox msg = new msgbox("Citadel was not able to load statistics.", "Error", 1, Color.DarkRed);
                    msg.Show();
                }
            }
        }

        void statPercentage(double param1, double param2, Panel panel1, Panel panel2, Label label1, Label label2)
        {
            double temp, temp2;
            temp = (param1 / studentLength) * 100;                          // Get the percentage of males.
            temp2 = statHeight * (temp / 100);                              // Get the height of the panel.
            label1.Text = Convert.ToInt32(temp).ToString() + "%";           // Display the percentage of males.
            label2.Text = Convert.ToInt32((100 - temp)).ToString() + "%";   // Display the percentage of females.
            panel1.Height = Convert.ToInt32(temp2);                         // Set the height of the male panel.
            panel2.Height = Convert.ToInt32(statHeight - temp2);            // Set the height of the female panel.

            // Translate the male and female panels down so that they rest above labels.
            panel1.Location = new Point(panel1.Location.X, Convert.ToInt32(statY + (statHeight - temp2)));
            panel2.Location = new Point(panel2.Location.X, Convert.ToInt32(statY + (statHeight - (statHeight - temp2))));

            // Move the male and female labels to be on top of the bars.
            label1.Location = new Point(label1.Location.X, panel1.Location.Y - 16);
            label2.Location = new Point(label2.Location.X, panel2.Location.Y - 16);
        }
    }
}
