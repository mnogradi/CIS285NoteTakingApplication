using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Note_Taking_Application
{
    public partial class Form1 : Form
    {
        DataTable table;
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "notes.txt");

        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Messages", typeof(String));

            dataGridView1.DataSource = table;

            dataGridView1.Columns["Messages"].Visible = false;
            dataGridView1.Columns["Title"].Width = 186;

            foreach (DataGridViewColumn column in dataGridView1.Columns) 
            { 
                column.SortMode = DataGridViewColumnSortMode.NotSortable; 
            }

            LoadNotesFromFile();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();
            txtMessage.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            table.Rows.Add(txtTitle.Text, txtMessage.Text);
            SaveNotesToFile();
            txtTitle.Clear();
            txtMessage.Clear();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            if (index > -1)
            {
                txtTitle.Text = table.Rows[index].ItemArray[0].ToString();
                txtMessage.Text = table.Rows[index].ItemArray[1].ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            table.Rows[index].Delete();
            SaveNotesToFile();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            table.Clear();
            SaveNotesToFile();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            if (index > -1)
            {
                txtTitle.Text = table.Rows[index].ItemArray[0].ToString();
                txtMessage.Text = table.Rows[index].ItemArray[1].ToString();
            }
            table.Rows[index].Delete();
            SaveNotesToFile();
        }

        private void SaveNotesToFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (DataRow row in table.Rows)
                {
                    writer.WriteLine($"{row["Title"]},{row["Messages"]}");
                }
            }
        }

        private void LoadNotesFromFile()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 2)
                        {
                            table.Rows.Add(parts[0], parts[1]);
                        }
                    }
                }
            }
        }
    }
}
