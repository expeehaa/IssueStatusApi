# IssueStatusApi

This is a small library that uses the Github API (using Octokit.Net) to get status information of the format that [Issue Status](https://github.com/tadhglewis/issue-status) is able to parse and display.
The information can be used to, for example, monitor changes of sites using this format automatically.

# Usage

First, the `IssueStatusApi.IssueStatus` class has to be initialized.
The constructor takes an optional (but recommended) API token.
Without an API token, requests are limited to 60 per hour, with an API token the limit goes up to 5000 requests per hour.
For public repositories, the token needs at least the scope `public_repo`.

```csharp
var issueStatus = new IssueStatusApi.IssueStatus(GITHUB_API_TOKEN);
```

The status issues can be fetched with the following method.

```csharp
var status = issueStatus.GetStatus(GITHUB_REPO_OWNER, GITHUB_REPO_NAME);
```

To produce a list of changed status issues (new/modified/deleted), use the following method on two `Status` objects.

```csharp
var changes = IssueStatusApi.IssueStatus.CompareStatus(oldStatus, newStatus);
```
