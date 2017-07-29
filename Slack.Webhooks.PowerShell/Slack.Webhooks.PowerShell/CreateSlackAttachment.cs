using System.Collections.Generic;
using System.Management.Automation;

namespace Slack.Webhooks.PowerShell
{
    /// <summary>
    /// <para type="synopsis">Used to create a SlackAttachment object.</para>
    /// <para type="description">SlackAttachments can be used to attach additional information to your message. A quick comparision of the SlackMessage and SlackAttachment cmdlets reveals that most of slack's functionality is contained within an attachment.</para>
    /// <para type="description">SlackAttachments are an array that you can attach to a message. You can add more than one, in fact you can add up to 100, but you should try and keep it below 20. If you need to add more compartmentalised information to a message you should look at SlackFields.</para>
    /// <para type="link" uri="(https://api.slack.com/docs/message-attachments)">[Slack Attachments]</para>
    /// </summary>
    /// <example>
    ///     <code>$AttachmentParams = @{&#xA;&#x9;FallbackMessage = "This is a Fallback Message" &#xA;&#x9;Title = "This is an Attachment Title" &#xA;&#x9;Pretext = "This is an Attachment Pretext" &#xA;&#x9;Text = "Attachment Text" &#xA;&#x9;TitleLink = "http://titleLink.com.au"&#xA;&#x9;Color = "#d6f218"&#xA;}&#xA;&#xA;$Attachment = New-SlackAttachment @AttachmentParams&#xA;$MessageParams = @{&#xA;&#x9;Username = "Username Text"&#xA;&#x9;Attachments = $Attachment&#xA;&#x9;IconEmoji = "A"&#xA;}&#xA;&#xA;$message = New-SlackMessage @MessageParams&#xA;Send-SlackMessage -URI "https://hooks.slack.com/services/T63QJj9PJ/B63335SU9/BaCnMZ1Gf27CPM2gYWPuptHk" -Message $message </code>
    ///     <para>In this example, I'm creating a SlackAttachment and putting it into a message and sending it off. I'm using a method called Parameter Splatting to parse the parameters to each cmdlet as I find that easier to read. </para>
    /// </example>
    /// <example>
    ///     <code>$AttachmentParams = @{&#xA;&#x9;FallbackMessage = "This is a Fallback Message" &#xA;&#x9;Title = "This is an Attachment Title"&#xA;&#x9;Pretext = "This is an Attachment Pretext" &#xA;&#x9;Text = "Attachment Text" &#xA;&#x9;TitleLink = "http://titleLink.com.au"&#xA;&#x9;Color = "#d6f218"&#xA;}&#xA;&#xA;New-SlackAttachment @AttachmentParams&#xA;$MessageParams = @{&#xA;&#x9;Username = "Username Text"&#xA;&#x9;IconEmoji = "A"&#xA;}&#xA;&#xA;New-SlackAttachment @AttachmentParams | New-SlackMessage @MessageParams | Send-SlackMessage -URI "https://hooks.slack.com/services/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"</code>
    ///     <para>This example, is exactly the same as the last example. I am just uisng the pipeline to make it even easier to read.</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "SlackAttachment", SupportsShouldProcess = false,DefaultParameterSetName = "None",ConfirmImpact = ConfirmImpact.None)]
    [OutputType(typeof(SlackAttachment))]
    public class CreateSlackAttachment : PSCmdlet
    {

        /// <summary>
        /// <para type="description"> A plain-text summary of the attachment.&#xA;This text will be used in clients that don't show formatted text (eg. IRC, mobile notifications) and should not contain any markup.</para>
        /// </summary>
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
        /// <summary>
        /// <para type="description">The title is displayed as larger, bold text near the top of a message attachment.&#xA;By passing a valid URL in the TitleLink parameter (optional), the title text will be hyperlinked.</para>
        /// </summary>
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
        /// <summary>
        /// <para type="description">This is optional text that appears above the message attachment block</para>
        /// </summary>
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
        /// <summary>
        /// <para type="description">This is the main text in a message attachment, and can contain standard message markup.&#xA;Links posted in the text field will not unfurl.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 3,
            HelpMessage = "This is the main text in a message attachment, and can contain standard message markup.&#xA;Links posted in the text field will not unfurl."
            )]
        public string Text
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">The URL for the Title text to point to</para>
        /// </summary>
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
        /// <summary>
        /// <para type="description">A valid URL that displays a small 16x16px image to the left of the AuthorName text.&#xA;Will only work if AuthorName is present.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL that displays a small 16x16px image to the left of the AuthorName text.\nWill only work if AuthorName is present."
            )]
        public string AuthorIconLink
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">A valid URL that will hyperlink the AuthorName text.&#xA;Will only work if AuthorName is present</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL that will hyperlink the AuthorName text.\nWill only work if AuthorName is present"
            )]
        public string AuthorLink
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">Small text used to display the author's name.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "Small text used to display the author's name."
            )]
        public string AuthorName
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">An optional value that can either be one of good, warning, danger, or any hex color code (eg. #439FE0).&#xA;This value is used to color the border along the left side of the message attachment.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "An optional value that can either be one of good, warning, danger, or any hex color code (eg. #439FE0).\nThis value is used to color the border along the left side of the message attachment."
            )]
        public string Color
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">A valid URL to an image file that will be displayed inside a message attachment.&#xA;The following formats are supported: GIF, JPEG, PNG, and BMP.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL to an image file that will be displayed inside a message attachment.\nThe following formats are supported: GIF, JPEG, PNG, and BMP."
            )]
        public string ImageUrl
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">A valid URL to an image file that will be displayed as a thumbnail on the right side of a message attachment.&#xA;The following formats are supported: GIF, JPEG, PNG, and BMP.&#xA;The filesize of the image must also be less than 500 KB.&#xA;For best results, please use images that are already 75px by 75px.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            HelpMessage = "A valid URL to an image file that will be displayed as a thumbnail on the right side of a message attachment.\nThe following formats are supported: GIF, JPEG, PNG, and BMP.\nThe filesize of the image must also be less than 500 KB.\nFor best results, please use images that are already 75px by 75px."
            )]
        public string ThumbUrl
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">Fields are a bit like a small spreadsheet you can add to your attachment. See New-SlackField for more info.</para>
        /// </summary>
        [Parameter(Mandatory = false,
            ValueFromPipeline = true,
            HelpMessage = "Fields are a bit like a small spreadsheet you can add to your attachment. See New-SlackField for more info.")]
        public SlackField[] Fields
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">By default attachment text is not processed as markdown.&#xA;To enable markdown processing on a parameter, specify its name here.&#xA;using 'fields' here enables processing for both the 'Pretext' and 'Text' parameters.</para>
        /// </summary>
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
