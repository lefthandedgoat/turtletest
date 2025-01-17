DROP OWNED BY turtletest;
DROP USER IF EXISTS turtletest;

CREATE SCHEMA IF NOT EXISTS turtletest;

CREATE USER turtletest WITH ENCRYPTED PASSWORD 'taconacho';
GRANT USAGE ON SCHEMA turtletest to turtletest;
ALTER DEFAULT PRIVILEGES IN SCHEMA turtletest GRANT SELECT ON TABLES TO turtletest;
GRANT CONNECT ON DATABASE "turtletest" to turtletest;
