namespace PerfLoader.Models;

public class UserMessage
{
       private string _errorMessage;
       private string _infoMessage;
       private string _successMessage;
       private string _warningMessage;

       public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; } }

       public string InfoMessage { get { return _infoMessage; } set { _infoMessage = value; }}

       public string SuccessMessage { get { return _successMessage; } set { _successMessage = value; }}

       public string WarningMessage { get { return _warningMessage; } set { _warningMessage = value; }}
}

