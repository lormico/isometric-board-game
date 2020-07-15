BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "board_tiles" (
	"x"	INTEGER NOT NULL,
	"y"	INTEGER NOT NULL,
	"room"	TEXT NOT NULL,
	"tile"	TEXT NOT NULL DEFAULT 'default',
	FOREIGN KEY("room") REFERENCES "rooms"("name"),
	PRIMARY KEY("x","y")
);
CREATE TABLE IF NOT EXISTS "meta" (
	"key"	TEXT NOT NULL,
	"value"	INTEGER,
	PRIMARY KEY("key")
);
CREATE TABLE IF NOT EXISTS "players" (
	"id"	INTEGER NOT NULL,
	"name"	TEXT NOT NULL UNIQUE,
	"tile"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "weapons" (
	"name"	TEXT NOT NULL,
	PRIMARY KEY("name")
);
CREATE TABLE IF NOT EXISTS "rooms" (
	"name"	TEXT NOT NULL,
	"door_x"	INTEGER,
	"door_y"	INTEGER,
	"door_opening"	INTEGER,
	PRIMARY KEY("name")
);
CREATE TABLE IF NOT EXISTS "shortcuts" (
	"room_a"	TEXT NOT NULL,
	"room_b"	TEXT NOT NULL,
	FOREIGN KEY("room_a") REFERENCES "rooms"("name"),
	FOREIGN KEY("room_b") REFERENCES "rooms"("name"),
	PRIMARY KEY("room_a","room_b")
);
INSERT INTO "meta" VALUES ('pack',NULL);
INSERT INTO "meta" VALUES ('level_name',NULL);
INSERT INTO "rooms" VALUES ('hallway',NULL,NULL,NULL);
INSERT INTO "rooms" VALUES ('entrypoint',NULL,NULL,NULL);
COMMIT;
