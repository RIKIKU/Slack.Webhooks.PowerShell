Param($solutionDirectory,$projectDirectory, $OutDir, $configurationName,$targetPath)

."$solutionDirectory\packages\XmlDoc2CmdletDoc.0.2.7\tools\XmlDoc2CmdletDoc.exe" $targetPath
Copy-Item "$($solutionDirectory)Scripting\Slack.Webhooks.PowerShell.tests.ps1" "$($projectDirectory)bin\$($configurationName)\" -Force
Copy-Item "$($solutionDirectory)Scripting\Slack.Webhooks.Powershell.psd1" "$($projectDirectory)$($OutDir)" -Force




<#
.Synopsis
   Adds a parameter to an existing, fully formatted help file.
.DESCRIPTION
   This thing just looks for your cmdlet name, clones the first, existing parameter and appends it to the bottom of the list of parameters for your cmdlet.
.EXAMPLE
   Add-HelpParameter -Path "C:\We've\all seen a \Filepath\before-Hlep.xml" -CmdletName New-Shiney -ParameterName "MissingParameter" -Position 1 -TyepName "String" -TypeFullName "System.String" 
#>
function Add-HelpParameter
{
    [CmdletBinding(SupportsShouldProcess=$false)]
    Param
    (
        # Path to the help file.
        [Parameter(Mandatory=$true,Position=0)]
        [string]
        $Path,

        #Name of the cmdlet that you want to add the parameter to.
        [Parameter(Mandatory=$true,Position=1)]
        [string]
        $CmdletName,
        
        # The name of your parameter.
        [Parameter(Mandatory=$true,Position=2)]
        [string]
        $ParameterName,

        # The help text that describes your parameter.
        [Parameter(Mandatory=$false)]
        [string]
        $HelpMessage,

        # Does this parameter accept Pipeline Input?
        [Parameter(Mandatory=$false)]
        [switch]
        $PipelineInput,

        # Is this parameter required?
        [Parameter(Mandatory=$false)]
        [switch]
        $IsRequired,

        # This can either be a number, or "named"
        [Parameter(Mandatory=$true)]
        [ValidatePattern("named|[0-99]")]
        [String]
        $Position,

        # The system type of your parameter. "String[]" for example.
        [Parameter(Mandatory=$true)]
        [string]
        $TyepName,

        # The Full Type Name of your parameter. "System.String" for eaxample.
        [Parameter(Mandatory=$true)]
        [string]
        $TypeFullName
    )

    Begin
    {
        $found = $false
    }
    Process
    {
        [xml] $xmlDoc = get-content $Path -ErrorAction Stop
        foreach ($item in ($xmlDoc.helpItems.ChildNodes).GetEnumerator())
        {
            if($item.details.name -eq $CmdletName)
            {
                $parameterClone = $item.parameters.parameter[0].Clone()
                $parameterClone.required = $IsRequired.ToString().ToLower()
                $parameterClone.pipelineInput = $PipelineInput.ToString().ToLower()
                $parameterClone.position = $Position.ToLower()
                $parameterClone.name = $ParameterName
                $parameterClone.description.para = $HelpMessage
                $parameterClone.parameterValue.'#text' = $TyepName
                $parameterClone.type.name = $TypeFullName

                $item.parameters.AppendChild($parameterClone)
                $found = $true
            }
        }
    }
    End
    {
        if(!$found){throw "The cmdlet was not found in the specified help file."}
        else{$xmlDoc.Save($Path)}
    }
}


$path = "$($projectDirectory)$($OutDir)Slack.Webhooks.PowerShell.dll-Help.xml"
$path
pause
Add-HelpParameter -Path $path -CmdletName New-SlackMessage -ParameterName "IconEmoji" -Position 1 -TyepName "String" -TypeFullName "System.String" -HelpMessage "A Slack Emoji to use as the avatar" -ErrorAction Stop