using Avalonia.Controls;
using System;

namespace PIM_Exceptions;
public class InvalidComboBoxException : Exception
{
    private ComboBox comboBox;
    public ComboBox ComboBox { get { return comboBox; } }

    public InvalidComboBoxException(ComboBox comboBox) : base()
    {
        this.comboBox = comboBox;
    }
    public InvalidComboBoxException(string message, ComboBox comboBox) : base(message)
    {
        this.comboBox = comboBox;
    }
    public InvalidComboBoxException(string message, Exception inner, ComboBox comboBox) : base(message, inner)
    {
        this.comboBox = comboBox;
    }
}