﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Slack.Webhooks;
using System.Reflection;

namespace Slack.Webhooks.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SlackAttachment", SupportsShouldProcess = false)]
    [OutputType(typeof(SlackAttachment))]
    public class CreateSlackAttachment : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0
            )]
        public string FallbackMessage
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 1
            )]
        public string Title
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 2
            )]
        public string Pretext
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 3
            )]
        public string Text
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 4
            )]
        public string TitleLink
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public string AuthorIconLink
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public string AuthorLink
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public string AuthorName
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false
            )]
        public string Color
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public string ImageUrl
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public string ThumbUrl
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        public SlackField[] Fields
        {
            get;
            set;
        }
        [Parameter(Mandatory = false)]
        [ValidateSet("pretext", "text", "fields")]
        public string[] MarkdownInParameters { get; set; }
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteDebug("Instansiating SlackAttachment object");
            SlackAttachment attachment = new SlackAttachment
            {
                AuthorIcon = AuthorIconLink,
                AuthorLink = AuthorLink,
                AuthorName = AuthorName,
                Color = Color,
                Fallback = FallbackMessage,
                ImageUrl = ImageUrl,
                Pretext = Pretext,
                Text = Text,
                ThumbUrl = ThumbUrl,
                Title = Title,
                TitleLink = TitleLink,
                Fields = new List<SlackField>(),
                MrkdwnIn = new List<string>()
            };

            if (MarkdownInParameters != null)
            {
                attachment.MrkdwnIn = new List<string>(MarkdownInParameters);
            } 
            
           if(Fields != null)
            {
                attachment.Fields = new List<SlackField>(Fields);
            }
            
            
            WriteObject(attachment);

        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
