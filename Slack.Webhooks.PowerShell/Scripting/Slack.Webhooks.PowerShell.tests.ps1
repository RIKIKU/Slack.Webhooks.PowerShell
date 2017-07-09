#
# This is a PowerShell Unit Test file.
# You need a unit test framework such as Pester to run PowerShell Unit tests. 
# You can download Pester from http://go.microsoft.com/fwlink/?LinkID=534084
#
Import-Module "C:\Users\kyles\Source\Repos\Slack.Webhooks.PowerShell\Slack.Webhooks.PowerShell\Slack.Webhooks.PowerShell\bin\Debug\Slack.Webhooks.PowerShell"

#Import-Module $PSScriptRoot\Slack.Webhooks.PowerShell
Get-Command -Module Slack.Webhooks.PowerShell
Describe "New-SlackField" {
	Context "Function Exists" {
		It "Should be a SlackField"{
        New-SlackField | Should BeOfType Slack.Webhooks.SlackField
        }
    }
    Context "Should Populate All Fields"{
        $Base = New-SlackField -Title "Field Title" -Value "Field Value"
        It "Should Return the correct Short Bool" {
        $Base.Short | Should BeExactly $false
		}        
        It "Should Return the correct Title" {
        $Base.Title | Should BeExactly "Field Title"
		}
        It "Should Return the correct Field Value" {
        $base.Value | Should BeExactly "Field Value"
		}
	}
}

Describe "New-SlackAttachment" {
	Context "Function Exists" {
		It "Should be a SlackField"{
        New-SlackAttachment | Should BeOfType Slack.Webhooks.SlackAttachment
        }
    }
    Context "Should Populate All Fields"{
        $BaseParams = @{
            FallbackMessage = "This is a Fallback Message" 
            Title = "This is a Title" 
            Pretext = "This is some Pretext" 
            Text = "Message Text" 
            TitleLink = "http://titleLink.com.au"
            AuthorIconLink = "https://AuthorIconLink.com.au/somesub" 
            AuthorLink = "https://AuthorLink.com.au/someOtherSub/profile" 
            AuthorName = "The Author's Name"
            Color = "#d6f218"
            ImageUrl = "https://ImageUrl.com/something/jdgdfg"
            ThumbUrl ="https://ThumbURL.com/my/Thumb"
            Fields = $fields
            MarkdownInParameter = "pretext"
        }
        
        $Base = New-SlackAttachment @BaseParams
        It "Should Return the correct Short Bool" {
        $Base.Short | Should BeExactly $false
		}        
        It "Should Return the correct Title" {
        $Base.Title | Should BeExactly "Field Title"
		}
        It "Should Return the correct Field Value" {
        $base.Value | Should BeExactly "Field Value"
		}
	}
}