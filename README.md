# WaterDelivery

A full-stack water-delivery store built with **ASP.NET Core** (minimal APIs), **Blazor Server**, and **MongoDB**, following Clean Architecture with a CQRS-style use-case layer (via the `Mediator` source generator). Product images are stored in **MinIO** (S3-compatible). A background worker drives deliveries through their lifecycle using the **Transactional Outbox** pattern.

---

## Architecture

```
WaterDelivery.Contracts      DTOs + enums shared across projects (no dependencies)
WaterDelivery.Backend        Domain entities, use-cases, repositories, REST API
WaterDelivery.UI             Blazor Server storefront (catalog, cart, checkout, order status)
WaterDelivery.BackgroundJobs Hosted worker that advances delivery status over time
WaterDelivery.Tests          xUnit unit + integration tests (integration needs a live Mongo)
```

The solution layers cleanly:

- **Core** — framework-free domain entities (`Order`, `Bill`, `Delivery`, `Address`, `CustomerAddresses`, …) that own their invariants and status machines.
- **Features** — one folder per aggregate, each with a controller (endpoint mapping), use-cases (`IRequestHandler<TRequest,TResponse>`), and repository interfaces.
- **Infrastructure** — MongoDB persistence (`*Db` entities + mapping extensions), MinIO, and DI registration.

---

## Prerequisites

- **.NET 8 SDK** (the projects target `net8.0`; `global.json` rolls forward to the latest installed major).
- **Docker** (for MongoDB and MinIO), or local installs of both.
- A **Google OAuth 2.0 Client** (for sign-in).

---

## Running locally

### 1. Start infrastructure

```bash
docker compose -f mongo-compose.yml up -d     # MongoDB on :27017 (root / password)
docker compose -f minio.yaml      up -d       # MinIO  on :9000 (API) and :9001 (console), admin / admin123
```

MinIO auto-creates the `waterbucket` bucket. The console is at http://localhost:9001.

### 2. Configure secrets

Copy the example settings and fill in your own values — **do not commit real secrets**:

```bash
cp WaterDelivery.Backend/appsettings.example.json WaterDelivery.Backend/appsettings.json
cp WaterDelivery.UI/appsettings.example.json      WaterDelivery.UI/appsettings.json
```

Set the Google client id/secret in **both** files under `ExternalAuthSettings:Google` (see the next section).

### 3. Run the apps

In three terminals (or via your IDE's multi-launch):

```bash
dotnet run --project WaterDelivery.Backend          # http://localhost:5017  (Swagger at /swagger)
dotnet run --project WaterDelivery.UI               # http://localhost:5167
dotnet run --project WaterDelivery.BackgroundJobs   # no HTTP surface; logs delivery progress
```

Open **http://localhost:5167** to use the store.

---

## Google sign-in setup (important)

Authentication is handled **entirely by the UI project** (the Blazor host on `:5167`). The UI performs the OAuth handshake, receives the Google profile, asks the backend to upsert the user (`POST /api/auth/google-user`), and then issues its own auth cookie carrying the internal user id. Because the cookie is issued on the same host the pages run on, the signed-in user id is available everywhere — including checkout — without cross-origin problems.

In the [Google Cloud Console](https://console.cloud.google.com/) → **APIs & Services → Credentials → your OAuth client**, set:

- **Authorized redirect URI:** `http://localhost:5167/signin-google`

> This is the UI host, **not** the backend. The handshake used to live in the backend on `:5017`; it has been moved to the UI, so the redirect URI changed accordingly. If you still have the old `:5017` URI registered you can remove it.

The `client_id` and `client_secret` go in `appsettings.json` under `ExternalAuthSettings:Google` for **both** the UI and backend projects (the backend keeps a copy only so the section binds; it no longer runs the OAuth challenge).

---

## How the order flow works

1. **Catalog → Cart** — browse products (served from MongoDB, images from MinIO) and add to an in-memory cart.
2. **Checkout** (`/proceed-to-checkout`)
   - Requires sign-in; unauthenticated users are sent to `/login?returnUrl=…`.
   - Loads the customer's **saved addresses** and shows them as selectable cards.
   - "Use a new address" reveals the address form. A new address is **saved to the customer's address book** (de-duplicated) on submit; an existing one is reused as-is.
   - Creates the **Order** and a **Bill** (`WaitForPayment`), then redirects to the order-status page.
3. **Order status** (`/order-status/{billId}`)
   - Shows the order summary and live bill/delivery status.
   - **Confirm payment** → marks the bill `Paid` and **creates a Delivery** (status `Assembly`), which writes an outbox row.
   - **Cancel order** → marks the bill `Cancelled`. No payment is taken.
4. **Delivery progression** — the background worker picks up the outbox row and advances the delivery one step per cycle along its state machine:
   `Assembly → TransferredDeliveryService → WaitToDelivery → Delivering → IssuedToCourier → Delivered`.
   Refresh the order-status page to watch it move (the delivery is persisted at each step).

### Transactional Outbox

When a delivery is created or updated, an `Outbox` row (payload = delivery id) is written in the same unit of work. The worker:

- reads `Pending` rows,
- loads the referenced delivery, advances it one legal step, and persists it,
- keeps the row `Pending` until the delivery reaches a **terminal** state (`Delivered`/`Rejected`/`Cancelled`), then marks it `Processed`.

This means a single create seeds the whole pipeline — the worker keeps nudging the delivery forward until it's delivered.

---

## Tests

```bash
dotnet test
```

- **Unit tests** (`UseCaseTests`, `EntitiesTests`) mock repositories with NSubstitute and run anywhere.
- **Integration tests** (`IntegrationTests`) talk to a **real MongoDB** and require the Mongo container from step 1 to be running.

---

## Notes & conventions

- **Status machines live in the domain.** `Bill.SetStatus` and `Delivery.SetStatus` reject illegal transitions; the worker and UI only request transitions the machines allow.
- **Money/quantities** are validated in entity constructors (e.g. positive prices, non-empty order items).
- **DateTimes** are handled in UTC; bill creation-date validation compares in UTC to stay correct regardless of server timezone.
- **Secrets**: `appsettings.json` is intended to hold local-only values. Keep real Google credentials out of source control (use the example files as templates, or user-secrets / environment variables).
