using System.Collections.Generic;
using System.Management.Automation;

namespace Slack.Webhooks.PowerShell
{
    /// <summary>
    /// <para type="synopsis">Send a Slack Messge to a Webhook</para>
    /// <para type="description">After you have created a SlackMessage you can send it to a webhook using this cmdlet.</para>
    /// </summary>
    /// <example>
    ///     <code>$message = New-SlackMessage -Text "Something has happened and I'm sending a message about it" -Username "Mr Magoo"&#xA;Send-SlackMessage -URI "https://hooks.slack.com/services/T63QJj9PJ/B63335SU9/BaCnMZ1Gf27CPM2gYWPuptHk" -Message $message </code>
    ///     <para>In this example I am sending a simple, plain text message.</para>
    /// </example>
    [Cmdlet(VerbsCommunications.Send,"SlackMessage",SupportsShouldProcess = false)]
    [OutputType(typeof(bool))]
    public class SendSlackMessage : PSCmdlet
    {
        /// <summary>
        /// <para type="description">A valid Slack Webhook URI</para>
        /// </summary>
        [Parameter(
            Mandatory = true,
            Position = 0
            )]
        public string URI
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">The SlackMessage you want to send to slack.</para>
        /// </summary>
        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = true
            )]
        public SlackMessage Message
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">If you need a longer timeout, it can be set here.</para>
        /// </summary>
        [Parameter(
            Mandatory = false,
            Position = 2
            )]
        public int TimeOut
        {
            get;
            set;
        }
        /// <summary>
        /// <para type="description">Send the message to multiple channels at once.</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public string[] PostToChannels
        {
            get;
            set;
        }
        
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {

            SlackClient slackClient;
            if (TimeOut.Equals(0)) { slackClient = new SlackClient(URI); }
            else { slackClient = new SlackClient(URI, TimeOut); }
            WriteDebug(string.Format("Sending Message to {0}", URI));
            if (PostToChannels != null)
            {

                IEnumerable<string> EnumiratorChan = PostToChannels;
                var output = slackClient.PostToChannels(Message, PostToChannels);
                WriteObject(output);

            }
            else
            {
                var output = slackClient.Post(Message);
                WriteObject(output);
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
    

    
    
}
