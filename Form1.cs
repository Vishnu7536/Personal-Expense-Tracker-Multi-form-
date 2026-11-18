
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace expense_tracker
{
    public partial class Form1 : Form
    {
        private BindingList<Expense> expenses = new BindingList<Expense>();
        private DataGridView grid = new DataGridView();
        private Label lblTotal = new Label();
        private string dataFile = "expenses.json";

        public Form1()
        {
            InitializeComponent();

            Text = "Personal Expense Tracker";
            Size = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;

            CreateInterface();
            LoadExpenses();
            RefreshGrid();
        }

        private void CreateInterface()
        {
            Label heading = new Label();
            heading.Text = "PERSONAL EXPENSE TRACKER";
            heading.Font = new Font("Arial", 20, FontStyle.Bold);
            heading.Location = new Point(25, 20);
            heading.Size = new Size(600, 40);
            Controls.Add(heading);

            lblTotal.Text = "Total Expenses: Rs. 0";
            lblTotal.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTotal.Location = new Point(25, 80);
            lblTotal.Size = new Size(400, 35);
            Controls.Add(lblTotal);

            Button btnAdd = new Button();
            btnAdd.Text = "Add Expense";
            btnAdd.Location = new Point(25, 135);
            btnAdd.Size = new Size(130, 40);
            btnAdd.Click += (s, e) => AddExpense();
            Controls.Add(btnAdd);

            Button btnEdit = new Button();
            btnEdit.Text = "Edit Expense";
            btnEdit.Location = new Point(170, 135);
            btnEdit.Size = new Size(130, 40);
            btnEdit.Click += (s, e) => EditExpense();
            Controls.Add(btnEdit);

            Button btnDelete = new Button();
            btnDelete.Text = "Delete Expense";
            btnDelete.Location = new Point(315, 135);
            btnDelete.Size = new Size(130, 40);
            btnDelete.Click += (s, e) => DeleteExpense();
            Controls.Add(btnDelete);

            grid.Location = new Point(25, 200);
            grid.Size = new Size(920, 350);
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.DataSource = expenses;
            Controls.Add(grid);
        }

        private void AddExpense()
        {
            using (ExpenseEntryForm form = new ExpenseEntryForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    expenses.Add(form.SavedExpense);
                    SaveExpenses();
                    RefreshGrid();
                }
            }
        }

        private void EditExpense()
        {
            if (grid.CurrentRow == null) return;

            Expense selected = grid.CurrentRow.DataBoundItem as Expense;
            if (selected == null) return;

            using (ExpenseEntryForm form = new ExpenseEntryForm(selected))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    int index = expenses.IndexOf(selected);
                    expenses[index] = form.SavedExpense;
                    SaveExpenses();
                    RefreshGrid();
                }
            }
        }

        private void DeleteExpense()
        {
            if (grid.CurrentRow == null) return;

            Expense selected = grid.CurrentRow.DataBoundItem as Expense;
            if (selected == null) return;

            if (MessageBox.Show("Delete this expense?",
                "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                expenses.Remove(selected);
                SaveExpenses();
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            grid.DataSource = null;
            grid.DataSource = expenses;

            decimal total = 0;
            foreach (Expense item in expenses)
                total += item.Amount;

            lblTotal.Text = "Total Expenses: Rs. " + total.ToString("0.00");
        }

        private void SaveExpenses()
        {
            try
            {
                File.WriteAllText(dataFile,
                    JsonSerializer.Serialize(expenses));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Error: " + ex.Message);
            }
        }

        private void LoadExpenses()
        {
            try
            {
                if (File.Exists(dataFile))
                {
                    var loaded = JsonSerializer.Deserialize<BindingList<Expense>>(
                        File.ReadAllText(dataFile));

                    if (loaded != null)
                        expenses = loaded;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }
    }
}
