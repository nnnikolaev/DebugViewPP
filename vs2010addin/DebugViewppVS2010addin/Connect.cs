using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
namespace DebugViewppVS2010addin
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2
	{
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
		}

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            Guid paneGuid = VSConstants.GUID_OutWindowDebugPane;
            IVsOutputWindowPane debugPane;
            outWindow.GetPane(ref paneGuid, out debugPane);

            debugPane.OutputString("DebugView++ Forwarding initializing...");
            debugPane.Activate(); // Brings this pane into view

            IVsUserData userData = (IVsUserData)debugPane;
            object o;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out o);

            IWpfTextViewHost viewHost = (IWpfTextViewHost)o;
            m_textView = viewHost.TextView;
            m_textView.TextBuffer.Changed += new EventHandler<Microsoft.VisualStudio.Text.TextContentChangedEventArgs>(TextBuffer_Changed);

            debugPane.OutputString("DebugView++ Forwarding installed.");
            Trace.AutoFlush = true;
		}

        void TextBuffer_Changed(object sender, Microsoft.VisualStudio.Text.TextContentChangedEventArgs e)
        {
            foreach (var change in e.Changes)
            {
                string text = change.NewText.ToString();
                while (text.Length > 4090)
                {
                    Trace.Write("Following lines exceeded 4090 chars!\n");
                    var part = text.Substring(0, 4090);
                    int nlIndex = part.LastIndexOf("\n", 0);
                    if (nlIndex == -1)
                    {
                        Trace.Write(part);
                        text = text.Substring(4091);
                    }
                    else
                    {
                        Trace.Write(part.Substring(0, nlIndex +1));
                        text = text.Substring(nlIndex+1);
                    }
                }
                Trace.Write(text);
            }
        }

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}

		private DTE2 _applicationObject;
		private AddIn _addInInstance;
        IWpfTextView m_textView;
    }
}