using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Huerto_Activity4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // 2c. Check for empty required fields
                if (string.IsNullOrWhiteSpace(txtBasicRate.Text) || string.IsNullOrWhiteSpace(txtBasicHours.Text))
                {
                    MessageBox.Show("Required input textboxes are empty.", "Input Error");
                    return;
                }

                // 2a & 2b. Income Calculations with numeric validation
                double basic = double.Parse(txtBasicRate.Text) * double.Parse(txtBasicHours.Text);
                double hon = GetVal(txtHonRate) * GetVal(txtHonHours);
                double otherInc = GetVal(txtOtherRate) * GetVal(txtOtherHours);

                txtBasicIncome.Text = basic.ToString("N2");
                txtHonTotal.Text = hon.ToString("N2");
                txtOtherTotal.Text = otherInc.ToString("N2");

                double gross = basic + hon + otherInc;
                txtGrossIncome.Text = gross.ToString("N2");

                // Process all Deductions (Regular + Other Loans)
                double regDed = GetVal(txtSSSCont) + GetVal(txtPhilHealth) + GetVal(txtPagibig) + GetVal(txtTax);
                double loanDed = GetVal(txtSSSLoan) + GetVal(txtPagibigLoan) + GetVal(txtFacultySavings) +
                                 GetVal(txtFacultyLoan) + GetVal(txtSalaryLoan) + GetVal(txtOtherLoan);

                double totalDeductions = regDed + loanDed;
                txtTotalDeductions.Text = totalDeductions.ToString("N2");
                txtNetIncome.Text = (gross - totalDeductions).ToString("N2");
            }
            catch (FormatException)
            {
                // Handles Page 121, Questions 2a & 2b
                MessageBox.Show("Invalid input! Please enter numbers only.", "Format Error");
            }
        }

        // Helper function to treat empty deduction/income boxes as 0
        private double GetVal(TextBox txt) => double.TryParse(txt.Text, out double val) ? val : 0;
        

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.png)|*.jpg; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                picEmployee.Image = new Bitmap(open.FileName);
                picEmployee.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void txtBasicRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double rate = double.Parse(txtBasicRate.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input! Please enter a numerical value for the Basic Rate.", "Format Error");
                txtBasicRate.Clear();
                txtBasicRate.Focus();
            }
        }

        private void txtOtherRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double otherRate = double.Parse(txtOtherRate.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: 'Other Income' rate must be a number.", "Input Error");
                txtOtherRate.Focus();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // Clears all textboxes in the form including deductions
            foreach (Control c in this.Controls)
            {
                if (c is TextBox) c.Text = "";
            }
            // Also clear the others ComboBox if you have one
            if (cmbOthers != null) cmbOthers.SelectedIndex = -1;

            picEmployee.Image = null;
            txtEmployeeNumber.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Specifically clears the income/deduction inputs
            txtBasicRate.Clear();
            txtBasicHours.Clear();
            txtHonRate.Clear();
            txtHonHours.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string summary = $"Employee No: {txtEmployeeNumber.Text}\n" +
                     $"Name: {txtFirstname.Text} {txtSurname.Text}\n" +
                     $"Gross Income: {txtGrossIncome.Text}\n" +
                     $"Net Income: {txtNetIncome.Text}";

            MessageBox.Show(summary, "Employee Payslip Summary");
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            // Check if Gross Income is empty before showing preview (Lesson 4 Requirement 2c)
            if (string.IsNullOrEmpty(txtGrossIncome.Text))
            {
                MessageBox.Show("Please click CALCULATE first to generate values for the preview.", "Notice");
                return;
            }

            // Gathering Employee and Payroll data
            string details = "=== PAYSLIP PREVIEW ===\n\n";
            details += $"Employee No: {txtEmployeeNumber.Text}\n";
            details += $"Name: {txtFirstname.Text} {txtMiddleName.Text} {txtSurname.Text}\n";
            details += $"Designation: {txtDesignation.Text}\n\n";

            details += "--- EARNINGS ---\n";
            details += $"Gross Income: {txtGrossIncome.Text}\n\n";

            details += "--- DEDUCTIONS ---\n";
            details += $"Regular Deductions: {txtTotalDeductions.Text}\n";

            // Summing other loans for the preview summary
            double loans = (double.Parse(txtSSSLoan.Text == "" ? "0" : txtSSSLoan.Text) +
                           double.Parse(txtPagibigLoan.Text == "" ? "0" : txtPagibigLoan.Text));
            details += $"Total Loans: {loans.ToString("N2")}\n\n";

            details += "--- SUMMARY ---\n";
            details += $"NET TAKE HOME PAY: {txtNetIncome.Text}";

            // Displaying the final summary
            MessageBox.Show(details, "Payroll System Preview");
        }

        private void txtMiddlename_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbOthers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
