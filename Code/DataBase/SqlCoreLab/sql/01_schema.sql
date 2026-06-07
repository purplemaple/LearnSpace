-- Core SQL lab schema.
-- This schema keeps the business model simple so the focus stays on SQL,
-- indexes, execution plans, transactions, locks, and concurrency.

DROP TABLE IF EXISTS lab_batch_steps;
DROP TABLE IF EXISTS lab_alarm_events;
DROP TABLE IF EXISTS lab_test_batches;
DROP TABLE IF EXISTS lab_devices;
DROP TABLE IF EXISTS lab_operators;

CREATE TABLE lab_devices (
    id BIGSERIAL PRIMARY KEY,
    device_code VARCHAR(50) NOT NULL,
    device_name VARCHAR(100) NOT NULL,
    model VARCHAR(50) NOT NULL,
    line_code VARCHAR(30) NOT NULL,
    status VARCHAR(20) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    version INTEGER NOT NULL DEFAULT 1,
    CONSTRAINT uq_lab_devices_device_code UNIQUE (device_code),
    CONSTRAINT ck_lab_devices_status CHECK (status IN ('idle', 'running', 'maintenance', 'disabled'))
);

CREATE TABLE lab_operators (
    id BIGSERIAL PRIMARY KEY,
    operator_code VARCHAR(50) NOT NULL,
    operator_name VARCHAR(100) NOT NULL,
    team_name VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uq_lab_operators_operator_code UNIQUE (operator_code)
);

CREATE TABLE lab_test_batches (
    id BIGSERIAL PRIMARY KEY,
    batch_code VARCHAR(80) NOT NULL,
    device_id BIGINT NOT NULL,
    operator_id BIGINT NOT NULL,
    status VARCHAR(20) NOT NULL,
    started_at TIMESTAMP NOT NULL,
    finished_at TIMESTAMP NULL,
    sample_count INTEGER NOT NULL,
    error_code VARCHAR(50) NULL,
    remark VARCHAR(200) NULL,
    CONSTRAINT uq_lab_test_batches_batch_code UNIQUE (batch_code),
    CONSTRAINT fk_lab_test_batches_device FOREIGN KEY (device_id) REFERENCES lab_devices (id),
    CONSTRAINT fk_lab_test_batches_operator FOREIGN KEY (operator_id) REFERENCES lab_operators (id),
    CONSTRAINT ck_lab_test_batches_status CHECK (status IN ('success', 'failed', 'cancelled', 'running')),
    CONSTRAINT ck_lab_test_batches_sample_count CHECK (sample_count >= 0)
);

CREATE TABLE lab_batch_steps (
    id BIGSERIAL PRIMARY KEY,
    batch_id BIGINT NOT NULL,
    step_no INTEGER NOT NULL,
    step_name VARCHAR(100) NOT NULL,
    status VARCHAR(20) NOT NULL,
    started_at TIMESTAMP NOT NULL,
    finished_at TIMESTAMP NULL,
    elapsed_ms INTEGER NOT NULL,
    error_code VARCHAR(50) NULL,
    CONSTRAINT fk_lab_batch_steps_batch FOREIGN KEY (batch_id) REFERENCES lab_test_batches (id),
    CONSTRAINT uq_lab_batch_steps_batch_step UNIQUE (batch_id, step_no),
    CONSTRAINT ck_lab_batch_steps_status CHECK (status IN ('success', 'failed', 'skipped')),
    CONSTRAINT ck_lab_batch_steps_elapsed_ms CHECK (elapsed_ms >= 0)
);

CREATE TABLE lab_alarm_events (
    id BIGSERIAL PRIMARY KEY,
    device_id BIGINT NOT NULL,
    batch_id BIGINT NULL,
    alarm_code VARCHAR(50) NOT NULL,
    alarm_level VARCHAR(20) NOT NULL,
    alarm_message VARCHAR(200) NOT NULL,
    occurred_at TIMESTAMP NOT NULL,
    cleared_at TIMESTAMP NULL,
    CONSTRAINT fk_lab_alarm_events_device FOREIGN KEY (device_id) REFERENCES lab_devices (id),
    CONSTRAINT fk_lab_alarm_events_batch FOREIGN KEY (batch_id) REFERENCES lab_test_batches (id),
    CONSTRAINT ck_lab_alarm_events_alarm_level CHECK (alarm_level IN ('info', 'warning', 'error', 'fatal'))
);
