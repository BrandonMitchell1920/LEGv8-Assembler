/*
 * Name:    Brandon Mitchell
 * Description: Some of the code for the GUI.  I reused some of the code from
 *              the lexical analyzer GUI.  Contains most of the events when you
 *              click on a button or menu option.
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Assembler__LEGv8_
{
    public partial class GUI : Form
    {
        // Default directories and files
        public static readonly string
            DEF_TABLE_DIR = "../../tables/",
            DEF_SOURCE_DIR = "../../testFiles/",

            DEF_SCAN = DEF_TABLE_DIR + "LEGv8ScanTable.csv",
            DEF_TOKEN = DEF_TABLE_DIR + "LEGv8TokenTable.csv",
            DEF_KEY = DEF_TABLE_DIR + "LEGv8KeywordTable.csv",
            DEF_OPCODES = DEF_TABLE_DIR + "LEGv8Opcodes.csv",
            DEF_REGISTERS = DEF_TABLE_DIR + "LEGv8Registers.csv",

            DEF_SOURCE = DEF_SOURCE_DIR + "DefaultTest.asm";

        private readonly Assembler assembler;

        /// <summary>
        /// Shows the user the README file to let them know how to use the 
        /// program
        /// </summary>
        private void about()
        {
            MessageBox.Show("View README.txt in /help for info on how to use.", 
                "README.txt", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        /// <summary>
        /// Shows the user my copyright so they don't get any sneaky ideas
        /// </summary>
        private void copyright()
        {
            MessageBox.Show("Copyright (C) Brandon Mitchell, 2022, All Rights Reserved\n" +
                "CSCI 361, Computer Architecture, S22\n\nDon't steal!!!",
                "Copyright Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Lets the user know that the file could not be opened
        /// </summary>
        /// <param name="fileName">string, the name of the file</param>
        /// <param name="message">string, what the error was</param>
        private void errorUserFileError(string fileName, string message)
        {
            // Probably not good from a security standpoint to show the user
            // detailed error messages, but this isn't a critical application.
            MessageBox.Show($"\"{fileName}\" could not be opened.\n\n" +
                $"{message}", "Error Opening File!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Creates a filedialog and allows the user to select a file
        /// </summary>
        private void openSourceFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = DEF_SOURCE_DIR,
                RestoreDirectory = true,
                Filter = "All Files|*.*",
                Title = "Choose a Source File"
            };

            // Only read file if the user selected one
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;

                try
                {
                    assembler.readSourceFile(fileName);
                    sourceFileTextBox.Text = Path.GetFileName(fileName);
                    outputTextBox.Clear();
                }
                catch (Exception err)
                {
                    errorUserFileError(fileName, err.Message);
                }
            }
        }

        /// <summary>
        /// Creates a save file dialog so the user can save the ouput to a 
        /// file of their choosing
        /// </summary>
        private void saveOutputText()
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Filter = "All Files|*.*",
                CheckFileExists = false,
                CheckPathExists = false,
                Title = "Save Output"
            };

            // If they didn't hit cancel or the X
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveDialog.FileName, outputTextBox.Text);
                }
                catch (Exception err)
                {
                    // Tad hard to test this, I will just assume it works fine
                    MessageBox.Show($"\"{saveDialog.FileName}\" could not " +
                        $"be saved.\n\n{err.Message}", "Error Saving " +
                        "File!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Calls the assembler's translate function
        /// </summary>
        private void translateManager()
        {
            // The file hasn't been translated yet
            if (assembler.translation.Count == 0)
            {
                assembler.translate();

                outputTextBox.Clear();
                assembler.translation.ForEach(line => outputTextBox.Text += line + "\n");
            }

            // If the user cleared the out put, go ahead and put it back
            else if (outputTextBox.Text == "")
            {
                assembler.translation.ForEach(line => outputTextBox.Text += line + "\n");
            }
            else
            {
                MessageBox.Show("The file is already translated.",
                    "Already Translated!", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Constructor for the GUI
        /// </summary>
        /// <param name="newAssembler">Assembler, an instance of the assembler with its necessary files set</param>
        public GUI(Assembler newAssembler)
        {
            assembler = newAssembler;

            InitializeComponent();

            sourceFileTextBox.Text = Path.GetFileName(DEF_SOURCE);

            translateButton.Select();
        }
    }
}