using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResetPassword : MonoBehaviour
{   
    protected Firebase.Auth.FirebaseAuth auth;
    private string logText = "";
    const int kMaxLogSize = 16382;
    private Vector2 scrollViewVector = Vector2.zero;

    public string email = "caioabe68@gmail.com";

    private void Start()
    {
      SendPasswordResetEmail();
    }

    public void SendPasswordResetEmail() 
    {
      auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread((authTask) => 
      {
        if (LogTaskCompletion(authTask, "Send Password Reset Email")) 
        {
          DebugLog("Password reset email sent to " + email);
        }
      });
    }

    bool LogTaskCompletion(Task task, string operation) {
      bool complete = false;
      if (task.IsCanceled) {
        DebugLog(operation + " canceled.");
      } else if (task.IsFaulted) {
        DebugLog(operation + " encounted an error.");
        foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
          string authErrorCode = "";
          Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
          if (firebaseEx != null) {
            authErrorCode = String.Format("AuthError.{0}: ",
              ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
          }
          DebugLog(authErrorCode + exception.ToString());
        }
      } else if (task.IsCompleted) {
        DebugLog(operation + " completed");
        complete = true;
      }
      return complete;
    }

    public void DebugLog(string s) {
      Debug.Log(s);
      logText += s + "\n";

      while (logText.Length > kMaxLogSize) {
        int index = logText.IndexOf("\n");
        logText = logText.Substring(index + 1);
      }
      scrollViewVector.y = int.MaxValue;
    }
}
