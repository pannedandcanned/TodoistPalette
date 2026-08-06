using Meziantou.Framework.Win32;

/// <summary>
/// Service to store API keys. Think this is the best option if I don't wanna store API keys in plaintext.
/// </summary>
public sealed class SecretStore
{	
	private const string CredentialName =
		"com.TodoistPalette.commandpalette.TodoistApiKey";

	public bool HasApiKey()
		=> CredentialManager.ReadCredential(CredentialName) is not null;

	public string? GetApiKey()
		=> CredentialManager.ReadCredential(CredentialName)?.Password;

	public void SaveApiKey(string apiKey)
	{
		CredentialManager.WriteCredential(
			CredentialName,
			"TodoistApiKey",
			apiKey,
			CredentialPersistence.LocalMachine);
	}

	public void DeleteApiKey()
		=> CredentialManager.DeleteCredential(CredentialName);
}