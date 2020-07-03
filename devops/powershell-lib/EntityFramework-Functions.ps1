function Create-ModelsFromDatabase([string]$server, [string]$dbUser, [string]$dbPassword, [string]$dbName, [string]$dbContextName, [string]$outputFolder)
{
    $connectionString = "Server=$server;User Id=$dbUser;Password=$password;Database=$dbName"
    & dotnet ef dbcontext scaffold $connectionString Pomelo.EntityFrameworkCore.MySql -c $dbContextName -o $outputFolder
}

function Invoke-Migration([string]$dbContextName)
{
    & dotnet ef database update --context $dbContextName
}