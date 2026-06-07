-- Small deterministic seed data for query practice.

INSERT INTO lab_devices (device_code, device_name, model, line_code, status)
VALUES
('DEV-001', 'Sample Loader 01', 'SL-A', 'LINE-A', 'idle'),
('DEV-002', 'Sample Loader 02', 'SL-A', 'LINE-A', 'running'),
('DEV-003', 'Reagent Module 01', 'RM-B', 'LINE-B', 'maintenance'),
('DEV-004', 'Washer 01', 'WS-C', 'LINE-B', 'idle'),
('DEV-005', 'Reader 01', 'RD-D', 'LINE-C', 'disabled');

INSERT INTO lab_operators (operator_code, operator_name, team_name)
VALUES
('OP-001', 'Alice', 'Alpha'),
('OP-002', 'Bob', 'Alpha'),
('OP-003', 'Cindy', 'Beta'),
('OP-004', 'David', 'Beta');

INSERT INTO lab_test_batches (
    batch_code,
    device_id,
    operator_id,
    status,
    started_at,
    finished_at,
    sample_count,
    error_code,
    remark
)
SELECT
    'BATCH-' || to_char(gs, 'FM0000') AS batch_code,
    ((gs - 1) % 5) + 1 AS device_id,
    ((gs - 1) % 4) + 1 AS operator_id,
    CASE
        WHEN gs % 11 = 0 THEN 'cancelled'
        WHEN gs % 5 = 0 THEN 'failed'
        ELSE 'success'
    END AS status,
    TIMESTAMP '2026-05-01 08:00:00' + (gs || ' hours')::INTERVAL AS started_at,
    TIMESTAMP '2026-05-01 08:00:00' + (gs || ' hours')::INTERVAL + (((gs % 40) + 5) || ' minutes')::INTERVAL AS finished_at,
    (gs % 80) + 1 AS sample_count,
    CASE
        WHEN gs % 5 = 0 THEN 'E-' || to_char(gs % 7, 'FM000')
        ELSE NULL
    END AS error_code,
    CASE
        WHEN gs % 5 = 0 THEN 'Generated failure case'
        ELSE NULL
    END AS remark
FROM generate_series(1, 80) AS gs;

INSERT INTO lab_batch_steps (
    batch_id,
    step_no,
    step_name,
    status,
    started_at,
    finished_at,
    elapsed_ms,
    error_code
)
SELECT
    b.id,
    s.step_no,
    s.step_name,
    CASE
        WHEN b.status = 'failed' AND s.step_no = ((b.id % 5) + 1) THEN 'failed'
        WHEN b.status = 'cancelled' AND s.step_no > 3 THEN 'skipped'
        ELSE 'success'
    END AS status,
    b.started_at + ((s.step_no - 1) * INTERVAL '3 minutes') AS started_at,
    b.started_at + (s.step_no * INTERVAL '3 minutes') AS finished_at,
    1000 + ((b.id * s.step_no) % 5000) AS elapsed_ms,
    CASE
        WHEN b.status = 'failed' AND s.step_no = ((b.id % 5) + 1) THEN b.error_code
        ELSE NULL
    END AS error_code
FROM lab_test_batches b
CROSS JOIN (
    VALUES
        (1, 'Load sample'),
        (2, 'Move arm'),
        (3, 'Add reagent'),
        (4, 'Wash'),
        (5, 'Read result')
) AS s(step_no, step_name);

INSERT INTO lab_alarm_events (
    device_id,
    batch_id,
    alarm_code,
    alarm_level,
    alarm_message,
    occurred_at,
    cleared_at
)
SELECT
    b.device_id,
    b.id,
    COALESCE(b.error_code, 'W-' || to_char(b.id % 5, 'FM000')) AS alarm_code,
    CASE
        WHEN b.status = 'failed' THEN 'error'
        WHEN b.status = 'cancelled' THEN 'warning'
        ELSE 'info'
    END AS alarm_level,
    CASE
        WHEN b.status = 'failed' THEN 'Batch failed'
        WHEN b.status = 'cancelled' THEN 'Batch cancelled'
        ELSE 'Generated informational alarm'
    END AS alarm_message,
    b.started_at + INTERVAL '10 minutes' AS occurred_at,
    b.started_at + INTERVAL '16 minutes' AS cleared_at
FROM lab_test_batches b
WHERE b.status <> 'success' OR b.id % 13 = 0;
