# 🔁 Feature Flags with Azure App Configuration | Tests and Examples in .NET

This repository contains a simple project with practical examples of using **Feature Manager** from Azure App Configuration in .NET applications. Here, you'll find real and well-documented implementations of different **Feature Flags** strategies, focusing on custom filtering and context-based activation control.

---

## 🚀 Purpose

The purpose of this repository is to serve as a study base and reference for implementing Feature Flags in ASP.NET Core applications, using best practices and advanced features from `Microsoft.FeatureManagement`.

---

## 🧱 Program.cs

Responsible for connecting your application to Azure App Configuration and setting up automatic refresh of feature flags.

---

### 🔄 What is the Sentinel Key?

🧠 **Tip:** Using a `SentinelKey` is a recommended practice to avoid inconsistencies when updating multiple flags. Changing this key in Azure triggers a refresh of all others.

![image](https://github.com/user-attachments/assets/f2782c21-f413-4e78-b03a-4636b769ab68)

The `SentinelKey` is a kind of *"pseudo-key"*, created solely for the purpose of controlling configuration and Feature Flags **refresh** in Azure App Configuration. It **does not need any specific value** – its function is to act as a trigger to force the refresh of all other settings.

> 💡 Instead of monitoring each Feature Flag individually, you monitor this SentinelKey. So whenever you need to update multiple flags at once, **just change the value of the SentinelKey in the Azure portal**, and all other keys registered in `AzureAppConfiguration` will be updated on the next check.

This helps avoid inconsistencies, especially in environments with multiple flags, by centralizing the control point in a single key.

#### ✅ Example usage in `Program.cs`:

```csharp
refresh.Register("SentinelKey", refreshAll: true)
       .SetRefreshInterval(TimeSpan.FromMinutes(1));
```

## 🧩 `Startup.cs` Structure

The `Startup` class is responsible for registering all the services the application will use, including:

- Azure App Configuration integration  
- Feature Flags configuration with custom filters  
- Dependency injection of `IFeatureManager`  
- Telemetry support with Application Insights (optional)  
- Registration of `IConfigurationRefresher` for manual updates  

This setup ensures that your application is ready to **interpret, react to, and dynamically update** features controlled by Feature Flags.

---

## 🧪 Feature Flag Usage Examples

Below are four practical examples demonstrating different strategies of using Feature Flags in Azure App Configuration, all implemented in the `SubscriptionController`.

---

### 1️⃣ Flag with Custom Filter by Unit

**🔗 Endpoint:** `GET /api/v1/subscription/CreateOrUpdateSubscription`

This example uses a **Custom Filter** that enables a feature only for users belonging to certain units (e.g., UnitA, UnitB...)

- The `EnableUpdateSubscription` flag is configured with an array of authorized units.
- The `CustomFilter` implements logic to check if the user in the request belongs to one of the allowed units.

> ✅ **Best use:** When you want to gradually enable a feature by organizational group (e.g., branches, departments, teams, etc.).

---

### 2️⃣ Simple Flag with `IsEnabledAsync`

**🔗 Endpoint:** `GET /api/v1/subscription/EnableAdminTools`

This is the classic example of a simple boolean flag (`true` or `false`) that is checked using:

```csharp
await _featureManager.IsEnabledAsync("EnableAdminTools")
await _refresher.TryRefreshAsync();
```

This endpoint also forces a refresher call, ensuring the app reads the latest data from Azure App Configuration before checking the flag value.

---

### 3️⃣ Using Feature Flag Snapshot

**🔗 Endpoint:** `GET /api/v1/subscription/EnvsSnapshot`

This example demonstrates the use of **snapshots** with Feature Flags, ensuring that all checks made within the same request cycle return the same value.

Flags are read using `IFeatureManagerSnapshot`, which keeps the flag values **stable throughout the request**, even if they've changed in the Azure portal during the process.

> ✅ **Best use:** When you want to avoid multiple reads of volatile values in the same request and ensure consistency in application behavior.

#### 🧪 Example usage:

```csharp
bool isEnableDarkMode = await _featureManager.IsEnabledAsync("EnableDarkMode");
bool isEnableDashboardV2 = await _featureManager.IsEnabledAsync("EnableDashboardV2");
```

---

### 4️⃣ Group or User-Based Filter with `CustomTargetingContextAccessor`

**🔗 Endpoint:** `GET /api/v1/subscription/BrowserFilter`

This example simulates a **targeting filter based on user context information**, such as browser, group, or ID.

The `AllowedBrowsers` flag can be configured to allow features only for specific browsers — for example, enabling functionality only in Chrome.

> ✅ **Best use:** Advanced feature targeting based on user profile, location, group, device type, browser, etc.

#### 🧪 Example usage:

```csharp
if (await _featureManager.IsEnabledAsync("AllowedBrowsers"))
{
    return Ok("Feature enabled for Chrome!");
}
```

You can also protect the entire endpoint using the `FeatureGate` attribute, ensuring it's only available if the flag is active:

```csharp
[FeatureGate("AllowedBrowsers")]
```

---

## ✅ Conclusion

This project demonstrates in practice how to apply Feature Flags intelligently and scalably in .NET applications using Azure App Configuration.

By mastering these patterns, you gain more flexibility in your feature lifecycle, allowing you to:

- Enable/disable features in real time
- Perform A/B testing safely
- Release features by group or environment
- Reduce risks in complex deployments

---

## 🧠 References

- [Official Azure App Configuration documentation](https://learn.microsoft.com/azure/azure-app-configuration/)
- [Feature Management in .NET with Microsoft.FeatureManagement](https://learn.microsoft.com/azure/azure-app-configuration/quickstart-feature-flag-aspnet-core)
- [Official GitHub sample](https://github.com/Azure/AppConfiguration)
- [Article example](https://www.daveabrock.com/2020/06/07/custom-filters-in-core-flags/)
- [Article example](https://github.com/microsoft/FeatureManagement-Dotnet/blob/main/examples/TargetingConsoleApp/Program.cs)

---

## 🙌 Contributions

Feel free to open an issue, suggest improvements, or submit a PR. Let’s improve this project together!

---

## 💬 Contact

If you have questions or want to talk about Feature Flags and .NET best practices, reach out:

📱 [LinkedIn](https://www.linkedin.com/in/gabriel-ribeiro96/)

---

⭐ If you liked this project, don’t forget to star the repository!
