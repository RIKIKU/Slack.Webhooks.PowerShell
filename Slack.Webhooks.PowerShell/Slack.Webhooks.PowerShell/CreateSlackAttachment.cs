using System.Collections.Generic;
using System.Management.Automation;

namespace Slack.Webhooks.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SlackAttachment", SupportsShouldProcess = false,DefaultParameterSetName = "None",ConfirmImpact = ConfirmImpact.None)]
    [OutputType(typeof(SlackAttachment))]
    public class CreateSlackAttachment : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            HelpMessage = "A plain-text summary of the attachment.\nThis text will be used in clients that don't show formatted text (eg. IRC, mobile notifications) and should not contain any markup."
            )]
        public string FallbackMessage
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 1,
            HelpMessage = "The title is displayed as larger, bold text near the top of a message attachment.\nBy passing a valid URL in the TitleLink parameter (optional), the title text will be hyperlinked.",
            ParameterSetName = "Title"
            )]
        public string Title
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 2,
            HelpMessage = "This is optional text that appears above the message attachment block"
            )]
        public string Pretext
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 3,
            HelpMessage = "This is the main text in a message attachment, and can contain standard message markup.\nLinks posted in the text field will not unfurl."
            )]
        public string Text
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 4,
            HelpMessage = "The URL for the Title text to point to",
            ParameterSetName = "Title"
            )]
        public string TitleLink
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL that displays a small 16x16px image to the left of the AuthorName text.\nWill only work if AuthorName is present."
            )]
        public string AuthorIconLink
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL that will hyperlink the AuthorName text.\nWill only work if AuthorName is present"
            )]
        public string AuthorLink
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            HelpMessage = "Small text used to display the author's name."
            )]
        public string AuthorName
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            HelpMessage = "An optional value that can either be one of good, warning, danger, or any hex color code (eg. #439FE0).\nThis value is used to color the border along the left side of the message attachment."
            )]
        public string Color
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL to an image file that will be displayed inside a message attachment.\nThe following formats are supported: GIF, JPEG, PNG, and BMP."
            )]
        public string ImageUrl
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL to an image file that will be displayed as a thumbnail on the right side of a message attachment.\nThe following formats are supported: GIF, JPEG, PNG, and BMP.\nThe filesize of the image must also be less than 500 KB.\nFor best results, please use images that are already 75px by 75px."
            )]
        public string ThumbUrl
        {
            get;
            set;
        }
        [Parameter(Mandatory = false,
            ValueFromPipeline = true,
            HelpMessage = "Fields are a bit like a small spreadsheet you can add to your attachment. See New-SlackField for more info.")]
        public SlackField[] Fields
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            HelpMessage = "By default attachment text is not processed as markdown.\nTo enable markdown processing on a parameter, specify its name here.\nusing 'fields' here enables processing for both the 'Pretext' and 'Text' parameters."
            )]
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
