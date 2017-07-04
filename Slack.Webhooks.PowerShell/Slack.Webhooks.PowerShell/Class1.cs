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
            slackClient.Post(Message);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
    [Cmdlet(VerbsCommon.New, "SlackMessage", SupportsShouldProcess = false)]
    public class CreateSlackMessage : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0
            )]
        public string Channel
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = true,
            Position = 1
            )]
        public string Text
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 2
            )]
        public string IconEmoji
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 3
            )]
        public string Username
        {
            get;
            set;
        }
        public bool LinkNames
        {
            get;
            set;
        }
        public bool Markdown
        {
            get;
            set;
        }

        [Parameter(
            Mandatory = false
            )]
        [ValidateSet("full","none")]
        public string ParseMode
        {
            get;
            set;
        }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {

            SlackMessage message = new SlackMessage
            {
                Channel = Channel,
                Text = Text,
                Username = Username,
                LinkNames = LinkNames,
                Mrkdwn = Markdown,
                
            };
           
            message.Parse
            
            WriteObject(message);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
