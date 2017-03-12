namespace BlackJackWPF
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows.Input;

  /// <summary>
  /// class for commandhelper
  /// </summary>
  public class CommandHelper : ICommand
  {
    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// The delegate that will be invoked that determines whether the command is available
    /// </summary>
    public Func<object, bool> CanExecuteDelegate { get; set; }

    /// <summary>
    /// The delegate that is invoked when the command is executed
    /// </summary>
    public Action<object> ExecuteDelegate { get; set; }

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>
    /// true if this command can be executed; otherwise, false.
    /// </returns>
    public bool CanExecute(object parameter)
    {
      if (this.CanExecuteDelegate != null)
      {
        return this.CanExecuteDelegate.Invoke(parameter);
      }

      return true;
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    public void Execute(object parameter)
    {
      if (this.ExecuteDelegate != null)
      {
        this.ExecuteDelegate.Invoke(parameter);
      }
    }
  }
}


