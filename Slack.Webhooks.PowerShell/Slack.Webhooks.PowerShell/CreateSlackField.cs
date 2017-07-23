using System.Management.Automation;


namespace Slack.Webhooks.PowerShell
{
    /// <summary>
    /// <para type="synopsis">Used to create a SlackField object.</para>
    /// <para type="description">Used to create a SlackField that you can put inside a SlackAttachment.</para>
    /// <para type="description">SlackFields are a bit like a table that goes to the bottom of the attachment.</para>
    /// <para type="link" uri="(https://api.slack.com/docs/message-attachments#fields)">[Slack Fields]</para>
    /// </summary>
    /// <example>
    ///     <code>$Field = New-SlackField -Title "Server 1 Status" -Value "OK" -Short &#xA; $Attachment = New-SlackAttachment -FallbackMessage "Server Status Report" -Title "Server Status Report" -Color good -Fields $Field &#xA; $Message = New-SlackMessage -Attachments $Attachment -Username "Mr Reporter" &#xA; Send-SlackMessage -URI "https://hooks.slack.com/services/T63QJj9PJ/B63335SU9/BaCnMZ1Gf27CPM2gYWPuptHk" -Message $Message -PostToChannels "#general","#reports"</code>
    ///     <para>In this example a field is used to report on the status of a server.</para>
    /// </example>
    /// <example>
    ///     <code>$computers = "server1", "server2","server3"&#xA;$Fields = @()&#xA;foreach ($computer in $computers)&#xA;{&#xA;    $Fields += New-SlackField -Title "$computer Status" -Value "OK" -Short&#xA;}&#xA;$Attachment = New-SlackAttachment -FallbackMessage "Server Group1 Status Report" -Title "Server Group1 Status Report" -Color good -Fields $Field&#xA;$Message = New-SlackMessage -Attachments $Attachment -Username "Mr Reporter"&#xA;Send-SlackMessage -URI "https://hooks.slack.com/services/T63QJj9PJ/B63335SU9/BaCnMZ1Gf27CPM2gYWPuptHk" -Message $Message -PostToChannels "#general","#reports"</code>
    ///     <para>In this example, an array of fields was created for reporting the status of a group of computers.</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "SlackField", SupportsShouldProcess = false, SupportsTransactions = false)]
    [OutputType(typeof(SlackField))]
    public class CreateSlackField : PSCmdlet
    {
        /// <summary>
        /// <para type="description">Shown as a bold heading above the value text. It cannot contain markup and will be escaped for you.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 0,
            HelpMessage = "Shown as a bold heading above the value text. It cannot contain markup and will be escaped for you."
            )]
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">The text value of the field. It may contain standard message markup and must be escaped as normal. May be multi-line.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 1,
            HelpMessage = "The text value of the field. It may contain standard message markup and must be escaped as normal. May be multi-line."
            )]
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">An optional switch indicating whether the value is short enough to be displayed side-by-side with other values.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 2,
            HelpMessage = "An optional switch indicating whether the value is short enough to be displayed side-by-side with other values."
            )]
        public SwitchParameter Short
        {
            get;
            set;
        }
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteDebug("Instansiating SlackAttachment object");
            SlackField Field = new SlackField
            {
                Short = Short.IsPresent,
                Title = Title,
                Value = Value
            };

            WriteObject(Field);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
