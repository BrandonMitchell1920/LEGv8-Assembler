Brandon Mitchell

To Run:
    
    Double click the solution file and open in Visual Studio.  From there, you
    can click the start button at the top.
    
    You can also double click the executable in the /bin/debug folder without
    needing to open Visual Studio.
    
To Use:
    
    The assembler has several tables it uses.  These tables need to be present 
    to run, and the user should not modify them.  In addition, several test 
    files are provided for the user to use, and the default one is required as
    well.
    
    There are several choices in the GUI, mainly the two important ones, 
    opening a file and translating the file.  The menu bar has these choices 
    repeated, but also some additional options like saving the file, clearing
    the output, and bringing up my copyright.
    
    With a file loaded, hit the translate button to translate the file and 
    output it to the text box.  Empty lines and comments are removed.  In 
    addition, the address and machine language translation are on the side so 
    you can see what each line's translation is.  It there is a problem with 
    the line, it will not be translated and instead an error will be present.
    Most of the time, the box is wide enough for the output, but a funky file 
    or a long error message may cause text wrapping.  The symbol table is 
    outputted at the end with each symbol and the address it points to.
    
What is this?:

    This is an assembler for LEGv8 code.  Well, it doesn't do any linking or
    anything like that, so it mainly just translates a subset of instructions.
    It uses a lexical analyzer, and I manually parse the tokens, pick out the 
    important values, and translate them.
    
Changes:

	The only thing I changed was the Assembler itself, not the GUI or lexical
	analyzer.  I don't save whitespace tokens now, so parsing is easier.  
	However, I do need to save the whitespace lexemmes to reconstruct the line
	and get everything in the original layout.  I put the code for each format 
	in their own function originally, but they needed access to many local 
	variables, so they had seven or so parameters.  I also would just copy and
	paste code.  If I needed the next token to be a register, I would copy and 
	paste my register code.  So, there was a lot of repetition.
	
	I placed the token recongition code into several local functions in the 
	translate function.  This way, I can just do "int rt = register()" if the
	next char should be a register or "comma()" if I am expecting a comma.
	Since they are local, they don't need several arguments passed into them.
	However, since some of them return a value, I can't just return false if
	the token is something else.  I am also in several levels of nesting, so
	I don't want a bunch of repeated code checking return values and then 
	breaking only to need to break again.  As such, I have these functions 
	throw exceptions and I then catch them and add the message to the 
	translation.  I don't like it and it looks bad as I did a presentation 
	where I said you shouldn't do that, but I couldn't think of another way.
	However, it does make it very easy to add in more formats or even deal 
	the opcodes with special layouts should I want to and reduced the repeated
	code.  The translate function is already too large for its own good, and 
	since I had to rely on a method I don't like to deal with the repeated 
	code issue, it means that method likely needs some refactoring and changes
	to make it smaller and simpler.