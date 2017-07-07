using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Slack.Webhooks;
using System.Reflection;

namespace Slack.Webhooks.PowerShell
{
    [Cmdlet(VerbsCommunications.Send,"SlackMessage",SupportsShouldProcess = false)]
    public class SendSlackMessage : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0
            )]
        public string URI
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = true,
            Position = 1
            )]
        public SlackMessage Message
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 2
            )]
        public int TimeOut
        {
            get;
            set;
        }
        SlackClient slackClient;
        protected override void BeginProcessing()
        {
            if (TimeOut.Equals(0)) { slackClient = new SlackClient(URI); }
            else {slackClient = new SlackClient(URI, TimeOut); }
            
        }

        protected override void ProcessRecord()
        {
            WriteDebug(string.Format("Sending Message to {0}", URI));
            slackClient.Post(Message);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
    

    
    
}
