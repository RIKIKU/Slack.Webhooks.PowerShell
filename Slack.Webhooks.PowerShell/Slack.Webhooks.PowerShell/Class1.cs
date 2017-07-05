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
    [Cmdlet(VerbsCommon.New, "SlackMessage", SupportsShouldProcess = false, SupportsTransactions = false)]
    public class CreateSlackMessage : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0
            )]
        public string Channel
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
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
        [Parameter(Mandatory = false)]
        public SwitchParameter LinkNames
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public SwitchParameter NotMarkdown
        {
            get;
            set;
        }

        [Parameter(
            Mandatory = false,
            HelpMessage = "https://api.slack.com/docs/message-formatting"
            )]
        [ValidateSet("full","none")]
        public string ParseMode
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public string IconUrl
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public SlackAttachment Attachments
        {
            get;
            set;
        }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteDebug("Instansiating SlackMessage")
            SlackMessage message = new SlackMessage
            {
                Channel = Channel,
                Text = Text,
                Username = Username,
                Parse = ParseMode
        };
            
            if(!string.IsNullOrEmpty(IconUrl))
            {
                WriteDebug("Building IconUrl URIBuilder");
                UriBuilder IconURI = new UriBuilder(IconUrl);
                message.IconUrl = IconURI.Uri;
            }
            if(NotMarkdown){message.Mrkdwn = false;}
            if(LinkNames) { message.LinkNames = true; }
            if(Attachments != null)
            {
                WriteDebug("Attachments Found. Converting to list");
                message.Attachments = new List<SlackAttachment> { Attachments };
            }
            WriteDebug("Message Created Ouputting to pipeline");
            WriteObject(message);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
    [Cmdlet(VerbsCommon.New, "SlackAttachment", SupportsShouldProcess = false, SupportsTransactions = false)]
    public class CreateSlackAttachment : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0
            )]
        public string Channel
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
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
        [Parameter(Mandatory = false)]
        public SwitchParameter LinkNames
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public SwitchParameter NotMarkdown
        {
            get;
            set;
        }

        [Parameter(
            Mandatory = false,
            HelpMessage = "https://api.slack.com/docs/message-formatting"
            )]
        [ValidateSet("full", "none")]
        public string ParseMode
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public string IconUrl
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public SlackAttachment Attachments
        {
            get;
            set;
        }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteDebug("Instansiating SlackMessage");
            SlackAttachment attachment = new SlackAttachment
            {
                AuthorIcon = "",
                AuthorLink = "",
                AuthorName = "",
                Color = "",
                Fallback = "",
                ImageUrl = "",
                Pretext = "",
                Text = "",
                ThumbUrl = "",
                Title = "",
                TitleLink = ""
            };
            //Fields
            //MrkdwnIn
            WriteObject(attachment);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
