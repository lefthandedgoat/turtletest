DROP OWNED BY emails;
DROP USER IF EXISTS emails;

CREATE SCHEMA IF NOT EXISTS emails;

CREATE USER emails WITH ENCRYPTED PASSWORD 'taconacho';
GRANT USAGE ON SCHEMA emails to emails;
ALTER DEFAULT PRIVILEGES IN SCHEMA emails GRANT SELECT ON TABLES TO emails;

CREATE DATABASE emails;
GRANT CONNECT ON DATABASE "emails" to emails;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA emails TO emails;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA emails TO emails;

--switch to the emails user here
DROP TABLE IF EXISTS main_page CASCADE;

CREATE TABLE main_page(
  id SERIAL PRIMARY KEY NOT NULL,
  email varchar(120) NULL,
  date_created timestamptz NOT NULL default (now() at time zone 'utc'));
