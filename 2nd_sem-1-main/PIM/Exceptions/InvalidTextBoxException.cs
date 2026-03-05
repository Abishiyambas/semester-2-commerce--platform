using Avalonia.Controls;
using System;

namespace PIM_Exceptions;
public class InvalidTextBoxException : Exception
{
    private TextBox textbox;
    public TextBox Textbox { get { return textbox; } }

    public InvalidTextBoxException(TextBox textbox) : base()
    {
        this.textbox = textbox;
    }
    public InvalidTextBoxException(string message, TextBox textbox) : base(message)
    {
        this.textbox = textbox;
    }
    public InvalidTextBoxException(string message, Exception inner, TextBox textbox) : base(message, inner)
    {
        this.textbox = textbox;
    }
}