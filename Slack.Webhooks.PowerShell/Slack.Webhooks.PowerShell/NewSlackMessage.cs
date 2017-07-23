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
    /// <summary>
    /// <para type="synopsis">Used to create a SlackMessage object.</para>
    /// <para type="description">Used to create a SlackMessage that you can send to a Slack incoming webhook.</para>
    /// <para type="description">A SlackMessage can be thought of as a vehicle forwhich your text, Attachments and fields travel to the API.</para>
    /// <para type="description">Only a SlackMessage can be sent to the webhook, however you may put an array of attachments into a SlackMessage.</para>
    /// <para type="link" uri="(https://api.slack.com/docs/message-formatting)">[Slack Message Formatting]</para>
    /// </summary>
    /// <example>
    ///     <code>$message = New-SlackMessage -Text "Something has happened and I'm sending a message about it" -Username "Mr Magoo" </code>
    ///     <code>Send-SlackMessage -URI "https://hooks.slack.com/services/T63QJj9PJ/B63335SU9/BaCnMZ1Gf27CPM2gYWPuptHk" -Message $message </code>
    ///     <para>In this example I am sending a simple, plain text message.</para>
    /// </example>
    /// <example>
    ///     <code>$message = New-SlackMessage -Text "&lt;!everyone&gt; some kind of thing has happened and I'm sending a message about it" -Username "Mr Magoo"  -NoMarkdown </code>
    ///     <code>Send-SlackMessage -URI "https://hooks.slack.com/services/T63QJj9PJ/B63335SU9/BaCnMZ1Gf27CPM2gYWPuptHk" -Message $message </code>
    ///     <para>In this example I am sending a simple, plain text message.</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "SlackMessage", SupportsShouldProcess = true, SupportsTransactions = false)]
    [OutputType(typeof(SlackMessage))]
    public class CreateSlackMessage : PSCmdlet, IDynamicParameters
    {
        /// <summary>
        /// <para type="description">The Channel you want to post the message to. Must include the # at the front of the channel name.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 0,
            HelpMessage = "The Channel you want to post the message to. Must include the # at the front of the channel name."
            )]
        public string Channel { get; set; }

        /// <summary>
        /// <para type="description">The main text of the message.</para>
        /// </summary>
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
        /// <summary>
        /// <para type="description">The name that you want to post a message as. Doesn't have to be an existing user.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 2,
            HelpMessage = "The name that you want to post a message as. Doesn't have to be an existing user."
            )]
        public string Username { get; set; }

        /// <summary>
        /// <para type="description">By default, Slack will not linkify channel names (starting with a '#') and usernames (starting with an '@').
        /// Include this switch to enable this behaviour.
        /// If you've set Parse to 'full' then this is enabled already enabled.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false,
                    HelpMessage = "By default, Slack will not linkify channel names (starting with a '#') and usernames (starting with an '@')\nInclude this switch to enable this behaviour.\nIf you've set Parse to 'full' then this is enabled already enabled.")]
        public SwitchParameter LinkNames { get; set; }

        /// <summary>
        /// <para type="description">Turn off Markdown processing on the message</para>
        /// </summary>
        [Parameter(Mandatory = false,
            HelpMessage = "Turn off Markdown processing on the message")]
        public SwitchParameter NoMarkdown {get;set;}
        /// <summary>
        /// <para type="description">If you don't want Slack to perform any processing on your message, pass an argument of none
        /// If you want Slack to treat your message as completely unformatted, use "full".
        /// This will ignore any markup formatting you added to your message.
        /// </para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "If you don't want Slack to perform any processing on your message, pass an argument of none\nIf you want Slack to treat your message as completely unformatted, use \"full\". This will ignore any markup formatting you added to your message."
            )]
        [ValidateSet("full", "none")]
        public string ParseMode {get; set;}
        /// <summary>
        /// <para type="description">A URL to a small image that will become the avatar for the message.
        /// IconEmoji takes precedence over this.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false,
            HelpMessage = "A URL to a small image that will become the avatar for the message.\nIconEmoji takes precedence over this"
            )]
        public string IconUrl {get; set;}
        /// <summary>
        /// <para type="description">An attachment or array of attachments to add to the message.
        /// Note this is not a file attachment.
        /// </para>
        /// </summary>
        [Parameter(Mandatory = false,
                    HelpMessage = "An attachment or array of attachments to add to the message.\nNote this is not a file attachment.")]
        public SlackAttachment[] Attachments {get; set;}
        /// <summary>
        /// <para type="description">Throw caution to the wind and carry on!</para>
        /// </summary>
        [Parameter(Mandatory = false,
            HelpMessage = "Throw caution to the wind and carry on!"
            )]
        public SwitchParameter Force { get; set; }
        


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
            if (NoMarkdown) { message.Mrkdwn = false; }
            if (LinkNames) { message.LinkNames = true; }
            if (Attachments != null)
            {
                //need to stop people from adding more than 20 attachments..unless the lib already does this?
                if ((Attachments.Length > 20 && Attachments.Length < 100) || Force)
                {
                    //ignore the guidelines that say you should not send a message with over 100 attachments. 
                    ErrorRecord x = new ErrorRecord(new IndexOutOfRangeException("Number of SlackAttachments exceeds 20"), "Number of SlackAttachments exceeds 20", ErrorCategory.LimitsExceeded, Attachments);
                    ThrowTerminatingError(x);
                }
                else if (Attachments.Length > 100)
                {
                    //might as well kill it here, the API won't process more than 100 attachments. 
                    ErrorRecord x = new ErrorRecord(new IndexOutOfRangeException("Number of SlackAttachments exceeds 100"), "Number of SlackAttachments exceeds 100", ErrorCategory.LimitsExceeded, Attachments);
                    ThrowTerminatingError(x);
                }
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
