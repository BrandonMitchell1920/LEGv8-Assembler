/*
 * Name:    Brandon Mitchell
 * Description: The assembler.  Can be used on its own without the GUI.  Supports
 *              all formats, but not all instructions (instructions with a special
 *              layout that differs from their format).  Whitespace is ignored and
 *              that makes parsing a lot easier.  
 *              
 *              External files with some info are still used.
 *              
 *              I did something I don't really like, and it looks bad that I did it 
 *              after giving a presentation saying not to do it last semester, but
 *              I am using exceptions for control flow.  I had an absurd amount of 
 *              repeated code.  For example, I would copy and paste the register 
 *              recoginition code whenever I needed a register, so about ten copies
 *              of the same code existed.  I put the recoginition code of several 
 *              tokens into several local functions in the translate routine.  Being
 *              in the translate function means they have access to everything they
 *              need.  However, to break out of the several levels of nesting, I used  
 *              an exception and passed the error message with it.  I don't like it,
 *              but it makes supporting more formats or special instructions really
 *              easy and doesn't require a bunch of repeated code.
 */

using System;
using System.IO;
using System.Collections.Generic;

// My Lexical Analyzer from last semester with a couple minor changes.
using LexicalAnalyzer;

namespace Assembler__LEGv8_
{
    public class Assembler
    {
        // Lexical analyzer instance used to make parsing easier
        private Lex lex;

        // The file we will be assembling
        private List<string> sourceFile;

        // A map of valid registers and their decimal values
        private Dictionary<string, int> registerMap;

        // Struct used to represent valid opcodes and their various values
        private struct opcode
        {
            public string name, format, binaryString, shamt;

            public opcode(string name, string format, string binaryString, string shamt)
            {
                this.name = name;
                this.format = format;
                this.binaryString = binaryString;
                this.shamt = shamt;
            }
        }

        // Store opcodes in dictionary for easy access
        private Dictionary<string, opcode> opcodeMap;

        // The translated file, stored as a list of strings
        public List<string> translation;

        /// <summary>
        /// Constructor for the class, only sets the lexical analyzer
        /// </summary>
        /// <param name="lexicalAnalyzer">Lex, and instance of the lexical analyzer</param>
        public Assembler(Lex lexicalAnalyzer)
        {
            lex = lexicalAnalyzer;

            sourceFile = new List<string>();
            translation = new List<string>();
            registerMap = new Dictionary<string, int>();
            opcodeMap = new Dictionary<string, opcode>();
        }

        /// <summary>
        /// Reads in the sourcefile from the file name given
        /// </summary>
        /// <param name="fileName">string, the name of the file to be read</param>
        public void readSourceFile(string fileName)
        {
            List<string> tempList = new List<string>();
            tempList = new List<string>(File.ReadAllLines(fileName));
            sourceFile = tempList;

            // Clear translation as it may no longer be valid
            translation.Clear();
        }

        /// <summary>
        /// Allows a user to set the source file with out needing to have the 
        /// assembler read it for them
        /// </summary>
        /// <param name="newSource">List<string>, a list of strings to assemble</param>
        public void setSourceFile(List<string> newSource)
        {
            sourceFile = newSource;
            translation.Clear();
        }

        /// <summary>
        /// Opens and reads the opcodes file for legal opcodes
        /// </summary>
        /// <remarks>File must be in CSV format</remarks>
        /// <param name="fileName">string, the file to be read in</param>
        public void readOpcodesFile(string fileName)
        {
            Dictionary<string, opcode> tempMap = new Dictionary<string, opcode>(); 

            // Opening in ReadLines allow iteration, is more efficient for large files
            foreach (string line in File.ReadLines(fileName))
            {
                string[] temp = line.Split(',');

                tempMap.Add(temp[0], new opcode(temp[0], temp[1], temp[2], temp[3]));
            }

            // Do the assignment after the reading, ensure that if reading
            // fails, the map is still valid.
            opcodeMap = tempMap;
        }

        /// <summary>
        /// Opens and reads the registers file for legal registers
        /// </summary>
        /// <remarks>File must be in CSV format</remarks>
        /// <param name="fileName">string, the file to be read in</param>
        public void readRegistersFile(string fileName)
        {
            Dictionary<string, int> tempMap = new Dictionary<string, int>();

            foreach (string line in File.ReadLines(fileName))
            {
                string[] temp = line.Split(',');

                tempMap.Add(temp[0], int.Parse(temp[1]));
            }

            // Do the assignment after the reading, ensure that if reading
            // failures, the map is still valid.
            registerMap = tempMap;
        }

        /// <summary>
        /// Translates the sourceFile into binary and puts it in a formatted
        /// form into the translation variable
        /// </summary>
        public void translate()
        {
            translation.Clear();
            Dictionary<string, int> symbolMap = new Dictionary<string, int>();

            int programCounter = 0x00400000;

            bool dataFlag = false, textFlag = false, doubleDone = false;

            // Easiest to go one line at a time
            foreach (string line in sourceFile)
            {
                List<string> tokens = new List<string>(),
                             lexemmes = new List<string>(),
                             lineReconstruct = new List<string>();

                string lineCopy = "";

                // The newline is important in parsing below, but the ReadAllLines removes it
                lex.setSourceFile(line + '\n'); 

                // Tokenzie the whole line, check for lex errors and break if needed
                while (!lex.eof())
                {
                    lex.readNextToken();

                    if (lex.errorFlag)
                    {
                        break;
                    }
                    else if (lex.curToken != "comment")
                    {
                        if (lex.curToken != "whitespace")
                        {
                            // Don't want the spaces to make parsing easier ...
                            tokens.Add(lex.curToken);
                            lexemmes.Add(lex.curLexemme);
                        }

                        // ... but need them to reconstruct the line
                        lineReconstruct.Add(lex.curLexemme);
                    }
                }

                if (lex.errorFlag)
                {
                    // If there is a lex error, use the whole line, could include comments
                    lineCopy = line.PadRight(60, ' ');
                    translation.Add(string.Format("{0}{1:X8}   {2}", lineCopy, programCounter, lex.errorMessage));
                    programCounter += 4;
                    continue;
                }

                // Construct a copy of the line from the lexemmes returned, lets us remove comments
                foreach (string lexemme in lineReconstruct)
                {
                    if (lexemme != "\n" && lexemme != "\r\n")
                    {
                        lineCopy += lexemme;
                    }
                }
                lineCopy = lineCopy.PadRight(60, ' ');

                // Used to walk through the tokens
                int index = 0;

                if (tokens[index] == "newline")
                {
                    // Don't bother including empty lines
                    continue;
                }
                else if (tokens[index] == "label")
                {
                    symbolMap.Add(lexemmes[index].Substring(0, lexemmes[index].Length - 1), programCounter);
                    index++;
                }

                if (tokens[index] == "newline")
                {
                    translation.Add(lineCopy);
                    continue;
                }
                else if (tokens[index] == "directive")
                {
                    // Could easily add support for other directives if wanted
                    if (lexemmes[index] == ".data")
                    {
                        if (dataFlag)
                        {
                            translation.Add(string.Format("{0}{1:X8}   -.data directive already present", lineCopy, programCounter));
                            programCounter += 4;

                            // Don't allow any more .double directives
                            doubleDone = true;
                        }
                        else
                        {
                            programCounter = 0x10000000;
                            translation.Add(lineCopy);
                            dataFlag = true;
                        }
                    }
                    else if (lexemmes[index] == ".text")
                    {
                        if (textFlag)
                        {
                            translation.Add(string.Format("{0}{1:X8}   -.text directive already present", lineCopy, programCounter));
                            programCounter += 4;
                        }
                        else
                        {
                            programCounter = 0x00400000;
                            translation.Add(lineCopy);
                            textFlag = true;
                        }
                    }
                    else if (lexemmes[index] == ".doubleword")
                    {
                        if (doubleDone)
                        {
                            translation.Add(string.Format("{0}{1:X8}   -.doubleword not allowed here", lineCopy, programCounter));
                            programCounter += 4;
                            continue;
                        }

                        index++;

                        if (tokens[index] == "newline")
                        {
                            // No doublewords after directive
                            translation.Add(string.Format("{0}{1:X8}   -Decimal or hexadecimal must follow directive", lineCopy, programCounter));
                            programCounter += 4;
                            continue;
                        }
                        else
                        {
                            List<string> nums = new List<string>();

                            // I made them seperate in the table so I could know how to convert them if needed
                            if (tokens[index] == "decimal" || tokens[index] == "hexadecimal")
                            {
                                nums.Add(lexemmes[index]);
                            }
                            else
                            {
                                translation.Add(string.Format("{0}{1:X8}   -Expected decimal or hexadecimal but got {2}", lineCopy, programCounter, tokens[index]));
                                programCounter += 4;
                                continue;
                            }

                            index++;

                            // Set to true when an error occurs so we don't continue with translating doublewords
                            bool doubleError = false;

                            while (tokens[index] != "newline")
                            {
                                if (tokens[index] != "comma")
                                {
                                    translation.Add(string.Format("{0}{1:X8}   -Expected comma but got {2}", lineCopy, programCounter, tokens[index]));
                                    programCounter += 4;
                                    doubleError = true;
                                    break;
                                }

                                if (tokens[index++] == "newline") {break;}

                                if (tokens[index] == "decimal" || tokens[index] == "hexadecimal")
                                {
                                    nums.Add(lexemmes[index++]);
                                }
                                else
                                {
                                    translation.Add(string.Format("{0}{1:X8}   -Expected decimal or hexadecimal but got {2}", lineCopy, programCounter, tokens[index]));
                                    programCounter += 4;
                                    doubleError = true;
                                    break;
                                }
                            }

                            if (doubleError) {continue;}

                            // Add each num into the translation
                            foreach (string num in nums)
                            {
                                string converted = "";

                                // Check if hexadecimal
                                if (num.ToLower().Contains("x"))
                                {
                                    // Always unsigned, should check size so make sure it fits in a Int64
                                    converted = string.Format("{0:X16}", Convert.ToUInt64(num, 16));
                                }
                                else
                                {
                                    // Could be signed, should check size
                                    converted = string.Format("{0:X16}", Convert.ToInt64(num, 10));
                                }

                                translation.Add(string.Format("{0}{1:X8}   {2}", lineCopy, programCounter, converted.Substring(0, 8)));
                                programCounter += 4;

                                // First line will be on the same line as the .doubleword directive
                                // following lines will be blank
                                lineCopy = new string(' ', 60);

                                translation.Add(string.Format("{0}{1:X8}   {2}", lineCopy, programCounter, converted.Substring(8)));
                                programCounter += 4;
                            }
                        }
                    }
                    else
                    {
                        // Illegal directive
                        translation.Add(string.Format("{0}{1:X8}   -Directive is not supported", lineCopy, programCounter));
                        continue;
                    }
                }
                else if (tokens[index] == "identifier")
                {
                    // Check our opcodeMap to see if it is valid
                    if (opcodeMap.ContainsKey(lexemmes[index]))
                    {
                        // Probably have a switch on the format later
                        opcode currentOp = opcodeMap[lexemmes[index]];

                        // Identifies a register and returns its value, throws exception if other token encountered
                        int register()
                        {
                            if (tokens[index] != "identifier")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected register but got {2}", lineCopy, programCounter, tokens[index++]));
                            }
                            else if (registerMap.ContainsKey(lexemmes[index]))
                            {
                                return registerMap[lexemmes[index++]];
                            }
                            else
                            {
                                index++;
                                throw new Exception(string.Format("{0}{1:X8}   -Invalid register", lineCopy, programCounter));
                            }
                        }

                        // Identifies a comma, throws exception if something else is encountered
                        void comma()
                        {                        
                            if (tokens[index++] != "comma")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Comma missing between tokens", lineCopy, programCounter));
                            }
                        }

                        // Identifies a newline, throws exception if something else is encountered
                        void newline()
                        {
                            if (tokens[index] != "newline")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Extra tokens after final token", lineCopy, programCounter));
                            }    
                        }

                        // Identifies an intermediate and returns it, throws exception if something else is encountered
                        int immediate()
                        {
                            if (tokens[index] != "immediate")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected immediate but got {2}", lineCopy, programCounter, tokens[index++]));
                            }
                            else
                            {
                                // Should ideally check sign before conversion, could throw an exception if immediate too large or too small
                                return Int32.Parse(lexemmes[index].Substring(1, lexemmes[index++].Length - 1));
                            }
                        }
                                
                        // Identifies an identifier and returns it, throws exception if something else is encountered
                        string identifier()
                        {
                            if (tokens[index] != "identifier")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected identifier but got {2}", lineCopy, programCounter, tokens[index++]));
                            }
                            
                            // This value is ignored below in the CB and B formats as we aren't doing actual linking
                            return lexemmes[index++];
                        }

                        // Identifies a lbracket, throws exception if something else is encountered
                        void lbracket()
                        {
                            if (tokens[index++] != "lbracket")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected lbacket to follow", lineCopy, programCounter));
                            }
                        }

                        // Identifies a rbracket, throws exception if something else is encountered
                        void rbracket()
                        {
                            if (tokens[index++] != "rbracket")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected rbacket to follow", lineCopy, programCounter));
                            }
                        }

                        // Identifies a colon, throws exception if something else is encountered
                        void colon()
                        {
                            if (tokens[index++] != "colon")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected colon to follow", lineCopy, programCounter));
                            }
                        }

                        // Identifies either an immediate or a hexadecimal and returns it, throws exception if something else is encountered
                        int immediateOrHexadecimal()
                        {
                            if (tokens[index] == "immediate")
                            {
                                return Int32.Parse(lexemmes[index].Substring(1, lexemmes[index++].Length - 1));
                            }
                            else if (tokens[index] == "hexadecimal")
                            {
                                return Convert.ToInt32(lexemmes[index++], 16);
                            }
                            else
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected immediate or hexadecimal but got {2}", lineCopy, programCounter, tokens[index++]));
                            }
                        }
                        
                        // Identifies a decimal, throws exception if something else is encountered
                        int decimalToken()
                        {
                            // Decimal is a keyword, so I have to call the function decimalToken instead
                            if (tokens[index] != "decimal")
                            {
                                throw new Exception(string.Format("{0}{1:X8}   -Expected decimal but got {2}", lineCopy, programCounter, tokens[index++]));
                            }

                            return Convert.ToInt32(lexemmes[index++], 10);
                        }

                        try
                        {
                            switch (currentOp.format)
                            {
                                case "R":
                                { 
                                    index++;

                                    int rd = register();
                                    comma();
                                    int rn = register();
                                    comma();        
                                    int rm = register(); 
                                    newline(); 

                                    // Convert all registers to binary strings of a fixed length
                                    string rdVal = Convert.ToString(rd, 2).PadLeft(5, '0');
                                    string rnVal = Convert.ToString(rn, 2).PadLeft(5, '0');
                                    string rmVal = Convert.ToString(rm, 2).PadLeft(5, '0');

                                    string binInstruction = currentOp.binaryString;
                                    binInstruction += rmVal;

                                    if (currentOp.shamt == "")
                                    {
                                        // If there is no shamt, then it is just zeros
                                        binInstruction += new string('0', 6);
                                    }
                                    else
                                    {
                                        binInstruction += currentOp.shamt;
                                    }

                                    binInstruction += rnVal;
                                    binInstruction += rdVal;

                                    // Add to the translation and convert to hex
                                    translation.Add(string.Format("{0}{1:X8}   {2:X8}", lineCopy, programCounter, Convert.ToUInt32(binInstruction, 2)));

                                    break;
                                }
                                case "I":
                                { 
                                    index++;

                                    int rd = register();
                                    comma();
                                    int rn = register();
                                    comma();
                                    int imm = immediate();
                                    newline();
                                
                                    if (imm < -2048 || imm > 2047)
                                    {
                                        // Immediate value too large
                                        translation.Add(string.Format("{0}{1:X8}   -Immediate does not fit in a 12 bit integer", lineCopy, programCounter));
                                        break;
                                    }

                                    string rdVal = Convert.ToString(rd, 2).PadLeft(5, '0');
                                    string rnVal = Convert.ToString(rn, 2).PadLeft(5, '0');
                                    string binInstruction = currentOp.binaryString;
                                
                                    // Pad out to twelve (if it isn't already longer) and then truncate, for
                                    // negative numbers as they have leading ones
                                    string binImm = Convert.ToString(imm, 2).PadLeft(12, '0');
                                    binInstruction += binImm.Substring(binImm.Length - 12);

                                    binInstruction += rnVal;
                                    binInstruction += rdVal;

                                    // Add to the translation and convert to hex
                                    translation.Add(string.Format("{0}{1:X8}   {2:X8}", lineCopy, programCounter, Convert.ToUInt32(binInstruction, 2)));

                                    break;
                                }                       
                                case "D":
                                {
                                    index++;

                                    int rt = register();
                                    comma();
                                    lbracket();
                                    int rn = register();
                                    comma();
                                    int imm = immediate();
                                    rbracket();
                                    newline();

                                    if (imm < -256 || imm > 255)
                                    {
                                        // Immediate value too large
                                        translation.Add(string.Format("{0}{1:X8}   -Immediate does not fit in a 9 bit integer", lineCopy, programCounter));
                                        break;
                                    }

                                    string rtVal = Convert.ToString(rt, 2).PadLeft(5, '0');
                                    string rnVal = Convert.ToString(rn, 2).PadLeft(5, '0');
                                    string binInstruction = currentOp.binaryString;

                                    string binImmediate = Convert.ToString(imm, 2).PadLeft(9, '0');
                                    binInstruction += binImmediate.Substring(binImmediate.Length - 9);

                                    // op is just zero in this case
                                    binInstruction += new string('0', 2);

                                    binInstruction += rnVal;
                                    binInstruction += rtVal;

                                    translation.Add(string.Format("{0}{1:X8}   {2:X8}", lineCopy, programCounter, Convert.ToUInt32(binInstruction, 2))); ;

                                    break;
                                }
                                case "IW":
                                {
                                    index++;

                                    int rd = register();
                                    comma();
                                    int imm = immediateOrHexadecimal();
                                    comma();
                                    string ident = identifier();
                                    string decVal = decimalToken().ToString();
                                    colon();
                                    newline();

                                    if (imm < -32768 || imm > 32767)
                                    {
                                        // Immediate value too large
                                        translation.Add(string.Format("{0}{1:X8}   -Second operand does not fit in a 16 bit integer", lineCopy, programCounter));
                                        break;
                                    }

                                    if (ident != "LSL")
                                    {
                                        translation.Add(string.Format("{0}{1:X8}   -Identifier must be \"LSL\"", lineCopy, programCounter));
                                        break;
                                    }

                                    Dictionary<string, string> validVals = new Dictionary<string, string>(){{"0", "00"}, {"16", "01"}, {"32", "10"}, {"48", "11"}};
                                    if (!validVals.ContainsKey(decVal))
                                    {
                                        translation.Add(string.Format("{0}{1:X8}   -Decimal must be 0, 16, 32, or 48", lineCopy, programCounter));
                                        break;
                                    }

                                    string lslVal = validVals[decVal];

                                    string rdVal = Convert.ToString(rd, 2).PadLeft(5, '0');
                                    string binInstruction = currentOp.binaryString;

                                    binInstruction += lslVal;

                                    string binImmediate = Convert.ToString(imm, 2).PadLeft(16, '0');
                                    binInstruction += binImmediate.Substring(binImmediate.Length - 16);
            
                                    binInstruction += rdVal;

                                    translation.Add(string.Format("{0}{1:X8}   {2:X8}", lineCopy, programCounter, Convert.ToUInt32(binInstruction, 2)));

                                    break;
                                }
                                case "CB":
                                {
                                    int rt = 0;

                                    index++;
                                
                                    rt = register();
                                    comma();
                                    identifier();
                                    newline();                                
                                
                                    string binInstruction = currentOp.binaryString.PadRight(27, '0');
                                    binInstruction += Convert.ToString(rt, 2).PadLeft(5, '0');
                                    translation.Add(string.Format("{0}{1:X8}   {2:X8}", lineCopy, programCounter, Convert.ToUInt32(binInstruction, 2)));

                                    break;
                                }
                                case "B":
                                {
                                    index++;

                                    identifier();
                                    newline();

                                    string binInstruction = currentOp.binaryString.PadRight(32, '0');
                                    translation.Add(string.Format("{0}{1:X8}   {2:X8}", lineCopy, programCounter, Convert.ToUInt32(binInstruction, 2)));

                                    break;
                                }
                            }
                        }

                        // Ideally, I should have made a custom Assembler Exception to throw instead of a super generic one
                        catch (Exception err)
                        {
                            translation.Add(err.Message);
                        }
                    }
                    else
                    {
                        // Couldn't find opcode in the map
                        translation.Add(string.Format("{0}{1:X8}   -Invalid opcode", lineCopy, programCounter));
                    }
                    programCounter += 4;
                }
                else
                {
                    // Line does not start with a label, opcode, or directive
                    translation.Add(string.Format("{0}{1:X8}   -Unexpected leading token", lineCopy, programCounter));
                    programCounter += 4;
                }
            }

            // Finally check that the necessary directives were present
            if (!dataFlag)
            {
                translation.Add(new string(' ', 71) + "-Missing requrired .data directive");
            }
            if (!textFlag)
            {
                translation.Add(new string(' ', 71) + "-Missing requrired .text directive");
            }

            // Stuff to make the output look nice
            translation.Insert(0, "LEGv8 Code" + new string(' ', 50) + "Address    Machine Language\n");
            translation.Add("\nSybmol Table:\nLabel      Address");

            // Add the symbols in
            foreach (KeyValuePair<string, int> symbol in symbolMap)
            {
                string hexVal = string.Format("{0:X8}", symbol.Value);
                translation.Add($"{symbol.Key, -10} {hexVal}");
            }
        }
    }
}