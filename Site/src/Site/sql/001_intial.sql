DROP OWNED BY turtletest;
DROP USER IF EXISTS turtletest;

CREATE SCHEMA IF NOT EXISTS turtletest;

CREATE USER turtletest WITH ENCRYPTED PASSWORD 'taconacho';
GRANT USAGE ON SCHEMA turtletest to turtletest;
ALTER DEFAULT PRIVILEGES IN SCHEMA turtletest GRANT SELECT ON TABLES TO turtletest;

CREATE DATABASE turtletest;
GRANT CONNECT ON DATABASE "turtletest" to turtletest;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA turtletest TO turtletest;
GRANT ALL PRIVELEGES ON ALL TABLES IN SCHEMA turtletest TO turtletest;

CREATE TABLE turtletest.Applications(
  application_id SERIAL PRIMARY KEY NOT NULL,
  name          varchar(1024),
  address       varchar(1024) NULL,
  documentation varchar(1024) NULL,
  owners        varchar(1024) NULL,
  developers    varchar(1024) NULL,
  notes         varchar(4096) NULL);

INSERT INTO turtletest.Applications
  (name
   ,address
   ,documentation
   ,owners
   ,developers
   ,notes
  ) VALUES (
   'Android'
   ,'http://www.android.com'
   ,'https://developer.android.com/guide/index.html'
   ,NULL
   ,'Bob Johnson, Susanne Billings, Tim Duncan'
   ,'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?'
  );
