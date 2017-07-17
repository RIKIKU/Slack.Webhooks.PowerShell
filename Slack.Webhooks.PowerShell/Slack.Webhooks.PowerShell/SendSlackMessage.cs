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
    [OutputType(typeof(bool))]
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
        [Parameter(Mandatory = false)]
        public string[] PostToChannels
        {
            get;
            set;
        }
        
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {

            SlackClient slackClient;
            if (TimeOut.Equals(0)) { slackClient = new SlackClient(URI); }
            else { slackClient = new SlackClient(URI, TimeOut); }
            WriteDebug(string.Format("Sending Message to {0}", URI));
            if (PostToChannels != null)
            {

                IEnumerable<string> EnumiratorChan = PostToChannels;
                var output = slackClient.PostToChannels(Message, PostToChannels);
                WriteObject(output);

            }
            else
            {
                var output = slackClient.Post(Message);
                WriteObject(output);
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
    

    
    
}
