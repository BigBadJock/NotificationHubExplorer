# NotificationHubExplorer

A desktop developer tool for browsing and managing Azure Notification Hubs Installations using Microsoft Entra ID authentication.

NotificationHubExplorer is designed as a lightweight, modern sibling to ServiceBusExplorer — focused specifically on Notification Hub device management.

## 🎯 Purpose

Azure Portal does not provide a rich UI for managing Notification Hub installations (device registrations).
This tool fills that gap by offering:
- Interactive Azure sign-in
- Subscription and hub browsing
- Full installation inspection
- Tag management
- Bulk operations
- Test notification sending
- Primary goal: ease of use for developers.

## 🚀 MVP Feature Set
### 🔐 Authentication
- Interactive sign-in via Microsoft Entra ID
- Multi-tenant support
- Subscription picker
- Resource group picker
- Namespace picker
- Hub picker
- Secure token caching (no connection strings required)

## 📦 Installation Management (Core Feature)
Full support for the Installations model (modern Notification Hubs registration model).
- Browse & Search
  - Paged installation listing
  - Search by Installation ID
  - Search by PNS handle (device token)
  - Filter by tag
  - Filter by platform (APNS / FCM / WNS / etc.)
- Inspect
  - View full installation JSON
  - View tags
  - View templates
  - View platform
  - View push channel (PNS handle)
- Modify
  -  Edit tags
  - Edit templates
- Update installation
- Delete installation
- Bulk delete by tag (with safety confirmation)
- Export
  - Export current results to:
    - JSON
    - CSV

## 📣 Send Test Notification
- Send to:
  - Installation ID
  - Tag
  - Tag expression
- Custom JSON payload
- Display send result status
- Show tracking ID

## ❌ Out of Scope (MVP)
- Classic Registration model support
- Namespace creation/deletion
- SAS key management
- Credential rotation
- Metrics dashboards
- Cross-hub search
- Role-based UI permissions
- ARM configuration editing

## 🏗 Architecture
- Application Type
-   Windows Desktop Application built with:
  - .NET 8
  - WinUI 3
  - Azure SDK for .NET
  - Azure APIs Used
  - Control Plane (Azure Resource Manager)
- Used for:
  - Listing subscriptions
  - Listing resource groups
  - Listing namespaces
  - Listing hubs
- SDK:
  - Azure.ResourceManager
  - Azure.ResourceManager.NotificationHubs
  - Data Plane (Notification Hub Runtime API)
- Used for:
  - Installation listing
  - Installation CRUD
  - Sending notifications
- SDK:
  - Azure.Messaging.NotificationHubs
  - REST fallback used where SDK coverage is incomplete.
- 🔑 Authentication Model
  - Uses InteractiveBrowserCredential
  - Supports multi-tenant login
  - Respects Azure RBAC
  - Required Azure role:
  - Azure Notification Hubs Data Owner
  or
  - Azure Notification Hubs Data Reader (read-only access)
  - No secrets or connection strings are stored.

## 🧠 Design Principles
- Zero configuration (no manual connection strings)
- Discoverable UI
- Safe bulk operations
- Clear confirmation dialogs
- Async, responsive UX
- Minimal friction for developers

## 📊 Performance Expectations
- Designed to support hubs with up to:
- 50,000 installations (paged)
- Uses:
- Continuation tokens
- Async API calls
- Cancellation support

## 🔒 Security
- Azure AD authentication only
- No SAS keys stored
- Token cache persisted securely via Windows credential storage
- All operations respect Azure RBAC

## 🗺 Roadmap (Post-MVP)
- Potential future enhancements:
- Classic registration support
- Tag analytics dashboard
- Dark / light theme toggle
- Cross-hub search
- Export-all background job
- macOS support (Avalonia)
- Read-only UI mode
- Advanced tag expression builder

## 🛠 Development
- Requirements
- .NET 8 SDK
- Windows 11 (recommended)
- Run Locally
- dotnet restore
- dotnet build
- dotnet run

## 🤝 Contributing
- Contributions welcome.
- Please:
  - Open an issue before large feature work
  - Keep PRs focused
  - Follow existing architecture patterns

## 📄 License
- MIT License (recommended for OSS tooling)

##⚠ Disclaimer
- This tool is not affiliated with or officially supported by Microsoft.
- Use at your own risk.
