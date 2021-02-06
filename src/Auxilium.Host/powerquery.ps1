Import-Module Az

$basedir = $PSScriptRoot
If ([string]::IsNullOrEmpty("$basedir")) {
    $basedir = "C:\dev\insight\Auxilium\Auxilium.Host\bin\Debug\netcoreapp2.2"; ## can move to env variable if diff among devs
}
$dll = [System.IO.Path]::Combine($basedir, "Auxilium.Core.dll")
Add-Type -Path $dll

function Await-Task {
    param (
        [Parameter(ValueFromPipeline=$true, Mandatory=$true)]
        $task
    )
    process {
        while (-not $task.AsyncWaitHandle.WaitOne(200)) { }
        $task.GetAwaiter().GetResult()
    }
}

$client = New-Object -Type Auxilium.Core.ApiClient 
[string] $username = [Environment]::GetEnvironmentVariable('AZURE_USERNAME', "User")
[string] $password = [Environment]::GetEnvironmentVariable('AZURE_PASSWORD', "User")

$result = Await-Task $client.SignIn($username, $password)
$subscriptions = Await-Task $client.SubscriptionList() | Format-List -Property value

$dll = $null
$client = $null