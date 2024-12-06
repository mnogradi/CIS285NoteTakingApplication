using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Note_Taking_Application
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// The DataTable that stores notes with "Title", "Messages", and "Tag" columns.
        /// </summary>
        private DataTable table;

        /// <summary>
        /// The file path for saving and loading notes, stored in the user's Documents folder.
        /// </summary>
        private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "notes.txt");

        /// <summary>
        /// Initializes the form and its components.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets up the DataTable, binds it to the DataGridView, and loads notes from the file when the form loads.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the form load event.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize the DataTable
            table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Messages", typeof(String));
            table.Columns.Add("Tag", typeof(String));

            // Bind the DataTable to the DataGridView
            dataGridView1.DataSource = table;

            // Hide the "Messages" and "Tag" columns and set the "Title" column width
            dataGridView1.Columns["Tag"].Visible = false;
            dataGridView1.Columns["Messages"].Visible = false;
            dataGridView1.Columns["Title"].Width = 186;

            // Disable sorting for all DataGridView columns
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Load existing notes from the file
            LoadNotesFromFile();
        }

        /// <summary>
        /// Clears the Title, Message, and Tag input fields for creating a new note.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click event.</param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();
            txtMessage.Clear();
            txtSetTag.Clear();
        }

        /// <summary>
        /// Saves the current Title, Message, and Tag into the DataTable and updates the notes file.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click event.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Add the note to the DataTable
            table.Rows.Add(txtTitle.Text, txtMessage.Text, txtSetTag.Text);

            // Save notes to the file
            SaveNotesToFile();

            // Clear the input fields after saving
            txtTitle.Clear();
            txtMessage.Clear();
            txtSetTag.Clear();
        }

        /// <summary>
        /// Displays the selected note's Title, Message, and Tag in the input fields.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click event.</param>
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            // Check if the selected row index is valid
            if (index > -1)
            {
                txtTitle.Text = table.Rows[index]["Title"].ToString();
                txtMessage.Text = table.Rows[index]["Messages"].ToString();
                txtSetTag.Text = table.Rows[index]["Tag"].ToString();
            }
        }

        /// <summary>
        /// Deletes the currently selected note from the DataTable and updates the notes file.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click event.</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            // Delete the selected row
            if (index > -1)
            {
                table.Rows[index].Delete();
                SaveNotesToFile();
            }
        }

        /// <summary>
        /// Clears all notes from the DataTable and updates the notes file.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click event.</param>
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            table.Clear();
            SaveNotesToFile();
        }

        /// <summary>
        /// Loads the selected note for editing and deletes the original row from the DataTable.
        /// Updates the notes file after deletion.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click event.</param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            // Check if the selected row index is valid
            if (index > -1)
            {
                txtTitle.Text = table.Rows[index]["Title"].ToString();
                txtMessage.Text = table.Rows[index]["Messages"].ToString();
                txtSetTag.Text = table.Rows[index]["Tag"].ToString();
                table.Rows[index].Delete();
                SaveNotesToFile();
            }
        }

        /// <summary>
        /// Saves all notes from the DataTable to a text file.
        /// Each note is stored as a '|'-separated line.
        /// </summary>
        private void SaveNotesToFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (DataRow row in table.Rows)
                {
                    writer.WriteLine($"{row["Title"]}|{row["Messages"]}|{row["Tag"]}");
                }
            }
        }

        /// <summary>
        /// Loads notes from the text file into the DataTable.
        /// Expects each line to be in the format "Title|Message|Tag".
        /// </summary>
        private void LoadNotesFromFile()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 3)
                        {
                            table.Rows.Add(parts[0], parts[1], parts[2]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Searches the list of notes for a tag matching the input in the textbox.
        /// Expects each line to be in the format "Title|Message|Tag".
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments for the button click event.</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataView search = table.DefaultView;
            search.RowFilter = "tag LIKE '%" + txtGetTag.Text + "%'";
        }
    }
}
