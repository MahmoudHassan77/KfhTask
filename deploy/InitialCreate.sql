CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Users" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "UserId" TEXT NOT NULL,
    "UserName" TEXT NOT NULL,
    "PasswordHash" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL
);

CREATE TABLE "Expenses" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Expenses" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Amount" TEXT NOT NULL,
    "currency" TEXT NOT NULL,
    "Category" TEXT NOT NULL,
    "OccurredOn" TEXT NOT NULL,
    "CreatedByUserIdId" TEXT NOT NULL,
    "RowVersion" BLOB NULL,
    CONSTRAINT "FK_Expenses_Users_CreatedByUserIdId" FOREIGN KEY ("CreatedByUserIdId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Expenses_CreatedByUserIdId" ON "Expenses" ("CreatedByUserIdId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260218092052_initial-migration', '9.0.9');

COMMIT;

