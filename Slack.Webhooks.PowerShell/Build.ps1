Param($solutionDirectory,$projectDirectory, $OutDir, $configurationName,$targetPath)

."$solutionDirectory\packages\XmlDoc2CmdletDoc.0.2.7\tools\XmlDoc2CmdletDoc.exe" $targetPath
Copy-Item "$($solutionDirectory)Scripting\Slack.Webhooks.PowerShell.tests.ps1" "$($projectDirectory)bin\$($configurationName)\" -Force
Copy-Item "$($solutionDirectory)Scripting\Slack.Webhooks.Powershell.psd1" "$($projectDirectory)$($OutDir)" -Force