/*
 * Name:    Brandon Mitchell
 * Description: I was right about seperating them out.  Made it real easy to
 *              translate into C# despite the different GUI stuff.  Everything 
 *              is mostly the same as before with a few minor tweaks due to C# 
 *              stuff or just small design changes.
 */

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace LexicalAnalyzer
{
    /// <summary>
    /// Lexical analyzer class, reads one token/lexemme at a time, requires
    /// several tables to run, returns whiteSpace, comments, can be used
    /// without GUI
    /// </summary>
    public class Lex
    {
        // Stored as a 2D array, use Lists for simplicity
        private List<List<string>> scanTable;

        // Stored as CSV, but on different rows, so just treat as 1D
        private List<string> tokenTable;

        // HashSet as we only need to know if an element is in it
        private HashSet<string> keywordTable;
        
        // Just treat as a single long string for indexing simplicity
        private string sourceFile;

        // The index into the sourceFile, private as messing with it could
        // mess up the Lex's functionality
        private int index = 0;

        // Values of the token and lexemme after reading, public as it 
        // doesn't matter if the client messes with it.
        public string curToken;
        public string curLexemme;

        // Set when error is found, cleared at the start of each loop
        public bool errorFlag;
        public string errorMessage;

        /// <summary>
        /// Looks into the scanning table to return the proper action
        /// </summary>
        /// <param name="curChar">char, the char just read in </param>
        /// <param name="curState">int, our current state (row) into the table</param>
        /// <returns>string, the value at the intersection of curChar and curState</returns>
        private string findAction(char curChar, int curState)
        {
            string charVal = ((int) curChar).ToString();

            if (scanTable[0].Contains(charVal))
            {
                // Character stored in table as int, represent as int to 
                // prevent funky things with quotes, commas, escapes
                int colVal = scanTable[0].IndexOf(charVal);
                return scanTable[curState][colVal];
            }

            return "-";
        }

        /// <summary>
        /// Creates the error message and sets and clears appropriate values
        /// </summary>
        /// <param name="tok">string, the error message from the table</param>
        /// <param name="image">string, the image at the time of error</param>
        /// <param name="curChar">char, the char responsible for the error</param>
        private void handleError(string tok, string image, char curChar)
        {
            // Get row by counting new lines up until the index, getting the 
            // col will require more work, row should be enough help
            int row = sourceFile.Substring(0, index).Count(ch => ch == '\n');

            errorFlag = true;

            // Add the row number and curChar to the table's error message
            // to improve messages
            errorMessage = tok + $", row {row + 1}: " + (image + curChar)
                .Trim();

            curToken = "";
            curLexemme = "";
        }

        /// <summary>
        /// Reads in the appropriate table
        /// </summary>
        /// <param name="fileName">string, the name of the file to be read</param>
        public void readScanTable(string fileName)
        {
            List<List<string>> newTable = new List<List<string>>();

            // This may throw an error, but I check that in the GUI now
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                newTable.Add(new List<string>(line.Split(',')));
            }

            scanTable = newTable;
            index = 0;
        }

        /// <summary>
        /// Reads in the appropriate table
        /// </summary>
        /// <param name="fileName">string, the name of the file to be read</param>
        public void readTokenTable(string fileName)
        {
            // This may throw an error, but I check that in the GUI now
            string[] lines = File.ReadAllLines(fileName);
            tokenTable = new List<string>(lines);
            index = 0;
        }

        /// <summary>
        /// Reads in the appropriate table
        /// </summary>
        /// <param name="fileName">string, the name of the file to be read</param>
        public void readKeywordTable(string fileName)
        {
            // This may throw an error, but I check that in the GUI now
            string[] lines = File.ReadAllLines(fileName);
            keywordTable = new HashSet<string>(lines);
            index = 0;
        }

        /// <summary>
        /// Reads in the source code file
        /// </summary>
        /// <param name="fileName">string, the name of the file to be read</param>
        public void readSourceFile(string fileName)
        {
            // This may throw an error, but I check that in the GUI now
            string text = File.ReadAllText(fileName);
            sourceFile = text.Trim();
            index = 0;
        }

        /// <summary>
        /// Sets thhe source file without needing to read it in
        /// </summary>
        /// <param name="str">The string to lex</param>
        public void setSourceFile(string str)
        {
            sourceFile = str;
            index = 0;
        }

        /// <summary>
        /// Checks if end-of-file has been reached
        /// </summary>
        /// <returns>bool, true or false if index has exceeded the file's length</returns>
        public bool eof() => index >= sourceFile.Length;

        /// <summary>
        /// Sets the index to 0 to restart from the beginning
        /// </summary>
        public void restartScanning() => index = 0;

        /// <summary>
        /// Reads through sourceFile and identifies one token/lexemme
        /// </summary>
        public void readNextToken()
        {
            // Don't bother doing anything below if this was hit
            if (eof()) { return; }

            errorFlag = false;
            errorMessage = "";

            string tok;
            char curChar;

            string image = "";

            // The default start state, 0 is invalid, impossible state
            int curState = 1;

            while (true)
            {
                // EOF could be a possible end for tokens
                if (eof())
                {
                    tok = tokenTable[curState];

                    // Recognize state, errors prepended with '-'
                    if (tok[0] != '-') { break; }

                    // Error state
                    else
                    {
                        handleError(tok, image, ' ');
                        return;
                    }
                }

                curChar = sourceFile[index++];
                string action = findAction(curChar, curState);

                // Move state
                if (action != "-") { curState = Int32.Parse(action); }

                else
                {
                    tok = tokenTable[curState];

                    // Recognize state, errors prepended with '-'
                    if (tok[0] != '-')
                    {
                        index--;
                        break;
                    }

                    // Error State
                    else
                    {
                        handleError(tok, image, curChar);
                        return;
                    }
                }

                image += curChar;
            }

            // Check to see if it is a keyword
            if (keywordTable.Contains(image)) { tok = image; }

            curToken = tok;
            curLexemme = image;
        }
    }
}
