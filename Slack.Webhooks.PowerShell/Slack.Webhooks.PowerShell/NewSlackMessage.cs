using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Language;
using Slack.Webhooks;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Slack.Webhooks.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SlackMessage", SupportsShouldProcess = false, SupportsTransactions = false)]
    [OutputType(typeof(SlackMessage))]
    public class CreateSlackMessage : PSCmdlet, IDynamicParameters
    {
        [Parameter(
            Mandatory = false,
            Position = 0
            )]
        public string Channel { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 1
            )]
        [ValidateNotNullOrEmpty()]
        public string Text { get; set; }

        private static RuntimeDefinedParameterDictionary _staticStorage;
        //https://stackoverflow.com/questions/25823910/pscmdlet-dynamic-auto-complete-a-parameter-like-get-process
        //https://gist.github.com/dhcgn/2f0d4b4d1f08088c438e
        public object GetDynamicParameters()
        {
            IEnumerable<string> emojinames = typeof(Emoji).GetProperties().Select(x => x.Name);
            var runtimeDefinedParameterDictionary = new RuntimeDefinedParameterDictionary();
            var attrib = new Collection<Attribute>() { new ParameterAttribute(), new ValidateSetAttribute(emojinames.ToArray()) };
            var parameter = new RuntimeDefinedParameter("IconEmoji", typeof(String), attrib);
            runtimeDefinedParameterDictionary.Add("IconEmoji", parameter);
            _staticStorage = runtimeDefinedParameterDictionary;
            return runtimeDefinedParameterDictionary;
        }
        [Parameter(
            Mandatory = false,
            Position = 2
            )]
        public string Username { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter LinkNames { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter NotMarkdown {get;set;}

        [Parameter(
            Mandatory = false,
            HelpMessage = "https://api.slack.com/docs/message-formatting"
            )]
        [ValidateSet("full", "none")]
        public string ParseMode {get; set;}

        [Parameter(Mandatory = false)]
        public string IconUrl {get; set;}

        [Parameter(Mandatory = false)]
        public SlackAttachment[] Attachments {get; set;}
        


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
                Parse = ParseMode,
                Attachments = new List<SlackAttachment>()
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
                foreach (SlackAttachment Attachment in Attachments)
                {
                        message.Attachments.Add(Attachment);
                }
            }
            var IconEmojiRuntime = new RuntimeDefinedParameter();
            _staticStorage.TryGetValue("IconEmoji", out IconEmojiRuntime);
            
            if (IconEmojiRuntime.IsSet)
            {
                WriteDebug("setting emoji property info");
                
                PropertyInfo emojiProperty = typeof(Emoji).GetProperty(IconEmojiRuntime.Value.ToString());
                WriteDebug("Selecting Emoji");
                message.IconEmoji = (string)emojiProperty.GetValue(null, null);
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
