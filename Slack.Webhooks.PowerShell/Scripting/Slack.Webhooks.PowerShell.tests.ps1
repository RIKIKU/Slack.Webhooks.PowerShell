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
Describe "New-SlackMessage" {
	Context "Function Exists" {
		It "Should be a SlackMessage"{
        New-SlackMessage | Should BeOfType Slack.Webhooks.SlackMessage
        }
    }
    Context "Should create a SlackMessage using arrays when they can be used."{
        $fields = @()
		for ($i = 0; $i -lt 2; $i++)
		{ 
			$fields += New-SlackField -Title "SlackField Title" -Value "SlackField Value$($i)" -Short
		}
		
        $Attachment = @()
		for ($i = 0; $i -lt 2; $i++)
		{ 
            $AttachmentParams = @{
                FallbackMessage = "This is a Fallback Message" 
                Title = "This is an Attachment Title $($i)" 
                Pretext = "This is an Attachment Pretext" 
                Text = "Attachment Text" 
                TitleLink = "http://titleLink.com.au"
                AuthorIconLink = "https://AuthorIconLink.com.au/somesub" 
                AuthorLink = "https://AuthorLink.com.au/someOtherSub/profile" 
                AuthorName = "The Author's Name"
                Color = "#d6f218"
                ImageUrl = "https://ImageUrl.com/something/jdgdfg"
                ThumbUrl ="https://ThumbURL.com/my/Thumb"
                Fields = $fields
                MarkdownInParameter = "pretext","text"
            }
        
            $Attachment += New-SlackAttachment @AttachmentParams
        }
		$MessageParams = @{
        Channel = "#general"
        Text = "Message Text"
        Username = "Username Text"
        LinkNames = $true
        NotMarkdown = $false
        ParseMode = "full"
        IconUrl = "https://iconurl.com/test"
        Attachments = $Attachment
        IconEmoji = "A"
        }

        $message = New-SlackMessage @MessageParams
        It "Should return the channel Name"{$message.Channel | should -BeExactly $MessageParams.Channel}
        It "Should return the A Emoji"{$message.IconEmoji | should -BeExactly ":a:"}
        It "Should return the IconURL and is of the correct type"{
            $message.IconUrl.ToString() | should -BeExactly $MessageParams.IconUrl
            $message.IconUrl | should -BeOfType Uri
        }
        It "Should return the LinkNames bool"{$message.LinkNames | should -BeExactly $MessageParams.LinkNames}
        It "Mrkdwn Should be false "{$message.Mrkdwn | should -Be $true}
        It "Parse Mode should be full"{$message.Parse | should -Be $MessageParams.ParseMode}
        It "The message text should be exactly what was entered"{$message.Text | should -BeExactly $MessageParams.Text}
        It "The Username should be the exact string that was entered."{$message.Username | should -BeExactly $MessageParams.Username}
        It "AuthorIcon should be the exact thing that was entered" {$message.AuthorIcon | Should BeExactly $BaseParams.AuthorIconLink}
                
        It "Should Return the correct Field Value" {$message.Attachments[0].AuthorName | Should BeExactly $AttachmentParams.AuthorName}
        It "Should Return the correct Color" {$message.Attachments[1].Color | Should BeExactly $AttachmentParams.Color}
        It "Should Return the correct Fallback" {$message.Attachments[0].Fallback | Should BeExactly $AttachmentParams.FallbackMessage}
        It "Should Return the correct ImageURL" {$message.Attachments[0].ImageUrl | Should BeExactly $AttachmentParams.ImageUrl}
        It "Should Return the correct MrkdwnIn" {$message.Attachments[1].MrkdwnIn | Should BeExactly $AttachmentParams.MarkdownInParameter}
        It "Should Return the correct Pretext" {$message.Attachments[1].Pretext | Should BeExactly $AttachmentParams.Pretext}
        It "Should Return the correct Text" {$message.Attachments[1].Text | Should BeExactly $AttachmentParams.Text}
        It "Should Return the correct ThumbUrl" {$message.Attachments[0].ThumbUrl | Should BeExactly $AttachmentParams.ThumbUrl}
        It "Should Return the correct Title" {$message.Attachments[1].Title | Should BeExactly $AttachmentParams.Title}
        It "Should Return the correct TitleLink" {$message.Attachments[0].TitleLink | Should BeExactly $AttachmentParams.TitleLink}
        It "Should Return the correct Nested Field Short Bool" {$message.Attachments[1].Fields[0].Short | Should Be $true}
        It "Should Return the correct Nested Field Title" {$message.Attachments[0].Fields[0].Title | Should BeExactly $AttachmentParams.Fields[0].Title}
        It "Should Return the correct Nested Field Value" {$message.Attachments[1].Fields[1].Value | Should BeExactly $AttachmentParams.Fields[1].Value}
        It "Should Return a Nested List<SlackAttachment>" {
            $message.Attachments.GetType().UnderlyingSystemType.ToString() | Should be System.Collections.Generic.List``1[Slack.Webhooks.SlackAttachment]
		}
        It "Should Return a Nested List<SlackField>" {
            $message.Attachments[0].Fields.GetType().UnderlyingSystemType.ToString() | Should be System.Collections.Generic.List``1[Slack.Webhooks.SlackField]
		}
        It "MrkdwnIn is a Nested List<string>" {
            $message.Attachments[0].MrkdwnIn.GetType().UnderlyingSystemType.ToString() | Should Be System.Collections.Generic.List``1[System.String]
		}
        It "Should Return the correct markdown in strings" {
            $message.Attachments[0].MrkdwnIn[0] | Should -BeExactly $AttachmentParams.MarkdownInParameter[0]
            $message.Attachments[0].MrkdwnIn[1] | Should -BeExactly $AttachmentParams.MarkdownInParameter[1]
		}
        
        Send-SlackMessage -URI "https://hooks.slack.com/services/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" -Message $message
	}
    Context "Should create a fully detailed message without using arrays"{
			$fields = New-SlackField -Title "SlackField Title" -Value "SlackField Value" -Short

            $AttachmentParams = @{
                FallbackMessage = "This is a Fallback Message" 
                Title = "This is an Attachment Title" 
                Pretext = "This is an Attachment Pretext" 
                Text = "Attachment Text" 
                TitleLink = "http://titleLink.com.au"
                AuthorIconLink = "https://AuthorIconLink.com.au/somesub" 
                AuthorLink = "https://AuthorLink.com.au/someOtherSub/profile" 
                AuthorName = "The Author's Name"
                Color = "#d6f218"
                ImageUrl = "https://ImageUrl.com/something/jdgdfg"
                ThumbUrl ="https://ThumbURL.com/my/Thumb"
                Fields = $fields
                MarkdownInParameters = "fields"
            }
        
        $Attachment = New-SlackAttachment @AttachmentParams
        $MessageParams = @{
        Channel = "#general"
        Text = "Message Text"
        Username = "Username Text"
        LinkNames = $true
        NotMarkdown = $false
        ParseMode = "full"
        IconUrl = "https://iconurl.com/test"
        Attachments = $Attachment
        IconEmoji = "A"
        }

        $message = New-SlackMessage @MessageParams
        It "Should return the channel Name"{$message.Channel | should -BeExactly $MessageParams.Channel}
        It "Should return the A Emoji"{$message.IconEmoji | should -BeExactly ":a:"}
        It "Should return the IconURL and is of the correct type"{
            $message.IconUrl.ToString() | should -BeExactly $MessageParams.IconUrl
            $message.IconUrl | should -BeOfType Uri
        }
        It "Should return the LinkNames bool"{$message.LinkNames | should -BeExactly $MessageParams.LinkNames}
        It "Mrkdwn Should be false "{$message.Mrkdwn | should -Be $true}
        It "Parse Mode should be full"{$message.Parse | should -Be $MessageParams.ParseMode}
        It "The message text should be exactly what was entered"{$message.Text | should -BeExactly $MessageParams.Text}
        It "The Username should be the exact string that was entered."{$message.Username | should -BeExactly $MessageParams.Username}
        It "AuthorIcon should be the exact thing that was entered" {$message.AuthorIcon | Should BeExactly $BaseParams.AuthorIconLink}
                
        It "Should Return the correct Field Value" {$message.Attachments.AuthorName | Should BeExactly $AttachmentParams.AuthorName}
        It "Should Return the correct Color" {$message.Attachments.Color | Should BeExactly $AttachmentParams.Color}
        It "Should Return the correct Fallback" {$message.Attachments.Fallback | Should BeExactly $AttachmentParams.FallbackMessage}
        It "Should Return the correct ImageURL" {$message.Attachments.ImageUrl | Should BeExactly $AttachmentParams.ImageUrl}
        It "Should Return the correct MrkdwnIn" {$message.Attachments.MrkdwnIn | Should BeExactly $AttachmentParams.MarkdownInParameters}
        It "Should Return the correct Pretext" {$message.Attachments.Pretext | Should BeExactly $AttachmentParams.Pretext}
        It "Should Return the correct Text" {$message.Attachments.Text | Should BeExactly $AttachmentParams.Text}
        It "Should Return the correct ThumbUrl" {$message.Attachments.ThumbUrl | Should BeExactly $AttachmentParams.ThumbUrl}
        It "Should Return the correct Title" {$message.Attachments.Title | Should BeExactly $AttachmentParams.Title}
        It "Should Return the correct TitleLink" {$message.Attachments.TitleLink | Should BeExactly $AttachmentParams.TitleLink}
        It "Should Return the correct Nested Field Short Bool" {$message.Attachments.Fields[0].Short | Should Be $true}
        It "Should Return the correct Nested Field Title" {$message.Attachments.Fields[0].Title | Should BeExactly $AttachmentParams.Fields[0].Title}
        It "Should Return the correct Nested Field Value" {$message.Attachments.Fields[0].Value | Should BeExactly $AttachmentParams.Fields[0].Value}
        It "Should Return a Nested List<SlackAttachment>" {
            $message.Attachments.GetType().UnderlyingSystemType.ToString() | Should be System.Collections.Generic.List``1[Slack.Webhooks.SlackAttachment]
		}
        It "Should Return a Nested List<SlackField>" {
            $message.Attachments[0].Fields.GetType().UnderlyingSystemType.ToString() | Should be System.Collections.Generic.List``1[Slack.Webhooks.SlackField]
		}
        It "MrkdwnIn is a Nested List<string>" {
            $message.Attachments[0].MrkdwnIn.GetType().UnderlyingSystemType.ToString() | Should Be System.Collections.Generic.List``1[System.String]
		}
 
        
        #Send-SlackMessage -URI "https://hooks.slack.com/services/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" -Message $message
	}

}