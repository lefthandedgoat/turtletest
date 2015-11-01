DROP OWNED BY turtletest;
DROP USER IF EXISTS turtletest;

CREATE SCHEMA IF NOT EXISTS turtletest;

CREATE USER turtletest WITH ENCRYPTED PASSWORD 'taconacho';
GRANT USAGE ON SCHEMA turtletest to turtletest;
ALTER DEFAULT PRIVILEGES IN SCHEMA turtletest GRANT SELECT ON TABLES TO turtletest;

CREATE DATABASE turtletest;
GRANT CONNECT ON DATABASE "turtletest" to turtletest;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA turtletest TO turtletest;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA turtletest TO turtletest;

CREATE TABLE turtletest.Applications(
  application_id SERIAL PRIMARY KEY NOT NULL,
  user_id       integer NOT NULL REFERENCES turtletest.Users (user_id),
  name          varchar(1024),
  address       varchar(1024) NULL,
  documentation varchar(1024) NULL,
  owners        varchar(1024) NULL,
  developers    varchar(1024) NULL,
  notes         varchar(4096) NULL);

CREATE INDEX applications_fk_users ON turtletest.Applications (user_id);

CREATE TABLE turtletest.Users(
  user_id SERIAL PRIMARY KEY NOT NULL,
  name          varchar(256),
  email         varchar(256));

INSERT INTO turtletest.Users
  (user_id
   ,name
   ,email
  ) VALUES (
   DEFAULT
   ,'tony'
   ,'tony@null.dev'
  );

CREATE INDEX users_name ON turtletest.Users (name);
