using System.Management.Automation;


namespace Slack.Webhooks.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SlackField", SupportsShouldProcess = false, SupportsTransactions = false)]
    [OutputType(typeof(SlackField))]
    public class CreateSlackField : PSCmdlet
    {
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
        [Parameter(
            Mandatory = false,
            Position = 2,
            HelpMessage = "An optional flag indicating whether the value is short enough to be displayed side-by-side with other values."
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
