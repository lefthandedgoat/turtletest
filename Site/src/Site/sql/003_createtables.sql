CREATE TABLE turtletest.Users(
  user_id        SERIAL       PRIMARY KEY NOT NULL,
  name           varchar(64)  NOT NULL UNIQUE,
  email          varchar(256) NOT NULL UNIQUE,
  password       varchar(60)  NOT NULL,
  scheme         smallint     NOT NULL);

CREATE INDEX users_name ON turtletest.Users (name);
CREATE INDEX users_email ON turtletest.Users (email);

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
  version        varchar(1024) NULL,
  owners         varchar(1024) NULL,
  notes          varchar(4096) NULL);

CREATE INDEX suites_fk_applications ON turtletest.Suites (application_id);

CREATE TABLE turtletest.TestCases(
  testcase_id SERIAL           PRIMARY KEY NOT NULL,
  application_id integer       NOT NULL REFERENCES turtletest.Applications (application_id),
  suite_id integer             NOT NULL REFERENCES turtletest.Suites (suite_id),
  name           varchar(1024) NOT NULL,
  version        varchar(1024) NULL,
  owners         varchar(1024) NULL,
  notes          varchar(4096) NULL,
  requirements   varchar(4096) NULL,
  steps          varchar(4096) NULL,
  expected       varchar(4096) NULL,
  history        varchar(4096) NULL,
  attachments    varchar(4096) NULL);

CREATE INDEX testcases_fk_applications ON turtletest.TestCases (application_id);
CREATE INDEX testcases_fk_suites ON turtletest.TestCases (suite_id);

CREATE TABLE turtletest.TestRuns(
  testrun_id SERIAL            PRIMARY KEY NOT NULL,
  application_id integer       NOT NULL REFERENCES turtletest.Applications (application_id),
  run_date       timestamptz   NOT NULL,
  description    varchar(1024) NOT NULL,
  not_run        integer ARRAY NOT NULL,
  passed         integer ARRAY NOT NULL,
  failed         integer ARRAY NOT NULL);

CREATE INDEX testruns_fk_applications ON turtletest.TestRuns (application_id);

GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA turtletest TO turtletest;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA turtletest TO turtletest;
