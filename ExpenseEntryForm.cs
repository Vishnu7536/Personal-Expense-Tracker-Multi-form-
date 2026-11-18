
using System;
using System.Drawing;
using System.Windows.Forms;

namespace expense_tracker
{
    public partial class ExpenseEntryForm : Form
    {
        private TextBox txtTitle = new TextBox();
        private TextBox txtAmount = new TextBox();
        private ComboBox cmbCategory = new ComboBox();
        private DateTimePicker dtpDate = new DateTimePicker();
        private TextBox txtDescription = new TextBox();

        public Expense SavedExpense { get; private set; }

        public ExpenseEntryForm(Expense existing = null)
        {
            InitializeComponent();

            Text = existing == null ? "Add Expense" : "Edit Expense";
            Size = new Size(450, 500);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            AddLabel("Expense Title", 25, 75);
            txtTitle.SetBounds(25, 100, 370, 30);
            Controls.Add(txtTitle);

            AddLabel("Amount (₹)", 25, 140);
            txtAmount.SetBounds(25, 165, 370, 30);
            Controls.Add(txtAmount);

            AddLabel("Category", 25, 205);
            cmbCategory.SetBounds(25, 230, 370, 30);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Items.AddRange(new object[]
            {
                "Food", "Travel", "Shopping",
                "Education", "Bills", "Health", "Other"
            });
            cmbCategory.SelectedIndex = 0;
            Controls.Add(cmbCategory);

            AddLabel("Date", 25, 270);
            dtpDate.SetBounds(25, 295, 370, 30);
            Controls.Add(dtpDate);

            AddLabel("Description", 25, 335);
            txtDescription.SetBounds(25, 360, 370, 30);
            Controls.Add(txtDescription);

            Button btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.SetBounds(205, 415, 90, 32);
            btnSave.Click += SaveExpense;
            Controls.Add(btnSave);

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.SetBounds(305, 415, 90, 32);
            btnCancel.Click += (s, e) => Close();
            Controls.Add(btnCancel);

            if (existing != null)
            {
                txtTitle.Text = existing.Title;
                txtAmount.Text = existing.Amount.ToString();
                cmbCategory.SelectedItem = existing.Category;
                dtpDate.Value = existing.Date;
                txtDescription.Text = existing.Description;
            }
        }

        private void AddLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            Controls.Add(lbl);
        }

        private void SaveExpense(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Enter expense title.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount)
                || amount <= 0)
            {
                MessageBox.Show("Enter a valid amount.");
                return;
            }

            SavedExpense = new Expense
            {
                Title = txtTitle.Text.Trim(),
                Amount = amount,
                Category = cmbCategory.Text,
                Date = dtpDate.Value,
                Description = txtDescription.Text.Trim()
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
