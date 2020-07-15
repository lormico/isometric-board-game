BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "board_tiles" (
	"tile_id"	INTEGER NOT NULL UNIQUE,
	"x"	INTEGER NOT NULL,
	"y"	INTEGER NOT NULL,
	"room"	TEXT NOT NULL,
	"tile"	TEXT NOT NULL DEFAULT 'default',
	PRIMARY KEY("x","y"),
	FOREIGN KEY("room") REFERENCES "rooms"("name")
);
CREATE TABLE IF NOT EXISTS "meta" (
	"key"	TEXT NOT NULL,
	"value"	TEXT,
	PRIMARY KEY("key")
);
CREATE TABLE IF NOT EXISTS "characters" (
	"id"	INTEGER NOT NULL,
	"name"	TEXT NOT NULL UNIQUE,
	"tile"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "weapons" (
	"id"	INTEGER NOT NULL,
	"name"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("id")
);
CREATE TABLE IF NOT EXISTS "rooms" (
	"name"	TEXT NOT NULL,
	PRIMARY KEY("name")
);
CREATE TABLE IF NOT EXISTS "shortcuts" (
	"room1"	TEXT NOT NULL,
	"room2"	TEXT NOT NULL,
	PRIMARY KEY("room1","room2"),
	FOREIGN KEY("room1") REFERENCES "rooms"("name")
	FOREIGN KEY("room2") REFERENCES "rooms"("name"),
);
CREATE TABLE IF NOT EXISTS "obstacles" (
	"tile_id1"	INTEGER NOT NULL,
	"tile_id2"	INTEGER NOT NULL,
	PRIMARY KEY("tile_id1","tile_id2"),
	FOREIGN KEY("tile_id2") REFERENCES "board_tiles"("tile_id"),
	FOREIGN KEY("tile_id1") REFERENCES "board_tiles"("tile_id")
);
INSERT INTO "meta" VALUES ('pack',NULL);
INSERT INTO "meta" VALUES ('level_name',NULL);
INSERT INTO "rooms" VALUES ('hallway');
INSERT INTO "rooms" VALUES ('entrypoint');
COMMIT;
