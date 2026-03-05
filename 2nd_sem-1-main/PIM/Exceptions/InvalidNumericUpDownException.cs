using Avalonia.Controls;
using System;

namespace PIM_Exceptions;
public class InvalidNumericUpDownException : Exception
{
    private NumericUpDown numericUpDown;
    public NumericUpDown NumericUpDown { get { return numericUpDown; } }

    public InvalidNumericUpDownException(NumericUpDown numericUpDown) : base()
    {
        this.numericUpDown = numericUpDown;
    }
    public InvalidNumericUpDownException(string message, NumericUpDown numericUpDown) : base(message)
    {
        this.numericUpDown = numericUpDown;
    }
    public InvalidNumericUpDownException(string message, Exception inner, NumericUpDown numericUpDown) : base(message, inner)
    {
        this.numericUpDown = numericUpDown;
    }
}