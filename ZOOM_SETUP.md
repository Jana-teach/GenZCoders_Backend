# Zoom Integration Setup

## Zoom SDK Key Migration (2026)

**SdkKey and SdkSecret are deprecated.** Use **Client ID** and **Client Secret** instead. Migration deadline: March 31, 2026.

---

## Credential Overview

| Credential | Purpose | App Type |
|------------|---------|----------|
| **AccountId, ClientId, ClientSecret** | Create meetings via Zoom API | Server-to-Server OAuth app |
| **ClientId, ClientSecret** | Join meetings **inside the app** (embedded) | Meeting SDK app |

**Single app option:** If you use one app for both creating and joining, use the same ClientId/ClientSecret for everything.

**Separate apps option:** Use `MeetingSdkClientId` and `MeetingSdkClientSecret` for in-app join when your Meeting SDK app has different credentials than your OAuth app.

---

## 1. Create Meetings (Server-to-Server OAuth)

Your `appsettings.json`:

```json
"Zoom": {
  "AccountId": "YOUR_ACCOUNT_ID",
  "ClientId": "YOUR_CLIENT_ID",
  "ClientSecret": "YOUR_CLIENT_SECRET"
}
```

- Go to [Zoom Marketplace](https://marketplace.zoom.us/)
- **Develop** → **Build App** → **Server-to-Server OAuth**
- Create app, add **meeting:write** scope
- Copy **Account ID**, **Client ID**, **Client Secret**

With these, "Create Zoom Meeting" works. Users can "Join in browser" (opens Zoom in a new tab).

---

## 2. In-App Embedding (Meeting SDK – Optional)

To show the meeting **inside** your app:

1. Go to [Zoom Marketplace](https://marketplace.zoom.us/)
2. **Develop** → **Build App** → **Meeting SDK**
3. In **App Credentials** → **SDK Credentials**, copy **Client ID** and **Client Secret**
4. Add to `appsettings.json`:

**Same app for both create + join:**
```json
"Zoom": {
  "AccountId": "...",
  "ClientId": "YOUR_CLIENT_ID",
  "ClientSecret": "YOUR_CLIENT_SECRET"
}
```

**Separate Meeting SDK app:**
```json
"Zoom": {
  "AccountId": "...",
  "ClientId": "...",
  "ClientSecret": "...",
  "MeetingSdkClientId": "YOUR_MEETING_SDK_CLIENT_ID",
  "MeetingSdkClientSecret": "YOUR_MEETING_SDK_CLIENT_SECRET"
}
```

In-app join uses `ClientId`/`ClientSecret` (or `MeetingSdkClientId`/`MeetingSdkClientSecret` if set).

---

## 3. Troubleshooting

### "Server error" when creating meeting

- Verify **AccountId**, **ClientId**, **ClientSecret**
- Ensure Server-to-Server OAuth app has **meeting:write** scope

### "Join in browser" only (no in-app meeting)

- Normal if **ClientId** and **ClientSecret** are not set for Meeting SDK
- Add a Meeting SDK app and set credentials (see step 2)

### References

- [SDK Key migration](https://developers.zoom.us/docs/meeting-sdk/sdk-key-migration/)
- [Meeting SDK credentials](https://developers.zoom.us/docs/meeting-sdk/get-credentials/)
- [Server-to-Server OAuth](https://developers.zoom.us/docs/internal-apps/create/)
