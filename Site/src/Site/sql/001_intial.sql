DROP OWNED BY turtletest;
DROP USER IF EXISTS turtletest;

CREATE SCHEMA IF NOT EXISTS turtletest;

CREATE USER turtletest WITH ENCRYPTED PASSWORD 'taconacho';
GRANT USAGE ON SCHEMA turtletest to turtletest;
ALTER DEFAULT PRIVILEGES IN SCHEMA turtletest GRANT SELECT ON TABLES TO turtletest;

CREATE DATABASE turtletest;
GRANT CONNECT ON DATABASE "turtletest" to turtletest;

CREATE TABLE turtletest.Users(
  user_id        SERIAL       PRIMARY KEY NOT NULL,
  name           varchar(64)  NOT NULL UNIQUE,
  email          varchar(256) NOT NULL,
  password       varchar(60)  NOT NULL,
  scheme         smallint     NOT NULL);

CREATE INDEX users_name ON turtletest.Users (name);

CREATE TABLE turtletest.Applications(
  application_id SERIAL       PRIMARY KEY NOT NULL,
  user_id        integer       NOT NULL REFERENCES turtletest.Users (user_id),
  private        boolean       NOT NULL,
  name           varchar(1024) NOT NULL,
  address        varchar(1024) NULL,
  documentation  varchar(1024) NULL,
  owners         varchar(1024) NULL,
  developers     varchar(1024) NULL,
  notes          varchar(4096) NULL);

CREATE INDEX applications_fk_users ON turtletest.Applications (user_id);

CREATE TABLE turtletest.Permissions(
  user_id        integer      NOT NULL REFERENCES turtletest.Users (user_id),
  application_id integer      NOT NULL REFERENCES turtletest.Applications (application_id),
  permission     smallint     NOT NULL,
  PRIMARY KEY (user_id, application_id));

CREATE TABLE turtletest.Suites(
  suite_id SERIAL              PRIMARY KEY NOT NULL,
  application_id integer       NOT NULL REFERENCES turtletest.Applications (application_id),
  name           varchar(1024) NOT NULL,
  version        varchar(1024) NOT NULL,
  owners         varchar(1024) NULL,
  notes          varchar(4096) NULL);

CREATE INDEX suites_fk_applications ON turtletest.Suites (application_id);

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA turtletest TO turtletest;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA turtletest TO turtletest;
