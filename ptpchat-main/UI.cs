namespace PtpChat.Main
{
    using System;
    using System.Windows.Forms;

    //we do all our UI invocation via this class
    public static class UI
    {
        //this is our base form instance
        private static Control internalControl;

        //pass in the form when we start up
        public static void Initialize(Control control)
        {
            internalControl = control;
        }

        //invoke actions on the form 
        public static void Invoke(Action action)
        {
            //if we're not in the UI thread
            if (internalControl.InvokeRequired)
            {
                //invoke the action on the UI thread
                internalControl.Invoke(action);
            }
            else
            {
                //else, on the UI thread, so just call the action
                action();
            }
        }
    }
}