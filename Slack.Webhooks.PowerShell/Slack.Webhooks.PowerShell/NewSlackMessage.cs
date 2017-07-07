using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Slack.Webhooks;

namespace Slack.Webhooks.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SlackMessage", SupportsShouldProcess = false, SupportsTransactions = false)]
    [OutputType(typeof(SlackMessage))]
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
            SlackMessage message = new SlackMessage
            {
                Channel = Channel,
                Text = Text,
                Username = Username,
                Parse = ParseMode
            };

            if (!string.IsNullOrEmpty(IconUrl))
            {
                WriteDebug("Building IconUrl URIBuilder");
                UriBuilder IconURI = new UriBuilder(IconUrl);
                message.IconUrl = IconURI.Uri;
            }
            if (NotMarkdown) { message.Mrkdwn = false; }
            if (LinkNames) { message.LinkNames = true; }
            if (Attachments != null)
            {
                WriteDebug("Attachments Found. Converting to list");
                message.Attachments = new List<SlackAttachment> { Attachments };
            }
            Type type = typeof(Emoji);
            foreach (var p in type.GetProperties())
            {
                var v = p.GetValue(null, null);
            }

            WriteDebug("Message Created Ouputting to pipeline");
            WriteObject(message);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
