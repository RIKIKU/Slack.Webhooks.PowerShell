using System.Management.Automation;


namespace Slack.Webhooks.PowerShell
{
    [Cmdlet(VerbsCommon.New, "SlackField", SupportsShouldProcess = false, SupportsTransactions = false)]
    [OutputType(typeof(SlackField))]
    public class CreateSlackField : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0
            )]
        public string Title
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 1
            )]
        public string Value
        {
            get;
            set;
        }
        [Parameter(
            Mandatory = false,
            Position = 2
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
