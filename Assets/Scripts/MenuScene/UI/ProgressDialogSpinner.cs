#if UNITY_ANDROID && !UNITY_EDITOR
using DeadMosquito.AndroidGoodies;
#endif

namespace ua.org.gdg.devfest
{
  public class ProgressDialogSpinner
  {
    //---------------------------------------------------------------------
    // Internal
    //---------------------------------------------------------------------
      
#if UNITY_ANDROID && !UNITY_EDITOR
    private AGProgressDialog _progressSpinner;
#endif

    //---------------------------------------------------------------------
    // Public
    //---------------------------------------------------------------------
      
    public ProgressDialogSpinner()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
      _progressSpinner = AGProgressDialog.CreateSpinnerDialog("", "", AGDialogTheme.Default);
#endif
    }

    public ProgressDialogSpinner(string title, string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
      _progressSpinner = AGProgressDialog.CreateSpinnerDialog(title, message, AGDialogTheme.Dark);
#endif
    }

    public void Show()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
          _progressSpinner.Show();
#endif
    }

    public void Dismiss()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
          _progressSpinner.Dismiss();
#endif
    }
  }
}