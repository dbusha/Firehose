CREATE TABLE feed_items
(
  id          INTEGER NOT NULL,
  feed_id     INT,
  post_id     INT,
  title       VARCHAR(1024),
  description VARCHAR(1024),
  post_date   DATE,
  item_id     VARCHAR(256),
  author      VARCHAR(256),
  link        VARCHAR(1024),
  is_encoded  BOOLEAN,
  CONSTRAINT feed_items_feed_id_post_id_pk
  PRIMARY KEY (feed_id, post_id)
);

CREATE TABLE feed_type
(
  id   INT NOT NULL,
  type VARCHAR(32)
    PRIMARY KEY
);

CREATE TABLE feeds
(
  id               INT,
  name             VARCHAR(256),
  address          VARCHAR(1024)
    PRIMARY KEY,
  last_update      DATETIME,
  refresh_interval INT,
  type             INT
    CONSTRAINT feeds_feed_type_id_fk
    REFERENCES feed_type (id),
  description      VARCHAR(2048)
);

ALTER TABLE feed_items
  ADD constraint feed_content_feeds_id_fk
  FOREIGN KEY (feed_id) REFERENCES feeds (id);

CREATE TABLE user_feed_content
(
  id         INT,
  user_id    INT,
  feed_id    INT
    CONSTRAINT user_feed_content_feeds_id_fk
    REFERENCES feeds (id),
  post_id    INT
    CONSTRAINT user_feed_content_feed_content_post_id_fk
    REFERENCES feed_items (post_id),
  is_read    BOOLEAN,
  is_starred BOOLEAN,
  CONSTRAINT user_feed_content_user_id_post_id_feed_id_pk
  PRIMARY KEY (user_id, post_id, feed_id)
);

CREATE TABLE user_feeds
(
  id      INTEGER NOT NULL,
  user_id INT     NOT NULL,
  feed_id INT     NOT NULL
    CONSTRAINT user_feeds_feeds_id_fk
    REFERENCES feeds (id),
  CONSTRAINT user_feeds_user_id_feed_id_pk
  PRIMARY KEY (user_id, feed_id)
);

CREATE TABLE users
(
  id           INT         NOT NULL,
  first_name   VARCHAR(64),
  last_name    VARCHAR(64),
  display_name VARCHAR(64) NOT NULL
    PRIMARY KEY,
  password     VARCHAR(1024),
  salt         VARCHAR(1024),
  email        VARCHAR(1024)
);

CREATE UNIQUE INDEX users_id_uindex
  ON users (id);

CREATE UNIQUE INDEX users_email_uindex
  ON users (email);

ALTER TABLE user_feed_content
  ADD constraint user_feed_content_users_id_fk
  FOREIGN KEY (user_id) REFERENCES users (id);

ALTER TABLE user_feeds
  ADD constraint user_feeds_users_id_fk
  FOREIGN KEY (user_id) REFERENCES users (id);

