﻿using System;
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
        public string Text { get; set; }

        public string IconEmoji { get; set; }
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
            //_staticStorage = runtimeDefinedParameterDictionary;
            return runtimeDefinedParameterDictionary;
        }
        
        [Parameter(
            Mandatory = false,
            Position = 3
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
        public SlackAttachment Attachments {get; set;}
        


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

            if (IconEmoji != string.Empty)
            {
                PropertyInfo emojiProperty = typeof(Emoji).GetProperty(IconEmoji);
                message.IconEmoji = (string)emojiProperty.GetValue(null, null);
            }
            

            WriteDebug("Message Created Ouputting to pipeline");
            WriteObject(message);
        }
        //https://stackoverflow.com/questions/33132028/powershell-binary-module-dynamic-tab-completion-for-cmdlet-parameter-values

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

    }
}