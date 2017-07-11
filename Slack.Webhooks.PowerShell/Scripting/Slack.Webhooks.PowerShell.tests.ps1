#
# This is a PowerShell Unit Test file.
# You need a unit test framework such as Pester to run PowerShell Unit tests. 
# You can download Pester from http://go.microsoft.com/fwlink/?LinkID=534084
#

set-location $PSScriptRoot
Set-Location ..
Import-Module "$((Get-Location).ToString())\Slack.Webhooks.PowerShell\bin\Debug\Slack.Webhooks.PowerShell"

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
		It "Should be a SlackAttachment"{
        New-SlackAttachment | Should BeOfType Slack.Webhooks.SlackAttachment
        }
    }
    Context "Should Populate All Fields"{
        $fields = New-SlackField -Title "SlackField Title" -Value "SlackField Value!@" -Short
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
        It "Should Return the AuthorIconLink" {
        $Base.AuthorIcon | Should BeExactly $BaseParams.AuthorIconLink
		}        
        It "Should Return the correct Field Value" {
        $base.AuthorName | Should BeExactly $BaseParams.AuthorName
		}
        It "Should Return the correct Color" {
        $base.Color | Should BeExactly $BaseParams.Color
		}
        It "Should Return the correct Fallback" {
        $base.Fallback | Should BeExactly $BaseParams.FallbackMessage
		}
        It "Should Return the correct ImageURL" {
        $base.ImageUrl | Should BeExactly $BaseParams.ImageUrl
		}
        It "Should Return the correct MrkdwnIn" {
        $base.MrkdwnIn | Should BeExactly $BaseParams.MarkdownInParameter
		}
        It "Should Return the correct Pretext" {
        $base.Pretext | Should BeExactly $BaseParams.Pretext
		}
        It "Should Return the correct Text" {
        $base.Text | Should BeExactly $BaseParams.Text
		}
        It "Should Return the correct ThumbUrl" {
        $base.ThumbUrl | Should BeExactly $BaseParams.ThumbUrl
		}
        It "Should Return the correct Title" {
        $base.Title | Should BeExactly $BaseParams.Title
		}
        It "Should Return the correct TitleLink" {
        $base.TitleLink | Should BeExactly $BaseParams.TitleLink
		}
        It "Should Return the correct Nested Field Short Bool" {
        $base.Fields[0].Short | Should BeExactly $fields.Short
		}
        It "Should Return the correct Nested Field Title" {
        $base.Fields[0].Title | Should BeExactly $fields.Title
		}
        It "Should Return the correct Nested Field Value" {
        $base.Fields[0].Value | Should BeExactly $fields.Value
		}
	}
    Context "Checking nested objects"{
        
        $fields = @()
		for ($i = 0; $i -lt 2; $i++)
		{ 
			$fields += New-SlackField -Title "SlackField Title" -Value "SlackField Value$($i)" -Short
		}
		
		$BaseParams = @{
            Fields = $fields
            MarkdownInParameter = "pretext","text"
        }
        
        $Base = New-SlackAttachment @BaseParams
        <#It "Should Return a Nested List<SlackField>" {
        $base.Fields[0] | Should BeOfType Slack.Webhooks.SlackField
        $base.Fields[1] | Should BeOfType Slack.Webhooks.SlackField
		}#>
        It "Should Return a Nested List<SlackField>" {
        $base.Fields.GetType().UnderlyingSystemType.ToString() | Should be System.Collections.Generic.List``1[Slack.Webhooks.SlackField]
		}
        It "Should Return a Nested SlackField with correct strings" {
        $base.Fields[0].Title | Should -Be $fields[0].Title
        $base.Fields[1].Value | Should -Be $fields[1].Value
		}
        It "MrkdwnIn is a Nested List<string>" {
        $base.MrkdwnIn.GetType().UnderlyingSystemType.ToString() | Should Be System.Collections.Generic.List``1[System.String]
		}
        It "Should Return the correct markdown in strings" {
        $base.MrkdwnIn[0] | Should -BeExactly $BaseParams.MarkdownInParameter[0]
        $base.MrkdwnIn[1] | Should -BeExactly $BaseParams.MarkdownInParameter[1]
		}
    }
}