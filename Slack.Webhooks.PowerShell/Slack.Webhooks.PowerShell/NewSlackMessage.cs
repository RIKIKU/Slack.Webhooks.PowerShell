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
            Position = 0,
            HelpMessage = "The Channel you want to post the message to. Must include the # at the front of the channel name."
            )]
        public string Channel { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 1,
            HelpMessage = "The main text of the message."
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
            var attrib = new Collection<Attribute>()
            {
                new ParameterAttribute() { HelpMessage = "A Slack Emoji to use as the avatar", Mandatory = false},
                new ValidateSetAttribute(emojinames.ToArray())
            };
            var parameter = new RuntimeDefinedParameter("IconEmoji", typeof(String), attrib);
            runtimeDefinedParameterDictionary.Add("IconEmoji", parameter);
            _staticStorage = runtimeDefinedParameterDictionary;
            return runtimeDefinedParameterDictionary;
        }
        [Parameter(
            Mandatory = false,
            Position = 2,
            HelpMessage = "The name that you want to post a message as. Doesn't have to be an existing user."
            )]
        public string Username { get; set; }

        [Parameter(Mandatory = false,
                    HelpMessage = "By default, Slack will not linkify channel names (starting with a '#') and usernames (starting with an '@')\nInclude this switch to enable this behaviour.\nIf you've set Parse to 'full' then this is enabled already enabled.")]
        public SwitchParameter LinkNames { get; set; }

        [Parameter(Mandatory = false,
            HelpMessage = "Turn off Markdown processing on the message")]
        public SwitchParameter NoMarkdown {get;set;}

        [Parameter(
            Mandatory = false,
            HelpMessage = "If you don't want Slack to perform any processing on your message, pass an argument of none\nIf you want Slack to treat your message as completely unformatted, use \"full\". This will ignore any markup formatting you added to your message."
            )]
        [ValidateSet("full", "none")]
        public string ParseMode {get; set;}

        [Parameter(Mandatory = false,
            HelpMessage = "A URL to a small image that will become the avatar for the message.\nIconEmoji takes precedence over this"
            )]
        public string IconUrl {get; set;}

        [Parameter(Mandatory = false,
                    HelpMessage = "An attachment or array of attachments to add to the message.\nNote this is not a file attachment.")]
        public SlackAttachment[] Attachments {get; set;}
        


        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            //need to stop people from adding more than 20 attachments..unless the lib already does this?
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
            if (NoMarkdown) { message.Mrkdwn = false; }
            if (LinkNames) { message.LinkNames = true; }
            if (Attachments != null)
            {
                WriteDebug("Attachments Found. Converting to list");
                message.Attachments = new List<SlackAttachment>(Attachments);
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
